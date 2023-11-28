using Handball.Core.Contracts;
using Handball.Models;
using Handball.Models.Contracts;
using Handball.Repositories;
using Handball.Repositories.Contracts;
using Handball.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Handball.Core
{
    public class Controller : IController
    {
        private IRepository<IPlayer> players;
        private IRepository<ITeam> teams;
        public Controller()
        {
            this.players = new PlayerRepository();
            this.teams = new TeamRepository();
        }
        public string LeagueStandings()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("***League Standings***");

            foreach (var team in teams.Models.OrderByDescending(x => x.PointsEarned)
                .ThenByDescending(x => x.OverallRating)
                .ThenBy(x=> x.Name))
            {
                sb.AppendLine(team.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        public string NewContract(string playerName, string teamName)
        {
            IPlayer player = null;
            ITeam team = null;
            if (!players.ExistsModel(playerName))
            {
                return string.Format(OutputMessages.PlayerNotExisting, playerName, nameof(PlayerRepository));
            }
            if (!teams.ExistsModel(teamName))
            {
                return string.Format(OutputMessages.TeamNotExisting, teamName, nameof(TeamRepository));
            }
            else
            {
                team = teams.GetModel(teamName);
            }
            if (players.ExistsModel(playerName))
            {
                player = players.GetModel(playerName);
                if (player.Team != null)
                {
                    return string.Format(OutputMessages.PlayerAlreadySignedContract, playerName, player.Team);
                }
            }
            player.JoinTeam(teamName);
            team.SignContract(player);
            return string.Format(OutputMessages.SignContract, playerName, teamName);
        }

        public string NewGame(string firstTeamName, string secondTeamName)
        {
            ITeam team1 = teams.GetModel(firstTeamName);
            ITeam team2 = teams.GetModel(secondTeamName);
            if (team1.OverallRating > team2.OverallRating)
            {
                team1.Win();
                team2.Lose();
                return string.Format(OutputMessages.GameHasWinner, team1.Name, team2.Name);
            }
            else if (team1.OverallRating < team2.OverallRating)
            {
                team2.Win();
                team1.Lose();
                return string.Format(OutputMessages.GameHasWinner, team2.Name, team1.Name);
            }
            else
            {
                team1.Draw();
                team2.Draw();
                return string.Format(OutputMessages.GameIsDraw, team1.Name, team2.Name);
            }
        }

        public string NewPlayer(string typeName, string name)
        {
            if (typeName!= nameof(Goalkeeper) && typeName!= nameof(CenterBack) && typeName != nameof(ForwardWing))
            {
                return string.Format(OutputMessages.InvalidTypeOfPosition, typeName);
            }
            if (players.ExistsModel(name))
            {
                string position = this.players.GetModel(name).GetType().Name;
                return string.Format(OutputMessages.PlayerIsAlreadyAdded, name, nameof(PlayerRepository), position);
            }
            IPlayer player;
            if (typeName == nameof(Goalkeeper))
            {
                player = new Goalkeeper(name);
            }
            else if (typeName == nameof(CenterBack))
            {
                player = new CenterBack(name);
            }
            else
            {
                player = new ForwardWing(name);
            }

            players.AddModel(player);
            return string.Format(OutputMessages.PlayerAddedSuccessfully, name);
        }

        public string NewTeam(string name)
        {
           if(this.teams.ExistsModel(name))
           {
               return string.Format(OutputMessages.TeamAlreadyExists, name, nameof(TeamRepository));
           }
           this.teams.AddModel(new Team(name));
           return string.Format(OutputMessages.TeamSuccessfullyAdded, name, nameof(TeamRepository));
        }

        public string PlayerStatistics(string teamName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"***{teamName}***");

            ITeam team = this.teams.GetModel(teamName);

            foreach (IPlayer player in team.Players.OrderByDescending(x=>x.Rating).ThenBy(x=>x.Name))
            {
                sb.AppendLine(player.ToString());
            }
            

            return sb.ToString().TrimEnd();
        }
    }
}
