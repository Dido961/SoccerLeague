﻿using Microsoft.EntityFrameworkCore;
using SoccerLeague.Data;
using SoccerLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SoccerLeague
{
    class Program
    {
        static void Main(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<SoccerLeagueContext>();
            optionsBuilder.UseMySQL("Server=localhost;Database=soccerleaguedb;user=root;password=;");
            using (var context = new SoccerLeagueContext(optionsBuilder.Options))
            {
                context.Matches.RemoveRange(context.Matches);
                DelTeamsData(context);
                context.SaveChanges();
                Console.WriteLine("Soccer League Simulator");
                Console.WriteLine("Select fill mode (Manual/Automatic data fill): ");
                Console.Write("Write 'automatic' for auto fill or 'manual' for manual fill: ");
                string mode = Console.ReadLine().ToLower();
                if (mode == "manual")
                {
                    EditTeamManually(context);
                }
                if (mode == "automatic")
                {
                    EditTeamAutomatically(context);
                }
                SimulateLeague(context);
                context.Matches.RemoveRange(context.Matches);
                DelTeamsData(context);
                context.SaveChanges();
            }
        }

        static void SimulateLeague(SoccerLeagueContext context)
        {
            var teams = context.Teams.ToList();
            var matches = GenerateRoundRobin(teams);

            for (int round = 1; round <= matches.Count; round++)
            {
                Console.WriteLine($"Round {round}");
                foreach (var match in matches[round - 1])
                {
                    context.Matches.Add(match);
                    UpdateTeamStats(match, context);
                    Console.WriteLine($"{match.HomeTeam.Name} {match.HomeGoals} - {match.AwayGoals} {match.AwayTeam.Name}");
                }
                context.SaveChanges();
                while (true)
                {
                    Console.Write("Select action (1, 2, 3). To continue forward without any action just hit enter -> ");
                    string act = Console.ReadLine().ToLower();
                    if (act == "1")
                    {
                        Action1(context);
                    }
                    else if (act == "2")
                    {
                        Action2(context);
                    }
                    else if (act == "3")
                    {
                        Action3(context);
                    }
                    else if (act == "") break;

                }
                
            }
        }

        static SoccerLeague.Models.Match GenerateMatch(Team homeTeam, Team awayTeam)
        {
            Random rand = new Random();
            int homeGoals = rand.Next(0, 6);
            int awayGoals = rand.Next(0, 5);

            if (homeTeam.IsGrand && rand.NextDouble() < 0.5)
                homeGoals++;
            if (awayTeam.IsGrand && rand.NextDouble() < 0.5)
                awayGoals++;

            return new SoccerLeague.Models.Match
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeGoals = homeGoals,
                AwayGoals = awayGoals,
                Date = DateTime.Now
            };
        }

        static List<List<SoccerLeague.Models.Match>> GenerateRoundRobin(List<Team> teams)
        {
            if (teams.Count % 2 != 0)
            {
                teams.Add(new Team { Name = "BYE" }); // Add a dummy team if the number of teams is odd
            }

            int numTeams = teams.Count;
            int numRounds = numTeams - 1;
            List<List<SoccerLeague.Models.Match>> rounds = new List<List<SoccerLeague.Models.Match>>();

            for (int round = 0; round < numRounds; round++)
            {
                List<SoccerLeague.Models.Match> roundMatches = new List<SoccerLeague.Models.Match>();
                for (int i = 0; i < numTeams / 2; i++)
                {
                    int home = (round + i) % (numTeams - 1);
                    int away = (numTeams - 1 - i + round) % (numTeams - 1);
                    if (i == 0)
                    {
                        away = numTeams - 1;
                    }

                    if (teams[home].Name != "BYE" && teams[away].Name != "BYE")
                    {
                        roundMatches.Add(GenerateMatch(teams[home], teams[away]));
                    }
                }
                rounds.Add(roundMatches);
            }

            return rounds;
        }

        static void UpdateTeamStats(SoccerLeague.Models.Match match, SoccerLeagueContext context)
        {
            var homeTeam = match.HomeTeam;
            var awayTeam = match.AwayTeam;

            homeTeam.GoalsFor += match.HomeGoals;
            homeTeam.GoalsAgainst += match.AwayGoals;
            awayTeam.GoalsFor += match.AwayGoals;
            awayTeam.GoalsAgainst += match.HomeGoals;

            if (match.HomeGoals > match.AwayGoals)
            {
                homeTeam.Wins++;
                homeTeam.Points += 3;
                awayTeam.Losses++;
            }
            else if (match.HomeGoals < match.AwayGoals)
            {
                awayTeam.Wins++;
                awayTeam.Points += 3;
                homeTeam.Losses++;
            }
            else
            {
                homeTeam.Draws++;
                awayTeam.Draws++;
                homeTeam.Points++;
                awayTeam.Points++;
            }

            context.Teams.Update(homeTeam);
            context.Teams.Update(awayTeam);
        }

        /*static void Action1(SoccerLeagueContext context)
        {
            var standings = context.Teams
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();

            Console.WriteLine("Standings:");
            int place = 1;
            foreach (var team in standings)
            {
                Console.WriteLine($"{team.Name} place: {place++} goals for:{team.GoalsFor} goals against:{team.GoalsAgainst} wins:{team.Wins} draws:{team.Draws} losses:{team.Losses} points:{team.Points}");
            }
        }*/
        static void Action1(SoccerLeagueContext context)
        {
            var standings = context.Teams
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();

            Console.WriteLine("Standings:");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("|    Team Name     |  Points  |  Goals For  |  Goals Against  |  Wins  |  Draws  |  Losses  |");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            foreach (var team in standings)
            {
                Console.WriteLine($"| {team.Name.Replace("Team ", " ").Replace("Manchester City", "Man City").Replace("Manchester United", "Man United"),-16} | {team.Points,8} | {team.GoalsFor,11} | {team.GoalsAgainst,15} | {team.Wins,6} | {team.Draws,7} | {team.Losses,8} |");
            }
            Console.WriteLine("---------------------------------------------------------------------------------------------");
        }



        static void Action2(SoccerLeagueContext context)
        {
            var matchesRes = context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .OrderByDescending(m => m.HomeGoals + m.AwayGoals)
                .ThenByDescending(m => m.Date)
                .Take(5)
                .ToList();

            Console.WriteLine("Most Resultative Matches:");
            foreach (var match in matchesRes)
            {
                Console.WriteLine($"{match.HomeTeam.Name} {match.HomeGoals} - {match.AwayGoals} {match.AwayTeam.Name} | {match.Date}");
            }
        }   
        static void Action3(SoccerLeagueContext context)
        {
            var teams = context.Teams.ToList();
            var matches = context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToList();

            int cellWidth = 11; // Width of each cell

            // Header row
            Console.Write("".PadRight(cellWidth)); // Top-left corner cell
            foreach (var team in teams)
            {
                Console.Write(CenterText(team.Name.Replace("Team ", "").Replace("Manchester United", "MUT").Replace("Manchester City", "MCY").Substring(0, 3), cellWidth));
            }
            Console.WriteLine();

            foreach (var homeTeam in teams)
            {
                // Row header
                Console.Write(CenterText(homeTeam.Name.Replace("Team ", "").Replace("Manchester United", "Man United").Replace("Manchester City", "Man City"), cellWidth));

                foreach (var awayTeam in teams)
                {
                    if (homeTeam.TeamId == awayTeam.TeamId)
                    {
                        Console.Write("".PadRight(cellWidth)); // Skip same team comparison
                        continue;
                    }

                    var match = matches
                        .FirstOrDefault(m => m.HomeTeamId == homeTeam.TeamId && m.AwayTeamId == awayTeam.TeamId);

                    if (match != null)
                    {
                        string result = $"{match.HomeGoals} - {match.AwayGoals}";
                        Console.Write(CenterText(result, cellWidth));
                    }
                    else
                    {
                        Console.Write(CenterText("N/A", cellWidth)); // No match found
                    }
                }
                Console.WriteLine();
            }
        }

        static string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text.Substring(0, width);
            int leftPadding = (width - text.Length) / 2;
            int rightPadding = width - text.Length - leftPadding;
            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }


        static void EditTeamManually(SoccerLeagueContext context)
        {
            for (int i = 1; i < 10; i++)
            {
                var team = context.Teams.Find(i);
                if (team == null)
                {
                    Console.WriteLine("Team not found!");
                    return;
                }
                Console.WriteLine(team.Name);
                Console.Write("Enter Goals For: ");
                team.GoalsFor = int.Parse(Console.ReadLine());

                Console.Write("Enter Goals Against: ");
                team.GoalsAgainst = int.Parse(Console.ReadLine());

                Console.Write("Enter Wins: ");
                team.Wins = int.Parse(Console.ReadLine());

                Console.Write("Enter Draws: ");
                team.Draws = int.Parse(Console.ReadLine());

                Console.Write("Enter Losses: ");
                team.Losses = int.Parse(Console.ReadLine());

                Console.Write("Enter Points: ");
                team.Points = int.Parse(Console.ReadLine());

                context.SaveChanges();
                Console.WriteLine("Team updated successfully!");
            }
        }

        static void EditTeamAutomatically(SoccerLeagueContext context)
        {
            for (int i = 1; i <=10; i++)
            {
                var team = context.Teams.Find(i);
                if (team == null)
                {
                    Console.WriteLine("Team not found!");
                    return;
                }

                Random rand = new Random();
                team.GoalsFor = rand.Next(0, 6);
                team.GoalsAgainst = rand.Next(0, 5);
                team.Wins = team.GoalsFor > team.GoalsAgainst ? 1 : 0;
                team.Draws = team.GoalsFor == team.GoalsAgainst ? 1 : 0;
                team.Losses = team.GoalsFor < team.GoalsAgainst ? 1 : 0;
                team.Points = team.Wins * 3 + team.Draws;

                context.SaveChanges();
                Console.WriteLine("Team updated automatically!");
            }
        }
        static void DelTeamsData(SoccerLeagueContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                var team = context.Teams.Find(i);



                team.GoalsFor = 0;
                team.GoalsAgainst = 0;
                team.Wins = 0;
                team.Draws = 0;
                team.Losses = 0;
                team.Points = 0;

                context.SaveChanges();     
            }
        }
    }
}
