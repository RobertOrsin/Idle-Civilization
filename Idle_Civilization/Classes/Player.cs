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

        public Player(Ressources _ressources)
        {
            ressources = _ressources;
        }
    }
}
