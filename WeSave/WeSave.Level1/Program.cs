using System;
using WeSave.Data;

namespace WeSave.Level1
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager.Level1();
            Console.WriteLine("Appuyer sur une touche...");
            Console.ReadKey();
        }
    }
}
