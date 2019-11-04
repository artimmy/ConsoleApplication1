using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            float largMax = 998f; //largura maxima ou maxWidht do retângulo
            float altMax = 680f; //altura maxima ou maxHeight do retângulo


            var cromossomo = new FloatingPointChromosome //Criando um cromossomo de distância euclidiana
                (
                    new double[] { 0, 0, 0, 0 },
                    new double[] { largMax, altMax, largMax, altMax },
                    new int[] { 10, 10, 10, 10 },
                    new int[] { 0, 0, 0, 0 }
                );

            var population = new Population(50, 100, cromossomo); // população, de no minimo 50 e no maximo 100, criada usando o cromossomo

            var fitness = new FuncFitness((c) => // criando uma função fitness 
            {
                var fc = c as FloatingPointChromosome; 

                var valores = fc.ToFloatingPoints();
                var x1 = valores[0];
                var y1 = valores[1];
                var x2 = valores[2];
                var y2 = valores[3];

                return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)); //retorno  

            });

            var peneira = new EliteSelection(); // seletor "peneira" do tipo ELITE ou seja só os melhores passarão >>>> Besides this, you can use the already implemented classic selections: Elite, Roulete Wheel, Stochastic Universal Sampling and Tournament.

            var crossover = new UniformCrossover(0.5f); // aqui ocorre a união dos cromossomos para gerar novas possibilidades de soluções da proxima geração

            var mutacao = new FlipBitMutation(); // classe de mutação 

            var terminator = new FitnessStagnationTermination(100); // essa parte passa uma "régua" no cromossomo e para de executar quando ele gera o melhor cromossomo 100 vezes.

            var creator = new GeneticAlgorithm(population, fitness, peneira, crossover, mutacao); // instaciei o algoritmo genetico>>>> 

            creator.Termination = terminator;// parametros de terminação passados atraves do terminator

            //creator.Start();// mando ele iniciar o algoritmo daqui pra baixo da pra apagar que vai funcionar

            Console.WriteLine("Generation: (x1, y1), (x2, y2) = distance");  

            var latestFitness = 0.0;

            creator.GenerationRan += (sender, e) =>
            {
                var bestChromosome = creator.BestChromosome as FloatingPointChromosome;
                var bestFitness = bestChromosome.Fitness.Value;

                if (bestFitness != latestFitness)
                {
                    latestFitness = bestFitness;
                    var phenotype = bestChromosome.ToFloatingPoints();

                    Console.WriteLine(
                        "Generation {0,2}: ({1},{2}),({3},{4}) = {5}",
                        creator.GenerationsNumber,
                        phenotype[0],
                        phenotype[1],
                        phenotype[2],
                        phenotype[3],
                        bestFitness
                    );
                }
            };

            creator.Start();

            Console.ReadKey();

        }
    }
}
