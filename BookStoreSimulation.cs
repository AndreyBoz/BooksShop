using BooksShop.ItemClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop
{
    class BookStoreSimulation
    {
        private List<Book> books = new List<Book>();
        private List<Book> orderToPublisher = new List<Book>();
        private List<Order> allCompletedOrders = new List<Order>(); // выполненные магазином заказы
        private List<Book> booksSold = new List<Book>();
        private List<Book> allOrders = new List<Book>(); // все заказы книг  (обработанные и не обработанные)
        private List<Book> completedOrdersByPublisher = new List<Book>(); // выполненный заказы издательств
        private int simulationDays;
        private decimal retailMarkupPercentage;
        private decimal newBookMarkupPercentage;
        private Random random;
        private decimal coefficientDeliver = 0;
        private int deliveryDays;
        private int booksInDelivery;

        public BookStoreSimulation(int simulationDays, decimal retailMarkupPercentage, decimal newBookMarkupPercentage,int deliveryDays,int booksInDelivery)
        {
            this.simulationDays = simulationDays;
            this.retailMarkupPercentage = retailMarkupPercentage;
            this.newBookMarkupPercentage = newBookMarkupPercentage;
            random = new Random();
            // создаю изначальную базу книг
            for (int i = 0; i < 25; i++)
            {
                books.Add(GenerateArtisticLiteratureOrder(retailMarkupPercentage, booksInDelivery));
                books.Add(GenerateChildrenBookOrder(retailMarkupPercentage, booksInDelivery));
                books.Add(GenerateForeignLiteratureOrder(retailMarkupPercentage, booksInDelivery));
                books.Add(GenerateManualOrder(retailMarkupPercentage, booksInDelivery));
                books.Add(GenerateScientificLiteratureOrder(retailMarkupPercentage, booksInDelivery));
            }
            int totalBookCount = books.Sum(book => book.getCount());
            int uniqueBooksCount = books.Select(book => book.getCount()).Distinct().Count(); // уникальный товар
            coefficientDeliver = (totalBookCount / uniqueBooksCount >= 0.5 ? 2 : 1); // коэффициент плотности заказов в зависимости от разнообразия товара
            this.deliveryDays = deliveryDays;
            this.booksInDelivery = booksInDelivery;
        }

        public void RunSimulation()
        {
            for (int i = 1; i <= simulationDays; i++) {
                GenerateBookOrders();
                int totalBookCount = books.Sum(book => book.getCount());
                int uniqueBooksCount = books.Select(book => book.getCount()).Distinct().Count();
                coefficientDeliver = (totalBookCount / uniqueBooksCount >= 0.5 ? 2 : 1);
                if (i % deliveryDays == 0) { // так же каждые 3 дня мы убираем наценку на новые книги
                    foreach (var book in books) {
                        if (book.getMargin() == newBookMarkupPercentage) {
                            book.setPrice(book.getPrice() / newBookMarkupPercentage);
                            book.setMargin(retailMarkupPercentage);
                        }
                    }
                    OrderDelivery(); // каждый период доставки, который пользователь задал, доставляются новые книги
                }
            }
            SaveStaff();
        }
        private void SaveStaff()
        {
            using (StreamWriter streamWriter = new StreamWriter("Book staff.txt")) { // Ассортимент магазина
                foreach (var book in books) {
                    streamWriter.WriteLine(book);
                }
            }
            using (StreamWriter streamWriter = new StreamWriter("All order info.txt")) {
                streamWriter.WriteLine("All orders: ");      
                foreach (var order in allOrders.Distinct()) {
                    streamWriter.WriteLine(order);
                }
            }
            using (StreamWriter streamWriter = new StreamWriter("Completed order info.txt"))
            {
                streamWriter.WriteLine("All completed orders: ");
                streamWriter.WriteLine("ArtisticLiterature: " + booksSold.OfType<ArtisticLiterature>().Sum(x => x.getCount()));
                streamWriter.WriteLine("ChildrenBook: " + booksSold.OfType<ChildrenBook>().Sum(x => x.getCount()));
                streamWriter.WriteLine("ForeignLiterature: " + booksSold.OfType<ForeignLiterature>().Sum(x => x.getCount()));
                streamWriter.WriteLine("Manual: " + booksSold.OfType<Manual>().Sum(x => x.getCount()));
                streamWriter.WriteLine("ScientificLiterature: " + booksSold.OfType<ScientificLiterature>().Sum(x => x.getCount()));
                foreach (var order in allCompletedOrders)
                {
                    streamWriter.WriteLine(order);
                }
            }
            using (StreamWriter streamWriter = new StreamWriter("Rating book.txt")) {
                streamWriter.WriteLine("The most popular book(top 10):");
                for(int i = 0;i<10;i++){
                    streamWriter.WriteLine(books.OrderByDescending(x => x.getRating()).Distinct().ToList()[i] + " " + books.OrderByDescending(x => x.getRating()).Distinct().ToList()[i].getRating());
                }
                streamWriter.WriteLine("Book rating:");
                foreach (var book in books.OrderByDescending(x => x.getRating())) {
                    streamWriter.WriteLine(book + " " + book.getRating());
                }
            }
            using (StreamWriter streamWriter = new StreamWriter("Completed orders by publisher.txt"))
            {
                streamWriter.WriteLine("Completed orders by publisher:");
                foreach (var order in completedOrdersByPublisher)
                {
                    streamWriter.WriteLine(order);
                }
            }
        }
        private void GenerateBookOrders()
        {
            Order order = null;
            for (int i = 0; i < 5 * coefficientDeliver; i++)
            {
                switch (random.Next(1, 3))
                {
                    case 1:
                        order = GenerateBookStoreOrder();
                        break;
                    case 2:
                        order = GeneratePhoneOrder();
                        break;
                    case 3:
                        order = GenerateEmailOrder();
                        break;
                }
                allCompletedOrders.Add(order);
            }
        }
        private Order GenerateBookStoreOrder() {
            Order order = new Order("Customer " + random.Next(1, 100), null, null);
            order.setOrdersItems(GenerateBook());
            return order;
        }
        private Order GeneratePhoneOrder() {
            Order order = new Order("Customer " + random.Next(1,100), "Phone " + random.Next(1,11), null);
            order.setOrdersItems(GenerateBook());
            return order;
        }
        private Order GenerateEmailOrder() {
            Order order = new Order("Customer " + random.Next(1, 100), null, "Email " + random.Next(1, 11));
            order.setOrdersItems(GenerateBook());
            return order;
        }
        private void CheckStaff(Book book,int countBook) {
            if (book.getCount() < 3) { // если книг меньше 3, то они заказываются из издательства
                if(!orderToPublisher.Contains(book)) // если книга уже в заказе, то больше  не заказывается у издательства
                    orderToPublisher.Add(book);
            }
            book.setCount(book.getCount() - countBook); // если больше, просто отнимаем от количества
            book.setRating(book.getRating() + 1); // увеличиваем рейтинг книг при каждом заказе
        }
        private void OrderDelivery() {
            foreach (Book book in orderToPublisher)
            {
                // Проверяем, существует ли книга в списке books
                Book existingBook = books.Find(b => b.Equals(book));

                completedOrdersByPublisher.Add(existingBook); // записываем выполненный издательствами заказы

                if (existingBook != null)
                {
                    // Книга уже существует в списке books, увеличиваем значение count с помощью setCount
                    existingBook.setCount(existingBook.getCount() + book.getCount());
                }
                else
                {
                    // Книги не существует в списке books, добавляем ее
                    books.Add(book);
                }
            }
        }
        private List<OrderItem> GenerateBook() { // генерируем случаную книгу
            List<OrderItem> result = new List<OrderItem>();

            for (int i = 0; i < 1 * coefficientDeliver; i++)
            {
                // Иногда выбираем книгу из списка books, а иногда генерируем новую
                if (random.NextDouble() < 0.7 && books.Count > 0) // в основном выбирают книги из последних 
                {
                    // Выбираем книгу из конца списка books
                    Book existingBook = books[books.Count - random.Next(1,5)];
                    
                    if (existingBook.getCount() < 1) { // если книги нет в наличии, то отправляем заявку в издательство и записываем в список основных заказов
                        orderToPublisher.Add(existingBook);
                        allOrders.Add(existingBook); // считаем заказы не обработанные
                        continue;
                    }
                    int countBook = random.Next(1, existingBook.getCount()); // случайное количество заказанных книг
                    CheckStaff(existingBook, countBook);
                    
                    List<Book> temp = books.Where(x => x.getAuthor() == existingBook.getAuthor()).ToList(); // проверяем последняя ли это книга автора по году
                    string author = existingBook.getAuthor();
                    bool lastBook = temp.OrderBy(x => x.getYear()).ToList()[temp.Count-1]==existingBook;
                    if (result.Any(x => (x.GetAuthor() == author) && (x.IsLastBook() == lastBook))) { // если заказ уже существует, то количество увеличиваем на единицу
                        OrderItem temp2 = result.Find(x => (x.GetAuthor() == author) && (x.IsLastBook() == lastBook));
                        temp2.SetCount(temp2.GetCount() + 1);
                        continue;
                    }
                    
                    booksSold.Add(existingBook);
                    allOrders.Add(existingBook);
                    result.Add(new OrderItem(author,lastBook,countBook));
                } else if(random.NextDouble()< 0.6 && books.Count > 0)
                {
                    int randomIndex = random.Next(books.Count);
                    Book existingBook = books[randomIndex];

                    if (existingBook.getCount() < 1){ // если книги нет в наличии, то отправляем заявку в издательство
                        orderToPublisher.Add(existingBook);
                        allOrders.Add(existingBook);
                        continue;
                    }
                    int countBook = random.Next(1, existingBook.getCount()); // случайное количество заказанных книг
                    CheckStaff(existingBook, countBook);

                    List<Book> temp = books.Where(x => x.getAuthor() == existingBook.getAuthor()).ToList(); // проверяем последняя ли это книга автора по году
                    string author = existingBook.getAuthor();
                    bool lastBook = temp.OrderBy(x => x.getYear()).ToList()[temp.Count - 1] == existingBook;
                    if (result.Any(x => (x.GetAuthor() == author) && (x.IsLastBook() == lastBook)))
                    {
                        OrderItem temp2 = result.Find(x => (x.GetAuthor() == author) && (x.IsLastBook() == lastBook));
                        temp2.SetCount(temp2.GetCount() + 1);
                        continue;
                    }
                    allOrders.Add(existingBook);
                    booksSold.Add(existingBook);
                    result.Add(new OrderItem(author, lastBook, countBook));
                } else
                {
                    // Генерируем случайное число для определения типа книги
                    int bookType = random.Next(1, 6);
                    // Генерируем новую книгу
                    Book newBook;

                    switch (bookType)
                    {
                        case 1:
                            newBook = GenerateArtisticLiteratureOrder(newBookMarkupPercentage, 15);
                            break;
                        case 2:
                            newBook = GenerateChildrenBookOrder(newBookMarkupPercentage, 15);
                            break;
                        case 3:
                            newBook = GenerateForeignLiteratureOrder(newBookMarkupPercentage, 15);
                            break;
                        case 4:
                            newBook = GenerateManualOrder(newBookMarkupPercentage, 15);
                            break;
                        case 5:
                            newBook = GenerateScientificLiteratureOrder(newBookMarkupPercentage, 15);
                            break;
                        default:
                            // Обработка других случаев (при необходимости)
                            continue;
                    }
                    orderToPublisher.Add(newBook);

                }
            }

            return result;
        }
        private Book GenerateArtisticLiteratureOrder(decimal setMargin, int maxCount)
        {
            string author = "Author " + random.Next(1, 11);
            string name = "Artistic Book " + random.Next(1, 101);
            string publisher = "Publisher " + random.Next(1, 6);
            int year = random.Next(1990, 2023);
            int countPage = random.Next(100, 501);
            decimal price = random.Next(10, 51)*(1+setMargin);
            decimal margin = setMargin;

            string genre = "Genre " + random.Next(1, 6);
            string[] heroes = new string[] { "Hero " + random.Next(1, 6), "Hero " + random.Next(1, 6) };
            int count = random.Next(1, maxCount);
            return new ArtisticLiterature(author, name, publisher, year, countPage, price, margin,count, genre, heroes);
        }

        private Book GenerateChildrenBookOrder(decimal setMargin,int maxCount)
        {
            string author = "Author " + random.Next(1, 11);
            string name = "Children Book " + random.Next(1, 101);
            string publisher = "Publisher " + random.Next(1, 6);
            int year = random.Next(1990, 2023);
            int countPage = random.Next(20, 101);
            decimal price = random.Next(5, 16) * (1 + setMargin);
            decimal margin = setMargin;

            string theme = "Theme " + random.Next(1, 6);
            int age = random.Next(1, 13);
            int count = random.Next(1, maxCount);
            return new ChildrenBook(author, name, publisher, year, countPage, price, margin,count, theme, age);
        }

        private Book GenerateForeignLiteratureOrder(decimal setMargin, int maxCount)
        {
            string author = "Author " + random.Next(1, 11);
            string name = "Foreign Book " + random.Next(1, 101);
            string publisher = "Publisher " + random.Next(1, 6);
            int year = random.Next(1990, 2023);
            int countPage = random.Next(100, 501);
            decimal price = random.Next(15, 41) * (1 + setMargin);
            decimal margin = setMargin;

            string language = "Language " + random.Next(1, 6);
            string translators = "Translator " + random.Next(1, 6);
            int count = random.Next(1, maxCount);

            return new ForeignLiterature(author, name, publisher, year, countPage, price, margin, count, language, translators);
        }

        private Book GenerateManualOrder(decimal setMargin,int maxCount)
        {
            string author = "Author " + random.Next(1, 11);
            string name = "Manual " + random.Next(1, 101);
            string publisher = "Publisher " + random.Next(1, 6);
            int year = random.Next(1990, 2023);
            int countPage = random.Next(50, 201);
            decimal price = random.Next(20, 61) * (1 + setMargin);
            decimal margin = setMargin;

            string subject = "Subject " + random.Next(1, 6);
            string[] writer = new string[] { "Writer " + random.Next(1, 6), "Writer " + random.Next(1, 6) };
            int count = random.Next(1, maxCount);
            return new Manual(author, name, publisher, year, countPage, price, margin,count, subject, writer);
        }

        private Book GenerateScientificLiteratureOrder(decimal setMargin,int maxCount)
        {
            string author = "Author " + random.Next(1, 11);
            string name = "Scientific Book " + random.Next(1, 101);
            string publisher = "Publisher " + random.Next(1, 6);
            int year = random.Next(1990, 2023);
            int countPage = random.Next(100, 501);
            decimal price = random.Next(30, 71) * (1 + setMargin);
            decimal margin = setMargin;

            string theme = "Theme " + random.Next(1, 6);
            string subject = "Subject" + random.Next(1, 7);
            int kvartel = random.Next(1, 3);
            int count = random.Next(1, maxCount);

            return new ScientificLiterature(author, name, publisher, year, countPage, price, margin,count, kvartel, subject,theme);
        }

       
    }
}
