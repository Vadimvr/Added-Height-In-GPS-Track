using LibSRMTHeight;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml;
namespace Added_Height_In_GPS_Track
{
    public class Test
    {
        public static void Main()
        {
            var allFiles = Directory.GetFiles(AppContext.BaseDirectory)
                .Where(x => x.Length > 4
                && x.Substring(x.Length - 4).Equals(".kml", StringComparison.InvariantCultureIgnoreCase) && !x.Contains("update"));
            int i = 0;
            foreach (var file in allFiles)
            {
                //var x = file.Substring(file.Length - 4) == ".kml";
                Worker(file);
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"added {i}");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private static void Worker(string path)
        {
            XmlDocument kml = new XmlDocument();
            kml.Load(path);
            XmlNode newNode = kml.CreateElement("name");
            // oruxmaps track name
            newNode.InnerXml = $"<![CDATA[{path.Substring(AppContext.BaseDirectory.Length)}]]>";


            XmlNodeList doc = kml.GetElementsByTagName("Document");
            if (doc != null && doc.Count == 1 && doc[0]!=null)
            {
                doc[0]?.AppendChild(newNode);
            }
            XmlNodeList elemList = kml.GetElementsByTagName("coordinates");
            if (elemList == null) return;
            for (int i = 0; i < elemList.Count; i++)
            {
                if (elemList[i] == null || elemList[i].InnerXml == null) { continue; }
                List<Coordinate> coordinates = CovertCoordinate.ConvertToDoubleCoordinates(elemList[i].InnerXml);
                var res = Coordinate.AddPoint(coordinates);
                elemList[i].InnerXml = res.Select(x => x.ToString()).Aggregate("", (current, next) => current + " " + next);
            }

            string newPath = path.Substring(0, path.Length - 4) + "-update" + path.Substring(path.Length - 4);
            if (Coordinate.GetError().Count() == 0)
            {
                kml.Save(newPath);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Added {newPath}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"no height titles for\n\t{path}");
                foreach (var item in Coordinate.GetError())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\tsrtm\\{item}");
                    //Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe", "https://srtm.kurviger.de/SRTM3/Eurasia/" + item + ".zip");
                }
                Console.ResetColor();
            }
        }
    }
}