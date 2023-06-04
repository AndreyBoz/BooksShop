using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.ItemClasses
{
    class OrderItem {
        private string Author;
        private bool LastBook; // true - книга последняя выпущенная автором, false - не последняя
        private int Count;
        public OrderItem(string author, bool lastBook, int count)
        {
            Author = author;
            LastBook = lastBook;
            Count = count;
        }
        public string GetAuthor() { return Author; }
        public bool IsLastBook() { return LastBook; }
        public int GetCount() { return Count; }
        public void SetCount(int count) { Count = count; }
        public override string ToString()
        {
            return "Author: " + Author + ". Is lastbook: " + LastBook + ". Count: " + Count;
        }
    }
    class Order
    {
        private string CustomerLastName;
        private string PhoneNumber;
        private string EmailAdress;
        private List<OrderItem> Books = new List<OrderItem>();

        public Order(string customerLastName, string phoneNumber, string emailAdress)
        {
            CustomerLastName = customerLastName;
            PhoneNumber = phoneNumber;
            EmailAdress = emailAdress;
        }
        public void setOrdersItems(List<OrderItem> orderItems) {
            this.Books = orderItems;
        }
        public int getOrderItemCount() {
            return Books.Count;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(CustomerLastName + ":");
            stringBuilder.AppendLine(string.Join("\n", Books.Select(book => book.ToString() + "\n")));
            return stringBuilder.ToString();
        }
    }

}
