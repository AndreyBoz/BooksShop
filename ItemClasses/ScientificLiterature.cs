using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.ItemClasses
{
    internal class ScientificLiterature : Book
    {
        private int kvartel;
        private string subject;
        private string theme;

        public ScientificLiterature(string author, string name, string publisher, int year, int countPage, decimal price, decimal margin, int count,int kvartel,string subject,string theme) : base(author, name, publisher, year, countPage, price, margin,count)
        {
            this.kvartel = kvartel;
            this.subject = subject;
            this.theme = theme;
        }
    }
}
