using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Pile : IEnumerable
	{
		public class CardReference
		{
			public readonly Card Card;
			public bool Hidden;

			public CardReference(Card card, bool hidden)
			{
				Card = card;
				Hidden = hidden;
			}

			public override string ToString()
			{
				return Card.ToString() + " : " + (Hidden ? "Hidden" : "Visible");
			}
		}

		protected List<CardReference> Cards;

		public int Count => Cards.Count;

		public Pile()
		{
			Cards = new List<CardReference>();
		}

		public CardReference this[int index] => Cards[index];

		public void Add(Card card, bool hidden)
		{
			Cards.Add(new CardReference(card, hidden));
		}

		public void AddRange(List<Card> cards, bool hidden = false)
		{
			foreach (Card c in cards)
				Cards.Add(new CardReference(c, hidden));
		}

		public void RemoveAt(int index)
		{
			Cards.RemoveAt(index);
		}

		public void RemoveUntilEnd(int index)
		{
			Cards.RemoveRange(index, Cards.Count - index);
		}

		public IEnumerator GetEnumerator()
		{
			return Cards.GetEnumerator();
		}
	}
}
