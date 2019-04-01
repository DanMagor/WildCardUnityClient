using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Prefabs.DataSaving
{
    public class CardInstanceSerializable
    {
        public int ID; // ID in database
        public string Type;
        public string Name;
        public bool IsComboCard; // is it possible to get this card by combo
        public int NForCombo; //number of Cards that are needed if this card could be made from combo
        public List<int> ComboCards; //IDs of such Cards
        public string CardImage; //filename for client
        public string ItemImage; // filename for client 
        public int Value; // Value of card. Damage, heal, bonus
        public string Animation; // Name of the animation for client
        public int direction; //0-self, 1 - enemy
    }
}
