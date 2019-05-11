using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Class that represents Card instance in match and in general as data holder
/// </summary>
namespace Assets.Prefabs.DataSaving
{
    public class CardEntity
    {
        public int Id; // ID in database
        public string Type;
        public string Name;
        public bool IsComboCard; // is it possible to get this card by combo
        public int NForCombo; //number of Cards that are needed if this card could be made from combo
        public List<int> ComboCards; //IDs of such Cards
        public string CardImage; //filename for client
        public string ItemImage; // filename for client 
        public int Value; // Value of card. Damage, heal, bonus
        public string Animation; // Name of the animation for client
        public int Direction; //0-self, 1 - enemy

        public CardEntity Clone()
        {
            var clone = new CardEntity()
            {
                Id = this.Id,
                Type = this.Type,
                Name = this.Name,
                IsComboCard = this.IsComboCard,
                NForCombo = this.NForCombo,
                ComboCards = new List<int>(this.ComboCards),
                CardImage = this.CardImage,
                ItemImage = this.ItemImage,
                Value = this.Value,
                Animation = this.Animation,
                Direction = this.Direction
            };
            return clone;
        }
    }
}
