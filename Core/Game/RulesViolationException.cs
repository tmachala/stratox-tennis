namespace StratoxTennis.Core.Game;

public class RulesViolationException : Exception
{
    public RulesViolationException(string message)
        : base(message)
    { }
}