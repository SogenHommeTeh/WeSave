using System;
using WeSave.Data;

namespace WeSave.Level5
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager.Level5();
            Console.WriteLine("Appuyer sur une touche...");
            Console.ReadKey();
        }
    }
}
