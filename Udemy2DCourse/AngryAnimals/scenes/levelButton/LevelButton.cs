using Godot;
using System;

public partial class LevelButton : TextureButton
{
	private readonly Vector2 HoverScale = new Vector2(1.1f, 1.1f);
	private readonly Vector2 DefaultScale = new Vector2(1.0f, 1.0f);

	[Export] private int LevelNumber;
	[Export] private Label LevelLabel;
	[Export] private Label ScoreLabel;

	private PackedScene LevelScene;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LevelLabel.Text = LevelNumber.ToString();
		LevelScene = (PackedScene) ResourceLoader.Load($"res://scenes/level/level{LevelNumber}.tscn");
	}

	private void OnMouseExited()
	{
		Scale = DefaultScale;
	}
	private void OnMouseEntered()
	{
		Scale = HoverScale;
	}
	
	private void OnPressed()
	{
		ScoreManager.SetLevelSelected(LevelNumber);
		GetTree().ChangeSceneToPacked(LevelScene);
	}
}
