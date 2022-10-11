using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace get_content
{
    internal class Program
    {
        static void Main(string[] args)
        {
            long MAX_LENGTH = 1024 * 1024 * 1;   // limit 1MB

            if (args.Length < 1)
            {
                Console.WriteLine("[-] Command Error \n");
                Console.WriteLine("[*] Usage: get-content.exe C:\\Some\\Path\\To\\File \n");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("[-] {0} not exit! \n", args[0]);
                return;
            }
            string path = Path.GetFullPath(args[0]);
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Length > MAX_LENGTH)
            {
                Console.WriteLine("[-] {0} size: {1} ==> {2}", args[0], fileInfo.Length, BytesToReadableValue(fileInfo.Length));
                Console.WriteLine("[-] {0} Exceeded the maximum limit of {1} \n", args[0], BytesToReadableValue(MAX_LENGTH));
                return ;
            }
            byte[] buffer = File.ReadAllBytes(path);
            System.Text.Encoding fileEncoding = GetEncoding(buffer);
            Console.WriteLine("[+] Current File Encoding: {0}\n", fileEncoding.BodyName);
            string text = File.ReadAllText(path, fileEncoding);
            Console.WriteLine(text);
        }

        public static System.Text.Encoding GetEncoding(byte[] buffer) {
            System.Text.Encoding targetEncoding = System.Text.Encoding.Default;
            var textDetect = new TextEncoding();
            TextEncoding.Encoding encoding = textDetect.DetectEncoding(buffer, buffer.Length);
            if (encoding == TextEncoding.Encoding.None)
            {
                targetEncoding = System.Text.Encoding.Default;
            }
            else if (encoding == TextEncoding.Encoding.Ascii)
            {
                targetEncoding = System.Text.Encoding.ASCII;
            }
            else if (encoding == TextEncoding.Encoding.Ansi)
            {
                targetEncoding = System.Text.Encoding.Default;
            }
            else if (encoding == TextEncoding.Encoding.Utf8Bom || encoding == TextEncoding.Encoding.Utf8Nobom)
            {
                targetEncoding = System.Text.Encoding.UTF8;
            }
            else if (encoding == TextEncoding.Encoding.Utf16LeBom || encoding == TextEncoding.Encoding.Utf16LeNoBom)
            {
                targetEncoding = System.Text.Encoding.Unicode;
            }
            else if (encoding == TextEncoding.Encoding.Utf16BeBom || encoding == TextEncoding.Encoding.Utf16BeNoBom)
            {
                targetEncoding = System.Text.Encoding.BigEndianUnicode;
            }

            return targetEncoding;
        }


       
        public static string BytesToReadableValue(long number)
        {
            string[] suffixes = new string[] { " B", " KB", " MB", " GB", " TB", " PB" };
            double last = 1;
            for (int i = 0; i < suffixes.Length; i++)
            {
                var current = Math.Pow(1024, i + 1);
                var temp = number / current;
                if (temp < 1)
                {
                    return (number / last).ToString("n2") + suffixes[i];
                }
                last = current;
            }
            return number.ToString();
        }
    
    }
}
