using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Move
	{
		public MoveType Type;
		public Card MovingCard;
		public Card TargetCard;
		public int SourceRow;
		public int SourceColumn;
		public int TargetRow;
		public bool MultipleCards;

		public Move(MoveType type, Card movingCard)
		{
			Type = type;
			MovingCard = movingCard;
			TargetCard = null;
			SourceRow = -1;
			SourceColumn = -1;
			TargetRow = -1;
			MultipleCards = false;
		}

		public Move(MoveType type, Card movingCard, int sourceRow, int sourceColumn) : this(type, movingCard)
		{
			SourceRow = sourceRow;
			SourceColumn = sourceColumn;
		}

		public Move(MoveType type, Card movingCard, int sourceRow, int sourceColumn, Card targetCard, int targetRow, bool multipleCards) : this(type, movingCard, sourceRow, sourceColumn)
		{
			TargetCard = targetCard;
			TargetRow = targetRow;
			MultipleCards = multipleCards;
		}

		public Move(MoveType type, Card movingCard, Card targetCard, int targetRow) : this(type, movingCard)
		{
			TargetCard = targetCard;
			TargetRow = targetRow;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Move))
				return false;

			Move move = (Move)obj;

			return this.Type == move.Type &&
				   ((this.MovingCard == null && move.MovingCard == null) || (this.MovingCard != null && move.MovingCard != null && this.MovingCard.Equals(move.MovingCard))) &&
				   ((this.TargetCard == null && move.TargetCard == null) || (this.TargetCard != null && move.TargetCard != null && this.TargetCard.Equals(move.TargetCard))) &&
				   this.SourceRow == move.SourceRow &&
				   this.TargetRow == move.TargetRow;
		}

		public bool Equals(Move other)
		{
			return Type == other.Type && Equals(MovingCard, other.MovingCard) && Equals(TargetCard, other.TargetCard) && SourceRow == other.SourceRow && TargetRow == other.TargetRow;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (int) Type;
				hashCode = (hashCode * 397) ^ (MovingCard != null ? MovingCard.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (TargetCard != null ? TargetCard.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ SourceRow;
				hashCode = (hashCode * 397) ^ TargetRow;
				return hashCode;
			}
		}

		public override string ToString()
		{
			string targetCard = TargetCard == null ? "(Empty)" : TargetCard.ToString();
			switch (Type)
			{
				case MoveType.RowToRow:
					return "Move " + MovingCard + " from row " + (SourceRow + 1) + " to row " + (TargetRow + 1) + " on top of " + targetCard;
				case MoveType.RowToWinPile:
					return "Move " + MovingCard + " from row " + (SourceRow + 1) + " to winning pile";
				case MoveType.CardPileToRow:
					return "Move " + MovingCard + " from card pile to row " + (TargetRow + 1) + " on top of " + targetCard;
				case MoveType.CardPileToWinPile:
					return "Move " + MovingCard + " from card pile to winning pile";
				case MoveType.WinPileToRow:
					return "Move " + MovingCard + " from winning pile to row " + (TargetRow + 1) + " on top of " + targetCard;
				default:
					return "Invalid Move Type: " + Type;
			}
		}
	}
}
