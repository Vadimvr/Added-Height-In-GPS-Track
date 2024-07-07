using System.Globalization;

namespace LibSRMTHeight
{

    internal class CovertCoordinate
    {
        internal static List<Coordinate> ConvertToDoubleCoordinates(string coordinatesString, Action<string, SystemMessageType> printer)
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
                    coordinate.GetHeight(printer);
                    return coordinate;
                }).ToList();
        }
    }
}
