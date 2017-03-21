using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Book
{
    class BookProgram
    {


        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("Books.txt");
            Book[] books = new Book[data.Length];
            for(int i=0;i<books.Length;++i)
            {
                string[] line = data[i].Split(' ');
                books[i] = new Book(line[0], line[1], uint.Parse(line[2]));
                //Console.WriteLine(books[i]);
            }

            string action;
            do
            {
                

                action = Console.ReadLine();
                string[] commands = action.Split(' ');
                if(commands.Length > 1)
                {
                    if(commands[0] == "sort")
                    {
                        if(commands[1] == "author")
                        {
                            Array.Sort(books, new Book.AuthorComparer());
                        }
                        else if (commands[1] == "name")
                        {
                            Array.Sort(books, new Book.BookNameComparer());
                        }
                        else if (commands[1] == "pages")
                        {
                            Array.Sort(books, new Book.PagesComparer());
                        }
                        else
                        {
                            Console.WriteLine("Unknown commands. Type \"help\" for help");
                        }
                    }
                    if (commands[0] == "find")
                    {
                        var matches = Book.findAuthor(books, commands[1]);
                        foreach(var i in matches)
                        {
                            Console.WriteLine(i);
                        }
                    }
                }
                else
                {
                    if(commands[0] == "help")
                    {
                        Console.WriteLine("sort author/name/pages\nfind arg\nshow\nexit");
                    }
                    if(commands[0] == "show")
                    {
                        foreach (var i in books)
                        {
                            Console.WriteLine(i);
                        }
                    }
                }
                Console.WriteLine();
            }
            while (action != "exit");
        }
    }
}
