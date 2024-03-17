using Godot;
using System;

public partial class water : Area2D
{
	[Export] private AudioStreamPlayer2D Splash;

	public void OnBodyEntered(Node2D body)
	{
		Splash.Play();
	}
}
