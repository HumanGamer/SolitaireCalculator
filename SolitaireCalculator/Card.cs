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

		public override string ToString()
		{
			return Value.ToString() + " of " + Suite.ToString();
		}
	}
}
