using System;
using WeSave.Data;

namespace WeSave.Level6
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager.Level<DataModel, Data.Level6.Output>();
            Console.WriteLine("Appuyer sur une touche...");
            Console.ReadKey();
        }
    }
}
