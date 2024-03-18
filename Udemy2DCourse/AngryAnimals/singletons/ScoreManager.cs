using Godot;
using System;

public partial class ScoreManager : Node
{

	private static int LevelSelected;
	
	public override void _Ready()
	{
		
	}

	public static void SetLevelSelected(int level)
	{
		LevelSelected = level;
	}

	public static int GetLevelSelected()
	{
		return LevelSelected;
	}
	
	
}
