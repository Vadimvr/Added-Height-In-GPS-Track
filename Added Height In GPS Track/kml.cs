using System.Xml.Serialization;

namespace Added_Height_In_GPS_Track
{
    [XmlRootAttribute("kml", Namespace = "http://earth.google.com/kml/2.2", IsNullable = false)]
    public class kml
    {
        public Document Document;
    }

    public class Document
    {
        public  string name { get; set; }
        public Placemark Placemark;
    }

    public class Placemark
    {
        public string name { get; set; }
        public string description { get; set; }
        public Style Style { get; set; }

        public LineString LineString { get; set; }
    }

    public class Style
    {
        public LineStyle LineStyle { get; set; }
    }
    public class LineStyle
    {
        public string color { get; set; } = "A60000FF";
        public int width { get; set; } = 2;
    }
}