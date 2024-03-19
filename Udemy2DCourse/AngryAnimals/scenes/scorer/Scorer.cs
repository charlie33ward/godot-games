using Godot;
using System;

public partial class Scorer : Node
{
	private int Attempts;
	private int CupsHit;
	private int TotalCups;
	private int LevelNumber = 1;

	private SignalManager SignalManager;
	private ScoreManager ScoreManager;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalManager = GetNode<SignalManager>("/root/SignalManager");
		SignalManager.OnAttemptMade += OnAttemptMade;
		SignalManager.OnCupDestroyed += OnCupDestroyed;
		ScoreManager = GetNode<ScoreManager>("/root/ScoreManager");
		
		TotalCups = GetTree().GetNodesInGroup("cup").Count;
		LevelNumber = ScoreManager.GetLevelSelected();
	}

	private void OnAttemptMade()
	{
		Attempts += 1;
		SignalManager.EmitSignal(nameof(SignalManager.OnScoreUpdated));
	}
	
	private void OnCupDestroyed()
	{
		CupsHit += 1;
		if (CupsHit == TotalCups)
		{
			SignalManager.EmitSignal(nameof(SignalManager.OnLevelComplete));
			ScoreManager.SetScoreForLevel(Attempts, LevelNumber.ToString());

			// level is completed, going to use a signal
			// then set score for level


		}
	}

	private void OnScorerTreeExiting()
	{
		SignalManager.OnAttemptMade -= OnAttemptMade;
		SignalManager.OnCupDestroyed -= OnCupDestroyed;
	}
	
}
