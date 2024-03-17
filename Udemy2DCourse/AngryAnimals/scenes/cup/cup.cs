using Godot;
using System;

public partial class cup : StaticBody2D
{
	private AnimationPlayer AnimationPlayer;
	private SignalManager SignalManager;

	public override void _Ready()
	{
		SignalManager = GetNode<SignalManager>("/root/SignalManager");
		AnimationPlayer = (AnimationPlayer) GetNode("AnimationPlayer");
	}

	private void Die()
	{
		AnimationPlayer.Play("vanish");
	}

	private void OnAnimationFinished(StringName name)
	{
		SignalManager.EmitSignal(nameof(SignalManager.OnCupDestroyed));
		QueueFree();
	}
}
