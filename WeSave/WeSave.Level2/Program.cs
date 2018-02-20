using System;
using WeSave.Data;

namespace WeSave.Level2
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager.Level<DataModel, Data.Level2.Output>();
            Console.WriteLine("Appuyer sur une touche...");
            Console.ReadKey();
        }
    }
}
