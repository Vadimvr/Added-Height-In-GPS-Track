using System.Globalization;

namespace LibSRMTHeight
{
    public class Coordinate
    {
        static Dictionary<string, int> filesIsNotExist = new Dictionary<string, int>();
        static Dictionary<string, byte[]> data = new Dictionary<string, byte[]>();
        static readonly int maxLengthArray = 1201 * 1201 * 2;
        static readonly string appPath = AppContext.BaseDirectory;
        static readonly string srtmPath = appPath + "srtm\\";

        // широта
        public double Latitude { get; set; }
        // долгота
        public double Longitude { get; set; }
        // высота
        public double Height { get; set; }

        public override string ToString()
        {
            return $"{Longitude.ToString(nfi)},{Latitude.ToString(nfi)},{Height.ToString(nfi)}";
        }
        static NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

        public void GetHeight()
        {
            string key = $"N{(int)Latitude}E{((int)Longitude).ToString("000")}.hgt";
            byte[] value;
            if (data.ContainsKey(key))
            {
                value = data[key];
            }
            else
            {
                if (!File.Exists(srtmPath + key))
                {
                    if (!filesIsNotExist.ContainsKey(srtmPath + key))
                    {
                        filesIsNotExist.Add(srtmPath + key, 0);
                    }
                    return;
                }
                using (Stream sr = new FileStream(srtmPath + key, FileMode.Open))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        sr.CopyTo(ms);
                        value = ms.ToArray();
                    }

                }
                data.Add(key, value);
            }

            int lat = (int)Math.Round((Latitude - (int)Latitude) * 1201);
            int lon = (int)Math.Round((Longitude - (int)Longitude) * 1201);

            int pos = ((1201 - lat - 1) * 1201 * 2) + lon * 2;

            if (pos < 0 || pos > maxLengthArray)
                throw new ArgumentOutOfRangeException("Coordinates out of range.", "coordinates");

            if ((value[pos] == 0x80) && (value[pos + 1] == 0x00))
                Height = 0;
            Height = (value[pos]) << 8 | value[pos + 1];
        }

        public static IEnumerable<string> GetError()
        {
            return filesIsNotExist.Select(f => f.Key);
        }
    }
}
