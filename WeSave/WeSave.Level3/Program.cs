using System;
using WeSave.Data;

namespace WeSave.Level3
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager.Level3();
            Console.WriteLine("Appuyer sur une touche...");
            Console.ReadKey();
        }
    }
}
