using System;

namespace MaxSumProject{
    class Program {

        static void Main(string[] args){

            Console.WriteLine("Hello!");
            Console.WriteLine("We are going to find a row with max sum of numbers.");
            Console.WriteLine("Please, enter the path");

            var fileManager = new FileManager();
            fileManager.SetFileData(args);
            fileManager.DisplayResult();

        }
    }
}
