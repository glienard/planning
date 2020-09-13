using System;
using System.Collections.Generic;
using System.Text;
using Planning.da;
using System.Linq;
using Combinatorics.Collections;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;

namespace Planning
{
    public class WinterTennis
    {

        Dictionary<int, Turn> turns = new Dictionary<int, Turn>();
        Dictionary<string, Participant> participants = new Dictionary<string, Participant>();
        Dictionary<(int, string), Unavailable> unavailabilities = new Dictionary<(int, string), Unavailable>();
        int participantsperturn = 8;
        int partnersperparticipant = 2;

        public WinterTennis(int ParticipantsPerTurn)
        {
            this.participantsperturn = ParticipantsPerTurn;
            

            #region Initialise Participants
            participants.Add("Gert Van Breda", new Participant() { Name = "Gert Van Breda" });
            participants.Add("Tom De Jong" , new Participant() { Name = "Tom De Jong" });
            participants.Add("Gert Van Herwegen", new Participant() { Name = "Gert Van Herwegen" });
            participants.Add("Rene Rombouts" , new Participant() { Name = "Rene Rombouts" });
            participants.Add("Danny Stouthuysen", new Participant() { Name = "Danny Stouthuysen" });
            participants.Add("Jef Lienard" , new Participant() { Name = "Jef Lienard" });
            participants.Add("Tom Engelen" , new Participant() { Name = "Tom Engelen" });
            participants.Add("Tim Lembrechts", new Participant() { Name = "Tim Lembrechts" });
            participants.Add("Eric Van den Eynde", new Participant() { Name = "Eric Van den Eynde" });
            participants.Add("Geert Claes", new Participant() { Name = "Geert Claes" });
            participants.Add("Dave Onincx", new Participant() { Name = "Dave Onincx" });
            participants.Add("Paul thienpont" , new Participant() { Name = "Paul thienpont" });
            participants.Add("Pat Pompier" , new Participant() { Name = "Pat Pompier" });
            participants.Add("Christophe Pedron" , new Participant() { Name = "Christophe Pedron" });
            participants.Add("Greg Lienard", new Participant() { Name = "Greg Lienard" });
            participants.Add("Peter Vergauwen", new Participant() { Name = "Peter Vergauwen" });



            #endregion

            #region Initialise Turns
            turns.Add(0, new Turn() { Dte = new DateTime(2020, 09, 29), ID = 0 });
            turns.Add(1, new Turn() { Dte = new DateTime(2020, 10, 6), ID = 1 });
            turns.Add(2, new Turn() { Dte = new DateTime(2020, 10, 13), ID = 2 });
            turns.Add(3, new Turn() { Dte = new DateTime(2020, 10, 20), ID = 3 });
            turns.Add(4, new Turn() { Dte = new DateTime(2020, 10, 27), ID = 4 });
            turns.Add(5, new Turn() { Dte = new DateTime(2020, 11, 10), ID = 5 });
            turns.Add(6, new Turn() { Dte = new DateTime(2020, 11, 17), ID = 6 });
            turns.Add(7, new Turn() { Dte = new DateTime(2020, 11, 24), ID = 7 });
            turns.Add(8, new Turn() { Dte = new DateTime(2020, 12, 01), ID = 8 });
            turns.Add(9, new Turn() { Dte = new DateTime(2020, 12, 08), ID = 9 });
            turns.Add(10, new Turn() { Dte = new DateTime(2020, 12, 15), ID = 10 });
            turns.Add(11, new Turn() { Dte = new DateTime(2020, 12, 22), ID = 11 });
            #endregion

            #region Set properties of participants: are all equal and 8 participants are required each turn.
            foreach (var p in participants)
            {
                p.Value.IdealTurns = turns.Count * ParticipantsPerTurn * 1.0 / participants.Count;
                p.Value.IdealEveryXTurns = turns.Count*1.0/ p.Value.IdealTurns;
            }
            #endregion

            #region Initialise unavailabilities: All participants are available each turn unless they are in the unavailability list.
            unavailabilities.Add((0, "Gert Van Breda"), new Unavailable()
            {
                Who = participants["Gert Van Breda"],
                When = turns[0]
            });

            unavailabilities.Add((1, "Tom Engelen"), new Unavailable()
            {
                Who = participants["Tom Engelen"],
                When = turns[1]
            });
            unavailabilities.Add((2, "Tom Engelen"),new Unavailable()
            {
                Who = participants["Tom Engelen"],
                When = turns[2]
            });
            unavailabilities.Add((3, "Tom Engelen"), new Unavailable()
            {
                Who = participants["Tom Engelen"],
                When = turns[3]
            }); unavailabilities.Add((4, "Tom Engelen"), new Unavailable()
            {
                Who = participants["Tom Engelen"],
                When = turns[4]
            });


            unavailabilities.Add((0, "Geert Claes"), new Unavailable()
            {
                Who = participants["Geert Claes"],
                When = turns[0]
            });
            unavailabilities.Add((1, "Geert Claes"), new Unavailable()
            {
                Who = participants["Geert Claes"],
                When = turns[1]
            });

            unavailabilities.Add((2, "Geert Claes"), new Unavailable()
            {
                Who = participants["Geert Claes"],
                When = turns[2]
            });

            unavailabilities.Add((3, "Geert Claes"), new Unavailable()
            {
                Who = participants["Geert Claes"],
                When = turns[3]
            });

            unavailabilities.Add((1, "Greg Lienard"), new Unavailable()
            {
                Who = participants["Greg Lienard"],
                When = turns[1]
            });

            unavailabilities.Add((3, "Greg Lienard"), new Unavailable()
            {
                Who = participants["Greg Lienard"],
                When = turns[3]
            });
            unavailabilities.Add((6, "Greg Lienard"), new Unavailable()
            {
                Who = participants["Greg Lienard"],
                When = turns[6]
            });
            unavailabilities.Add((8, "Greg Lienard"), new Unavailable()
            {
                Who = participants["Greg Lienard"],
                When = turns[8]
            });
            unavailabilities.Add((10, "Greg Lienard"), new Unavailable()
            {
                Who = participants["Greg Lienard"],
                When = turns[10]
            });
            #endregion
        }
        double bestscore = double.MaxValue;
        double weightidealturns = 0.4;
        double weightidealturnsinbetween = 0.2;
        double weightdifferentpartners = 0.6;
        ConcurrentBag<IList<IList<string>>> BestCombos = new ConcurrentBag<IList<IList<string>>>();

        public void Calculate ()
        {
            var allcombinationsperturn = new Combinations<string>(participants.Keys.ToList(), participantsperturn, GenerateOption.WithoutRepetition).ToList();
            long done = 0;

            List<IList<string>>[] combinationsperturn = new List<IList<string>>[turns.Count];
            //long maxcombinations = 1;
            #region All valid combinations per turn.
            for (int turn = 0; turn < turns.Count; turn++)
            {
                combinationsperturn[turn] = new List<IList<string>>();

                foreach (var combination in allcombinationsperturn)
                {
                    var isvalid = true;

                    foreach (var participant in combination)
                    {
                        if (unavailabilities.ContainsKey((turn, participant)))
                        {
                            isvalid = false;
                            break;
                        }
                    }
                    if (isvalid)
                        combinationsperturn[turn].Add(combination);
                }
                //maxcombinations = maxcombinations * combinationsperturn[turn].Count;
            }
            #endregion
            DateTime dtConsoleTitle = DateTime.Now;
            DateTime dtStart = DateTime.Now;

            #region Initiaze counters on 0
            List<int> counter = new List<int>(turns.Count);
            for (int i = 0; i < turns.Count; i++)
                counter.Add(0);
            #endregion

            var currentcombo = new List<IList<string>>(turns.Count);
            bool valid = false;
            bool next = false;
            bool alldone = false;
            while(!alldone)
            {
                next = false;
                done++;
                try
                {
                    if (DateTime.Now.Subtract(dtConsoleTitle).TotalSeconds > 1)
                    {
                        dtConsoleTitle = DateTime.Now;
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < turns.Count; i++)
                            sb.Append($"|{i}:{counter[i]}");
                        Console.Title = $"{done} ({Math.Round(counter[0]*100.0/combinationsperturn[0].Count,2)}%) done in {Math.Round(DateTime.Now.Subtract(dtStart).TotalHours, 2)} hours | {Math.Round(done / DateTime.Now.Subtract(dtStart).TotalHours, 0)} = {Math.Round((done) / DateTime.Now.Subtract(dtStart).TotalHours, 0)} per hour {sb.ToString()}";
                    }
                }
                catch
                {
                    Console.WriteLine($"Console.title error");
                }

                currentcombo.Clear();
                currentcombo.Add(combinationsperturn[0][counter[0]]);
                //currentcombo.Add(combinationsperturn[1][counter[1]]);
                //valid = isValidCombo(currentcombo);
                //if (!valid)
                //{
                //    counter[1]++;
                //    if (counter[1]>=combinationsperturn[1].Count)
                //    {
                //        counter[0]++;
                //        if (counter[0] >= combinationsperturn[0].Count)
                //            break;
                //        counter[1] = 0;
                //    }
                //    continue;
                //}
                //currentcombo.Add(combinationsperturn[2][counter[2]]);
                //valid = isValidCombo(currentcombo);
                //if (!valid)
                //{
                //    counter[2]++;
                //    if (counter[2] >= combinationsperturn[2].Count)
                //    {
                //        counter[2] = 0;
                //        counter[1]++;
                //        if (counter[1] >= combinationsperturn[1].Count)
                //        {
                //            counter[0]++;
                //            if (counter[0] >= combinationsperturn[0].Count)
                //                break;
                //            counter[1] = 0;
                //        }
                //    }
                //    continue;
                //}
                for (int turn = 1; turn < turns.Count; turn++)
                {
                    currentcombo.Add(combinationsperturn[turn][counter[turn]]);
                    valid = isValidCombo(currentcombo);
                    if (!valid)
                    {
                        for (int goback = turn; goback >= 0; goback--)
                        {
                            counter[goback]++;
                            if (counter[goback] >= combinationsperturn[turn].Count)
                            {
                                counter[goback] = 0;
                                if (goback == 0)
                                    alldone = true;
                            }
                            else
                            {
                                next = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (!next)
                    Calculate(currentcombo, done);
                       
              


            }




        }

        private bool isValidCombo (IList<IList<string>> combination)
        {
            #region Check if combination is valid
            Dictionary<string, Participant> perParticipant = new Dictionary<string, Participant>();

            for (int turn = 0; turn < combination.Count; turn++)
            {
                for (int iparticipant = 0; iparticipant < combination[turn].Count; iparticipant++)
                {
                    var participant = combination[turn][iparticipant];

                    #region Add partners
                    var partners = new List<string>();

                    {
                        for (int i = 0; i < partnersperparticipant - 1 && i < iparticipant % partnersperparticipant; i++)
                        {
                            partners.Add(combination[turn][iparticipant - i - 1]);
                        }
                        for (int i = iparticipant % partnersperparticipant; i < partnersperparticipant - 1; i++)
                        {
                            partners.Add(combination[turn][iparticipant + i + 1]);
                        }

                    }


                    #endregion
                    if (perParticipant.ContainsKey(participant))
                    {
                        perParticipant[participant].PlannedTurns.Add(turn);
                        perParticipant[participant].Partners.AddRange(partners);
                    }
                    else
                        perParticipant.Add(participant, new Participant() { IdealEveryXTurns=participants[participant].IdealEveryXTurns, IdealTurns=participants[participant].IdealTurns, Name=participant,  Partners = partners, PlannedTurns = new List<int>() { turn } });


                }
            }

            #endregion
            foreach (var participant in perParticipant)
                {
                    #region Check ideal turns
                    if (participant.Value.PlannedTurns.Count > Math.Ceiling(participant.Value.IdealTurns))
                        return false;
                    #endregion

                    #region Check differentiation of partners. 
                    var partners = participant.Value.Partners.GroupBy(x => x).Select(g => new { g.Key, SamePartner = g.Count() });

                    var samepartners = partners.Any(x => x.SamePartner > Math.Ceiling(participant.Value.IdealTurns / (participants.Count - 1)));
                    if (samepartners)
                        return false;

                    #endregion
                }
                return true;
 
        }

        public void CalculateAll ()
        {


            


            var allcombinationsperturn = new Combinations<string>(participants.Keys.ToList(), participantsperturn, GenerateOption.WithoutRepetition).ToList();
            var allcombinations = new Combinations<IList<string>>(allcombinationsperturn, turns.Count, GenerateOption.WithoutRepetition);
            long done = 0;
            long totaltodo = allcombinations.Count;
            DateTime dtConsoleTitle = DateTime.Now;
            DateTime dtStart = DateTime.Now;

            //foreach (var combination in allcombinations)
            Parallel.ForEach(allcombinations, (combination) =>
            {
                done++;
                try
                {
                    if (DateTime.Now.Subtract(dtConsoleTitle).TotalSeconds > 1)
                    {
                        dtConsoleTitle = DateTime.Now;
                        Console.Title = $"{done}/{totaltodo} ({Math.Round(done*100.0/totaltodo,10)}%) done in {Math.Round(DateTime.Now.Subtract(dtStart).TotalHours, 2)} hours | {Math.Round(done / DateTime.Now.Subtract(dtStart).TotalHours, 0)} = {Math.Round((done) / DateTime.Now.Subtract(dtStart).TotalHours, 0)} per hour";
                    }
                    Calculate(combination, done);
                }
                catch
                {
                    Console.WriteLine($"Console.title error");
                }

   


            });








        }

        public void CalculateRandom()
        {
            var allcombinationsperturn = new Combinations<string>(participants.Keys.ToList(), participantsperturn, GenerateOption.WithoutRepetition).ToList();
            long done = 0;

            List<IList<string>>[] combinationsperturn = new List<IList<string>>[turns.Count];
            long maxcombinations = 1;
            #region All valid combinations per turn.
            for (int turn = 0; turn < turns.Count; turn++)
            {
                combinationsperturn[turn] = new List<IList<string>>();

                foreach (var combination in allcombinationsperturn)
                {
                    var isvalid = true;

                    foreach (var participant in combination)
                    {
                        if (unavailabilities.ContainsKey((turn, participant)))
                        {
                            isvalid = false;
                            break;
                        }
                    }
                    if (isvalid)
                        combinationsperturn[turn].Add(combination);
                }
                maxcombinations = maxcombinations * combinationsperturn[turn].Count;
            }
            #endregion

            Random rnd = new Random();
            DateTime dtConsoleTitle = DateTime.Now;
            DateTime dtStart = DateTime.Now;


            //while (true)
            Parallel.For(0, maxcombinations, x =>
             {
                 done++;
                 try
                 {
                     if (DateTime.Now.Subtract(dtConsoleTitle).TotalSeconds > 1)
                     {
                         dtConsoleTitle = DateTime.Now;
                         Console.Title = $"{done}/{maxcombinations} ({Math.Round(done * 100.0 / maxcombinations, 10)}%) done in {Math.Round(DateTime.Now.Subtract(dtStart).TotalHours, 2)} hours | {Math.Round(done / DateTime.Now.Subtract(dtStart).TotalHours, 0)} = {Math.Round((done) / DateTime.Now.Subtract(dtStart).TotalHours, 0)} per hour";
                     }
                 }
                 catch
                 {
                     Console.WriteLine($"Console.title error");
                 }

                 var combination = new List<IList<string>>();
                 int indexperturn;

                 for (int i = 0; i < turns.Count; i++)
                 {
                     indexperturn = rnd.Next(0, combinationsperturn[i].Count);
                     combination.Add(combinationsperturn[i][indexperturn]);
                 }
                 Calculate(combination, done);
             }
            );

        }

        private void Calculate(IList<IList<string>> combination, long done)
        {
            #region Check if combination is valid
            Dictionary<string, Participant> perParticipant = new Dictionary<string, Participant>();

            for (int turn = 0; turn < combination.Count; turn++)
            {
                for (int iparticipant = 0; iparticipant < combination[turn].Count; iparticipant++)
                //foreach (var participant in combination[turn])
                {
                    var participant = combination[turn][iparticipant];

                    #region Add partners
                    var partners = new List<string>();
                    //if (iparticipant% partnersperparticipant == 0)
                    //{
                    //    for (int i=1;i<partnersperparticipant;i++)
                    //        partners.Add(combination[turn][iparticipant + i]);
                    //}
                    //else
                    {
                        for (int i = 0; i < partnersperparticipant - 1 && i < iparticipant % partnersperparticipant; i++)
                        {
                            partners.Add(combination[turn][iparticipant - i - 1]);
                        }
                        for (int i = iparticipant % partnersperparticipant; i < partnersperparticipant - 1; i++)
                        {
                            partners.Add(combination[turn][iparticipant + i + 1]);
                        }

                    }


                    #endregion
                    if (unavailabilities.ContainsKey((turn, participant)))
                    {
                        return;
                    }
                    if (perParticipant.ContainsKey(participant))
                    {
                        perParticipant[participant].PlannedTurns.Add(turn);
                        perParticipant[participant].Partners.AddRange(partners);
                    }
                    else
                        perParticipant.Add(participant, new Participant() { Partners = partners, PlannedTurns = new List<int>() { turn } });


                }
            }
            #endregion

            #region Check ideal turns and turns inbetween.
            bool worseAsBest = false;
            #region All participants must be at least once in the combination
            if (perParticipant.Count != participants.Count)
                return;
            #endregion

            double scoreidealturns = 0;
            double scoreidealturnsinbetween = 0;
            double scoredifferentpartners = 0;
            foreach (var participant in participants)
            {

                #region Check ideal turns
                var participantturns = perParticipant[participant.Key].PlannedTurns;
                double pidealturns = 0;
                pidealturns += Math.Abs(participant.Value.IdealTurns - participantturns.Count);
                if (pidealturns > 0)
                    return;
                #endregion

                #region Check turns inbetween.
                double pidealturnsinbetween = 0;


                int previousturn = -1;
                for (int i = 0; i < participantturns.Count; i++)
                {

                    #region if the difference between the first time is less than the EveryXtimes, it's as good as possible.
                    if (i == 0 && participantturns[i] < participant.Value.IdealEveryXTurns)
                        pidealturnsinbetween += 0;
                    #endregion
                    #region calculate difference betwee 

                    #endregion
                    else

                    {
                        var delta = Math.Abs(participantturns[i] - previousturn - participant.Value.IdealEveryXTurns);
                        pidealturnsinbetween += delta;
                    }
                    #region if it's the last turn for this participant, take between last turn and the last possible turn.
                    if (i == participantturns.Count - 1)
                    {
                        var next = Math.Abs(turns.Count - participantturns[i] - participant.Value.IdealEveryXTurns);
                        if (turns.Count - participantturns[i] < participant.Value.IdealEveryXTurns)
                            next = 0;
                        pidealturnsinbetween += next;
                    }
                    #endregion
                    previousturn = participantturns[i];
                }
                #endregion

                #region Check differentiation of partners. TODO: What if the amount of turns > amount of possible partners???
                var allpartners = perParticipant[participant.Key].Partners;
                var partners = allpartners.GroupBy(x => x).Select(g => new { g.Key, SamePartner = g.Count() });
                var samepartners = partners.Where(x => x.SamePartner > 1).Select(x => x.SamePartner - 1).Sum();
                if (samepartners > 0)
                    return;

                #endregion

                //Console.WriteLine($"{done} - {participant}: {pidealturns} ({Math.Round((participant.Value.IdealTurns - pidealturns) * 100 / participant.Value.IdealTurns, 0)}%) T, {pidealturnsinbetween} ({Math.Round((turns.Count - pidealturnsinbetween) * 100 / turns.Count, 0)}%) XT, {samepartners}P ({Math.Round(samepartners * 100.0 / (participantturns.Count - 1), 0)}% ");
                if (pidealturnsinbetween > turns.Count)
                {
                    Console.ReadKey();
                }
                scoreidealturns += (pidealturns/participant.Value.IdealTurns) * weightidealturns;
                scoreidealturnsinbetween += (pidealturnsinbetween / turns.Count) * weightidealturnsinbetween ;
                scoredifferentpartners += samepartners*1.0/(participantturns.Count-1) * weightdifferentpartners;

                if (scoreidealturns + scoreidealturnsinbetween + scoredifferentpartners > bestscore)
                {
                    worseAsBest = true;
                    return;
                }
            }

            if (scoreidealturns + scoreidealturnsinbetween + scoredifferentpartners < bestscore)
            {
                BestCombos.Clear();
                BestCombos.Add(combination);
                bestscore = scoreidealturns + scoreidealturnsinbetween;

                Console.WriteLine($"new best found: {bestscore}");
                for (int i = 0; i < combination.Count; i++)
                {
                    Console.Write($"{i}: ");
                    foreach (var participant in combination[i])
                    {
                        Console.Write(participant + ", ");
                    }
                    Console.Write("\n");
                }
            }
            else if (scoreidealturns + scoreidealturnsinbetween == bestscore)
                BestCombos.Add(combination);


            #endregion
        }



    }
}
