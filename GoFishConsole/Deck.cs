using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;

namespace GoFish
{
    public class Deck : ObservableCollection<Card>
    {
        private static readonly Random random = Player.Random;
        
        public Deck()
        {
            Reset();
        }

        public void Reset()
        {
            this.Clear();
            for (int i = 0; i <= 3; i++)
            {
                for (int v = 1; v <= 13; v++)
                {                   
                    this.Add(new Card(((Values)v), ((Suits)i)));
                }
            }            
        }

        public Card Deal(int index)
        {
            Card card = base[index];
            this.RemoveAt(index);
            return card;
        }

        /// <summary>
        /// Draw card(s) from Deck
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /*public List<Card> Deal(int count)
        {
            var deal = this.Take(count).Select(card => card).ToList();
            foreach (var card in deal) this.Remove(card);
            return deal;            
        }
        */

        /// <summary>
        /// Copies deck into temp object. Clears all items from Deck.
        /// Take random index number and creates a random card object.
        /// Remove random card (index) from deck copy and adds it to the Deck.
        /// </summary>
        public void Shuffle()
        {
            List<Card> deckCopy = new List<Card>(this);
            this.Clear();
            while (deckCopy.Count > 0)
            {
                int index = random.Next(deckCopy.Count);
                Card randomCard = deckCopy[index];
                deckCopy.RemoveAt(index);
                this.Add(randomCard);
            }
        }

        public void Sort()
        {
            List<Card> sortedCards = new List<Card>(this);
            sortedCards.Sort(new CompareByValue());
            this.Clear();
            foreach (Card card in sortedCards) this.Add(card);
        }
    }
}
