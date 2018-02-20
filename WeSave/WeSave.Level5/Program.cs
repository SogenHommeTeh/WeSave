using System;
using WeSave.Data;

namespace WeSave.Level5
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager.Level<DataModel, Data.Level5.Output>();
            Console.WriteLine("Appuyer sur une touche...");
            Console.ReadKey();
        }
    }
}
