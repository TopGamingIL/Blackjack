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
        public int Balance { get; set; }
        // Bet: Amount of chips player bets
        public int Bet { get; set; }
        // Hand: List of cards player holds
        public List<Card> Hand { get; set; }

        // Constructor
        public Player()
        {
            // Initialize player with 2500 chips, 0 bet, and an empty hand
            Balance = 2500;
            Bet = 0;
            Hand = new List<Card>();
        }

        // Methods
        
        public void PrintBalance()
        {
            // Print player's balance
            Console.WriteLine($"Balance: {Balance}");
        }

        public void PlaceBet()
        {
            // Prompt player to place a bet
            Console.WriteLine("Enter your bet:");
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

        public int Option(Deck deck)
        {
            // Prompt player to hit, stand, or double down
            Console.Write("[1] Hit | [2] Stand");
            if (Hand.Count == 2) Console.Write(" | [3] Double Down");
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
                    Stand();
                    return 2;
                case 3:
                    if (Hand.Count == 2) DoubleDown(deck);
                    else Console.WriteLine("You can only double down on first turn."); Option(deck);
                    return 3;
                default:
                    Console.WriteLine("Please enter a valid input.");
                    Option(deck);
                    break;
            }
            return 0;
        }

        private void DoubleDown(Deck deck)
        { 
            Balance -= Bet;
            Bet *= 2;
            Deal(deck);
            Stand();
        }

        public void Deal(Deck deck)
        {
            Hand.Add(deck.Cards.First());
            deck.Cards.RemoveAt(0);
            AceCheck();
        }

        public void Stand()
        {
            Console.WriteLine("Player stands.");
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
            // If hand has more than 2 cards, aces are valued at 1 except for the last ace
            if (Hand.Count > 2)
            {
                foreach (Card card in Hand)
                {
                    if (card.Rank == 'A')
                    {
                        card.Value = 1;
                    }
                }
                if (Hand.Last().Rank == 'A' && HandValue() + 10 <= 21) Hand.Last().Value = 11;
            }
            // If hand has 2 cards, aces are valued at 11 if hand value is less than or equal to 21
            else
            {
                foreach (Card card in Hand)
                {
                    if (card.Rank == 'A' && HandValue() + 10 <= 21)
                    {
                        card.Value = 11;
                    }
                }
            }

        }
    }
}
