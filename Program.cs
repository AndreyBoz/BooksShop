using System;

namespace BooksShop
{
    internal class Program/*Реализовать симуляцию клиент работник*/
    {
        static void Main(string[] args)
        {
            int simulationDays = 25;
            decimal retailMarkupPercentage = 0.15m;
            decimal newBookMarkupPercentage = 0.20m;
            Random random = new Random();


            BookStoreSimulation simulation = new BookStoreSimulation(simulationDays, retailMarkupPercentage, newBookMarkupPercentage,5,15);
            simulation.RunSimulation();

            Console.ReadLine();
        }
    }
}
