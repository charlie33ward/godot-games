using Godot;
using System;

public partial class Scorer : Node
{
	private int Attempts;
	private int CupsHit;
	private int TotalCups;
	private int LevelNumber = 1;

	private SignalManager SignalManager;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalManager = GetNode<SignalManager>("/root/SignalManager");
		SignalManager.OnAttemptMade += OnAttemptMade;
		SignalManager.OnCupDestroyed += OnCupDestroyed;
		
		TotalCups = GetTree().GetNodesInGroup("cup").Count;
		LevelNumber = ScoreManager.GetLevelSelected();
	}

	private void OnAttemptMade()
	{
		Attempts += 1;
	}
	
	private void OnCupDestroyed()
	{
		CupsHit += 1;
		if (CupsHit == TotalCups)
		{
			
		}
	}
}
