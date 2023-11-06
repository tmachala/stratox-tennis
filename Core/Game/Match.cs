namespace StratoxTennis.Core.Game;

public class Match
{
    public string Name { get; }
    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }
    public MatchState State { get; private set; }

    public Match(string name, int player1Exp, int player2Exp)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        
        Name = name;
        Player1 = new Player(player1Exp);
        Player2 = new Player(player2Exp);
        State = MatchState.WaitingForPlayer1;
    }

    public void AdvancePoints(int scoringPlayer)
    {
        EnsureMatchInProgress();

        switch (scoringPlayer)
        {
            case 1:
                Player1 = Player1.Score(Player2);
                Player2 = Player2.Lose(Player1);
                break;
            case 2:
                Player1 = Player1.Lose(Player2);
                Player2 = Player2.Score(Player1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(scoringPlayer));
        }

        if (Player1.IsVictorious)
        {
            State = MatchState.Player1Won;
        }
        else if (Player2.IsVictorious)
        {
            State = MatchState.Player2Won;
        }
    }

    private void EnsureMatchInProgress()
    {
        if (!MatchInProgressStates.Contains(State))
        {
            throw new RulesViolationException("The match isn't in progress!");
        }
    }

    private static readonly MatchState[] MatchInProgressStates = { MatchState.BallOnPlayer1Side, MatchState.BallOnPlayer2Side };
}