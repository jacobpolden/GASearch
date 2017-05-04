using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Search
{
    static class Configuration
    {
        public static string Target => ConfigurationManager.AppSettings["target"];
        public static int PopulationSize => Convert.ToInt32(ConfigurationManager.AppSettings["populationSize"]);
        public static int MutationRate => Convert.ToInt32(ConfigurationManager.AppSettings["mutationRate"]);
    }

    class Program
    {

        static void Main(string[] args)
        {
            var random = new Random();
            var population = new Poplation(Configuration.PopulationSize, Configuration.Target, random);

            var fittestDna = string.Empty;
            decimal topFitness = 0;
            var generation = 0;
            decimal accumAllFitness = 0;
            decimal accumTopFitness = 0;

            while (topFitness != 1)
            {
                generation++;
                foreach (var person in population.People)
                {
                    person.CalcualteFitness(Configuration.Target);

                    var dna = new string(person.Genes);
                    if (person.Fitness > topFitness)
                    {
                        topFitness = person.Fitness;
                        fittestDna = dna;
                    }
                    accumAllFitness = accumAllFitness + person.Fitness;
                }
                accumTopFitness = accumTopFitness + topFitness;
                Console.WriteLine($"Fittest DNA \"{fittestDna}\" -- {topFitness}");
                population.EvolvePopulation();
            }
            Console.WriteLine("************SUCCESS************");
            Console.WriteLine($"Generation {generation}");
            Console.WriteLine($"Average total fitness {accumAllFitness / (generation * Configuration.PopulationSize) * 100}%");
            Console.WriteLine($"Average top fitness {accumTopFitness / generation * 100}%");
            Console.ReadLine();
        }
    }

    class Person
    {
        public char[] Genes { get; }
        public decimal Fitness { get; set; }

        public Person(int length, Random randomGenerator)
        {
            Genes = new char[length];
            for (var i = 0; i < length; i++)
                Genes[i] = (char)randomGenerator.Next(32, 122);
        }

        public Person(char[] genes)
        {
            Genes = genes;
        }

        public void CalcualteFitness(string target)
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
                Fitness = 0;
            else
                Fitness = matchingCharCount / targetLength;
        }

        public Person Breed(Person partner, Random random)
        {
            var partnerGenesLength = partner.Genes.Length;

            if (Genes.Length != partnerGenesLength)
                throw new ArgumentException("Partners must have the same length of genes!");

            var midpoint = random.Next(0, partnerGenesLength - 1);

            var genes = new char[partnerGenesLength];

            for (var i = 0; i < partnerGenesLength; i++)
            {
                if (i > midpoint)
                    genes[i] = partner.Genes[i];
                else
                    genes[i] = Genes[i];

            }

            var child = new Person(genes);

            for (var i = 0; i < child.Genes.Length; i++)
            {
                if (random.Next(0, 100) <= Configuration.MutationRate)
                    child.Genes[i] = (char)random.Next(32, 122);

            }

            return child;
        }
    }

    class Poplation
    {
        public Person[] People { get; set; }
        private readonly Random _random;

        public Poplation(int populationSize, string target, Random random)
        {
            _random = random;
            People = new Person[populationSize];

            for (var i = 0; i < populationSize; i++)
                People[i] = new Person(target.Length, _random);

        }

        public void EvolvePopulation()
        {
            var matingPool = new List<Person>();
            var survivors = People.ToList();
            var maxFitness = survivors.OrderByDescending(person => person.Fitness).First().Fitness;

            foreach (var person in survivors)
            {
                var normalisedFitness = person.Fitness / maxFitness;
                for (var i = 0; i < normalisedFitness * 100; i++)
                    matingPool.Add(person);

            }

            for (var i = 0; i < People.Length; i++)
            {
                var a = _random.Next(0, matingPool.Count);
                var b = _random.Next(0, matingPool.Count);

                var partnerA = matingPool[a];
                var partnerB = matingPool[b];

                People[i] = partnerA.Breed(partnerB, _random);
            }
        }
    }
}
