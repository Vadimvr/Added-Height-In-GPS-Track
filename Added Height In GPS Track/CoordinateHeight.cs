using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Added_Height_In_GPS_Track
{
    internal class CoordinateHeight
    {
        internal static void AddHeights(kml kml)
        {
            
            List<coordinate> coordinateList = ConvertToDoubleCoordinates(kml);
            double x = 10;
            foreach (coordinate coordinate in coordinateList)
            {
                coordinate.Height = x;
                x += 15;
            }
            kml.Document.Placemark.LineString.coordinates = coordinateList.Select(x => x.ToString())
                .Aggregate("", (current, next) => current + " " + next);
        }
        internal static List<coordinate> ConvertToDoubleCoordinates(kml kml)
        {
            var coordinatesString = kml.Document.Placemark.LineString.coordinates;

            return coordinatesString.Trim()
                .Replace("\t", "")
                .Replace("\n", " ")
                .Split(' ').Select(cr =>
                {
                    var arr = cr.Trim().Split(',').ToList();

                    var coordinate = new coordinate()
                    {
                        Longitude = double.Parse(arr[0], CultureInfo.InvariantCulture),
                        Latitude = double.Parse(arr[1], CultureInfo.InvariantCulture),
                        Height = double.Parse(arr[2], CultureInfo.InvariantCulture)
                    };
                    return coordinate;
                }).ToList();
        }
    }
}
