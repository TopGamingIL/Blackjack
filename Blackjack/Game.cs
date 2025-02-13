using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Game
    {
        // Properties

        // Pot: Amount of chips in pot
        public int Pot { get; set; }

        // Player: Player object
        public Player Player { get; set; }

        // Dealer: Dealer object
        public Player Dealer { get; set; }

        // Deck: Deck object
        public Deck Deck { get; set; }

        public Game() {
            // Initialize game with a shuffled deck, player, dealer, and 0 pot
            Random random = new Random();
            Deck = new Deck(random.Next(1, 9));
            Deck.Shuffle();
            Player = new Player();
            Dealer = new Player();
            Pot = 0;
        }

        public void Play()
        {
            // Start game
            Player.PlaceBet();
            Console.WriteLine($"Balance: {Player.Balance + Player.Bet} - {Player.Bet} = {Player.Balance}");
            Player.Deal(Deck);
            Dealer.Deal(Deck);
            Player.Deal(Deck);
            Dealer.Deal(Deck);

            // Player's turn
            while (Player.HandValue() < 21)
            {
                // Print player's hand and dealer's hand
                Console.WriteLine("Player Hand:");
                Console.WriteLine("[" + Player.HandValue() + "]");
                Player.PrintHand();
                Console.WriteLine("Dealer Hand:");
                Dealer.Hand[0].Print();
                Console.WriteLine(", *");

                // Prompt player to hit or stand
                int option = Player.Option(Deck);
                if (option == 2) break;
            }

            // Check if player busts
            if (Player.HandValue() > 21)
            {
                // Player busts
                Console.WriteLine("Player Hand:");
                Console.WriteLine("[" + Player.HandValue() + "]");
                Player.PrintHand();
                Console.WriteLine("Dealer Hand:");
                Dealer.PrintHand();

                Console.WriteLine("Player busts.");
                Console.WriteLine("Dealer wins.");
                return;
            }

            // Dealer's turn
            while (Dealer.HandValue() < 17)
            {
                // Dealer hits
                Dealer.Deal(Deck);

                Console.WriteLine("Player Hand:");
                Console.WriteLine("[" + Player.HandValue() + "]");
                Player.PrintHand();
                Console.WriteLine("Dealer Hand:");
                Console.WriteLine("[" + Dealer.HandValue() + "]");
                Dealer.PrintHand();
            }

            // Check if dealer busts
            if (Dealer.HandValue() > 21)
            {
                // Dealer busts
                Console.WriteLine("Dealer busts.");
                Console.WriteLine("Player wins.");
                Player.Win();
                Console.WriteLine($"[+{2 * Player.Bet}]");
                return;
            }

            // Print player's hand and dealer's hand
            Console.WriteLine("Player Hand:");
            Console.WriteLine("[" + Player.HandValue() + "]");
            Player.PrintHand();
            Console.WriteLine("Dealer Hand:");
            Console.WriteLine("[" + Dealer.HandValue() + "]");
            Dealer.PrintHand();
            // Check who wins
            if (Player.HandValue() > Dealer.HandValue())
            {
                Console.WriteLine("Player wins.");
                Player.Win();
                Console.WriteLine($"[+{2 * Player.Bet}]");
            }
            else if (Player.HandValue() < Dealer.HandValue())
            {
                Console.WriteLine("Dealer wins.");
            }
            // If there is a tie, return bet to player
            else
            {
                Console.WriteLine("It's a tie.");
                Player.Tie();
                Console.WriteLine($"[+{Player.Bet}]");
            }


        }
    }
}
