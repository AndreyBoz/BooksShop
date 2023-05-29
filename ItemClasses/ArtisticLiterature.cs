using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.ItemClasses
{
    internal class ArtisticLiterature : Book
    {
        private string genre;
        private string[] heroes;

        public ArtisticLiterature(string author, string name, string publisher, int year, int countPage, decimal price, decimal margin,int count, string genre, string[] heroes):base(author,name,publisher,year,countPage,price,margin,count)
        {
            this.genre = genre;
            this.heroes = heroes;
        }
    }
}
