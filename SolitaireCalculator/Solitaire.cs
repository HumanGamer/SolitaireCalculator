using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Solitaire
	{
		private static bool ShowHidden = true;
		private static bool SameSuite = false;

		protected Deck Deck;

		protected Pile[] Rows;
		protected List<Card> CardPile;
		protected Dictionary<Suite, List<Card>> WinningPiles;
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
			WinningPiles = new Dictionary<Suite, List<Card>>();
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

		public static bool IsValidSuite(Suite suite, Suite parentSuite)
		{
			if (SameSuite)
				return suite == parentSuite;

			// Diamonds
			if (suite == Suite.Clubs && parentSuite == Suite.Diamonds)
				return true;
			if (suite == Suite.Diamonds && parentSuite == Suite.Clubs)
				return true;

			if (suite == Suite.Spades && parentSuite == Suite.Diamonds)
				return true;
			if (suite == Suite.Diamonds && parentSuite == Suite.Spades)
				return true;

			// Hearts
			if (suite == Suite.Clubs && parentSuite == Suite.Hearts)
				return true;
			if (suite == Suite.Hearts && parentSuite == Suite.Clubs)
				return true;

			if (suite == Suite.Spades && parentSuite == Suite.Hearts)
				return true;
			if (suite == Suite.Hearts && parentSuite == Suite.Spades)
				return true;

			return false;
		}

		public static bool IsValidChildCard(Card card, Card parentCard)
		{
			if (parentCard == null)
			{
				if (card.Value == CardValue.King)
					return true;
				return false;
			}

			if (card.Value != parentCard.Value - 1)
				return false;

			return IsValidSuite(card.Suite, parentCard.Suite);
		}

		public Card FindTopCard(int row, bool allowHidden = false)
		{	
			int topIndex = Rows[row].Count - 1;

			if (topIndex < 0)
				return null;

			var c = Rows[row][topIndex];
			if (c.Hidden)
				return null;

			return c.Card;
		}

		public int FindValidRow(Card card, int currentRow = -1)
		{
			// TODO: Return list of all possible rows
			for (int i = 0; i < Rows.Length; i++)
			{
				if (currentRow > -1 && i == currentRow)
					continue;
				Card topCard = FindTopCard(i);

				if (IsValidChildCard(card, topCard))
					return i;
			}

			return -1;
		}

		public bool FindWinningPile(Card card)
		{
			if (!WinningPiles.ContainsKey(card.Suite))
				WinningPiles.Add(card.Suite, new List<Card>());

			List<Card> pile = WinningPiles[card.Suite];
			if ((pile.Count == 0 && card.Value != CardValue.Ace) ||
				(pile.Count > 0 && card.Value != pile[pile.Count - 1].Value + 1))
			{
				return false;
			}

			return true;
		}

		public void PutWinningCard(int row, Card card)
		{
			WinningPiles[card.Suite].Add(card);
			Rows[row].RemoveAt(Rows[row].Count - 1);

			string s = "Moving " + card + " from row " + (row + 1) + " to winning pile";

			Console.WriteLine(GetLayoutAsString());
			Console.WriteLine(s);

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		public void MoveSubRow(int row, int column, int newRow)
		{
			Pile r = Rows[row];
			List<Card> cards = new List<Card>();
			for (int i = column; i < r.Count; i++)
				cards.Add(r[i].Card);

			string s = "Moving " + cards[0] + " from row " + (row + 1) + " to " + FindTopCard(newRow) + " row " + (newRow + 1);

			Rows[row].RemoveUntilEnd(column);

			if (FindTopCard(row) == null && Rows[row].Count > 0)
				Rows[row][Rows[row].Count - 1].Hidden = false;

			Rows[newRow].AddRange(cards);

			Console.WriteLine(GetLayoutAsString());
			Console.WriteLine(s);

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		public bool TestCardFromPile(int index)
		{
			Card c = CardPile[index];

			int row = FindValidRow(c);
			if (row == -1)
			{
				if (FindWinningPile(c))
				{
					CardPile.RemoveAt(index);
					WinningPiles[c.Suite].Add(c);
					Console.WriteLine("Moved " + c + " from card pile to winning pile");
					return true;
				}
				return false;
			}

			Console.WriteLine("Moved " + c + " from card pile to row " + (row + 1) + " on top of " + FindTopCard(row));

			CardPile.RemoveAt(index);
			Rows[row].Add(c, false);

			return true;
		}

		public bool NextMove()
		{
			// TODO: Find all valid moves and pick best option
			// Find a valid move
			int i;
			for (i = 0; i < Rows.Length; i++)
			{
				for (int j = 0; j < Rows[i].Count; j++)
				{
					var cardref = Rows[i][j];
					if (cardref.Hidden)
						continue;

					int row = FindValidRow(cardref.Card, i);
					if (row == -1)
						continue;

					MoveSubRow(i, j, row);
					return true;
				}
			}

			// Check card pile if no more standard moves
			for (i = 0; i < CardPile.Count; i += PickAmount)
			{
				if (TestCardFromPile(i))
					return true;
			}

			if (i > CardPile.Count)
			{
				i -= PickAmount;
				for (; i < CardPile.Count; i++)
				{
					if (TestCardFromPile(i))
						return true;
				}
			}

			// Put cards in win pile if no other moves
			for (i = 0; i < Rows.Length; i++)
			{
				Card c = FindTopCard(i);

				if (FindWinningPile(c))
				{
					PutWinningCard(i, c);
					return true;
				}
			}

			return false;
		}

		public bool ProcessMoves()
		{
			while (true)
			{
				if (!NextMove())
					break;
			}

			return true;
		}

		public bool HasMoreMoves()
		{
			// TODO: Implement
			return true;
		}

		public bool CanWin()
		{
			return ProcessMoves();
			//// TODO: Implement
			//return true;
		}

		public string GetGameString()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Rows.Length; i++)
			{
				Pile row = Rows[i];
				sb.Append("Row " + (i + 1) + " [" + (row.Count - 1) + " Hidden]" + ": ");
				//int visibleCount = 0;
				bool first = true;
				for (int j = 0; j < row.Count; j++)
				{
					if (row[j].Hidden)
						continue;

					if (!first)
						sb.Append(", ");
					first = false;
					sb.Append(/*"[" + (++visibleCount) + "] " + */ row[j].Card.ToString());
				}

				sb.AppendLine();
			}

			sb.AppendLine("Card Pile");
			for (int i = 0; i < CardPile.Count; i++)
			{
				sb.AppendLine("    - Card[" + (i + 1) + "]: " + CardPile[i]);
			}

			return sb.ToString();
		}

		public string GetLayoutAsString()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Rows.Length; i++)
			{
				Pile row = Rows[i];
				sb.Append("Row[" + (i + 1) + "]: ");
				if (ShowHidden)
					sb.AppendLine();
				else
					sb.AppendLine((row.Count - 1) + " Hidden");
				int visibleCount = 0;
				for (int j = 0; j < row.Count; j++)
				{
					if (ShowHidden || !row[j].Hidden)
					{
						sb.Append("    - Card[" + (++visibleCount) + "]: ");
						if (ShowHidden)
							sb.AppendLine(row[j].ToString());
						else
							sb.AppendLine(row[j].Card.ToString());
					}
				}
			}

			sb.AppendLine("Card Pile");
			for (int i = 0; i < CardPile.Count; i++)
			{
				sb.AppendLine("    - Card[" + (i + 1) + "]: " + CardPile[i]);
			}

			sb.AppendLine("Winning Pile");
			bool noCardsInAnyPile = true;
			foreach (var pile in WinningPiles)
			{
				if (pile.Value.Count > 0)
				{
					noCardsInAnyPile = false;
					break;
				}
			}
			if (WinningPiles.Count == 0 || noCardsInAnyPile)
				sb.AppendLine("    - Empty");
			foreach (var pile in WinningPiles)
			{
				if (pile.Value.Count == 0)
					continue;
				sb.AppendLine("    - " + pile.Key + ":");
				for (int i = 0; i < pile.Value.Count; i++)
				{
					sb.AppendLine("          - " + (i + 1) + " " + pile.Value[i]);
				}
			}

			return sb.ToString();
		}
	}
}
