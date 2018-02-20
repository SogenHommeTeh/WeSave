using System;
using WeSave.Data;

namespace WeSave.Level4
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager.Level<DataModel, Data.Level4.Output>();
            Console.WriteLine("Appuyer sur une touche...");
            Console.ReadKey();
        }
    }
}
