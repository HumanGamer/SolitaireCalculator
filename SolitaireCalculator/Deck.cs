using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Deck
	{
		protected Card[] Cards;

		public bool InitialDeck
		{
			get;
			protected set;
		}

		public Deck()
		{
			Cards = new Card[52];
			InitDeck();
		}

		public void InitDeck()
		{
			// Init standard deck
			for (int i = 0; i < Cards.Length; i++)
				Cards[i] = new Card((Suite)(i / 13), (CardValue)(i + 1));
			InitialDeck = true;
		}

		public Card this[int index]
		{
			get
			{
				return Cards[index];
			}
		}

		public void Shuffle()
		{
			Random rand = new Random();
			List<Card> originalCards = Cards.ToList();
			List<Card> newCards = new List<Card>();
			while (originalCards.Count > 0)
			{
				int r = rand.Next(0, originalCards.Count - 1);
				newCards.Add(originalCards[r]);
				originalCards.RemoveAt(r);
			}

			Cards = newCards.ToArray();

			InitialDeck = false;
		}

		public Deck Clone()
		{
			Deck result = new Deck();
			for (int i = 0; i < Cards.Length; i++)
			{
				result.Cards[i] = this.Cards[i];
			}

			return result;
		}
	}
}
