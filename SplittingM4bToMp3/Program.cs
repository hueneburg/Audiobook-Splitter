using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SplittingM4bToMp3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("-x"))
            {
                foreach (var arg in args)
                {
                    if (arg != "-x")
                    {
                        ExtractInformation("\"" + arg + "\"");
                    }
                }
            }
            else
            {
                if (!Directory.Exists("output"))
                {
                    Directory.CreateDirectory("output");
                }
                foreach (var path in args)
                {
                    string path1 = path;
                    var thread = new Thread(() =>
                    {
                        Console.WriteLine("Started " + path1);
                        var file = ExtractInformation("\"" + path1 + "\"");
                        CutFiles(path1, file);
                        Console.WriteLine("Finished " + path1);
                    });
                    thread.Start();
                }
            }
        }

        private static void CutFiles(string path, string name)
        {
            const string arguments = "{2} -i \"{1}\" -acodec libmp3lame -ss {0} {3}";
            using (var reader = new StreamReader(name))
            {
                var line1 = reader.ReadLine().Split(';');
                string[] line2;
                string l;
                while ((l = reader.ReadLine()) != null)
                {
                    line2 = l.Split(';');
                    var temp = string.Format(arguments, line1[0], path, "-t " + line2[0], "output/\"" + line1[1] + ".mp3\"");
                    var p = new Process
                    {
                        StartInfo = {Arguments = temp, FileName = "/usr/bin/ffmpeg", UseShellExecute = false, RedirectStandardOutput = true}
                    };
                    var thread = new Thread(() => p.Start());
                    thread.Start();
                    line1 = line2;
                }
                var t = string.Format(arguments, line1[0], path, "", "output/\"" + line1[1] + ".mp3\"");
                var pro = new Process
                {
                    StartInfo = { Arguments = t, FileName = "/usr/bin/ffmpeg", UseShellExecute = false }
                };
                var th = new Thread(() => pro.Start());
                th.Start();
            }
        }

        private static string ExtractInformation(string file)
        {
            var p = new Process
            {
                StartInfo = {UseShellExecute = false, RedirectStandardOutput = true, FileName = "/usr/bin/mediainfo", Arguments = file}
            };
            p.Start();
            var stream = p.StandardOutput;
            string line;
            var inMenuOne = false;
            var output = "";
            while ((line = stream.ReadLine()) != null)
            {
                if (inMenuOne && line != "")
                {
                    output += line + "\n";
                } 
                else if (!inMenuOne && line.StartsWith("Menu"))
                {
                    inMenuOne = true;
                }
                else if (inMenuOne && line == "")
                {
                    break;
                }
            }
            {
                var ori = output.Replace("  ", " ");
                while (ori != output)
                {
                    output = ori;
                    ori = output.Replace("  ", " ");
                }
            }
            output = output.Replace(" : ", ";");
            var name = file.Split('/')[file.Split('/').Length - 1];
            name = name.Substring(0, name.Length - 1);
            using (var writer = new StreamWriter(name + ".names"))
            {
                writer.Write(output);
            }
            return name + ".names";
        }
    }
}
