using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.ItemClasses
{
    internal class Manual : Book
    {
        private string subject;
        private string[] writer;

        public Manual(string author, string name, string publisher, int year, int countPage, decimal price, decimal margin,int count,string subject, string[] writer) : base(author, name, publisher, year, countPage, price, margin,count)
        {
            this.subject = subject;
            this.writer = writer;
        }
    }
}
