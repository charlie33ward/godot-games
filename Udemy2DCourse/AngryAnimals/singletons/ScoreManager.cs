using Godot;
using System;
using Godot.Collections;

public partial class ScoreManager : Node
{
	public readonly int DEFAULT_SCORE = 1000;
	private readonly string SCORES_PATH = "user://animals.dat";
	
	private Dictionary<string, int> levelScores = new Dictionary<string, int>()
	{
		{ "1", 1000 },
		{ "2", 1000 },
		{ "3", 1000 }
	};
	

	private static int levelSelected;
	

	public override void _Ready()
	{
		
	}

	public static void SetLevelSelected(int level)
	{
		levelSelected = level;
	}

	public static int GetLevelSelected()
	{
		return levelSelected;
	}

	private void CheckAndAdd(string level)
	{
		if (!levelScores.ContainsKey(level))
		{
			levelScores.Add(level, DEFAULT_SCORE);
		}
	}

	public void SetScoreForLevel(int score, string level)
	{
		CheckAndAdd(level);
		if (levelScores[level] > score)
		{
			levelScores[level] = score;
		}
	}

	public int GetBestForLevel(string level)
	{
		CheckAndAdd(level);
		return levelScores[level];
	}
	
	
}
