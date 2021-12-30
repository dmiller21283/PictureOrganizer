using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PictureOrganizer
{
    class Program
    {
        private static string _path;
        private static string _destinationRoot;
        private static CommandArgs _command;

        static void Main(string[] args)
        {
            if (args.Length >= 3)
            {
                var validCommand = Enum.TryParse(args[0], true, out _command);

                if (!validCommand)
                {
                    Console.WriteLine($"{args[0]} not a recognized command\r\nUsage: pictureorganizer.exe [copy | move] [source] [desitination]");
                    Console.ReadKey();
                    Environment.Exit(1);
                }

                _path = args[1];
                _destinationRoot = args[2];
            }
            else
            {
                Console.WriteLine(new ArgumentException("Missing source and destination arguments.\r\nUsage: pictureorganizer.exe [copy | move] [source] [desitination]"));
                Console.ReadKey();
                Environment.Exit(1);
            }

            var defaultColor = Console.ForegroundColor;

            if (Directory.Exists(_path))
            {
                var files = Directory.GetFiles(_path, "*.jpg");
                var skipped = new List<string>();
                var errored = new List<string>();
                var processed = 0;

                foreach (var file in files)
                {
                    var info = new FileInfo(file);
                    DateTime? dateTaken = Helper.GetDateTakenFromImage(file);
                    
                    if (!dateTaken.HasValue)
                    {
                        dateTaken = info.CreationTime;
                    }

                    if (dateTaken.HasValue)
                    {
                        var destination = Path.Combine(_destinationRoot, $"{dateTaken.Value.Year}\\{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTaken.Value.Month)}");

                        if (!Directory.Exists(destination))
                        {
                            Directory.CreateDirectory(destination);
                        }

                        switch (_command)
                        {
                            case CommandArgs.Copy:
                                Console.WriteLine($"Copying: {file} to {destination}");
                                try
                                {
                                    File.Copy(file, Path.Combine(new string[] { _destinationRoot, destination, info.Name }), false);
                                }
                                catch(Exception ex)
                                { 
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ex.Message);
                                    Console.WriteLine(file);
                                    Console.ForegroundColor = defaultColor;
                                    errored.Add(file);
                                }
                                break;
                            case CommandArgs.Move:
                                Console.WriteLine($"Moving: {file} to {destination}");
                                try
                                {
                                    File.Move(file, Path.Combine(new string[] { _destinationRoot, destination, info.Name }));
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ex.Message);
                                    Console.WriteLine(file);
                                    Console.ForegroundColor = defaultColor;
                                    errored.Add(file);
                                }
                                break;
                            case CommandArgs.List:
                                Console.WriteLine($"{file} to {destination}");
                                break;
                            default:
                                Console.WriteLine($"{_command} not a recognized command\r\nUsage: pictureorganizer.exe [copy | move] [source] [desitination]");
                                Console.ReadKey();
                                Environment.Exit(1);
                                break;
                        }
                    }
                    else
                    {
                        skipped.Add(file);
                    }

                    processed += 1;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("SKIPPED FILES");

                foreach (var file in skipped)
                {
                    Console.WriteLine(file);
                }

                Console.ForegroundColor = defaultColor;


                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("ERRORED FILES");
                foreach (var file in errored)
                {
                    Console.WriteLine(file);
                }

                Console.ForegroundColor = defaultColor;

                Console.WriteLine($"Processed {processed} files.");
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
