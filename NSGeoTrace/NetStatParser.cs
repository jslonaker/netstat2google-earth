using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSGeoTrace
{
    public class NetStatParser
    {
        public List<string> NetStatIpList(string path)
        {
            var ret = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(path);
                for (int i = 4; i < lines.Count(); i++)
                {

                    lines[i] = Regex.Replace(lines[i], @"\s+", " ");

                    string[] tmp = lines[i].Split(' ');
                    string[] ip_port = tmp[3].Split(':');
                    if (ip_port[0] != "127.0.0.1")
                    {
                        if (!ret.Contains(ip_port[0]))
                        {
                            ret.Add(ip_port[0]);

                        }

                    }


                }
            }
            catch (Exception ex)
            {

            }

            return ret;
        }

    }
}
