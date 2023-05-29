using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.ItemClasses
{
    abstract class Book
    {
        private string author;
        private string name;
        private string publisher;
        private int year;
        private int countPage;
        private decimal price;
        private decimal margin;
        private int rating =0;
        private int count;

        protected Book(string author, string name, string publisher, int year, int countPage, decimal price, decimal margin,int count)
        {
            this.author = author;
            this.name = name;
            this.publisher = publisher;
            this.year = year;
            this.countPage = countPage;
            this.price = price;
            this.margin = margin;
            this.count = count;
        }
        public int getCount() { return count; }
        public void setCount(int count) { this.count = count; }
        public int getRating() { return rating; }
        public void setRating(int rating) { this.rating = rating; }
        public int getYear() { return year; }
        public override string ToString()
        {
            return "Name book: "+name+" Price: "+price;
        }
        public void setPrice(decimal price) { this.price = price; }
        public decimal getPrice() { return price; }
        public void setMargin(decimal margin) { this.margin = margin; }
        public decimal getMargin() { return margin; }
        public string getAuthor() { return author; }

    }
}
