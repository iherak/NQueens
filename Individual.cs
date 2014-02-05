using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NASPLabos2_kraljice
{
    public class Individual
    {
        public Gene[] genes;
        public int fitness;
        public int turnedFitness;
        public double cumulativeFitness;

        public Individual(GeneticAlgorithm ga)
        {
            genes = new Gene[ga.boardSize];
            for (int i = 0; i < ga.boardSize; i++)
            {
                genes[i] = new Gene(ga);
            }
        }

        public Individual(Individual old, GeneticAlgorithm ga)
        {
            genes = new Gene[ga.boardSize];
            for (int i = 0; i < ga.boardSize; i++)
            {
                genes[i] = new Gene(old.genes[i].position);
            }
        }

        public void mutation(GeneticAlgorithm ga)
        {
            int geneToMutate = ga.rand.Next(0, ga.boardSize);
                        
            if (ga.rand.NextDouble() < ga.mutationRate)
            {
                genes[geneToMutate].position = ga.rand.Next(0, ga.boardSize);                
            }
        }
    }
}
