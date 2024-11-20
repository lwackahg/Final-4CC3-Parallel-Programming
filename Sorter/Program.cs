using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelNameSorter
{
    struct Person
    {
        public readonly string FullName;
        public readonly string LastName;

        public Person(string fullName)
        {
            FullName = fullName;
            LastName = ExtractLastName(fullName);
        }

        private static string ExtractLastName(string name)
        {
            // Assuming the format is "FirstName LastName"
            int lastSpace = name.LastIndexOf(' ');
            return lastSpace >= 0 ? name.Substring(lastSpace + 1) : name;
        }
    }

    class Program
    {
        // Threshold to decide when to switch to sequential sort
        const int Threshold = 10000;

        // ParallelOptions to limit the degree of parallelism
        static readonly ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        static void Main(string[] args)
        {
            string filePath = "names.txt"; // Path to the file containing names

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            // Preprocess names to extract last names
            Person[] persons = PreprocessNames(filePath);

            Stopwatch stopwatch = Stopwatch.StartNew();
            ParallelQuickSort(persons, 0, persons.Length - 1);
            stopwatch.Stop();

            Console.WriteLine("Code took {0} milliseconds to execute", stopwatch.ElapsedMilliseconds);

            // Uncomment the following lines to verify the sorted list
            // foreach (var person in persons)
            // {
            //     Console.WriteLine(person.FullName);
            // }
        }

        static Person[] PreprocessNames(string filePath)
        {
            // Read all lines and convert to Person structs
            return File.ReadLines(filePath)
                       .AsParallel()
                       .WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism)
                       .Select(line => new Person(line))
                       .ToArray();
        }

        static void ParallelQuickSort(Person[] array, int low, int high)
        {
            if (low < high)
            {
                if (high - low < Threshold)
                {
                    // Use sequential QuickSort for small partitions
                    SequentialQuickSort(array, low, high);
                    return;
                }

                int pivotIndex = Partition(array, low, high);

                // Parallelize sorting of partitions
                Parallel.Invoke(
                    () => ParallelQuickSort(array, low, pivotIndex - 1),
                    () => ParallelQuickSort(array, pivotIndex + 1, high)
                );
            }
        }

        static void SequentialQuickSort(Person[] array, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(array, low, high);
                SequentialQuickSort(array, low, pivotIndex - 1);
                SequentialQuickSort(array, pivotIndex + 1, high);
            }
        }

        static int Partition(Person[] array, int low, int high)
        {
            Person pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (ComparePersons(array[j], pivot) <= 0)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            Swap(array, i + 1, high);
            return i + 1;
        }

        static void Swap(Person[] array, int indexA, int indexB)
        {
            Person temp = array[indexA];
            array[indexA] = array[indexB];
            array[indexB] = temp;
        }

        static int ComparePersons(Person x, Person y)
        {
            int lastNameComparison = string.Compare(x.LastName, y.LastName, StringComparison.Ordinal);
            if (lastNameComparison != 0)
                return lastNameComparison;

            return string.Compare(x.FullName, y.FullName, StringComparison.Ordinal);
        }
    }
}