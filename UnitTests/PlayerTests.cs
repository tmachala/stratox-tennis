using StratoxTennis.Core.Game;
// ReSharper disable ConvertToLocalFunction

namespace StratoxTennis.UnitTests;

public class PlayerTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(67)]
    [InlineData(100)]
    public void Constructor_GivenValidExperienceCreatesInstance(int experience)
    {
        // Act
        var player = new Player(experience);
        
        // Assert
        Assert.Equal(experience, player.Experience);
        Assert.Equal(Points.Zero, player.Points);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Constructor_GivenOutOfRangeExperienceThrows(int experience)
    {
        // Arrange
        var test = () => new Player(experience);
        
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(test);
    }

    [Fact]
    public void Score_AdvancesPointsCorrectlyGivenOpponentWithNoAdvantage()
    {
        // Arrange
        var opponent = new Player(50);
        var p1 = new Player(50);
        
        // Act
        var p2 = p1.Score(opponent);
        var p3 = p2.Score(opponent);
        var p4 = p3.Score(opponent);
        var p5 = p4.Score(opponent);
        
        // Assert
        Assert.Equal(Points.Zero, p1.Points);
        Assert.Equal(Points.Fifteen, p2.Points);
        Assert.Equal(Points.Thirty, p3.Points);
        Assert.Equal(Points.Forty, p4.Points);
        Assert.Equal(Points.GamePoint, p5.Points);
    }
    
    [Fact]
    public void Score_ThrowsWhenGameOver()
    {
        // Arrange
        var opponent = new Player(50);
        var initial = new Player(50);
        
        // Act
        var gameOver = initial.Score(opponent).Score(opponent).Score(opponent).Score(opponent);
        
        // Assert
        Assert.Throws<RulesViolationException>(() => gameOver.Score(opponent));
    }
    
    [Fact]
    public void SampleMatchWithAdvantages()
    {
        // Arrange
        var a1 = new Player(50);
        var b1 = new Player(50);
        
        // Act
        
        // Player A scores --> 15:0
        var a2 = a1.Score(b1);
        var b2 = b1.Lose(a1);
        
        // Player A scores --> 30:0
        var a3 = a2.Score(b2);
        var b3 = b2.Lose(a2);
        
        // Player A scores --> 40:0
        var a4 = a3.Score(b3);
        var b4 = b3.Lose(a3);
        
        // Player B scores --> 40:15
        var a5 = a4.Lose(b4);
        var b5 = b4.Score(a4);
        
        // Player B scores --> 40:30
        var a6 = a5.Lose(b5);
        var b6 = b5.Score(a5);
        
        // Player B scores --> 40:40
        var a7 = a6.Lose(b6);
        var b7 = b6.Score(a6);
        
        // Player B scores --> 40:ADVANTAGE
        var a8 = a7.Lose(b7);
        var b8 = b7.Score(a7);
        
        // Player A scores --> ADVANTAGE:40
        var a9 = a8.Score(b8);
        var b9 = b8.Lose(a8);
        
        // Player A scores --> Game over, A won
        var a10 = a9.Score(b9);
        var b10 = b9.Lose(a9);
        
        // Assert
        Assert.Equal(Points.Zero, a1.Points);
        Assert.Equal(Points.Zero, b1.Points);
        
        Assert.Equal(Points.Fifteen, a2.Points);
        Assert.Equal(Points.Zero, b2.Points);
        
        Assert.Equal(Points.Thirty, a3.Points);
        Assert.Equal(Points.Zero, b3.Points);
        
        Assert.Equal(Points.Forty, a4.Points);
        Assert.Equal(Points.Zero, b4.Points);
        
        Assert.Equal(Points.Forty, a5.Points);
        Assert.Equal(Points.Fifteen, b5.Points);
        
        Assert.Equal(Points.Forty, a6.Points);
        Assert.Equal(Points.Thirty, b6.Points);
        
        Assert.Equal(Points.Forty, a7.Points);
        Assert.Equal(Points.Forty, b7.Points);
        
        Assert.Equal(Points.Forty, a8.Points);
        Assert.Equal(Points.Advantage, b8.Points);
        
        Assert.Equal(Points.Advantage, a9.Points);
        Assert.Equal(Points.Forty, b9.Points);
        
        Assert.Equal(Points.GamePoint, a10.Points);
        Assert.Equal(Points.Forty, b10.Points);
    }

    [Fact]
    public void IsVictorious_TrueOnlyOnGamePoint()
    {
        // Arrange
        var opponent = new Player(50);
        var p1 = new Player(50);
        
        // Act
        var p2 = p1.Score(opponent);
        var p3 = p2.Score(opponent);
        var p4 = p3.Score(opponent);
        var p5 = p4.Score(opponent);
        
        // Assert
        Assert.False(p1.IsVictorious);
        Assert.False(p2.IsVictorious);
        Assert.False(p3.IsVictorious);
        Assert.False(p4.IsVictorious);
        Assert.True(p5.IsVictorious);
    }
}