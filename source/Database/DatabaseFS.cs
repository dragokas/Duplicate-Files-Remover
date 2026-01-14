using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace DuplicatesFinder
{
    internal class DatabaseFS
    {
        internal static class ColumnName
        {
            internal static readonly string id = "id";
            internal static readonly string path = "Path";
            internal static readonly string extension = "Ext";
            internal static readonly string size = "Size";
        }

        private SQLiteConnection Connection;
        private readonly static string TableName = "Files";
        private bool disposedValue = false;

        internal DatabaseFS(bool eraseDatabase)
        {
            Log.Initialize(false);

            string databasePath = GetDatabasePath();

            if (eraseDatabase)
            {
                if (File.Exists(databasePath))
                {
                    try
                    {
                        File.Delete(databasePath);
                        File.Delete(databasePath + "-shm");
                        File.Delete(databasePath + "-wal");
                    }
                    catch (Exception ex)
                    {
                        Globals.ReportError(ex, true, "Failed to delete previous database file!");
                    }
                }
            }
            if (!File.Exists(databasePath))
            {
                Log.Warning("Cannot find database file: " + GetDatabasePath());
                Connection = CreateDatabase();
            }
            else
            {
                Connection = new SQLiteConnection(GetDatabaseConnectionString());
            }
        }

        internal void Open()
        {
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                this.Connection.Open();
            }
        }

        internal void Close()
        {
            if (this.Connection.State == System.Data.ConnectionState.Open)
            {
                this.Connection.Close();
            }
        }

        private string GetDatabasePath()
        {
            return Path.Combine(Application.StartupPath, "FileSystem.db");
        }

        private string GetDatabaseConnectionString()
        {
            return "Data Source=" + GetDatabasePath() +
                ";New=False;Compress=False;Journal Mode=Wal;Page Size=4096;" +
                "BusyTimeout=100;PrepareRetries=5;Synchronous=Normal;";
        }

        private SQLiteConnection CreateDatabase()
        {
            try
            {
                Log.Warning("Creating new database.");

                SQLiteConnection.CreateFile(GetDatabasePath());
                SQLiteConnection Conn = new SQLiteConnection(GetDatabaseConnectionString());
                Conn.Open();

                SQLiteCommand Command = Conn.CreateCommand();
                Command.CommandType = CommandType.Text;

                Command.CommandText = "CREATE TABLE " + TableName + " (" +
                    ColumnName.id + " INTEGER NOT NULL UNIQUE, " +
                    ColumnName.path + " TEXT NOT NULL, " +
                    ColumnName.extension + " TEXT NOT NULL, " +
                    ColumnName.size + " INTEGER NOT NULL, " +
                    "UNIQUE(" + ColumnName.path + "), " +
                    "PRIMARY KEY(" + ColumnName.id + " AUTOINCREMENT));";
                Command.ExecuteNonQuery();
                return Conn;
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex);
            }
            return new SQLiteConnection();
        }

        internal bool InsertPath(string path, long size)
        {
            bool isChanged = false;
            try
            {
                SQLiteCommand Command = new SQLiteCommand();
                Command = this.Connection.CreateCommand();
                Command.CommandText = String.Format("REPLACE INTO {0} ({1},{2},{3}) VALUES (@p1,@p2,@p3)",
                    TableName,
                    ColumnName.path,      // p1
                    ColumnName.extension, // p2
                    ColumnName.size       // p3
                    );
                Command.CommandType = CommandType.Text;
                Command.Parameters.Add(new SQLiteParameter("@p1", path));
                Command.Parameters.Add(new SQLiteParameter("@p2", Path.GetExtension(path)));
                Command.Parameters.Add(new SQLiteParameter("@p3", size));
                if (Command.ExecuteNonQuery() > 0)
                {
                    isChanged = true;
                }
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false);
            }
            return isChanged;
        }

        internal void RemoveAll()
        {
            SQLiteCommand Command = this.Connection.CreateCommand();
            Command.CommandText = String.Format("DELETE FROM {0}", TableName);
            Command.CommandType = CommandType.Text;
            Command.ExecuteNonQuery();
            Command.CommandText = String.Format("UPDATE sqlite_sequence SET seq = 0 WHERE name = '{0}'", TableName);
            Command.ExecuteNonQuery();
        }

        internal List<string> GetFilePathsBySize(long size)
        {
            DataTable table = new();
            SQLiteCommand command = this.Connection.CreateCommand();
            command.CommandText = "SELECT " + ColumnName.path + " FROM " + TableName + " "
                + "WHERE " + ColumnName.size + " = @p1";
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new SQLiteParameter("@p1", size));
            SQLiteDataAdapter adapter = new();
            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table.AsEnumerable()
                .Select(row => row.Field<string>(ColumnName.path)!)
                .ToList();
        }

        internal List<long> GetUniqueSizes()
        {
            List<long> uniqueSizes = new();
            string query = "SELECT " + ColumnName.size + " FROM " + TableName + 
                " GROUP BY " + ColumnName.size + " HAVING COUNT(*) > 1;";

            using (SQLiteCommand command = new(query, this.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        uniqueSizes.Add(Convert.ToInt64(reader[ColumnName.size]));
                    }
                }
            }
            return uniqueSizes.ToList();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Close();
                }
                disposedValue = true;
            }
        }

        ~DatabaseFS()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    
}
