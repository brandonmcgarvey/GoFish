using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GoFish
{
    public class GameController
    {
        public static Random Random = new Random();
        private GameState gameState;
        public bool GameOver { get { return gameState.GameOver; } }
        public Player HumanPlayer { get { return gameState.HumanPlayer; } }
        public IEnumerable<Player> Opponents { get { return gameState.Opponents; } }
        public string Status { get; private set; }
        /// <summary>
        /// Constructs a new GameController
        /// </summary>
        /// <param name="humanPlayerName">Name of the human player</param>
        /// <param name="computerPlayerNames">Names of the computer players</param>
        public GameController(string humanPlayerName, IEnumerable<string> computerPlayerNames)
        {
            gameState = new GameState(humanPlayerName, computerPlayerNames, new Deck().Shuffle());
            Status = $"Starting a new game with players {string.Join(", ", gameState.Players)}";
            /*string startMessage = "Starting a new game with players ";            
            foreach (var player in gameState.Players)
            {
                startMessage += player == gameState.Players.Last() ? $"{player.Name}" : $"{player.Name}, ";
            }
            Status = startMessage;*/
        }
        /// <summary>
        /// Plays the next round, ending the game if everyone ran out of cards
        /// </summary>
        /// <param name="playerToAsk">Which player the human is asking for a card</param>
        /// <param name="valueToAskFor">The value of the card the human is asking for</param>
        public void NextRound(Player playerToAsk, Values valueToAskFor)
        {
            Status = gameState.PlayRound(HumanPlayer, playerToAsk, valueToAskFor, gameState.Stock) + Environment.NewLine;
            ComputerPlayersPlayNextRound();
            
            Status += string.Join(Environment.NewLine, gameState.Players.Select(player => player.Status))
                + Environment.NewLine
                + $"The stock has {gameState.Stock.Count()} card{Player.S(gameState.Stock.Count())}"
                + Environment.NewLine;
            gameState.CheckForWinner();
        }
        /// <summary>
        /// All of the computer players that have cards play the next round. If the human is
        /// out of cards, then the deck is depleted and they play out the rest of the game.
        /// </summary>        
        private void ComputerPlayersPlayNextRound()
        {
            IEnumerable<Player> computerPlayers;

            do
            {
                computerPlayers = gameState.Opponents.Where(player => player.Hand.Count() > 0);
                foreach (var player in computerPlayers)
                {
                    var randomPlayer = gameState.RandomPlayer(player);
                    var randomValue = player.RandomValueFromHand();

                    Status += gameState.PlayRound(player, randomPlayer, randomValue, gameState.Stock) + Environment.NewLine;
                }
            } while ((gameState.HumanPlayer.Hand.Count() == 0) && (computerPlayers.Count() > 0));
        }
        /// <summary>
        /// Starts a new game with the same player names
        /// </summary>
        public void NewGame()
        {
            gameState = new GameState(HumanPlayer.Name, Opponents.Select(player => player.Name), new Deck().Shuffle());
            Status = $"Starting a new game";
        }
    }
}
