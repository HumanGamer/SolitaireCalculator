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

		public string CodeName
		{
			get
			{
				string suite = "";
				switch (Suite)
				{
					case Suite.Diamonds:
						suite = "D";
						break;
					case Suite.Clubs:
						suite = "C";
						break;
					case Suite.Hearts:
						suite = "H";
						break;
					case Suite.Spades:
						suite = "S";
						break;
				}

				string value = "";
				switch(Value)
				{
					case CardValue.Ace:
						value = "A ";
						break;
					case (CardValue)10:
						value = "10";
						break;
					case CardValue.Jack:
						value = "J ";
						break;
					case CardValue.Queen:
						value = "Q ";
						break;
					case CardValue.King:
						value = "K ";
						break;
					default:
						value = ((int)Value).ToString() + " ";
						break;
				}

				return value + suite;
			}
		}

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
