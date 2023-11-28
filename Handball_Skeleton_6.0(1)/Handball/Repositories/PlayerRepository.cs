using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handball.Repositories;
using Handball.Models.Contracts;
using Handball.Models;
using System.Reflection.Metadata.Ecma335;

namespace Handball.Repositories
{
    public class PlayerRepository : IRepository<IPlayer>
    {
        private List<IPlayer> players;

        public PlayerRepository()
        {
            players = new List<IPlayer>();
        }
        public IReadOnlyCollection<IPlayer> Models => players;

        public void AddModel(IPlayer model)
        {
            players.Add(model);
        }

        public bool ExistsModel(string name) => players.Any(x => x.Name == name);
        

        public IPlayer GetModel(string name) => players.FirstOrDefault(x => x.Name == name);
        

        public bool RemoveModel(string name) => players.Remove(players.FirstOrDefault(x => x.Name == name));
        
    }


}
