using System.Runtime.CompilerServices;
using System.Text;

namespace Base64
{
    internal class Program
    {
        static void Main(string[] args)
        {

            String argument;
            if (args == null || args.Length == 0)
            {
                argument = "-h";
            } else
            {
                argument = args[0].ToLower();
            }

            switch (argument)
            {
                case "-h":
                case "--help":
                    {
                        PrintHelp();
                        return;
                    }
                default:
                    {
                        if (!File.Exists(argument))
                        {
                            Console.WriteLine("File not found: " + argument);
                            return;
                        }
                        byte[] decoded = DecodeFromFile(argument);

                        string[] newFile = SplitFileExtension(argument);
                        if (decoded.Length > 0)
                        {
                            newFile[0] += "_decoded";
                            newFile[1] = IdentifyFileType(decoded);
                            Console.WriteLine("Decoded from base64");
                            SaveToFile(JoinFileExtension(newFile), decoded);
                        }
                        else
                        {
                            String coded = CodeFromFile(argument);
                            newFile[0] += "_base64";
                            newFile[1] = "txt";
                            Console.WriteLine("Coded to base64");
                            SaveToFile(JoinFileExtension(newFile), Encoding.UTF8.GetBytes(coded));
                        }

                        return;
                    }
            }
        }

        private static String IdentifyFileType(byte[] bytes)
        {
            if (bytes.Length < 7)
            {
                return "bin";
            }
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47 && bytes[4] == 0x0D && bytes[5] == 0x0A)
            {
                return "png";
            }
            if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            {
                return "jpg";
            }
            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38)
            {
                return "gif";
            }
            if (bytes[0] == 0x42 && bytes[1] == 0x4D)
            {
                return "bmp";
            }
            if (bytes[0] == 0x4C && bytes[1] == 0x5A && bytes[2] == 0x49 && bytes[3] == 0x50)
            {
                return "lz";
            }
            if (bytes[0] == 0x50 && bytes[1] == 0x4B && bytes[2] == 0x03 && (bytes[3] == 0x04 || bytes[3] == 0x06 || bytes[3] == 0x08))
            {
                return "zip";
            }
            if (bytes[0] == 0x52 && bytes[1] == 0x61 && bytes[2] == 0x72 && bytes[3] == 0x21 && bytes[4] == 0x1A && bytes[5] == 0x07)
            {
                return "rar";
            }
            if (bytes[0] == 0xCA && bytes[1] == 0xFE && bytes[2] == 0xBA && bytes[3] == 0xBE)
            {
                return "class";
            }
            if (bytes[0] == 0x25 && bytes[1] == 0x50 && bytes[2] == 0x44 && bytes[3] == 0x46 && bytes[4] == 0x2D)
            {
                return "pdf";
            }
            if (bytes[0] == 0x38 && bytes[1] == 0x42 && bytes[2] == 0x50 && bytes[3] == 0x53)
            {
                return "pdf";
            }
            if (bytes[0] == 0xFF && bytes[1] == 0xFB)
            {
                return "mp3";
            }
            if (bytes[0] == 0xFF && bytes[1] == 0xF3)
            {
                return "mp3";
            }
            if (bytes[0] == 0xFF && bytes[1] == 0xF2)
            {
                return "mp3";
            }
            if (bytes[0] == 0x49 && bytes[1] == 0x44 && bytes[2] == 0x33)
            {
                return "mp3";
            }
            if (bytes[0] == 0x37 && bytes[1] == 0x7A && bytes[2] == 0xBC && bytes[3] == 0xAF && bytes[4] == 0x27 && bytes[5] == 0x1C)
            {
                return "7z";
            }
            if (bytes[0] == 0x1F && bytes[1] == 0x8B)
            {
                return "gz";
            }
            if (bytes.Length >= 10)
            {
                if (bytes[4] == 0x66 && bytes[5] == 0x74 && bytes[6] == 0x79 && bytes[7] == 0x70 && bytes[8] == 0x69 && bytes[9] == 0x73 && bytes[10] == 0x6F)
                {
                    return "mp4";
                }
                if (bytes[4] == 0x66 && bytes[5] == 0x74 && bytes[6] == 0x79 && bytes[7] == 0x70 && bytes[8] == 0x4D && bytes[9] == 0x53 && bytes[10] == 0x4E)
                {
                    return "mp4";
                }
            }
            return "bin";
        }


        private static void SaveToFile(String file, byte[] data)
        {
            try
            {
                File.WriteAllBytes(file, data);
                Console.WriteLine("File saved: " + file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving file: " + ex.Message);
            }
        }

        private static String CodeFromFile(String file)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(file);
                String coded = Convert.ToBase64String(bytes);
                return coded;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static byte[] DecodeFromFile(String file)
        {
            try
            {
                String textFile = File.ReadAllText(file);
                byte[] decoded = Convert.FromBase64String(textFile);
                return decoded;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }

        private static string[] SplitFileExtension(String filename)
        {
            var parts = filename.Split('.');
            if (parts.Length == 1)
            {
                return new string[] { parts[0], "" };
            }
            else
            {
                String extension = parts[parts.Length - 1];
                return new string[] { filename.Substring(0, filename.Length-1 - extension.Length), extension};
            }
        }

        private static String JoinFileExtension(string[] fileExtension)
        {
            String result = String.Join('.', fileExtension);
            return result;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: Base64 <<my_file.bin>>");
            Console.WriteLine();
            Console.WriteLine("   Decoding: if the file is already in Base64, it will decode and save the file as <<my_file_decoded.bin.txt>>");
            Console.WriteLine("   Coding  : if the file is not in Base64, it will code as Base64 and save the file as <<my_file_base64.bin>>");
            Console.WriteLine("   If its type can be identified, the extension of the file will be changed accordingly.");

        }
    }
}
