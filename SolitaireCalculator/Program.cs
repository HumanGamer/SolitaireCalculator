using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("============================");
			Console.WriteLine("=== Solitaire Calculator ===");
			Console.WriteLine("============================");
			Console.WriteLine("----------------------------");
			Console.WriteLine("Deck Layout  (Back to Front)");
			Console.WriteLine("----------------------------");

			Deck deck = new Deck();
			bool winnable;
			do
			{
				deck.Shuffle();

				/*Console.WriteLine("============================");
				Console.WriteLine("----------------------------");
				Console.WriteLine("Solitaire Game");
				Console.WriteLine("----------------------------");*/

				int rows = 7;
				int pick = 1;

				Solitaire solitaire = new Solitaire(deck);
				solitaire.Setup(rows, pick);

				//Console.WriteLine(solitaire.GetGameString());

				winnable = solitaire.CanWin();
			} while (!winnable);

			foreach (Card c in deck)
				Console.WriteLine(" - " + c.ToString());

			Console.WriteLine("Winnable: " + (winnable ? "Yes" : "No"));
			
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}
