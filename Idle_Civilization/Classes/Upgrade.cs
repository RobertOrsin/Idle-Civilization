using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idle_Civilization.Classes
{
    class Upgrade
    {
        public string name, tooltip;
        public Ressources cost;
        public Ressources modifier;

        #region construktor
        public Upgrade()
        {
            name = "no name";
            tooltip = "no tooltip";

            cost = new Ressources();
            modifier = new Ressources();
        }
        public Upgrade(string _name, string _tooltip, Ressources _cost, Ressources _modifier)
        {
            name = _name;
            tooltip = _tooltip;
            cost = _cost;
            modifier = _modifier;
        }
        public Upgrade(string _name, string _tooltip, string _cost, string _modifier)
        {
            name = _name;
            tooltip = _tooltip;
            cost = new Ressources(_cost);
            modifier = new Ressources(_modifier);
        }
        #endregion
    }
}
