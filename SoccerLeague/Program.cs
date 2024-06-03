using Microsoft.EntityFrameworkCore;
using SoccerLeague.Data;
using SoccerLeague.Models;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Xml.Linq;

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
                context.SaveChanges();
            }
            
        }

        static void SimulateLeague(SoccerLeagueContext context)
        {
            var teams = context.Teams.ToList();
            int numberOfRounds = teams.Count - 1;

            for (int round = 1; round <= numberOfRounds; round++)
            {
                Console.WriteLine($"Round {round}");
                foreach (var match in GenerateMatches(round, teams))
                {
                    context.Matches.Add(match);
                    Console.WriteLine($"{match.HomeTeam.Name} {match.HomeGoals} - {match.AwayGoals} {match.AwayTeam.Name}");
                }
                context.SaveChanges();
                PrintStandings(context);
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

        static List<SoccerLeague.Models.Match> GenerateMatches(int round, List<Team> teams)
        {
            var matches = new List<SoccerLeague.Models.Match>();
            int n = teams.Count;
            for (int i = 0; i < n / 2; i++)
            {
                int homeIndex = (round + i) % (n - 1);
                int awayIndex = (n - 1 - i + round) % (n - 1);

                if (i == 0)
                    awayIndex = n - 1;

                matches.Add(GenerateMatch(teams[homeIndex], teams[awayIndex]));
            }
            return matches;
        }

        static void PrintStandings(SoccerLeagueContext context)
        {
            var standings = context.Teams
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();

            Console.WriteLine("Standings:");
            foreach (var team in standings)
            {
                Console.WriteLine($"{team.Name}: {team.Points} points, {team.GoalsFor} goals for, {team.GoalsAgainst} goals against");
            }
        }
        static void EditTeamManually(SoccerLeagueContext context)
        {
            /*Console.Write("Enter team ID to edit: ");
            int teamId = int.Parse(Console.ReadLine());*/
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
            
            for (int i = 1; i < 10; i++)
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
    }
}
