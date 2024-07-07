using System.Globalization;

namespace LibSRMTHeight
{
    internal class Coordinate
    {
        public static double deltaY = 0.001;
        public static double deltaX = 0.001;
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

        private static readonly string formatAfterDot = "0.0000000";
        public override string ToString()
        {
            return $"{Longitude.ToString(formatAfterDot, nfi)},{Latitude.ToString(formatAfterDot, nfi)},{Height.ToString(nfi)}";
        }
        static NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };

        public void GetHeight(Action<string, SystemMessageType> printer)
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
                    if (!filesIsNotExist.ContainsKey(key))
                    {
                        filesIsNotExist.Add(key, 0);
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

            int lat = (int)((Latitude - (int)Latitude) * 1201);
            int lon = (int)((Longitude - (int)Longitude) * 1201);
            int pos = ((1201 - lat - 1) * 1201 * 2) + lon * 2;

            if (pos < 0 || pos > maxLengthArray)
            {
                printer?.Invoke($"{Latitude}, {Longitude}, Coordinates out of range.", SystemMessageType.Error);
                return;
            }

            if ((value[pos] == 0x80) && (value[pos + 1] == 0x00))
            {
                printer?.Invoke($"{Latitude}, {Longitude},(value[pos] == 0x80) && (value[pos + 1] == 0x00)", SystemMessageType.Error);
            }
            Height = (value[pos]) << 8 | value[pos + 1];
            if (Height == 0)
            {
                printer?.Invoke($"{Latitude}, {Longitude},height is 0", SystemMessageType.Error);
            }
        }

        internal static IEnumerable<string> GetError()
        {
            return filesIsNotExist.Select(f => f.Key);
        }

        internal static List<Coordinate> AddPoint(List<Coordinate> coordinates, Action<string, SystemMessageType> printer)
        {
            List<Coordinate> result = new List<Coordinate>();

            for (int i = 0; i < coordinates.Count - 1; i++)
            {
                result.AddRange(AddPoint(coordinates[i], coordinates[i + 1],printer));
            }
            result.Add(coordinates[^1]);
            return result;
        }

        private static List<Coordinate> AddPoint(Coordinate first, Coordinate second, Action<string, SystemMessageType> printer)
        {
            if (first.Latitude == 53.9730629533484)
            {

            }
            var y = Math.Abs(first.Latitude - second.Latitude);
            var x = Math.Abs(first.Longitude - second.Longitude);

            List<Coordinate> result = new List<Coordinate>() { first };
            if (x > deltaX || y > deltaY)
            {
                var mul = (int)(x / deltaX > y / deltaY ? x / deltaX : y / deltaY);

                var mulY = first.Latitude > second.Latitude ? -1 : 1;
                var mulX = first.Longitude > second.Longitude ? -1 : 1;

                var sepY = y / mul * mulY;
                var sepX = x / mul * mulX;
                var last = first;

                for (int i = 0; i < mul - 1; i++)
                {
                    var cor = new Coordinate() { Latitude = last.Latitude + sepY, Longitude = last.Longitude + sepX };
                    cor.GetHeight(printer);
                    last = cor;
                    result.Add(cor);
                }
            }

            return result;
        }
    }
}
