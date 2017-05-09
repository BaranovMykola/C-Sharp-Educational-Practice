using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IOSystem
{
    class Program
    {
        private static string location = string.Empty;
        private static Dictionary<string, FileInfoWrapper> list = new Dictionary<string, FileInfoWrapper>();
        private static string help = string.Empty;

        private static readonly Dictionary<string, string> fileConfiguration = new Dictionary<string, string>();

        static Program()
        {
            try
            {
                ReadConfiguration(new StreamReader("../../.config"));
                help = File.ReadAllText(fileConfiguration["help"]);
                LoadLocation();
                LoadLog();
            }
            catch (Exception error)
            {
                Console.WriteLine("Error while reading config: {0}", error.Message);
            }
        }

        private static void AppendLocation(string to)
        {
            string oldLocation = location.Clone() as string;
            if (!string.IsNullOrEmpty(location))
            {
                location += @"\";
            }
            location += to;
            DirectoryInfo dirInfo = new DirectoryInfo(location);
            if (!dirInfo.Exists && to != "." || Regex.Match(to, @"(../)|(\.){2,}").Success)
            {
                location = oldLocation;
                throw new System.IO.DirectoryNotFoundException(string.Format("{0} not found", (location + @"\" + to)));
            }
        }
        private static void LoadLog()
        {
            StreamReader sw = new StreamReader(fileConfiguration["fileInfoLog"]);
            string line;
            while((line = sw.ReadLine()) != null)
            {
                var rawData = line.Split(' ');
                FileInfo fileInfo = new FileInfo(rawData[1]);
                if (fileInfo.Exists)
                {
                    list.Add(rawData[0], new FileInfoWrapper(rawData[1]));
                }
            }
            sw.Close();
        }
        private static void SaveLog()
        {
            StreamWriter saver = new StreamWriter(fileConfiguration["fileInfoLog"]);
            foreach (var item in list)
            {
                saver.Write(string.Format("{0} {1}\n", item.Key, item.Value.FullName));
            }
            saver.Close();
        }
        private static void LoadLocation()
        {
            StreamReader sr = new StreamReader(fileConfiguration["location"]);
            location = sr.ReadLine();
            sr.Close();
            DirectoryInfo dirInfo = new DirectoryInfo(location);
            if (!dirInfo.Exists)
            {
                location = string.Empty;
                Console.WriteLine("Corrupted saved location");
            }
        }
        private static void SaveLocation()
        {
            StreamWriter sw = new StreamWriter(fileConfiguration["location"]);
            sw.Write(location);
            sw.Close();
        }
        private static void ReadConfiguration(StreamReader sr)
        {
            string line;
            do
            {
                line = sr.ReadLine();
                if (line == null || line[0] == '#')
                {
                    continue;
                }
                var rawData = line.Split('=');
                fileConfiguration.Add(rawData[0], rawData[1]);
            }
            while (line != null);
        }

        public static void Cd(string to)
        {
            if (!Up(to))
            {
                switch (to)
                {
                    case ".":
                        location = string.Empty;
                        break;
                    default:
                        AppendLocation(to);
                        break;
                }
            }
        }
        public static void Pwd()
        {
            Console.WriteLine(location);
        }
        public static void Help() => Console.WriteLine(help);
        public static void Push(string file)
        {
            string fullPath = location + @"\" + file;

            var files = Directory.GetFiles(location, file, System.IO.SearchOption.TopDirectoryOnly);
            foreach (var item in files)
            {
                FileInfo fileInfo = new FileInfo(item);
                if (!list.ContainsKey(fileInfo.Name))
                {
                    Console.WriteLine(item);
                    list.Add(fileInfo.Name, new FileInfoWrapper(item));
                }
            }

        }
        public static void Show(string sort)
        {
            var toSort = list.ToList<KeyValuePair<string, FileInfoWrapper>>();
            if (Regex.Match(sort, "-d").Success)
            {
                toSort.Sort((pair1, pair2) => pair1.Value.Directory.CompareTo(pair2.Value.Directory));
            }
            foreach (var item in toSort)
            {
                Console.WriteLine(string.Format("{0,28}\n{1}", item.Key, item.Value));
            }
        }
        public static void Exit()
        {
            SaveLocation();
            SaveLog();
            Environment.Exit(0);
        }
        public static bool Up(string up)
        {
            string checkSyntaxis = Regex.Match(up, @"(..\\)+").Value;
            if (checkSyntaxis == up)
            {
                int upTimes = Regex.Matches(up, @"..\\").Count;
                for (int i = 0; i < upTimes; ++i)
                {
                    var dirs = location.Split('\\');
                    location = string.Join(@"\", dirs, 0, dirs.Length - 1);
                }
                return true;
            }
            return false;
        }
        public static void Remove(string key)
        {
            bool removed = list.Remove(key);
            string message = "removed successfully";
            if (!removed)
            {
                message = "not found. Removing failed";
            }
            Console.WriteLine("'{0}' {1}", key, message);
        }

        public static void Find(string param)
        {
            //-n=1.jpg -t=.pdf -d=-y=2017
            Dictionary<string, FileInfoWrapper> result = list;

            string nameParam = Regex.Match(param, @"-n=[^\s]+").Value;
            if (!string.IsNullOrEmpty(nameParam))
            {
                result = FindByName(nameParam, result);
            }

            string extensionParam = Regex.Match(param, @"-e=[^\s]+").Value;
            if (!string.IsNullOrEmpty(extensionParam))
            {
                result = FindByExtension(extensionParam, result);
            }

            string atributeParam = Regex.Match(param, @"-a=[^\s]+").Value;
            if (!string.IsNullOrEmpty(atributeParam))
            {
                result = FindByAtribute(atributeParam, result);
            }

            string dataParam = Regex.Match(param, @"-d=[\w.\-=]+").Value;
            if(!string.IsNullOrEmpty(dataParam))
            {
                result = FindByData(dataParam, result);
            }

            foreach (var item in result)
            {
                Console.WriteLine("{0,30}\n{1}", item.Key, item.Value);
            }
        }
        private static string ClearParam(string param)
        {
            string name = Regex.Match(param, @"=[\w.]*").Value;
            name = name.Remove(0, 1);
            return name;
        }
        private static Dictionary<string, FileInfoWrapper> FindByAtribute(string param, Dictionary<string, FileInfoWrapper> source)
        {
            string name = GetParam(param, 'a');
            var result = source.Where(n => Regex.Match(n.Value.Atributes.ToString(), name).Success).ToDictionary(k => k.Key, k => k.Value);
            return result;
        }
        private static Dictionary<string, FileInfoWrapper> FindByName(string param, Dictionary<string, FileInfoWrapper> source)
        {
            string name = GetParam(param, 'n');
            var result = source.Where(n => Regex.Match(n.Key, name).Success).ToDictionary(k => k.Key, k => k.Value);
            return result;
        }
        private static Dictionary<string, FileInfoWrapper> FindByExtension(string param, Dictionary<string, FileInfoWrapper> source)
        {
            string type = GetParam(param, 'e');
            var result = source.Where(n => Regex.Match(n.Value.Extension, type).Success).ToDictionary(k => k.Key, k => k.Value);
            return result;
        }
        private static Dictionary<string, FileInfoWrapper> FindByData(string param, Dictionary<string, FileInfoWrapper> source)
        {
            string bareParam = param.Remove(0, param.IndexOf('=')+1);
            int year;
            int month;
            int day;
            int.TryParse(GetParam(bareParam, 'y'), out year);
            int.TryParse(GetParam(bareParam, 'm'), out month);
            int.TryParse(GetParam(bareParam, 'd'), out day);
            var result = source.Where(v =>
                (v.Value.CreationTime.Day == day || day == 0) &&
                (v.Value.CreationTime.Month == month || month == 0) &&
                (v.Value.CreationTime.Year == year || year == 0))
                .ToDictionary(s => s.Key, s => s.Value);
            return result;
        }

        private static string GetParam(string param, char key)
        {
            string result = Regex.Match(param, @"-" + key + @"=[^\s-]+").Value;
            string bareResult = result.Remove(0, result.IndexOf('=') + 1);
            return bareResult;
        }

        public static void Command()
        {
            Console.Write("{0} >> ", location);
            try
            {
                string command = Console.ReadLine();
                string[] words = command.Split(' ');
                string first = words[0];
                string second = string.Join(" ", words, 1, words.Length - 1);
                switch (first)
                {
                    case "cd":
                        Cd(second);
                        break;
                    case "pwd":
                        Pwd();
                        break;
                    case "help":
                        Help();
                        break;
                    case "push":
                        Push(second);
                        break;
                    case "show":
                        Show(second);
                        break;
                    case "exit":
                        Exit();
                        break;
                    case "remove":
                        Remove(second);
                        break;
                    case "find":
                        Find(second);
                        break;
                    default:
                        Console.WriteLine("Unknown command. Type 'help' for help");
                        break;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
            finally
            {
                Command();
            }
        }

        static void Main(string[] args)
        {
            Command();
            Console.ReadKey();
        }
    }
}
