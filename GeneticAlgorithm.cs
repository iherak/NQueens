using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NASPLabos2_kraljice
{
    public class GeneticAlgorithm
    {
        public int boardSize;
        public int populationSize;
        public int maxGenerations;
        public double recombinationRate;
        public double mutationRate;
        public int numElites;
        public Random rand;

        public GeneticAlgorithm()
        {
            boardSize = 8;
            populationSize = 50;
            maxGenerations = 10000;
            recombinationRate = 70;
            mutationRate = 0.1;
            numElites = 2;
            rand = new Random(System.DateTime.Now.Millisecond);
        }
    }
}
