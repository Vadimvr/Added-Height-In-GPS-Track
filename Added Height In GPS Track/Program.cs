

using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
namespace Added_Height_In_GPS_Track
{
    public class Test
    {
        public static void Main()
        {
            string newName = "trackNew.kml";
            string oldName = "track.kml";
            Test t = new Test();
            //t.CreatePO("po.kml");

            var kml = t.ReadKml(oldName);
            CoordinateHeight.AddHeights(kml);
            kml.Document.name = newName;
            t.WriteKml(newName, kml);
            var kmlOld = t.ReadKml(oldName);
            var kmlNew = t.ReadKml(newName);
            var coordinateOld = CoordinateHeight.ConvertToDoubleCoordinates(kmlOld);
            var coordinateNew = CoordinateHeight.ConvertToDoubleCoordinates(kmlNew);

            for (int i = 0; i < coordinateNew.Count; i++)
            {
                Console.WriteLine($"{i,-5} {coordinateOld[i].Height,-5} {coordinateNew[i].Height,-5}");
                // Console.WriteLine($"{i,-5} {coordinateOld[i]} {coordinateNew[i]}");
            }
        }


        private kml ReadKml(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(kml));
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            FileStream fs = new FileStream(filename, FileMode.Open);
            kml? _kml;

            var temp = serializer.Deserialize(fs);
            _kml = temp as kml;
            fs.Close();
            return _kml == null ? throw new ArgumentNullException() : _kml;
        }

        private void WriteKml(string filename, kml _kml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(kml));
            TextWriter writer = new StreamWriter(filename);
            serializer.Serialize(writer, _kml);
            writer.Close();
        }

        private void CreatePO(string filename)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(kml));
            TextWriter writer = new StreamWriter(filename);
            kml _kml = new kml();

            LineStyle LineStyle = new LineStyle();
            Style style = new Style() { LineStyle = LineStyle };


            List<coordinate> coordinates = new List<coordinate>()
            {
                new(){Longitude = 82.5487, Latitude = 82.54877, Height =10},
                 new(){Longitude = 82, Latitude = 80, Height =10},
                new(){Longitude = 82, Latitude = 80, Height =10},
                new(){Longitude = 82, Latitude = 80, Height =10},
                new(){Longitude = 82, Latitude = 80, Height =10},
                 new(){Longitude = 82, Latitude = 80, Height =10},
            };



            LineString lineString = new LineString()
            {
                coordinates = coordinates
                .Select(x => x.ToString())
                .Aggregate("", (current, next) => current + " " + next),
                extrude = 1
            };




            Placemark placemark = new Placemark() { description = "testTrack", name = "TestName", Style = style, LineString = lineString };

            Document document = new Document();
            document.Placemark = placemark;

            _kml.Document = document;
            serializer.Serialize(writer, _kml);
            writer.Close();
        }

        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}