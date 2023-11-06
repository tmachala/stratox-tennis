namespace StratoxTennis.Core.Game;

public enum MatchState
{
    /// <summary>
    /// Not playing yet. Waiting for player 1 to join the game.
    /// </summary>
    WaitingForPlayer1,
    
    /// <summary>
    /// Not playing yet. Waiting for player 2 to join the game.
    /// </summary>
    WaitingForPlayer2,
    
    /// <summary>
    /// Player 1 should take action.
    /// </summary>
    BallOnPlayer1Side,
    
    /// <summary>
    /// Player 2 should take action.
    /// </summary>
    BallOnPlayer2Side,
    
    /// <summary>
    /// The match is over. Player 1 won.
    /// </summary>
    Player1Won,
    
    /// <summary>
    /// The match is over. Player 2 won.
    /// </summary>
    Player2Won
}