using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Configuration;
using System.IO;

namespace DateChanger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var newDate = DateTime.Parse(ConfigurationManager.AppSettings["newDate"]);
                var directoryValid = false;
                var directoryPath = string.Empty;

                while (!directoryValid)
                {
                    Console.Write("Enter folder path: ");
                    directoryPath = Console.ReadLine();

                    if (directoryPath != null)
                        directoryValid = Directory.Exists(directoryPath);

                    if (!directoryValid)
                    {
                        Console.WriteLine("Invalid path!");
                        Console.WriteLine();
                    }
                }

                var filePaths = Directory.GetFiles(directoryPath);

                Console.WriteLine();
                Console.WriteLine("Found {0} files", filePaths.Length);
                Console.WriteLine("Changing dates to {0}", newDate);
                Console.WriteLine("Press any key...");
                Console.ReadLine();

                foreach (var filePath in filePaths)
                {
                    Console.WriteLine("Processing file {0}", filePath);

                    File.SetCreationTime(filePath, newDate);

                    using (var picture = ShellObject.FromParsingName(filePath))
                    {
                        using (var propertyWriter = picture.Properties.GetPropertyWriter())
                        {
                            propertyWriter.WriteProperty(SystemProperties.System.Photo.DateTaken, newDate);
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Processing complete");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error: {0}", ex.Message);
                Console.ReadLine();
            }
        }
    }
}