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

		protected List<Move> Moves;
		protected List<Move> LastMoves;

		public Solitaire(Deck deck = null)
		{
			if (deck == null)
			{
				Deck = new Deck();
				Deck.Shuffle();
			}
			else
				Deck = deck;

			Moves = new List<Move>();
			LastMoves = new List<Move>();
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
		}

		public void MoveSubRow(int row, int column, int newRow)
		{
			Pile r = Rows[row];
			List<Card> cards = new List<Card>();
			for (int i = column; i < r.Count; i++)
				cards.Add(r[i].Card);

			Rows[row].RemoveUntilEnd(column);

			if (FindTopCard(row) == null && Rows[row].Count > 0)
				Rows[row][Rows[row].Count - 1].Hidden = false;

			Rows[newRow].AddRange(cards);
		}

		public void TestMoveCardFromPile(int index)
		{
			Card c = CardPile[index];

			if (FindWinningPile(c))
				Moves.Add(new Move(MoveType.CardPileToWinPile, c, -1, index));

			int row = FindValidRow(c);
			if (row == -1)
				return;

			Moves.Add(new Move(MoveType.CardPileToRow, c, -1, index, FindTopCard(row), row, false));
		}

		public bool HasWon()
		{
			if (CardPile.Count > 0)
				return false;

			foreach (Pile row in Rows)
			{
				if (row.Count > 0)
					return false;
			}

			return true;
		}

		public void BuildPossibleMoveList()
		{
			Moves.Clear();

			// Find a valid row->row move
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

					bool multipleCards = j < Rows[i].Count - 1;

					Moves.Add(new Move(MoveType.RowToRow, cardref.Card, i, j, FindTopCard(row), row, multipleCards));
				}
			}

			// Check card pile if no more standard moves
			for (i = 0; i < CardPile.Count; i += PickAmount)
				TestMoveCardFromPile(i);

			if (i > CardPile.Count)
			{
				i -= PickAmount;
				for (; i < CardPile.Count; i++)
					TestMoveCardFromPile(i);
			}

			// Put cards in win pile if no other moves
			for (i = 0; i < Rows.Length; i++)
			{
				Card c = FindTopCard(i);
				if (c == null)
					continue;

				if (FindWinningPile(c))
					Moves.Add(new Move(MoveType.RowToWinPile, c, i, Rows[i].Count - 1));
			}
		}

		public bool ProcessNextMove()
		{
			BuildPossibleMoveList();
			if (Moves.Count == 0)
				return true;

			Move selectedMove = Moves[0];
			Move originalSelectedMove = selectedMove;
			LastMoves.Add(selectedMove);

			//if (LastMoves.Count > 20)
			//	LastMoves.RemoveAt(0);

			int index = 1;
			while (LastMoves.Contains(selectedMove))
			{
				if (index >= Moves.Count)
					return false;
				//Console.WriteLine("Detected infinite loop of moves, trying another option");
				selectedMove = Moves[index++];

				//Console.WriteLine("Press any key to continue...");
				//Console.ReadKey();
			}

			if (selectedMove != originalSelectedMove)
			{
				LastMoves.Add(selectedMove);

				if (LastMoves.Count > 4)
					LastMoves.RemoveAt(0);
			}

			//Console.WriteLine(GetLayoutAsString());
			/*Console.WriteLine("Selected Move: " + selectedMove);*/

			//Console.WriteLine("Press any key to continue...");
			//Console.ReadKey();

			switch (selectedMove.Type)
			{
				case MoveType.RowToRow:
					MoveSubRow(selectedMove.SourceRow, selectedMove.SourceColumn, selectedMove.TargetRow);
					break;
				case MoveType.RowToWinPile:
					PutWinningCard(selectedMove.SourceRow, selectedMove.MovingCard);
					break;
				case MoveType.CardPileToRow:
					CardPile.RemoveAt(selectedMove.SourceColumn);
					Rows[selectedMove.TargetRow].Add(selectedMove.MovingCard, false);
					break;
				case MoveType.CardPileToWinPile:
					CardPile.RemoveAt(selectedMove.SourceColumn);
					WinningPiles[selectedMove.MovingCard.Suite].Add(selectedMove.MovingCard);
					break;
				case MoveType.WinPileToRow:
					WinningPiles[selectedMove.MovingCard.Suite].RemoveAt(selectedMove.SourceColumn);
					Rows[selectedMove.TargetRow].Add(selectedMove.MovingCard, false);
					break;
			}

			return true;
		}

		public bool ProcessMoves()
		{
			while (HasMoreMoves())
			{
				if (!ProcessNextMove())
					break;
			}

			return HasWon();
		}

		public bool HasMoreMoves()
		{
			BuildPossibleMoveList();
			return Moves.Count > 0;
		}

		public bool CanWin()
		{
			return ProcessMoves();
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
