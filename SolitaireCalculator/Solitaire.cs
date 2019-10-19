using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Solitaire
	{
		protected Deck Deck;

		protected Pile[] Rows;
		protected List<Card> CardPile;
		protected int PickAmount;

		public Solitaire(Deck deck = null)
		{
			if (deck == null)
			{
				Deck = new Deck();
				Deck.Shuffle();
			}
			else
				Deck = deck;
		}

		public void Setup(int rows = 7, int pick = 1)
		{
			Rows = new Pile[rows];
			CardPile = new List<Card>();
			PickAmount = pick;

			// Initialize Rows
			for (int i = 0; i < Rows.Length; i++)
				Rows[i] = new Pile();

			// Place Cards in Rows
			int index = 0;
			for (int i = 0; i < rows; i++)
			{
				Rows[i].Add(Deck[index++], false);
				for (int j = 0; j < rows - (i + 1); j++)
				{
					Rows[i + j + 1].Add(Deck[index++], true);
				}
			}

			// Card Pile
			for (; index < Deck.Count; index++)
			{
				CardPile.Add(Deck[index]);
			}
		}

		public string GetLayoutAsString()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Rows.Length; i++)
			{
				sb.AppendLine("Row[" + (i + 1) + "]");
				Pile row = Rows[i];
				for (int j = 0; j < row.Count; j++)
				{
					sb.AppendLine("    - Card[" + (j + 1) + "]: " + row[j]);
				}
			}

			sb.AppendLine("Card Pile");
			for (int i = 0; i < CardPile.Count; i++)
			{
				sb.AppendLine("    - Card[" + (i + 1) + "]: " + CardPile[i]);
			}

			return sb.ToString();
		}
	}
}
