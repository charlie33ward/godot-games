using Godot;
using System;

public partial class GameUI : Control
{
	private PackedScene MainScene;
	private SignalManager SignalManager;

	[Export] private Label LevelLabel;
	[Export] private Label AttemptsLabel;
	[Export] private VBoxContainer VB2;
	[Export] private AudioStreamPlayer GameSound;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalManager = GetNode<SignalManager>("/root/SignalManager");
		SignalManager.OnScoreUpdated += UpdateAttempts;
		SignalManager.OnLevelComplete += OnLevelComplete;
		MainScene = (PackedScene) ResourceLoader.Load("res://scenes/main/main.tscn");

		LevelLabel.Text = $"LEVEL {ScoreManager.GetLevelSelected()}";
		UpdateAttempts(0);
	}

	private void OnLevelComplete()
	{
		VB2.Show();
		GameSound.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("menu"))
		{
			GetTree().ChangeSceneToPacked(MainScene);
		}
	}

	private void UpdateAttempts(int attempts)
	{
		AttemptsLabel.Text = $"Attempts {attempts}";
	}
}
