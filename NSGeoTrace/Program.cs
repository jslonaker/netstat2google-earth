// Decompiled with JetBrains decompiler
// Type: NSGeoTrace.Program
// Assembly: NSGeoTrace, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 138666A4-5550-4537-8014-40438833AB04
// Assembly location: D:\Projects\netstat2google-earth-master\NSGeoTrace\bin\Debug\NSGeoTrace.exe

using GeoIpAPI.DataAccess;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using SystemControl;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
 using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System;
using System.Linq;

namespace NSGeoTrace
{
    internal static class Program
    {
        private static NetstatController nsctrl = new NetstatController();
        private static NetStatParser nsparse = new NetStatParser();
        private static GoogleEarthHandler geh = new GoogleEarthHandler();
        private static GeoApiAccess api = new GeoApiAccess();
        //private static TaskListController tlc = new TaskListController();
        //private static TaskListParser tlp = new TaskListParser();
        private static DataTable _dt_report = new DataTable();
        private static Dictionary<string, object> dict_ip_2_geoloc = new Dictionary<string, object>();
        private static string apiKey = ConfigurationManager.AppSettings["geoapikey"].ToString();
        private static void Main(string[] args)
        {

            _dt_report.Columns.Add("ip_addr", typeof(string));
            _dt_report.Columns.Add("pid", typeof(int));
            _dt_report.Columns.Add("latitude", typeof(string));
            _dt_report.Columns.Add("longitude", typeof(string));
            _dt_report.Columns.Add("city", typeof(string));
            _dt_report.Columns.Add("state", typeof(string));
            _dt_report.Columns.Add("process_name", typeof(string));
            _dt_report.Columns.Add("zip", typeof(string));



            nsctrl.Run();
            nsparse.Run();
            foreach(KeyValuePair<string,int> k in nsparse.IPxPID)
            {
                if (k.Key != "0.0.0.0")
                {
                    DataRow dr = _dt_report.NewRow();
                    dr["ip_addr"] = k.Key;
                    dr["pid"] = k.Value;
                    api.GeoApiRequest(dr["ip_addr"].ToString(), apiKey);
                    dynamic apiResult = JsonConvert.DeserializeObject(api.Result);
                    dr["latitude"] = apiResult.latitude;
                    dr["longitude"] = apiResult.longitude;
                    dr["city"] = apiResult.city;
                    dr["state"] = apiResult.region_name;
                    dr["zip"] = apiResult.zip;
                    var cApp = new ConsoleAppExec(@"c:\windows\system32\tasklist", string.Empty);
                    string[] lines = cApp.Data.Split(new[]
                    {
                        Environment.NewLine
                    }, StringSplitOptions.None);

                    foreach(string s in lines)
                    {
                        if (s.Contains(k.Value.ToString()))
                        {
                            string cleaned = Regex.Replace(s, @"\s+", " ");
                            string[] colArr = cleaned.Split(' ');
                            dr["process_name"] = colArr[0];


                        }
                    }
                         _dt_report.Rows.Add(dr);

                }

            }
            
            geh.Run(_dt_report);
            if (File.Exists(@".\doc.kml"))
            {
                string pwd = Directory.GetCurrentDirectory();
                new ConsoleAppExec(ConfigurationManager.AppSettings["google_earth_exe"].ToString(),string.Concat(pwd,@"\doc.kml"));
            }


        }
    }
}
