using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Card
	{
		public readonly Suite Suite;
		public readonly CardValue Value;

		public Card(Suite suite, CardValue value)
		{
			Suite = suite;
			Value = value;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Card))
				return false;

			Card c = (Card) obj;

			return this.Suite == c.Suite && this.Value == c.Value;
		}

		protected bool Equals(Card other)
		{
			return Suite == other.Suite && Value == other.Value;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((int) Suite * 397) ^ (int) Value;
			}
		}

		public override string ToString()
		{
			return Value.ToString() + " of " + Suite.ToString();
		}
	}
}
