using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessingAroundWithFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileToMove = string.Empty;
            while(true)
            {
                Console.WriteLine("Please enter file to move");
                fileToMove = Console.ReadLine();

                if (!File.Exists(fileToMove))
                {
                    Console.WriteLine("File Doesn't exist, try again...");
                }
                else
                {
                    break;
                }
            }
            var directory = string.Empty;

            while (true)
            {
                var success = false;
                Console.WriteLine("Please enter new destination directory");
                directory = Console.ReadLine();

                if(!Directory.Exists(directory))
                {
                    Console.WriteLine("Directory doesn't exist, create it? Y/N?");
                    var answer = Console.ReadLine();
                    switch (answer.ToUpper())
                    {
                        case "Y":
                            Directory.CreateDirectory(directory);
                            success = true;
                            break;
                        default:
                            Console.WriteLine("Enter a different one then");
                            break;
                    }
                }
                else
                {
                    success = true;
                }
                if (success)
                {
                    break;
                }
            }

            var filename = string.Empty;

            while (true)
            {
                Console.WriteLine("Please enter the filename for your new file");
                filename = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(filename))
                {
                    Console.WriteLine("Gotta have name, try again");
                }
                else
                {
                    break;
                }
            }

            var newFilePath = Path.Combine(directory, filename);

            Console.WriteLine("STREAM or FILECOPY ?");
            var streamOrFileCopy = Console.ReadLine().ToUpper();

            if (streamOrFileCopy == "FILECOPY")
            {
                Console.WriteLine("Beginning the copy");

                File.Copy(fileToMove, newFilePath);

                if (File.Exists(newFilePath))
                {
                    var orginialColour = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SUCCES!");
                    Console.ForegroundColor = orginialColour;
                    Console.ReadLine();
                }
                else
                {
                    var orginialColour = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Oh no, something went wrong...");
                    Console.ForegroundColor = orginialColour;
                    Console.ReadLine();
                } 
            }
            else if(streamOrFileCopy == "STREAM" )
            {
                Console.WriteLine("BEGGINING WITH STREAMS");

                using (var inputStream = new FileStream(fileToMove, FileMode.Open, FileAccess.Read))
                {
                    using (var outputStream = new FileStream(newFilePath, FileMode.Create, FileAccess.Write))
                    {
                        var streamLength = inputStream.Length;
                        var totalBytes = 0;
                        var currentBlockSize = 0;
                        var buffer = 1024;
                        var bufferArray = new byte[buffer];
                        while ((currentBlockSize = inputStream.Read(bufferArray, 0, buffer)) > 0)
                        {
                            outputStream.Write(bufferArray, 0, currentBlockSize);
                            totalBytes += currentBlockSize;
                            Console.Write(Math.Round(((decimal)totalBytes / (decimal)streamLength) * 100, 2) + "%");
                            Console.CursorLeft = 0;
                        }
                        Console.WriteLine("GOT HERE");
                    }
                }

                if (File.Exists(newFilePath))
                {
                    var orginialColour = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SUCCES!");
                    Console.ForegroundColor = orginialColour;
                    Console.ReadLine();
                }
                else
                {
                    var orginialColour = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Oh no, something went wrong...");
                    Console.ForegroundColor = orginialColour;
                    Console.ReadLine();
                }

            }


            
            

        }
    }
}
