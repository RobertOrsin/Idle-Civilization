using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idle_Civilization.Classes
{
    class Player
    {
        public Ressources ressources; //food,wood,ore the player has
        public Ressources ressource_demand; //

        public int cityCount = 0;

        public Player(Ressources _ressources)
        {
            ressources = _ressources;
        }

        public bool CanAfford(Ressources price)
        {
            if (ressources.food >= price.food &&
               ressources.wood >= price.wood &&
               ressources.ore >= price.ore &&
               ressources.army >= price.army)
                return true;
            else
                return false;
        }
    }
}
