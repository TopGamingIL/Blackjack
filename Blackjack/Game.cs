using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        // Methods

        public void Start(int bet)
        {

            // Clear player and dealer hands
            Player.Hand.Clear();
            Dealer.Hand.Clear();

            // Deal cards to player and dealer
            Player.Deal(Deck);
            Dealer.Deal(Deck);
            Player.Deal(Deck);
            Dealer.Deal(Deck);

            Play(bet);
        }

        public void Play(int bet)
        {
            // Start game
            Console.Clear();
            Console.WriteLine($"Balance: {Player.Balance + bet} - {bet} = {Player.Balance}");

            // Player's turn
            while (Player.HandValue() < 21)
            {
                // Print player's hand and dealer's hand
                Console.WriteLine("----------\nPlayer Hand:");
                Console.WriteLine("[" + Player.HandValue() + "]");
                Player.PrintHand();
                Console.WriteLine("Dealer Hand:");
                Dealer.Hand[0].Print();
                Console.WriteLine(", *\n----------");

                // Prompt player to hit or stand
                int option = Player.Option(Deck, this);
                Console.Clear();
                if (option == 2) break;
            }

            // Check if player busts
            if (Player.HandValue() > 21)
            {
                // Player busts
                Console.Clear();
                Console.WriteLine("----------\nPlayer Hand:");
                Console.WriteLine("[" + Player.HandValue() + "]");
                Player.PrintHand();
                Console.WriteLine("Dealer Hand:");
                Console.WriteLine("[" + Dealer.HandValue() + "]");
                Dealer.PrintHand();
                Console.WriteLine("----------");

                Console.WriteLine("Player busts.");
                Console.WriteLine("Dealer wins.");
                Console.WriteLine($"Balance: {Player.Balance}");
                return;
            }

            // Dealer's turn
            while (Dealer.HandValue() < 17)
            {
                Console.Clear();
                Console.WriteLine("----------\nPlayer Hand:");
                Console.WriteLine("[" + Player.HandValue() + "]");
                Player.PrintHand();
                Console.WriteLine("Dealer Hand:");
                Console.WriteLine("[" + Dealer.HandValue() + "]");
                Dealer.PrintHand();
                Console.WriteLine("Taking card...\n----------");

                // Delay for 2 second
                Thread.Sleep(2000);

                // Dealer hits
                Dealer.Deal(Deck);
            }

            // Check if dealer busts
            if (Dealer.HandValue() > 21)
            {
                Console.Clear();
                Console.WriteLine("----------");
                Console.WriteLine("Player Hand:");
                Console.WriteLine("[" + Player.HandValue() + "]");
                Player.PrintHand();
                Console.WriteLine("Dealer Hand:");
                Console.WriteLine("[" + Dealer.HandValue() + "]");
                Dealer.PrintHand();
                Console.WriteLine("----------");
                Console.WriteLine("Dealer busts.");
                Console.WriteLine("Player wins.");
                Player.Win();
                Console.WriteLine($"[+{2 * bet}]\nBalance: {Player.Balance}");
                Thread.Sleep(2000);
                return;
            }

            // Print player's hand and dealer's hand
            Console.Clear();
            Console.WriteLine("----------");
            Console.WriteLine("Player Hand:");
            Console.WriteLine("[" + Player.HandValue() + "]");
            Player.PrintHand();
            Console.WriteLine("Dealer Hand:");
            Console.WriteLine("[" + Dealer.HandValue() + "]");
            Dealer.PrintHand();
            Console.WriteLine("----------");
            // Check who wins
            if (Player.HandValue() > Dealer.HandValue())
            {
                Console.WriteLine("Player wins.");
                Player.Win();
                Console.WriteLine($"[+{2 * bet}]\nBalance: {Player.Balance}");
                Thread.Sleep(2000);
            }
            else if (Player.HandValue() < Dealer.HandValue())
            {
                Console.WriteLine("Dealer wins.");
                Console.WriteLine($"Balance: {Player.Balance}");
                Thread.Sleep(2000);
            }
            // If there is a tie, return bet to player
            else
            {
                Console.WriteLine("It's a tie.");
                Player.Tie();
                Console.WriteLine($"[+{bet}]\nBalance: {Player.Balance}");
                Thread.Sleep(2000);
            }


        }


    }
}
