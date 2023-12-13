using PyShop.Task1;

namespace PyShop.Task1.Tests
{
	public class GameTests
	{
		[Fact]
		public void getScoreValue()
		{
			// Arrange
			GameStamp[] gameStamps = new GameStamp[5];

			for(int i = 0; i < 5; i++)
			{
				gameStamps[i] = new GameStamp(i, i, i);
			}

			Game game = new Game(gameStamps);

			// Act
			Score score0 = game.getScore(0);
			Score score1 = game.getScore(1);
			Score score2 = game.getScore(2);
			Score score3 = game.getScore(3);
			Score score4 = game.getScore(4);

			// Assert
			Assert.Equal(new int[] { 0, 0 }, new int[] { score0.away, score0.home });
			Assert.Equal(new int[] { 1, 1 }, new int[] { score1.away, score1.home });
			Assert.Equal(new int[] { 2, 2 }, new int[] { score2.away, score2.home });
			Assert.Equal(new int[] { 3, 3 }, new int[] { score3.away, score3.home });
			Assert.Equal(new int[] { 4, 4 }, new int[] { score4.away, score4.home });
		}

		[Fact]
		public void getScoreNullReferenceException()
		{
			// Arrange
			GameStamp[] gameStamps = new GameStamp[5];

			for (int i = 0; i < 5; i++)
			{
				gameStamps[i] = new GameStamp(i, i, i);
			}

			Game game = new Game(gameStamps);

			// Assert
			Assert.Throws<NullReferenceException>(() => game.getScore(6));
		}

		[Fact]
		public void getScoreAboveZero()
		{
			// Arrange
			GameStamp[] gameStamps = new GameStamp[5];

			for (int i = 0; i < 5; i++)
			{
				gameStamps[i] = new GameStamp(i, i, i);
			}

			Game game = new Game(gameStamps);

			// Act
			Score score = game.getScore(2);

			// Assert
			Assert.True(score.away >= 0 && score.home >= 0);
		}

		[Fact]
		public void getScoreType()
		{
			// Arrange
			GameStamp[] gameStamps = new GameStamp[5];

			for (int i = 0; i < 5; i++)
			{
				gameStamps[i] = new GameStamp(i, i, i);
			}

			Game game = new Game(gameStamps);

			// Act
			Score score = game.getScore(2);

			// Assert
			Assert.IsType<Score>(score);
		}
	}
}