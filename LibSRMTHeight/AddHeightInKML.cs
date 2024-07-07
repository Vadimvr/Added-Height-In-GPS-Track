using System.Xml;

namespace LibSRMTHeight
{
    public class AddHeightInKML
    {
        public static void Worker(string path, Action<string, SystemMessageType> printer, string titleOruxMaps, string addedToName = "-update")
        {
            XmlDocument kml = new XmlDocument();
            kml.Load(path);
            XmlNode newNode = kml.CreateElement("name");
            // oruxmaps track name
            newNode.InnerXml = $"<![CDATA[{titleOruxMaps}]]>";


            XmlNodeList doc = kml.GetElementsByTagName("Document");
            if (doc != null && doc.Count == 1 && doc[0] != null)
            {
                doc[0]?.AppendChild(newNode);
            }
            XmlNodeList elemList = kml.GetElementsByTagName("coordinates");
            if (elemList == null) return;
            for (int i = 0; i < elemList.Count; i++)
            {
                if (elemList[i] == null || elemList[i].InnerXml == null) { continue; }
                List<Coordinate> coordinates = CovertCoordinate.ConvertToDoubleCoordinates(elemList[i].InnerXml, printer);
                var res = Coordinate.AddPoint(coordinates, printer);
                elemList[i].InnerXml = res.Select(x => x.ToString()).Aggregate("", (current, next) => current + " " + next);
            }

            string newPath = path.Substring(0, path.Length - 4) + addedToName + path.Substring(path.Length - 4);
            if (Coordinate.GetError().Count() == 0)
            {
                kml.Save(newPath);
                printer?.Invoke($"Added {newPath}", SystemMessageType.Ok);
            }
            else
            {
                printer?.Invoke($"no height titles for\n\t{path}", SystemMessageType.Warning);
                foreach (var item in Coordinate.GetError())
                {
                    printer?.Invoke($"\tsrtm\\{item}", SystemMessageType.Error);
                    //Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe", "https://srtm.kurviger.de/SRTM3/Eurasia/" + item + ".zip");
                }
            }
        }
    }
}
