using System;
using System.Collections.Generic;
using System.Text;

namespace Planning.da
{
    public class Participant
    {
        public string Name { get; set; }
        public double IdealTurns { get; set; }
        public double IdealEveryXTurns { get; set; }

        public List <int> PlannedTurns { get; set; }
        public List<byte> Partners { get; set; }
    }

    public class Unavailable
    {
        public Turn When { get; set; }
        public Participant Who { get; set; }

    }

    public class Turn
    {
        public DateTime Dte { get; set; }

        public int ID { get; set; }
    }

}
