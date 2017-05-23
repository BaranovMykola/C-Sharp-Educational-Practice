using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using FitnesCentre.FitnesClub;
using MoreLinq;

namespace FitnesCentre
{
    class FitnesCentreProgram
    {
        static List<FitnesClub.Client> clientList = new List<FitnesClub.Client>();
        const string BinClientsFile = "../../clients.bin";

        static FitnesCentreProgram()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            PopulateCliets();
            ShowClients();
        }

        static void PopulateCliets()
        {
            using (var stream = File.Open(BinClientsFile, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                clientList = bin.Deserialize(stream) as List<FitnesClub.Client>;
            }
        }
        bool DefualtSelector(FitnesClub.Client item) => true;
        static void ShowClients(Func<FitnesClub.Client, bool> selector = null)
        {
            var resultSeq =
                from item in clientList
                where selector == null ? true : selector(item)
                select item;
            foreach (var item in clientList)
            {
                Console.WriteLine(item);
            }
        }
        static void AddClient(int year, int month, int duration)
        {
            clientList.Add(new FitnesClub.Client(year, month, duration));
        }
        static void InputClientFromKeyboard()
        {
            try
            {
                Console.Write("Year: ");
                int year = int.Parse(Console.ReadLine());
                Console.Write("Month: ");
                int month = int.Parse(Console.ReadLine());
                Console.Write("Duration (days): ");
                int durations = int.Parse(Console.ReadLine());
                AddClient(year, month, durations);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine();
            var yearGroups =
                from item in clientList
                group item.DurationDays by item.Year
                into grp
                orderby grp.Key descending 
                select new {Year = grp.Key, Duration = grp.Sum()};
            var res = yearGroups.MaxBy(s => s.Duration);
            Console.WriteLine("Grouped by year: ");
            foreach (var item in yearGroups)
            {
                Console.WriteLine($"Year [{item.Year}] => [{item.Duration}]");
            }
            Console.WriteLine();
            Console.WriteLine("Max duration: ");
            Console.WriteLine($"Year [{res.Year}] => [{res.Duration}]");
            Console.ReadKey();
        }
    }
}
