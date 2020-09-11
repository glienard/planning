﻿using System;
using System.Collections.Generic;
using System.Text;
using Planning.da;
using System.Linq;
using Combinatorics.Collections;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Planning
{
    public class WinterTennis
    {

        Dictionary<int, Turn> turns = new Dictionary<int, Turn>();
        Dictionary<string, Participant> participants = new Dictionary<string, Participant>();
        Dictionary<(int, string), Unavailable> unavailabilities = new Dictionary<(int, string), Unavailable>();
        int participantsperturn = 8;

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

        public void Calculate ()
        {

            double weightidealturns = 0.4;
            double weightidealturnsinbetween = 0.2;
            double weightdifferentteams = 0.4;
            double bestscore = double.MaxValue;

            ConcurrentBag<IList<IList<string>>> BestCombos = new ConcurrentBag<IList<IList<string>>>();

            var allcombinationsperturn = new Combinations<string>(participants.Keys.ToList(), participantsperturn, GenerateOption.WithoutRepetition).ToList();
            var allcombinations = new Combinations<IList<string>>(allcombinationsperturn, turns.Count, GenerateOption.WithoutRepetition);
            long done = 0;
            long totaltodo = allcombinations.Count;
            DateTime dtConsoleTitle = DateTime.Now;
            DateTime dtStart = DateTime.Now;

            //            foreach (var combination in allcombinations)
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
                }
                catch
                {
                    Console.WriteLine($"Console.title error");
                }

                Dictionary<string, List<int>> perParticipant = new Dictionary<string, List<int>>(turns.Count);
                bool allParticipantsAreAvailableInThisCombination = true;
                #region Check if combination is valid
                for (int turn = 0; turn < combination.Count; turn++)
                {
                    foreach (var participant in combination[turn])
                    {
                        if (unavailabilities.ContainsKey((turn, participant)))
                        {
                            allParticipantsAreAvailableInThisCombination = false;
                            break;
                        }
                        if (perParticipant.ContainsKey(participant))
                            perParticipant[participant].Add(turn);
                        else
                            perParticipant.Add(participant, new List<int> { turn });

                    }
                    if (!allParticipantsAreAvailableInThisCombination)
                    {
                        break;
                    }
                }
                if (!allParticipantsAreAvailableInThisCombination)
                    return;
                #endregion

                #region Check ideal turns and turns inbetween.
                bool worseAsBest = false;
                if (allParticipantsAreAvailableInThisCombination)
                {
                    #region All participants must be at least once in the combination
                    if (perParticipant.Count != participants.Count)
                        return;
                    #endregion

                    double scoreidealturns = 0;
                    double scoreidealturnsinbetween = 0;
                    double scoredifferentteams = 0;
                    foreach (var participant in participants)
                    {
                        var participantturns = perParticipant[participant.Key];
                        double pidealturns = 0;
                        double pidealturnsinbetween = 0;

                        pidealturns += Math.Abs(participant.Value.IdealTurns - participantturns.Count);
                        int previousturn = -1;
                        for (int i = 0; i < participantturns.Count; i++)
                        {
                            #region if it's the last turn for this participant, take the worst between the previous one and the last turn.
                            if (i == participantturns.Count - 1)
                            {

                                var previous = Math.Abs(participantturns[i] - previousturn - participant.Value.IdealEveryXTurns);

                                var next = Math.Abs(participantturns.Count - 1 - i - participant.Value.IdealEveryXTurns);
                                if (participantturns[i] < participant.Value.IdealEveryXTurns)
                                    next = 0;

                                pidealturnsinbetween += next > previous ? next : previous;
                            }
                            #endregion
                            #region if the difference between the first time is less than the EveryXtimes, it's as good as possible.
                            else if (i == 0 && participantturns[i] < participant.Value.IdealEveryXTurns)
                                pidealturnsinbetween += 0;
                            #endregion
                            else

                            {
                                var delta = Math.Abs(participantturns[i] - previousturn - participant.Value.IdealEveryXTurns);
                                pidealturnsinbetween += delta;
                            }
                            previousturn = participantturns[i];
                        }

                        Console.WriteLine($"{done} - {participant}: {pidealturns} ({(participant.Value.IdealTurns - pidealturns) * 100 / participant.Value.IdealTurns}%) T, {pidealturnsinbetween} ({(turns.Count - pidealturnsinbetween) * 100 / turns.Count}%) XT ");
                        if (pidealturnsinbetween > turns.Count)
                            Console.ReadKey();
                        scoreidealturns += (participant.Value.IdealTurns - pidealturns) * weightidealturns / participant.Value.IdealTurns;
                        scoreidealturnsinbetween += (turns.Count - pidealturnsinbetween) * weightidealturnsinbetween / turns.Count;

                        if (scoreidealturns + scoreidealturnsinbetween > bestscore)
                        {
                            worseAsBest = true;
                            break;
                        }
                    }
                    if (worseAsBest)
                        return;

                    if (scoreidealturns + scoreidealturnsinbetween < bestscore)
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

                }
                #endregion
            });








        }
    }
}