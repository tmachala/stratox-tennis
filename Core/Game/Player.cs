namespace StratoxTennis.Core.Game;

public class Player
{
    public int Experience { get; }
    public Points Points { get; }
    public bool IsVictorious => Points == Points.GamePoint;

    public Player(int experience)
    {
        if (experience < MinExperience || experience > MaxExperience)
        {
            var msg = $"Player's experience must be between {MinExperience} and {MaxExperience}!";
            throw new ArgumentOutOfRangeException(nameof(experience), msg);
        }

        Experience = experience;
        Points = Points.Zero;
    }

    private Player(int experience, Points points)
        : this(experience)
    {
        Points = points;
    }

    /// <summary>
    /// Creates a new <see cref="Player"/> with points adjusted after this player scoring.
    /// </summary>
    /// <param name="opponent">The other player (who lost the ball)</param>
    /// <exception cref="RulesViolationException"></exception>
    public Player Score(Player opponent)
    {
        var newPoints = Points switch
        {
            Points.Zero => Points.Fifteen,
            Points.Fifteen => Points.Thirty,
            Points.Thirty => Points.Forty,
            Points.Forty => ShouldGetAdvantage() 
                ? Points.Advantage 
                : Points.GamePoint,
            Points.Advantage => Points.GamePoint,
            _ => throw new RulesViolationException("Can't increase score in this situation!")
        };

        return new Player(Experience, newPoints);

        bool ShouldGetAdvantage() => HasFortyOrAdvantage(this) && HasFortyOrAdvantage(opponent);
        bool HasFortyOrAdvantage(Player p) => p.Points is Points.Forty or Points.Advantage;
    }

    /// <summary>
    /// Creates a new <see cref="Player"/> with points adjusted after this player losing a ball.
    /// </summary>
    /// <param name="opponent">The other player (who scored)</param>
    /// <exception cref="RulesViolationException"></exception>
    public Player Lose(Player opponent)
    {
        // The points don't change after losing a ball except for when this player had an advantage.
        // In that case he's going to lose it.
        return Points == Points.Advantage 
            ? new Player(Experience, Points.Forty) 
            : this;
    }

    private const int MinExperience = 0;
    private const int MaxExperience = 100;
}