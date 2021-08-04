using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoFish
{    
    public class GameState
    {
        public readonly IEnumerable<Player> Players;        
        public readonly IEnumerable<Player> Opponents;
        public readonly Player HumanPlayer;
        public bool GameOver { get; private set; } = false;
        public readonly Deck Stock;
        /// <summary>
        /// Constructor creates the players and deals their first hands
        /// </summary>
        /// <param name="humanPlayerName">Name of the human player</param>
        /// <param name="opponentNames">Names of the computer players</param>
        /// <param name="stock">Shuffled stock of cards to deal from</param>
        public GameState(string humanPlayerName, IEnumerable<string> opponentNames, Deck stock)
        {
            this.Stock = stock;
            HumanPlayer = new Player(humanPlayerName);
            HumanPlayer.GetNextHand(Stock);

            var opponents = new List<Player>();
            foreach (string name in opponentNames)
            {
                var player = new Player(name);
                opponents.Add(player);
                player.GetNextHand(Stock);                
            }
            Opponents = opponents;            
            Players = new List<Player>() { HumanPlayer }.Concat(Opponents);            
        }
        /// <summary>
        /// Gets a random player that doesn't match the current player
        /// </summary>
        /// <param name="currentPlayer">The current player</param>
        /// <returns>A random player that the current player can ask for a card</returns>
        /*public Player RandomPlayer(Player currentPlayer)
        {            
            var randomPlayer =
                Players.Where(player => player.Name != currentPlayer.Name).Select(players => players).ToList();
            return randomPlayer[Player.Random.Next()];
        }*/
        public Player RandomPlayer(Player currentPlayer) => Players
            .Where(player => player != currentPlayer)
            .Skip(Player.Random.Next(Players.Count() - 1))
            .First();         
        /// <summary>
        /// Makes one player play a round
        /// </summary>
        /// <param name="player">The player asking for a card</param>
        /// <param name="playerToAsk">The player being asked for a card</param>
        /// <param name="valueToAskFor">The value to ask the player for</param>
        /// <param name="stock">The stock to draw cards from</param>
        /// <returns>A message that describes what just happened</returns>
        public string PlayRound(Player player, Player playerToAsk,
        Values valueToAskFor, Deck stock)
        {            
            string pluralizeSixValue = valueToAskFor == Values.Six ? valueToAskFor.ToString() + "e" : valueToAskFor.ToString();
            string message = $"{player.Name} asked {playerToAsk.Name} for {pluralizeSixValue}s" + Environment.NewLine;
            var cards = playerToAsk.DoYouHaveAny(valueToAskFor, stock);
            if (cards.Any())
            {
                message += $"{playerToAsk.Name} has {cards.Count()} {valueToAskFor} card{Player.S(cards.Count())}";
                player.AddCardsAndPullOutBooks(cards);
            }
            else if (!cards.Any() && stock.Count() > 0)
            {
                //message += $"{playerToAsk.Name} does not have any {valueToAskFor} cards" + Environment.NewLine + "Go fish" + Environment.NewLine;
                player.DrawCard(stock);
                message += $"{player.Name} drew a card";                
            }
            else
                message += $"The stock is out of cards";

            if (player.Hand.Count() == 0 && stock.Count() > 0)
            {                
                player.GetNextHand(stock);
                message += $"{player.Name} ran out of cards and drew {player.Hand.Count()} card{Player.S(player.Hand.Count())}";
            }

            return message;
        }
        /// <summary>
        /// Checks for a winner by seeing if any players have any cards left, sets GameOver
        /// if the game is over and there's a winner
        /// </summary>
        /// <returns>A string with the winners, an empty string if there are no winners</returns>
        public string CheckForWinner()
        {
            string message = "";            
            bool playersHandsEmpty = Players.All(player => player.Hand.Count() == 0);
            if (playersHandsEmpty)
            {
                var maxBooks = Players.Max(player => player.Books.Count());
                var winners = Players.TakeWhile(player => player.Books.Count() == maxBooks);

                if (winners.Count() > 1)
                {
                    message = $"The winners are ";
                    foreach (var winner in winners)                    
                        message += winner == winners.Last() ? $"{winner.Name}" : $"{winner.Name} and ";                                       
                }
                else
                    message = $"The winners is {winners.First().Name}";
                GameOver = true;                
            }            

            return message;
        }
    }
}
