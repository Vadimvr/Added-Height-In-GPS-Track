﻿using System.IO.Compression;
namespace LibSRMTHeight
{
    public class AddHeightInKMZ
    {
        public static void Worker(string file, Action<string, SystemMessageType> message)
        {
            string tempFolder = file.Substring(0, file.Length - 4);
            string updateKmz = tempFolder + "-update" + file.Substring(file.Length - 4);
            string kmzFile = tempFolder + "\\doc.kml";

            if (Directory.Exists(tempFolder)) { Directory.Delete(tempFolder, true); }
            if (File.Exists(updateKmz)) { File.Delete(updateKmz); }

            ZipFile.ExtractToDirectory(file, tempFolder);

            AddHeightInKML.Worker(kmzFile, message, file.Substring(AppContext.BaseDirectory.Length), "");
            ZipFile.CreateFromDirectory(tempFolder, updateKmz);
            if (Directory.Exists(tempFolder)) { Directory.Delete(tempFolder, true); }
        }
    }
}
