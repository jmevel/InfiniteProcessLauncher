using System;
using System.Diagnostics;

namespace InfiniteProcessLauncher
{
    public class InfiniteProcessHelper
    {
        private string _programName { get; set; }

        private string _arguments { get; set; }

        private Process _process { get; set; }

        public event EventHandler ProcessLaunched;

        public InfiniteProcessHelper(string programPath, string arguments = null)
        {
            _programName = programPath;
            _arguments = arguments;
        }

        public void Run()
        {
            try
            {
                KillProcessIfRunning();
                CreateProcessAndWait();
            }
            catch(Exception)
            {
                KillProcessIfRunning();
            }
            finally
            {
                Run();
            }
        }

        //Just extra security but should never happen
        private void KillProcessIfRunning()
        {
            if (_process != null && _process.HasExited == false)
            {
                _process.Kill();
                _process = null;
            }
        }

        private void CreateProcessAndWait()
        {
            _process = new Process();
            _process.StartInfo.FileName = _programName;
            _process.StartInfo.Arguments = _arguments;

            //No need to subscribe to the event. The WaitForExit is going to end and the Run() is alled again in the Finally
            //_process.Exited += _process_Exited;

            _process.Start();
            ProcessLaunched(this, new EventArgs());
            _process.WaitForExit();
        }
    }
}
