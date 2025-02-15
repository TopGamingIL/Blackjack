using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Game
    {
        // Properties

        // Player: Player object
        public Player Player { get; set; }

        // Dealer: Dealer object
        public Player Dealer { get; set; }

        // Deck: Deck object
        public Deck Deck { get; set; }

        // Players: List of players
        public List<Player> Players { get; set; }

        public Game() {
            // Initialize game with a shuffled deck, player, dealer, and 0 pot
            Random random = new Random();
            Deck = new Deck(random.Next(1, 9));
            Deck.Shuffle();
            Player = new Player();
            Dealer = new Player();
            Players = new List<Player>
            {
                Player
            };
        }

        // Methods

        public void Start()
        {

            // Clear player and dealer hands
            Player.Hand.Clear();
            Dealer.Hand.Clear();

            // Deal cards to player and dealer
            Player.Deal(Deck);
            Dealer.Deal(Deck);
            Player.Deal(Deck);
            Dealer.Deal(Deck);

            Play(Player.Instance);
            End();
        }

        public void HandStatus(Player player)
        {
            // Print player's hand and dealer's hand
            Console.WriteLine($"----------\nGame {player.Instance}:");
            Console.WriteLine("Player Hand:");
            Console.WriteLine("[" + player.HandValue() + "]");
            player.PrintHand();
            Console.WriteLine("Dealer Hand:");
            Console.WriteLine("[" + Dealer.HandValue() + "]");
            Dealer.PrintHand();
            Console.WriteLine("----------");
        }

        public void Play(int instance)
        {
            // Start game
            
            // Player's turn
            while (Player.HandValue() < 21)
            {
                // Print player's hand and dealer's hand
                Console.WriteLine($"\nBalance: ${Player.Balance}");
                Console.WriteLine("----------\nPlayer Hand:");
                Console.WriteLine("[" + Player.HandValue() + "]");
                Player.PrintHand();
                Console.WriteLine("Dealer Hand:");
                Dealer.Hand[0].Print();
                Console.WriteLine(", *\n----------");

                if (Dealer.Hand[0].Value >= 10 && Player.Hand.Count == 2)
                {
                    Console.WriteLine($"Would you like to buy insurance? [${Player.Bet/2}]");
                    Console.WriteLine("[1] Yes | [2] No");
                    int input = Convert.ToInt32(Console.ReadLine());
                    if (input == 1)
                    {
                        Console.WriteLine("Insurance bought.");
                        Player.Balance -= Player.Bet / 2;
                        Player.BoughtInsurance = true;
                    }
                }

                if (Dealer.HandValue() == 21)
                {
                    Console.WriteLine("Dealer has Blackjack. Dealer wins.");
                    if (Player.BoughtInsurance)
                    {
                        Console.WriteLine("Insurance pays 2:1.");
                        Player.Balance += Player.Bet;
                        Player.BoughtInsurance = false;
                    }
                    break;
                }
                else if (Player.BoughtInsurance)
                {
                    Console.WriteLine("Dealer doesn't have BlackJack.");
                    Player.BoughtInsurance = false;
                }

                // Prompt player to hit or stand
                int option = Player.Option(Deck, this);
                if (option == 2) break;
            }
            Players[instance - 1] = Player;
        }

        public void End()
        {

            // Dealer's turn
            while (Dealer.HandValue() < 17)
            {
                foreach (Player player in Players)
                {
                    HandStatus(player);
                    Console.WriteLine("Drawing card...");
                }
                Thread.Sleep(2000);
                Dealer.Deal(Deck);
            }

            // Print player's hand and dealer's hand
            foreach (Player player in Players)
            {
                HandStatus(player);
            }

            // Determine winner
            for (int i = Players.Count-1; i >= 0; i--)
            {
                DetermineWinner(Players[i]);
                Player.Balance = Players[i].Balance;
            }

            Console.WriteLine($"Balance: ${Player.Balance}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void DetermineWinner(Player player)
        {
            if (player.HandValue() > 21)
            {
                Console.WriteLine($"On game {player.Instance}:\n Player busts. Dealer wins.");
            }
            else if (Dealer.HandValue() > 21)
            {
                Console.WriteLine($"On game {player.Instance}:\n Dealer busts. Player wins.");
                player.Balance += player.Bet * 2;
            }
            else if (player.HandValue() > Dealer.HandValue())
            {
                Console.Write($"On game {player.Instance}:\n Player wins");
                if (player.HandValue() == 21)
                {
                    Console.WriteLine(" with Blackjack!");
                    player.Balance += player.Bet * 2.5;
                }
                else
                {
                    Console.WriteLine(".");
                    player.Balance += player.Bet * 2;
                }
            }
            else if (player.HandValue() < Dealer.HandValue())
            {
                Console.WriteLine($"On game {player.Instance}:\n Dealer wins.");
            } else if (player.HandValue() == Dealer.HandValue())
            {
                Console.WriteLine($"On game {player.Instance}:\n there's a tie.");
                Player.Balance += Player.Bet;
            }
        }
    }
}
