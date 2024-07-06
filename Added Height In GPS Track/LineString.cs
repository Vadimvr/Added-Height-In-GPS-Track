using System.Globalization;

namespace Added_Height_In_GPS_Track
{
    public class LineString
    {
        public int extrude { get; set; }
        public string coordinates { get; set; }
    }

    public class coordinate
    {
        // долгота
        public double Longitude { get; set; }
        // широта
        public double Latitude { get; set; }
        // высота
        public double Height { get; set; }

        public override string ToString()
        {
            return $"{Longitude.ToString(nfi)},{Latitude.ToString(nfi)},{Height.ToString(nfi)}";
        }
        static NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
    }
}