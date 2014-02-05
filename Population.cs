using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NASPLabos2_kraljice
{
    public class Population
    {
        public Individual[] population;
        public Individual[] evolvedGeneration;
        public GeneticAlgorithm ga;
        public int bestFitness;
        private int minFitness;
        public double sumTurnedFitness;
        public int worstFitness;

        public Population(GeneticAlgorithm ga)
        {
            this.ga = ga;
            population = new Individual[ga.populationSize];
            worstFitness = 28;
            for(int i=0; i<ga.populationSize;i++)
            {
                Individual indy = new Individual(ga);
                population[i] = indy;
            }
            determineFitness();
            findCumulativeFitness();
        }

        public void evolve()
        {

            Individual parent1, parent2, child1, child2;
            evolvedGeneration = (Individual[]) population.Clone();
            int i;
            Individual[] elites = new Individual[ga.numElites];
            for (int j = 0; j < ga.populationSize; j++)
            {
                for (int k=0; k < ga.numElites; k++)
                {
                    if (elites[k] == null)
                    {
                        elites[k] = population[j];
                        break;
                    }
                    else if (population[j].fitness > elites[k].fitness)
                    {
                        elites[k] = population[j];
                        break;
                    }
                    else
                        continue;
                }
            }
            for (int k = 0; k < ga.numElites ;k++)
            {
                evolvedGeneration[k] = new Individual(elites[k], ga);
            }
            Array.Clear(elites, 0, ga.numElites);
            for (i = ga.numElites; i < ga.populationSize; i=i+2)
            {
                parent1 = rouletteWheel();
                parent2 = rouletteWheel();
                while (parent1 == parent2)
                    parent2 = rouletteWheel();
                
                recombinate(parent1, parent2, out child1, out child2);
                
                child1.mutation(ga);
                child2.mutation(ga);

                evolvedGeneration[i] = child1;
                evolvedGeneration[i + 1] = child2;
            }

            //population = evolvedGeneration;
            Array.Clear(population, 0, ga.populationSize);
            evolvedGeneration.CopyTo(population, 0);
            Array.Clear(evolvedGeneration, 0, ga.populationSize);
            determineFitness();
            findCumulativeFitness();
        }

        private Individual elitism()
        {
            int tmpBest = -100;
            Individual selected = null;
            int indexSelected = 0; ;
            for (int k = 0; k < ga.populationSize; k++)
            {
                if (population[k].fitness > tmpBest)
                {
                    tmpBest = population[k].fitness;
                    selected = population[k];
                    indexSelected = k;
                } 
            }
            population[indexSelected].fitness = -100;

            return selected;
        }

        private void recombinate(Individual parent1, Individual parent2, out Individual child1, out Individual child2)
        {
            int crossPoint = ga.rand.Next(ga.boardSize - 1);
            child1 = new Individual(ga);
            child2 = new Individual(ga);

            for (int i = 0; i < crossPoint; i++)
            {
                child1.genes[i] = parent1.genes[i];
                child2.genes[i] = parent2.genes[i];
            }
            for (int j = crossPoint; j < ga.boardSize - 1; j++)
            {
                child1.genes[j] = parent2.genes[j];
                child2.genes[j] = parent1.genes[j];
            }
        }

        private Individual rouletteWheel()
        {
            Individual selected = null;
            for (int i = ga.populationSize-1; i>= 0; i--)
            {
                double ball = ga.rand.NextDouble();

                if (ball < population[i].cumulativeFitness)
                    selected = population[i];
            }
            return selected;
        }

        private void setMinFitness()
        {
            int min = population[0].fitness;

            foreach (Individual indy in population)
            {
                if (indy.fitness < min)
                {
                    min = indy.fitness;
                }
            }
            minFitness = min;
        }

        private void findCumulativeFitness()
        {
            double sumTurnedFitness = 0;
            double cumFitness = 0;
            setMinFitness();
            for (int i = 0; i < ga.populationSize; i++)
            {
                population[i].turnedFitness = population[i].fitness + minFitness+1;
                sumTurnedFitness += population[i].turnedFitness;
            }
            for (int j = 0; j < ga.populationSize; j++)
            {
                cumFitness += population[j].turnedFitness / sumTurnedFitness;
                population[j].cumulativeFitness = cumFitness;
            }
        }

        private void determineFitness()
        {
            for (int k=0; k<ga.populationSize;k++)
            {
                int fitness = 0;
                for (int i = 0; i < ga.boardSize-1; i++)
                {
                    for (int j = i+1; j < ga.boardSize; j++)
                    {
                        if (population[k].genes[i].position == population[k].genes[j].position)
                            ++fitness;
                        else if ((j-i) == Math.Abs((int)(population[k].genes[i].position - population[k].genes[j].position)))
                            ++fitness;
                    }
                }
                population[k].fitness = (int)(-fitness);
            }
        }
        public int getBestFitness()
        {
            int best = -ga.boardSize;
            int i = 0;
            for (i=0; i<ga.populationSize;i++)
            {
                if (population[i].fitness > best)
                    best=population[i].fitness;
            }
            bestFitness = best;
            return bestFitness;
        }

        public Individual getBestIndividual() 
        {
            int i = 0;
            for (i = 0; i < ga.populationSize; i++) 
            {
                if (population[i].fitness == this.bestFitness) 
                {
                    return population[i];
                }
            }
            return population[i-1];
        
        }
    }
}
