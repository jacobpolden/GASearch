using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
    class Program
    {
        private static string target = "Find me";
        static void Main(string[] args)
        {
            var population = new Poplation(100, target);

            foreach (var dna in population.Dna)
            {
                Console.WriteLine(new string(dna.Genes));
                Console.WriteLine(dna.CalcualteFitness(target));

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

        public decimal CalcualteFitness(string target)
        {
            if (target.Length != Genes.Length)
                throw new ArgumentException($"{nameof(target)} cannot have a different size to the gene");

            var targetCharArr = target.ToCharArray();
            decimal targetLength = targetCharArr.Length;
            var matchingCharCount = 0;

            for (var i = 0; i < targetLength; i++)
                if (targetCharArr[i] == Genes[i])
                    matchingCharCount++;

            if (matchingCharCount == 0)
                return 0;

            return matchingCharCount / targetLength;
        }
    }

    class Poplation
    {
        private readonly string _target;
        public Dna[] Dna { get; set; }


        public Poplation(int populationSize, string target)
        {
            _target = target;
            Dna = new Dna[populationSize];
            var randomGen = new Random();

            for (var i = 0; i < populationSize; i++)
                Dna[i] = new Dna(_target.Length, randomGen);

        }

        public void Breed()
        {
            var survivors = Dna.Where(dna => dna.CalcualteFitness(_target) > 0);
        }
    }
}
