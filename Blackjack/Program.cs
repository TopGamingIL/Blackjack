using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            Player player = new Player();
            //Deck deck = new Deck();
            //deck.DeckOf(10);
            //game.Deck = deck;
            while (player.Balance > 0)
            {
                Console.WriteLine("[1] Play | [2] Balance | [3] Quit");
                Console.Write("Choose an option: ");
                int input = Convert.ToInt32(Console.ReadLine());
                if (input == 1)
                {
                    Console.Clear();
                    player.PlaceBet();
                    int bet = player.Bet;
                    game.Player = player;
                    game.Start();
                    Console.Clear();
                }
                else if (input == 2)
                {
                    Console.Clear();
                    player.PrintBalance();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (input == 3)
                {
                    break;
                }
            }
            if (player.Balance == 0)
            {
                Console.WriteLine("You're out of chips.");
            }
        }

    }
}
