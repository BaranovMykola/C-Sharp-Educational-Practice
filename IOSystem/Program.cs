using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IOSystem
{
    class Program
    {
        private static string location = string.Empty;
        private static Dictionary<string, FileInfoWrapper> list = new Dictionary<string, FileInfoWrapper>();
        private static string help = string.Empty;
        private delegate bool selector(KeyValuePair<string, FileInfoWrapper> pair);
        private delegate bool RegexModifier(string full, string newStr);
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
        #region Configuration
        private static void LoadLog()
        {
            using (var loader = File.Open(fileConfiguration["fileInfoLog"], FileMode.Open))
            {
                var xml = new XmlSerializer(typeof(SerializableKeyValuePair<string, FileInfoWrapper>[]));
                var obj = xml.Deserialize(loader);
                list = (obj as SerializableKeyValuePair<string, FileInfoWrapper>[]).ToDictionary(s => s.Key, s => s.Value);
            }
        }
        private static void SaveLog()
        {
            File.WriteAllText(fileConfiguration["fileInfoLog"], string.Empty);
            using (var saver = File.Open(fileConfiguration["fileInfoLog"], FileMode.OpenOrCreate))
            {
                var xml = new XmlSerializer(typeof(SerializableKeyValuePair<string, FileInfoWrapper>[]));
                xml.Serialize(saver, list.Select( s => new SerializableKeyValuePair<string, FileInfoWrapper>(s.Key, s.Value)).ToArray());
            }
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
        #endregion
        #region Commands body
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
                try
                {
                    FileInfo fileInfo = new FileInfo(item);
                    if (!list.ContainsKey(fileInfo.Name))
                    {
                        int dot = fileInfo.Name.LastIndexOf('.');
                        string s = fileInfo.Name.Substring(0, dot == -1 ? fileInfo.Name.Length : dot);
                        list.Add(s, new FileInfoWrapper(item));
                        Console.WriteLine(item);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

        }
        public static void Show(string sort, Dictionary<string, FileInfoWrapper> data)
        {
            if (data.Count == 0)
            {
                throw new InvalidDataException("The list is empty");
            }
            var toSort = data.ToList();
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
            if (string.IsNullOrEmpty(key))
            {
                list.Clear();
                return;
            }
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
            Dictionary<string, FileInfoWrapper> result = list;

            string nameParam = Regex.Match(param, @"-n=[^\s]+").Value;
            if (!string.IsNullOrEmpty(nameParam))
            {
                string bareParam = param.Remove(0, param.IndexOf('=') + 1);
                string paramN = GetParam(param, 'n');
                bareParam = bareParam.Replace(paramN, string.Empty);
                RegexModifier modifier = null;
                if (bareParam == "-f")
                {
                    modifier = (f, s) => (f == s);
                }
                else if (bareParam != "-f" && !string.IsNullOrEmpty(bareParam))
                {
                    throw new ArgumentException("Unknown parameter modifier to -n=[regex]. Did you mean '-f'?");
                }
                else
                {
                    modifier = (f, s) => true;
                }
                result = FindBy(s =>
                    {
                        var match = Regex.Match(s.Key, GetParam(param, 'n'));
                        return match.Success && modifier(match.Value, s.Key);
                    }
                    , result);
            }

            string extensionParam = Regex.Match(param, @"-e=[^\s]+").Value;
            if (!string.IsNullOrEmpty(extensionParam))
            {
                result = FindBy(s => Regex.Match(s.Value.Extension, GetParam(extensionParam, 'e')).Success, result);
            }

            string atributeParam = Regex.Match(param, @"-a=[^\s]+").Value;
            if (!string.IsNullOrEmpty(atributeParam))
            {
                string name = GetParam(param, 'a');
                result = FindBy((s => Regex.Match(s.Value.Atributes.ToString() + ",", string.Format(@"({0},)", name)).Success), result);
            }

            string dataParam = Regex.Match(param, @"-d=[\w.\-=]+").Value;
            if (!string.IsNullOrEmpty(dataParam))
            {
                result = FindByData(dataParam, result);
            }

            Show(string.Empty, result);
        }
        private static string ClearParam(string param)
        {
            string name = Regex.Match(param, @"=[\w.]*").Value;
            name = name.Remove(0, 1);
            return name;
        }
        private static Dictionary<string, FileInfoWrapper> FindBy(selector s, Dictionary<string, FileInfoWrapper> source)
        {
            var result = source.Where(p => s(p)).ToDictionary(k => k.Key, k => k.Value);
            return result;
        }
        private static Dictionary<string, FileInfoWrapper> FindByData(string param, Dictionary<string, FileInfoWrapper> source)
        {
            string bareParam = param.Remove(0, param.IndexOf('=') + 1);
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
        #endregion
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
                        Show(second, list);
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
                    case "clear":
                        Console.Clear();
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
            Console.WriteLine("IO storage system:");
            Console.WriteLine();
            Console.WriteLine();
            Command();
            Console.ReadKey();
        }
    }
}
