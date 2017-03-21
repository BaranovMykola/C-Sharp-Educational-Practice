using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book
{
    class Book
    {
        public string Author { get; set; }
        public string BookName { get; set; }
        public uint Pages { get; set; }

        public class AuthorComparer : IComparer<Book>
        {
            int IComparer<Book>.Compare(Book x, Book y)
            {
                return string.Compare(x.Author, y.Author);
            }
        }
        public class BookNameComparer : IComparer<Book>
        {
            int IComparer<Book>.Compare(Book x, Book y)
            {
                return string.Compare(x.BookName, y.BookName);
            }
        }
        public class PagesComparer : IComparer<Book>
        {
            int IComparer<Book>.Compare(Book x, Book y)
            {
                return (int)x.Pages - (int)y.Pages;
            }
        }

        public Book(string author, string bookName, uint pages)
        {
            Author = author;
            BookName = bookName;
            Pages = pages;
        }
        public override string ToString()
        {
            return String.Format("Book Author: {0} Name: {1} Pages: {2}", Author, BookName, Pages);
        }

        public static IEnumerable<Book> findAuthor(IEnumerable<Book> data, string author)
        {
            foreach(var i in data)
            {
                if(i.Author == author)
                {
                    yield return i;
                }
            }
        }
    }
}
