using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
    class Program
    {
        private static string target = "Find me";
        static void Main(string[] args)
        {
            var population = new Poplation(100, target.Length);

            foreach (var dna in population.Dna)
            {
                Console.WriteLine(new string(dna.Genes));
            }
            Console.ReadLine();
        }
    }

    class Dna
    {
        public char[] Genes { get; set; }

        public Dna(int length, Random randomGenerator)
        {
            Genes = new char[length];
            for (var i = 0; i < length; i++)
            {
                Genes[i] = (char)randomGenerator.Next(34, 95);
            }
        }
    }

    class Poplation
    {
        public Dna[] Dna { get; set; }


        public Poplation(int populationSize, int targetLength)
        {
            Dna = new Dna[populationSize];
            var randomGen = new Random();

            for (var i = 0; i < populationSize; i++)
            {
                Dna[i] = new Dna(targetLength, randomGen);
            }
        }
    }
}
