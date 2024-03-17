using Godot;
using System;

public partial class Bird2 : RigidBody2D
{
	[Export] private Label Label;
	private bool clickedOn = false;
	
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		Label.Text = Sleeping.ToString();
	}

	public void OnTimeout()
	{
		Freeze = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion && clickedOn)
		{
			var position = GetGlobalMousePosition();
			Position = position;
		}
	}
	
	public void OnInputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Left)
		{
			if (mouseButtonEvent.Pressed)
			{
				clickedOn = true;
			}
			else
			{
				clickedOn = false;
			}
		}
		
	}
}
