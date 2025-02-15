using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Player
    {
        // Properties
        // Balance: Player's chips
        public double Balance { get; set; }
        // Bet: Amount of chips player bets
        public int Bet { get; set; }
        // Hand: List of cards player holds
        public List<Card> Hand { get; set; }
        // Instance: Player's instance number
        public int Instance { get; set; }
        // BoughtInsurance: Whether player bought insurance
        public bool BoughtInsurance { get; set; }

        // Constructor
        public Player()
        {
            // Initialize player with 2500 chips, 0 bet, and an empty hand
            Balance = 2500;
            Bet = 0;
            Hand = new List<Card>();
            Instance = 1;
            BoughtInsurance = false;
        }

        // Methods
        
        public void PrintBalance()
        {
            // Print player's balance
            Console.WriteLine($"Balance: ${Balance}");
        }

        public void PlaceBet()
        {
            // Prompt player to place a bet
            Console.Write("Enter your bet: ");
            // Check if input is a valid integer
            Bet = Convert.ToInt32(Console.ReadLine());
            if (Bet > Balance)
            {
                // If bet is greater than balance, prompt player to enter a valid bet
                Console.WriteLine("You don't have enough chips.");
                PlaceBet();
            }
            else if (Bet <= 0)
            {
                // If bet is less than or equal to 0, prompt player to enter a valid bet
                Console.WriteLine("Please enter a valid amount of chips to bet.");
                PlaceBet();
            }
            // Subtract bet from balance
            else Balance -= Bet;
        }

        public void Win()
        {
            // Add bet to balance and reset bet
            Balance += Bet * 2;
            Bet = 0;
        }

        public void Tie()
        {
            // Add bet to balance and reset bet
            Balance += Bet;
            Bet = 0;
        }

        public int Option(Deck deck, Game game)
        {
            // Prompt player to hit, stand, or double down
            Console.Write("[1] Hit | [2] Stand");
            if (Hand.Count == 2 && Bet < Balance)
            {
                Console.Write(" | [3] Double Down");
                if (Hand[0].Rank == Hand[1].Rank) Console.Write(" | [4] Split");
            }
            Console.Write("\nPick an option to make: ");

            // Check if input is a valid integer
            int option = Convert.ToInt32(Console.ReadLine());

            // Switch statement to handle player's choice
            switch (option)
            {
                case 1:
                    Deal(deck);
                    return 1;
                case 2:
                    return 2;
                case 3:
                    if (Hand.Count == 2 && Bet < Balance) { DoubleDown(deck); return 2; }
                    else Console.WriteLine("You can only double down on first turn."); Option(deck, game);
                    break;
                case 4:
                    if (Hand.Count == 2 && Hand[0].Rank == Hand[1].Rank && Bet < Balance) { game.Players = Split(deck, game); return 4; }
                    else Console.WriteLine("You can only split on first turn with a pair."); Option(deck, game);
                    break;
                default:
                    Console.WriteLine("Please enter a valid input.");
                    Option(deck, game);
                    break;
            }
            return 0;
        }

        public List<Player> Split(Deck deck, Game game)
        {
            // Split hand
            // Create new player and game
            Player secondPlayer = new Player();
            Game secondGame = new Game();

            // Initialize second player with same balance, bet, and instance number
            Balance -= Bet;
            secondPlayer.Balance = Balance;
            secondPlayer.Bet = Bet;
            secondPlayer.Instance = Instance + 1;

            // Add second card from first hand to second player's hand
            secondPlayer.Hand.Add(Hand[1]);
            Hand.RemoveAt(1);

            // Create new game with same dealer and players
            secondGame.Player = secondPlayer;
            secondGame.Dealer = game.Dealer;
            secondGame.Players = game.Players;
            secondGame.Players.Add(secondPlayer);

            // Deal new cards to both players
            secondPlayer.Hand.Add(deck.Cards.First());
            deck.Cards.RemoveAt(0);
            Hand.Add(deck.Cards.First());
            deck.Cards.RemoveAt(0);
            secondPlayer.AceCheck();
            AceCheck();

            // Play second game
            secondGame.Play(Instance);

            // Return list of players
            return secondGame.Players;
        }

        private void DoubleDown(Deck deck)
        {
            Console.WriteLine($"Balance: {Balance} - {Bet} = {Balance - Bet}");
            Balance -= Bet;
            Bet *= 2;
            Deal(deck);
        }

        public void Deal(Deck deck)
        {
            Hand.Add(deck.Cards.First());
            deck.Cards.RemoveAt(0);
            AceCheck();
        }

        public void PrintHand()
        {
            Card last = Hand.Last();
            foreach (Card card in Hand)
            {
                card.Print();
                if (card != last) Console.Write(", ");
            }
            Console.WriteLine();
        }

        public int HandValue()
        {
            // Calculate value of hand
            int value = 0;
            foreach (Card card in Hand)
            {
                value += card.Value;
            }
            return value;
        }

        public void AceCheck()
        {
            // Check for aces in hand and adjust value accordingly
            // If hand value is more than 21, change value of ace to 11
            foreach (Card card in Hand)
            {
                if (card.Rank == 'A' && HandValue() > 21)
                {
                    card.Value = 1;
                }
            }

        }
    }
}
