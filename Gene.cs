using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NASPLabos2_kraljice
{
    public class Gene
    {
        public int position;

        public Gene(GeneticAlgorithm ga)
        {
            position = ga.rand.Next(0, ga.boardSize);
        }

        public Gene(int newPosition)
        {
            this.position = newPosition;
        }
    }
}
