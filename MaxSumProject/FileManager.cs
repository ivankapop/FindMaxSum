using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MaxSumProject
{
    public class FileManager
    {
        public FileData FileData { get; private set; } = new FileData();

        private const string _validRowRegex = @"^([\,\ ]*[\-]?([\d]+|([\d]+[.]{1}[\d]+))[ ]*)([,]+[ ]*([\-]?([\d]+|[\d]+[.]{1}[\d]+))*[ ]*)*$";

        public void SetFileData(string[] args)
        {
            if (args != null && args.Length > 0 && File.Exists(args[0]))
            {
                FileData.FilePath = args[0];
                return;
            }

            do
            {
                FileData.FilePath = Console.ReadLine();
            }
            while (!CheckIfExists(FileData.FilePath));

            if (!TryInitFileData())
            {
                Console.WriteLine("File is Empty");
            };
        }

        private bool CheckIfExists(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("There is no such file.");
                return false;
            }
            return true;
        }

        private bool TryInitFileData()
        {
            using (var reader = new StreamReader(FileData.FilePath, Encoding.Default))
            {
                string row;
                int rowNumber = 0;
                var sumsOfRows = new Dictionary<int, double>();

                while ((row = reader.ReadLine()) != null)
                {
                    ProcessFileRow(row, ++rowNumber, sumsOfRows);
                }

                if (rowNumber == 0)
                {
                    return false;
                }

                if (sumsOfRows.Any())
                {
                    double maxValue = sumsOfRows.Select(t => t.Value).Max();
                    FileData.RowsWithMaxSum = sumsOfRows.Where(t => t.Value == maxValue).Select(t => t.Key).ToList();
                }
            }
            return true;
        }

        private void ProcessFileRow(string row, int rowNumber, Dictionary<int, double> sumsOfRows)
        {
            if (!Regex.IsMatch(row, _validRowRegex))
            {
                FileData.DefectedRows.Add(rowNumber);
                return;
            }

            double sum = 0;
            var rowArray = row.Split(',');
            for (var i = 0; i < rowArray.Length; i++)
            {
                sum += double.TryParse(rowArray[i], out double value) ? value : 0;
            }
            sumsOfRows.Add(rowNumber, sum);
        }

        public void DisplayResult()
        {
            Console.WriteLine($" ------Result----->");
            if (FileData.RowsWithMaxSum.Any())
            {
                Console.WriteLine($"Lines with the max sum of numbers are");
                foreach (var num in FileData.RowsWithMaxSum)
                {
                    Console.Write($"{num}; ");
                }
                Console.WriteLine();
            }

            if (FileData.DefectedRows.Any())
            {
                Console.WriteLine($"Defected lines are: ");
                foreach (var num in FileData.DefectedRows)
                {
                    Console.Write($"{num}; ");
                }
            }
            Console.ReadKey();
        }
    }
}
