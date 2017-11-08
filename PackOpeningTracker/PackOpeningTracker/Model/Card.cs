using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackOpeningTracker
{
    public class Card
    {
        private string Name;
        private string Rarity;
        private string Class;
        private string Set;
        private string Premium;
        private int count;

        public Card()
        {
            Name = string.Empty;
            Rarity = string.Empty;
            Class = string.Empty;
            Set = string.Empty;
            Premium = null;
            count = 0;
        }

        public Card(string Name_, string Rarity_, string Class_, string Set_)
        {
            Name = Name_;
            Rarity = Rarity_;
            Class = Class_;
            Set = Set_;
            Premium = null;
            count = 0;
        }

        public Card(string Name_, string Rarity_, string Class_, string Set_, string Premium_)
        {
            Name = Name_;
            Rarity = Rarity_;
            Class = Class_;
            Set = Set_;
            Premium = Premium_;
            count = 1;
        }

        public Card(string Name_, string Rarity_, string Class_, string Set_, string Premium_,int count_)
        {
            Name = Name_;
            Rarity = Rarity_;
            Class = Class_;
            Set = Set_;
            Premium = Premium_;
            count = count_;
        }

        public string cardName
        {
            get 
            { 
                return Name;
            }
            set
            {
                Name = value;
            }
        }

        public string cardRarity
        {
            get
            {
                return Rarity;
            }
            set
            {
                Rarity = value;
            }
        }

        public string cardClass
        {
            get
            {
                return Class;
            }
            set
            {
                Class = value;
            }
        }

        public string cardSet
        {
            get
            {
                return Set;
            }
            set
            {
                Set = value;
            }
        }

        public string cardPremium
        {
            get
            {
                return Premium;
            }
            set
            {
                Premium = value;
            }
        }

        public int cardCount
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        public Card Clone()
        {
            return new Card(Name, Rarity, Class, Set, Premium, count);
        }
    }
}
