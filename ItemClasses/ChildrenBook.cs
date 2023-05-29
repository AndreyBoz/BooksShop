using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.ItemClasses
{
    internal class ChildrenBook : Book
    {
        private string theme;
        private int age;

        public ChildrenBook(string author, string name, string publisher, int year, int countPage, decimal price, decimal margin, int count, string theme, int age) : base(author, name, publisher, year, countPage, price, margin,count)
        {
            this.theme = theme;
            this.age = age;
        }
    }
}
