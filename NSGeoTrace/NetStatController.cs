using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
namespace NSGeoTrace
{
    public class NetstatController
    {
        private string _nsdata = null;
        public string Data
        {
            get
            {
                return _nsdata;
            }
        }

        public void Run()
        {
            using (Process proc = new Process())
            {

                proc.StartInfo.FileName = @"c:\windows\system32\netstat.exe";
                proc.StartInfo.Arguments = @"-a -n -o";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                _nsdata = proc.StandardOutput.ReadToEnd();
                
                proc.WaitForExit();

                
            }
            if (File.Exists("@netstat.log"))
            {
                File.Delete(@"netstat.log");
            }
            File.WriteAllText(@"netstat.log", _nsdata);

        }
    }

}
