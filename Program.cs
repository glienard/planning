using System;

namespace Planning
{
    class Program
    {
        static void Main(string[] args)
        {
            WinterTennis wt = new WinterTennis(8);
            if (args.Length == 0)
                wt.Calculate();
            else
                wt.CalculateRandom();
        }
    }
}
