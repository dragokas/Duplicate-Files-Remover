namespace DuplicatesFinder
{
    internal static class Program
    {
        private static Mutex? mutex = null;
        private const string MutexName = "DragoDuplicatesFinder_mtx";

        [STAThread]
        static void Main()
        {
            Globals.AppVersion = "1.6";

            ApplicationConfiguration.Initialize();

            bool createdNew;
            mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Application is already running!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            InitTools();
            Application.Run(new FormAccept());
            Log.Release();
        }

        static void InitTools()
        {
            Log.Initialize(false);
            Globals.DB.Open();
        }
    }
}