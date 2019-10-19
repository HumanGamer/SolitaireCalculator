using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Deck : IEnumerable
	{
		protected Card[] Cards;

		public int Count => Cards.Length;

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
				Cards[i] = new Card((Suite)(i / 13), (CardValue)((i % 13) + 1));
			InitialDeck = true;
		}

		public Card this[int index] => Cards[index];

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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Cards.GetEnumerator();
		}
	}
}
