using System;
using System.Runtime.InteropServices;

namespace InfiniteProcessLauncher
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            try
            {
                var programPath = args[0];
                var stringPath = programPath.Replace(@"\", @"\\");
                stringPath = $"\"{stringPath}\"";

                string arguments = null;
                string stringArguments = null;
                var hideWindow = false;

                if (args.Length >= 2)
                {
                    try
                    {
                        //If it successes, no arguments have been passed to the program called
                        hideWindow = Convert.ToBoolean(args[1]);
                    }
                    catch (Exception)
                    {
                        arguments = args[1];
                        stringArguments = arguments.Replace(@"\", @"\\");
                        stringArguments = $"\"{stringArguments}\"";
                    }
                }
                if(args.Length >= 3)
                {
                    hideWindow = Convert.ToBoolean(args[2]);
                }
                if(hideWindow)
                {
                    var handle = GetConsoleWindow();
                    ShowWindow(handle, SW_HIDE);
                }

                try
                {
                    var infiniteProcessHelper = new InfiniteProcessHelper(programPath, stringArguments);
                    infiniteProcessHelper.ProcessLaunched += InfiniteProcessHelper_ProcessLaunched;
                    Console.WriteLine($"Executing: \"{programPath}\" \"{arguments}\"");
                    infiniteProcessHelper.Run();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Usage: dotnet InfiniteProcessLauncher.dll \"Path\\To\\Program.exe\" [\"Optional Parameters\"] [hide window (default: false)]");
                Console.Read();
            }   
        }

        private static void InfiniteProcessHelper_ProcessLaunched(object sender, EventArgs e)
        {
            Console.WriteLine($"Process launched at {DateTime.Now}");
        }
    }
}
