using System.Globalization;

namespace LibSRMTHeight
{

    public class CovertCoordinate
    {
        public static List<Coordinate> ConvertToDoubleCoordinates(string coordinatesString)
        {
            return coordinatesString.Trim()
                .Replace("\t", "")
                .Replace("\n", " ")
                .Split(' ').Select(cr =>
                {
                    var arr = cr.Trim().Split(',').ToList();

                    var coordinate = new Coordinate()
                    {
                        Longitude = double.Parse(arr[0], CultureInfo.InvariantCulture),
                        Latitude = double.Parse(arr[1], CultureInfo.InvariantCulture),
                        Height = double.Parse(arr[2], CultureInfo.InvariantCulture)
                    };
                    coordinate.GetHeight();
                    return coordinate;
                }).ToList();
        }
    }
}
