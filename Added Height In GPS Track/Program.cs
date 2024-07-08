using LibSRMTHeight;
namespace Added_Height_In_GPS_Track
{
    public class Test
    {
        public static void Main()
        {
            var allFiles = Directory.GetFiles(AppContext.BaseDirectory)
                .Where(x => x.Length > 4
                &&
                (x.Substring(x.Length - 4).Equals(".kml", StringComparison.InvariantCultureIgnoreCase)
                || x.Substring(x.Length - 4).Equals(".kmz", StringComparison.InvariantCultureIgnoreCase))
                && !x.Contains("update"));
            int i = 0;
            foreach (var file in allFiles)
            {
                if (file.Substring(file.Length - 4) == ".kml")
                {
                    AddHeightInKML.Worker(file, Message, file.Substring(AppContext.BaseDirectory.Length));
                    i++;
                }
                else if (file.Substring(file.Length - 4) == ".kmz")
                {
                    AddHeightInKMZ.Worker(file, Message);
                    i++;
                }
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"added {i}");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        public static void Message(string message, SystemMessageType type)
        {
            switch (type)
            {
                case SystemMessageType.Ok:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case SystemMessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case SystemMessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}