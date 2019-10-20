using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolitaireCalculator
{
	public class Program
	{
		private static void DisplayWinningGame()
		{
			Console.WriteLine("----------------------------");
			Console.WriteLine("Deck Layout  (Back to Front)");
			Console.WriteLine("----------------------------");

			Deck deck = Solitaire.GetWinningGame();

			foreach (Card c in deck)
				Console.WriteLine(" - " + c.ToString());
		}

		public static void Main(string[] args)
		{
			Console.WriteLine("============================");
			Console.WriteLine("=== Solitaire Calculator ===");
			Console.WriteLine("============================");

			int numGames = 1000;
			string outputFile = "solitaire.csv";

			Console.WriteLine("Generating " + numGames + " winning games of solitaire > " + outputFile);

			using (Stream s = File.Open(outputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
			using (StreamWriter sw = new StreamWriter(s))
			{
				for (int i = 0; i < numGames; i++)
				{
					Deck deck = Solitaire.GetWinningGame();
					for (int j = 0; j < deck.Count; j++)
					{
						Card c = deck[j];

						sw.Write(c.CodeName);
						//sw.Write(c);

						if (j < deck.Count - 1)
							sw.Write(",");
					}
					sw.WriteLine();
					Thread.Sleep(1);
				}
			}
			
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}
