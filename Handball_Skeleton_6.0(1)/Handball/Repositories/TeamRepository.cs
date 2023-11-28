using Handball.Models;
using Handball.Models.Contracts;
using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Repositories
{
    internal class TeamRepository : IRepository<ITeam>
    {
        private List<ITeam> teams;
        public TeamRepository()
        {
            teams = new List<ITeam>();
        }
        public IReadOnlyCollection<ITeam> Models => teams;


        public void AddModel(ITeam model)
        {
            teams.Add(model);
        }

        public bool ExistsModel(string name) => teams.Any(x => x.Name == name);


        public ITeam GetModel(string name) => teams.FirstOrDefault(x => x.Name == name);

        public bool RemoveModel(string name) => teams.Remove(teams.FirstOrDefault(x => x.Name == name));
    }
}
