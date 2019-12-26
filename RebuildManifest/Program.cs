using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RebuildManifest
{
    class Program
    {
        static void Main(string[] args)
        {
            string sFilePath = args[0];
            string sPackageName = args[1];
            string sDebugMode = args[2];
            System.IO.File.Copy(sFilePath, sFilePath + ".bak", true);
            if (sDebugMode.ToUpper() == "DEBUG")
            {
                sPackageName += ".Debug";
            }
            //System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            //xmlDoc.Load(sFilePath);
            //xmlDoc.SelectSingleNode("//")
            string sContent = System.IO.File.ReadAllText(sFilePath);
            int iPos1 = sContent.IndexOf("package=\"");
            if (iPos1 > 0)
            {
                int iPos2 = sContent.IndexOf("\"", iPos1 + 9);
                string sNewContent = sContent.Substring(0, iPos1);
                sNewContent += "package=\"" + sPackageName + "\"";
                sNewContent += sContent.Substring(iPos2 + 1);
                System.IO.File.WriteAllText(sFilePath, sNewContent);
                Console.WriteLine("OK!");
            }
            else
            {
                Console.WriteLine("ERROR!");
            }
        }
    }
}
