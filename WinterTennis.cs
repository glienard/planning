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
        Dictionary<byte, Participant> participants = new Dictionary<byte, Participant>();
        Dictionary<(int, byte), Unavailable> unavailabilities = new Dictionary<(int, byte), Unavailable>();
        int participantsperturn = 8;
        int partnersperparticipant = 2;

        public WinterTennis(int ParticipantsPerTurn)
        {
            this.participantsperturn = ParticipantsPerTurn;
            

            #region Initialise Participants
            participants.Add(0, new Participant() { Name = "Gert Van Breda" });
            participants.Add(1 , new Participant() { Name = "Tom De Jong" });
            participants.Add(2, new Participant() { Name = "Gert Van Herwegen" });
            participants.Add(3, new Participant() { Name = "Rene Rombouts" });
            participants.Add(4, new Participant() { Name = "Danny Stouthuysen" });
            participants.Add(5 , new Participant() { Name = "Jef Lienard" });
            participants.Add(6 , new Participant() { Name = "Tom Engelen" });
            participants.Add(7, new Participant() { Name = "Tim Lembrechts" });
            participants.Add(8, new Participant() { Name = "Eric Van den Eynde" });
            participants.Add(9, new Participant() { Name = "Geert Claes" });
            participants.Add(10, new Participant() { Name = "Dave Onincx" });
            participants.Add(11 , new Participant() { Name = "Paul thienpont" });
            participants.Add(12 , new Participant() { Name = "Pat Pompier" });
            participants.Add(13 , new Participant() { Name = "Christophe Pedron" });
            participants.Add(14, new Participant() { Name = "Greg Lienard" });
            participants.Add(15, new Participant() { Name = "Peter Vergauwen" });



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
            unavailabilities.Add((0, 0), new Unavailable()
            {
                Who = participants[0],
                When = turns[0]
            });

            unavailabilities.Add((6, 5), new Unavailable()
            {
                Who = participants[5],
                When = turns[6]
            });
            unavailabilities.Add((7, 5), new Unavailable()
            {
                Who = participants[5],
                When = turns[7]
            });

            unavailabilities.Add((1, 6), new Unavailable()
            {
                Who = participants[6],
                When = turns[1]
            });
            unavailabilities.Add((2, 6),new Unavailable()
            {
                Who = participants[6],
                When = turns[2]
            });
            unavailabilities.Add((3, 6), new Unavailable()
            {
                Who = participants[6],
                When = turns[3]
            }); unavailabilities.Add((4, 6), new Unavailable()
            {
                Who = participants[6],
                When = turns[4]
            });



            unavailabilities.Add((0, 9), new Unavailable()
            {
                Who = participants[9],
                When = turns[0]
            });
            unavailabilities.Add((1, 9), new Unavailable()
            {
                Who = participants[9],
                When = turns[1]
            });

            unavailabilities.Add((2, 9), new Unavailable()
            {
                Who = participants[9],
                When = turns[2]
            });

            unavailabilities.Add((3, 9), new Unavailable()
            {
                Who = participants[9],
                When = turns[3]
            });

            unavailabilities.Add((1, 14), new Unavailable()
            {
                Who = participants[14],
                When = turns[1]
            });

            unavailabilities.Add((3, 14), new Unavailable()
            {
                Who = participants[14],
                When = turns[3]
            });
            unavailabilities.Add((6, 14), new Unavailable()
            {
                Who = participants[14],
                When = turns[6]
            });
            unavailabilities.Add((8, 14), new Unavailable()
            {
                Who = participants[14],
                When = turns[8]
            });
            unavailabilities.Add((10, 14), new Unavailable()
            {
                Who = participants[14],
                When = turns[10]
            });



            unavailabilities.Add((9, 8), new Unavailable()
            {
                Who = participants[8],
                When = turns[9]
            });
            unavailabilities.Add((10, 8), new Unavailable()
            {
                Who = participants[8],
                When = turns[10]
            });

            unavailabilities.Add((11, 8), new Unavailable()
            {
                Who = participants[8],
                When = turns[11]
            });

            #endregion
        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }
        

        public double factorial_Recursion(int number)
        {
            if (number == 1)
                return 1;
            else
                return number * factorial_Recursion(number - 1);
        }


        double bestscore = double.MaxValue;
        int BestTurns = 0;
        double weightidealturns = 0.4;
        double weightidealturnsinbetween = 0.2;
        double weightdifferentpartners = 0.6;
        ConcurrentBag<IList<IList<byte>>> BestCombos = new ConcurrentBag<IList<IList<byte>>>();

        public void Calculate ()
        {
            //var allcombinationsperturn = new Combinations<string>(participants.Keys.ToList(), GenerateOption.WithoutRepetition);
            //var allcombinationsperturn = GetPermutations<string>(participants.Keys.ToList(), participantsperturn);
            //            var allcombinationsperturncount = factorial_Recursion(participants.Count()) / (factorial_Recursion(participants.Count() - participantsperturn));
            var allcombinationsperturn = new Variations<byte>(participants.Keys.ToList(),participantsperturn, GenerateOption.WithoutRepetition);
            var allcombinationsperturncount = allcombinationsperturn.Count;
            long done = 0;

            var combinationsperturn = new List<IList<byte>>((int)allcombinationsperturncount);
            //List<IList<byte>>[] combinationsperturn = new List<IList<byte>>[turns.Count];
            //long maxcombinations = 1;
            #region All valid combinations per turn.

            //for (int turn = 0; turn < turns.Count; turn++)
            //{
            //    combinationsperturn[turn] = new List<IList<byte>>((int)allcombinationsperturncount);
            //}

            DateTime dtConsoleTitle = DateTime.Now;
            DateTime dtStart = DateTime.Now;
            var LimitedAmountOfAvailabilities = participants.Where(x => x.Value.IdealTurns >= turns.Count - unavailabilities.Keys.Where(y => y.Item2 == x.Key).Count()).Any();

            var combocounter = 0;
//            Parallel.ForEach(allcombinationsperturn, combination =>  // 
            foreach (var combination in allcombinationsperturn.AsParallel())
            {
                combocounter++;
                try
                {
                    if (DateTime.Now.Subtract(dtConsoleTitle).TotalSeconds > 1)
                    {
                        dtConsoleTitle = DateTime.Now;
                        StringBuilder sb = new StringBuilder();
                        Console.Title = $"{combocounter} / {allcombinationsperturncount} ({Math.Round(combocounter * 100.0 / allcombinationsperturncount, 2)}%) done in {Math.Round(DateTime.Now.Subtract(dtStart).TotalHours, 2)} hours | {Math.Round(done / DateTime.Now.Subtract(dtStart).TotalHours, 0)} = {Math.Round((combocounter) / DateTime.Now.Subtract(dtStart).TotalHours, 0)} per hour {sb.ToString()}";
                    }
                }
                catch
                {
                    Console.WriteLine($"Console.title error");
                }

                //for (int turn = 0; turn < turns.Count; turn++)
                {
                    var isvalid = true;

                    #region If there is any participant who's amount of availabilities is less or equal to the ideal amount, he must be on it.
                    if (LimitedAmountOfAvailabilities)
                    {
                        foreach (var participant in participants.Where(x => x.Value.IdealTurns >= turns.Count - unavailabilities.Keys.Where(y => y.Item2 == x.Key).Count()))
                        {

                            if (!combination.Contains(participant.Key))
                            {
                                isvalid = false;
                                break;
                            }
                        }
                    }

                    #endregion
                    //if (isvalid)
                    //{
                    //    foreach (var participant in combination)
                    //    {
                    //        if (unavailabilities.ContainsKey((turn, participant)))
                    //        {
                    //            isvalid = false;
                    //            break;
                    //        }


                    //    }
                    //}
                    if (isvalid)
                        //combinationsperturn[turn].Add(combination.ToList());
                        combinationsperturn.Add(combination.ToList());
                }
                //maxcombinations = maxcombinations * combinationsperturn[turn].Count;
            }
            //);
            #endregion

            dtStart = DateTime.Now;

            #region Initiaze counters on 0
            List<int> counter = new List<int>(turns.Count);
            for (int i = 0; i < turns.Count; i++)
            {
                if (i % 2 == 0)
                    counter.Add(0);
                else
                    counter.Add(combinationsperturn.Count - 1);

            }
            #endregion

            var currentcombo = new List<IList<byte>>(turns.Count);
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
                        Console.Title = $"{done} ({Math.Round(counter[0]*100.0/combinationsperturn.Count,2)}%) done in {Math.Round(DateTime.Now.Subtract(dtStart).TotalHours, 2)} hours | {Math.Round(done / DateTime.Now.Subtract(dtStart).TotalHours, 0)} = {Math.Round((done) / DateTime.Now.Subtract(dtStart).TotalHours, 0)} per hour {sb.ToString()}";
                    }
                }
                catch
                {
                    Console.WriteLine($"Console.title error");
                }

                currentcombo.Clear();

                for (int turn = 0; turn < turns.Count; turn++)
                {
                    currentcombo.Add(combinationsperturn[counter[turn]]);



                    valid = isValidCombo(currentcombo);
                    if (!valid)
                    {
                        for (int goback = turn; goback >= 0; goback--)
                        {
                            if (goback % 2 == 0)
                            {
                                counter[goback]++;
                                if (counter[goback] >= combinationsperturn.Count)
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
                            else
                            {
                                counter[goback]--;
                                if (counter[goback] < 0)
                                {
                                    counter[goback] = combinationsperturn.Count-1;
                                    if (goback == 0)
                                        alldone = true;
                                }
                                else
                                {
                                    next = true;
                                    break;
                                }

                            }

                        }
                        break;
                    }
                }
                if (!next)
                    CalculateScore(currentcombo);
                       
              


            }




        }
        private bool isValidCombo (IList<IList<byte>> combination)
        {
            #region Check if combination is valid
            Dictionary<byte, Participant> perParticipant = new Dictionary<byte, Participant>();

            for (int turn = 0; turn < combination.Count; turn++)
            {
                for (int iparticipant = 0; iparticipant < combination[turn].Count; iparticipant++)
                {
                    var participant = combination[turn][iparticipant];

                    #region Add partners
                    var partners = new List<byte>();

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
                        return false;
                    }

                    if (perParticipant.ContainsKey(participant))
                    {
                        perParticipant[participant].PlannedTurns.Add(turn);
                        perParticipant[participant].Partners.AddRange(partners);
                    }
                    else
                        perParticipant.Add(participant, new Participant() { IdealEveryXTurns=participants[participant].IdealEveryXTurns, IdealTurns=participants[participant].IdealTurns, Name= participants[participant].Name,  Partners = partners, PlannedTurns = new List<int>() { turn } });


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

            #region If there is any participant who's remaining amount of availabilities is less or equal to the ideal amount, he must be on it.
            foreach (var participant in participants) //.Where(x => x.Value.IdealTurns >= turns.Count - unavailabilities.Keys.Where(y => y.Item2 == x.Key).Count()))
            {
                var plannedturns = 0;
                if (perParticipant.ContainsKey(participant.Key))
                    plannedturns = perParticipant[participant.Key].PlannedTurns.Count;
                var remainingimpossibleturns = unavailabilities.Keys.Where(x => x.Item1 >= combination.Count && x.Item2 == participant.Key).Count();
                if (participant.Value.IdealTurns - plannedturns > turns.Count - combination.Count - remainingimpossibleturns)
                    return false;

            }
            #endregion

            if (combination.Count >= BestTurns)
                CalculateScore(combination);

            return true;
 
        }

        //public void CalculateAll ()
        //{





        //    var allcombinationsperturn = new Combinations<byte>(participants.Keys.ToList(), participantsperturn, GenerateOption.WithoutRepetition).ToList();
        //    var allcombinations = new Combinations<IList<byte>>(allcombinationsperturn, turns.Count, GenerateOption.WithoutRepetition);
        //    long done = 0;
        //    long totaltodo = allcombinations.Count;
        //    DateTime dtConsoleTitle = DateTime.Now;
        //    DateTime dtStart = DateTime.Now;

        //    //foreach (var combination in allcombinations)
        //    Parallel.ForEach(allcombinations, (combination) =>
        //    {
        //        done++;
        //        try
        //        {
        //            if (DateTime.Now.Subtract(dtConsoleTitle).TotalSeconds > 1)
        //            {
        //                dtConsoleTitle = DateTime.Now;
        //                Console.Title = $"{done}/{totaltodo} ({Math.Round(done*100.0/totaltodo,10)}%) done in {Math.Round(DateTime.Now.Subtract(dtStart).TotalHours, 2)} hours | {Math.Round(done / DateTime.Now.Subtract(dtStart).TotalHours, 0)} = {Math.Round((done) / DateTime.Now.Subtract(dtStart).TotalHours, 0)} per hour";
        //            }
        //            Calculate(combination, done);
        //        }
        //        catch
        //        {
        //            Console.WriteLine($"Console.title error");
        //        }




        //    });








        //}

        public void CalculateRandom()
        {
            var allcombinationsperturn = new Variations<byte>(participants.Keys.ToList(), participantsperturn, GenerateOption.WithoutRepetition).ToList();
            long done = 0;

            //var combinationsperturn = new List<IList<byte>>[turns.Count];
            int maxcombinations = (int) allcombinationsperturn.Count;
            //#region All valid combinations per turn.
            //for (int turn = 0; turn < turns.Count; turn++)
            //{
            //    combinationsperturn[turn] = new List<IList<byte>>();

            //    foreach (var combination in allcombinationsperturn)
            //    {
            //        var isvalid = true;

            //        foreach (var participant in combination)
            //        {
            //            if (unavailabilities.ContainsKey((turn, participant)))
            //            {
            //                isvalid = false;
            //                break;
            //            }
            //        }
            //        if (isvalid)
            //            combinationsperturn[turn].Add(combination);
            //    }
            //    maxcombinations = maxcombinations * combinationsperturn[turn].Count;
            //}
            //#endregion

            
            DateTime dtConsoleTitle = DateTime.Now;
            DateTime dtStart = DateTime.Now;
            


            //while (true)
            Parallel.For(0, int.MaxValue, x =>
            //foreach (var x in allcombinationsperturn.AsParallel())
             {
                 Random rnd = new Random(x);
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

                 var combination = new List<IList<byte>>();
                 int indexperturn;

                 for (int i = 0; i < turns.Count; i++)
                 {
                     indexperturn = rnd.Next(0, maxcombinations);
                     combination.Add(allcombinationsperturn[indexperturn]);
                 }
                 CalculateScore(combination);
             }
            );

        }

        //private void Calculate(IList<IList<byte>> combination, long done)
        //{
        //    #region Check if combination is valid
        //    Dictionary<byte, Participant> perParticipant = new Dictionary<byte, Participant>();

        //    for (int turn = 0; turn < combination.Count; turn++)
        //    {
        //        for (int iparticipant = 0; iparticipant < combination[turn].Count; iparticipant++)
        //        //foreach (var participant in combination[turn])
        //        {
        //            var participant = combination[turn][iparticipant];

        //            #region Add partners
        //            var partners = new List<byte>();
        //            {
        //                for (int i = 0; i < partnersperparticipant - 1 && i < iparticipant % partnersperparticipant; i++)
        //                {
        //                    partners.Add(combination[turn][iparticipant - i - 1]);
        //                }
        //                for (int i = iparticipant % partnersperparticipant; i < partnersperparticipant - 1; i++)
        //                {
        //                    partners.Add(combination[turn][iparticipant + i + 1]);
        //                }

        //            }


        //            #endregion
        //            if (unavailabilities.ContainsKey((turn, participant)))
        //            {
        //                return;
        //            }
        //            if (perParticipant.ContainsKey(participant))
        //            {
        //                perParticipant[participant].PlannedTurns.Add(turn);
        //                perParticipant[participant].Partners.AddRange(partners);
        //            }
        //            else
        //                perParticipant.Add(participant, new Participant() { Partners = partners, Name=participants[participant].Name, PlannedTurns = new List<int>() { turn } });


        //        }
        //    }
        //    #endregion

        //    #region Check ideal turns and turns inbetween.
        //    bool worseAsBest = false;
        //    #region All participants must be at least once in the combination
        //    if (perParticipant.Count != participants.Count)
        //        return;
        //    #endregion

        //    double scoreidealturns = 0;
        //    double scoreidealturnsinbetween = 0;
        //    double scoredifferentpartners = 0;
        //    foreach (var participant in participants)
        //    {

        //        #region Check ideal turns
        //        var participantturns = perParticipant[participant.Key].PlannedTurns;
        //        double pidealturns = 0;
        //        pidealturns += Math.Abs(participant.Value.IdealTurns - participantturns.Count);
        //        if (pidealturns > 0)
        //            return;
        //        #endregion

        //        #region Check turns inbetween.
        //        double pidealturnsinbetween = 0;


        //        int previousturn = -1;
        //        for (int i = 0; i < participantturns.Count; i++)
        //        {

        //            #region if the difference between the first time is less than the EveryXtimes, it's as good as possible.
        //            if (i == 0 && participantturns[i] < participant.Value.IdealEveryXTurns)
        //                pidealturnsinbetween += 0;
        //            #endregion
        //            #region calculate difference betwee 

        //            #endregion
        //            else

        //            {
        //                var delta = Math.Abs(participantturns[i] - previousturn - participant.Value.IdealEveryXTurns);
        //                pidealturnsinbetween += delta;
        //            }
        //            #region if it's the last turn for this participant, take between last turn and the last possible turn.
        //            if (i == participantturns.Count - 1)
        //            {
        //                var next = Math.Abs(turns.Count - participantturns[i] - participant.Value.IdealEveryXTurns);
        //                if (turns.Count - participantturns[i] < participant.Value.IdealEveryXTurns)
        //                    next = 0;
        //                pidealturnsinbetween += next;
        //            }
        //            #endregion
        //            previousturn = participantturns[i];
        //        }
        //        #endregion

        //        #region Check differentiation of partners. TODO: What if the amount of turns > amount of possible partners???
        //        var allpartners = perParticipant[participant.Key].Partners;
        //        var partners = allpartners.GroupBy(x => x).Select(g => new { g.Key, SamePartner = g.Count() });
        //        var samepartners = partners.Where(x => x.SamePartner > 1).Select(x => x.SamePartner - 1).Sum();
        //        if (samepartners > 0)
        //            return;

        //        #endregion

                
        //    }

        //    CalculateScore(combination);




        //    #endregion
        //}

        private void CalculateScore(IList<IList<byte>> combination)
        {
            #region Check if combination is valid
            Dictionary<byte, Participant> perParticipant = new Dictionary<byte, Participant>();

            for (int turn = 0; turn < combination.Count; turn++)
            {
                for (int iparticipant = 0; iparticipant < combination[turn].Count; iparticipant++)
                //foreach (var participant in combination[turn])
                {
                    var participant = combination[turn][iparticipant];

                    #region Add partners
                    var partners = new List<byte>();
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

                #endregion

                scoreidealturns += (pidealturns / participant.Value.IdealTurns) * weightidealturns;
                scoreidealturnsinbetween += (pidealturnsinbetween / turns.Count) * weightidealturnsinbetween;
                scoredifferentpartners += samepartners * 1.0 / (participantturns.Count - 1) * weightdifferentpartners;

                if ((scoreidealturns + scoreidealturnsinbetween + scoredifferentpartners)/combination.Count > bestscore)
                {
                    worseAsBest = true;
                    return;
                }
            }

            if ((scoreidealturns + scoreidealturnsinbetween + scoredifferentpartners) / combination.Count < bestscore)
            {
                BestCombos.Clear();
                BestCombos.Add(combination);
                bestscore = (scoreidealturns + scoreidealturnsinbetween + scoredifferentpartners) / combination.Count;
                BestTurns = combination.Count;

                Console.WriteLine($"new best found for {combination.Count} turns: {bestscore} with {scoreidealturns/ weightidealturns} ideal turns difference ");
                for (int i = 0; i < combination.Count; i++)
                {
                    Console.Write($"{i}: ");
                    foreach (var participant in combination[i])
                    {
                        Console.Write($"{participant}, ");
                    }
                    Console.Write("\n");
                }
            }
            else if ((scoreidealturns + scoreidealturnsinbetween + scoredifferentpartners) / combination.Count == bestscore)
                BestCombos.Add(combination);


            #endregion
        }



    }
}
