using System;
using System.Collections.Generic;
using System.Linq;

namespace GoFish
{
    class Program
    {        
        static GameController gameController;
        static void Main(string[] args)
        {
            while (true)
            {
                bool getComputerPlayers = true;
                List<string> computerPlayersNames = new List<string>();

                Console.Write($"Enter your name: ");
                string humanPlayerName = Console.ReadLine();
                
                while (getComputerPlayers)
                {
                    Console.Write("Enter the number of computer opponents: ");
                    bool validInput = int.TryParse(Console.ReadLine(), out int computerPlayerCount);

                    if (validInput && (computerPlayerCount >= 1 && computerPlayerCount <= 5))
                    {
                        for (int i = 1; i <= computerPlayerCount; i++)
                            computerPlayersNames.Add($"{ComputerPlayers.Computer} #" + i);                        
                        getComputerPlayers = false;
                    }
                        
                    else
                        Console.WriteLine("Computer opponents must be a value of 1 to 5");
                }               

                gameController = new GameController(humanPlayerName, computerPlayersNames);
                Console.WriteLine($"Starting a new game with players {gameController.HumanPlayer}, "
                    + string.Join(", ", gameController.Opponents));                
                
                while (gameController.GameOver == false)
                {
                    Console.WriteLine($"Your hand: ");
                    foreach (var card in gameController.HumanPlayer.Hand.OrderBy(card => card.Suit).OrderBy(card => card.Value))
                        Console.WriteLine(card);
                    var value = PromptForAValue();
                    var playerToAsk = PromptForAnOpponent();

                    gameController.NextRound(playerToAsk, value);
                    Console.WriteLine(gameController.Status);
                }
                
                // Final prompt to break out of game loop or run again
                Console.WriteLine($"Press 'Q' to quit, or any other key for a new game");
                char input = Console.ReadKey(true).KeyChar;
                if (input == 'q' || input == 'Q')
                    break;             
            }
        }
        /// <summary>
        /// Prompt the human player for a card value
        /// in their hand
        /// Uses a while (true) loop with if-else to check for valid text input 
        /// by using Enum.TryParse to validate the text and output as a Values value
        /// which is then passed into a LINQ query to verify the value is of a card
        /// in the player's hand. When all of this is true, the out object is returned.
        /// When false, a message is returned that an invalid value was selected and the
        /// while loop is returned.
        /// </summary>
        /// <returns>The value to ask for</returns>
        static Values PromptForAValue()
        {            
            Console.Write($"What card value do you want to ask for? ");
            
            while (true)
            {
                if (Enum.TryParse(typeof(Values), Console.ReadLine(), true, out object result)
                    && gameController.HumanPlayer.Hand.Any(card => card.Value == (Values)result))
                    return (Values)result;
                else
                    Console.Write("You must specify a card value in your hand: ");
            }            
        }
        /// <summary>
        /// Prompt the human player for an opponent
        /// to ask for a card
        /// </summary>
        /// <returns>The opponent to ask</returns>
        static Player PromptForAnOpponent()
        {
            var opponents = gameController.Opponents.ToList();
            for (int i = 1; i <= opponents.Count(); i++)
                Console.WriteLine($"{i}) {opponents[i - 1]}");        // Creates a numeric menu to select opponent beginning at index 0 (opponents[i - 1])

            Console.Write($"Who do you want to ask for a card? ");
            
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int result)
                    && result <= opponents.Count()
                    && result >= 1)
                    return opponents[result - 1];
                else
                    Console.Write($"Select a valid opponent (1 to {opponents.Count()}): ");
            }
        }  

    }

}
