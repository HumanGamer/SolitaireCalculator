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
			deck.Shuffle();

			foreach (Card c in deck)
				Console.WriteLine(" - " + c.ToString());

			Console.WriteLine("============================");
			Console.WriteLine("----------------------------");
			Console.WriteLine("Solitaire Game");
			Console.WriteLine("----------------------------");

			int rows = 7;
			int pick = 1;

			Solitaire solitaire = new Solitaire(deck);
			solitaire.Setup(rows, pick);

			Console.WriteLine(solitaire.GetLayoutAsString());

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}
