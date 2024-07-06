using LibSRMTHeight;
using System.Xml;
namespace Added_Height_In_GPS_Track
{
    public class Test
    {
        public static void Main()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("тест2__20240706_1341.kml");
            XmlNodeList elemList = doc.GetElementsByTagName("coordinates");
            if (elemList == null) return;
            for (int i = 0; i < elemList.Count; i++)
            {
                if (elemList[i] == null || elemList[i].InnerXml == null) { continue; }

                List<Coordinate> coordinates = CovertCoordinate.ConvertToDoubleCoordinates(elemList[i].InnerXml);

                elemList[i].InnerXml = coordinates.Select(x => x.ToString()).Aggregate("", (current, next) => current + " " + next);
            }
            doc.Save("тест2__20240706_1341.kml");

            foreach (var item in Coordinate.GetError())
            {
                Console.WriteLine(item);
            }
        }
    }
}