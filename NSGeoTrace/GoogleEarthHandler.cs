﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSGeoTrace
{
    public class GoogleEarthHandler
    {
        /* (out of place text to replace: [[replaceme]]
         * 
         * (template)
         
         <Placemark>
        <name>Simple placemark</name>
        <description>Attached to the ground. Intelligently places itself at the
          height of the underlying terrain.</description>
        <Point>
          <coordinates>-122.0822035425683,37.42228990140251,0</coordinates>
        </Point>
      </Placemark>
      
        
    */

        private string _kmldocument;
        public string KmlDocument
        {
            get
            {
                return _kmldocument;
            }
        }
        private string _kmlDoc = File.ReadAllText(@".\resources\template.kml");
        private string _kmlPlacemarkXml = @"<Placemark>
        <name>--name--</name>
<styleUrl>#msn_cross-hairs</styleUrl>        
<description>--desc--</description>
        <Point>
          <coordinates>--lon--,--lat--,0</coordinates>
        </Point>
      </Placemark>";

        public void BuildKml(Dictionary<string,dynamic> GeoApiResult)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, dynamic> kvp_ip_result in GeoApiResult)
            {
                string tmp = _kmlPlacemarkXml;
                dynamic Result = kvp_ip_result.Value;
                string humanReadableLoc = string.Empty;
                humanReadableLoc = string.Concat(humanReadableLoc, "City: ", Result.city, ", State:  ", Result.region_name);
                tmp = tmp.Replace("--name--", Result.ip.ToString());
                tmp = tmp.Replace("--desc--", humanReadableLoc);
                tmp = tmp.Replace("--lat--", Result.latitude.ToString());
                tmp = tmp.Replace("--lon--", Result.longitude.ToString());
                tmp = string.Concat(tmp,Environment.NewLine);
                sb.Append(tmp);

            }
            string documentData = _kmlDoc.Replace("--replaceme--", sb.ToString());
            if (File.Exists(@"doc.kml"))
            {
                File.Delete(@"doc.kml");
            }

            File.WriteAllText(@"doc.kml",documentData);

            /*
             string x = _placemark_syntax;
             x = x.Replace("--desc--", GeoApiResult.ipaddress);
             x = x.Replace("--lat--", GeoApiResult.latitude);
             x = x.Replace("--lon--", GeoApiResult.longitude);
             */
        }
    }
}
