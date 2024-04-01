using Godot;
using System;
using System.IO;
using Godot.Collections;
using FileAccess = System.IO.FileAccess;

public partial class ScoreManager : Node
{
	public readonly int DEFAULT_SCORE = 1000;
	private readonly string ScoresPath = "user://animals.json";
	private string NewScoresPath;
	
	private Dictionary<string, int> levelScores = new Dictionary<string, int>()
	{
		{ "1", 1000 },
		{ "2", 1000 },
		{ "3", 1000 }
	};
	

	private static int levelSelected;
	

	public override void _Ready()
	{
		NewScoresPath = ProjectSettings.GlobalizePath(ScoresPath);
		LoadFromDisc();
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
			SaveToDisc();
		}
	}

	public int GetBestForLevel(string level)
	{
		CheckAndAdd(level);
		return levelScores[level];
	}

	private void SaveToDisc()
	{
		string scoreJson = Json.Stringify(levelScores);
		GD.Print(scoreJson);
		
		using (StreamWriter writer = new StreamWriter(NewScoresPath))
		{
			writer.Write(scoreJson);
		}
	}

	private void LoadFromDisc()
	{
		if (!File.Exists(NewScoresPath))
		{
			SaveToDisc();
		}

		string data;
		using (FileStream file = File.Open(NewScoresPath, FileMode.Open, FileAccess.Read))
		{
			using (StreamReader reader = new StreamReader(file))
			{
				data = reader.ReadToEnd();
			}
		}
		
		Dictionary<string, Variant> parsedData = (Dictionary<string, Variant>)Json.ParseString(data);
		levelScores = new Dictionary<string, int>();

		foreach (var pair in parsedData)
		{
			levelScores.Add(pair.Key, (int)pair.Value);
		}
		
	}
	
	
}
