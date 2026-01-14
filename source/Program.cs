namespace DuplicatesFinder
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
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