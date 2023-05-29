using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.ItemClasses
{
    internal class ForeignLiterature : Book
    {
        private string language;
        private string translators;

        public ForeignLiterature(string author, string name, string publisher, int year, int countPage, decimal price, decimal margin,int count, string language, string translators) : base(author, name, publisher, year, countPage, price, margin,count)
        {
            this.language = language;
            this.translators = translators;
        }
    }
}
