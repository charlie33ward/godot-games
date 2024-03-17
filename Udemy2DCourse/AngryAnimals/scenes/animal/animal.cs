using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class animal : RigidBody2D
{
	private SignalManager SignalManager;
	
	[Export] private Label label;
	[Export] private Sprite2D Arrow;
	[Export] private AudioStreamPlayer2D StretchSound;
	[Export] private AudioStreamPlayer2D LaunchSound;
	[Export] private AudioStreamPlayer2D KickSound;
	
	private readonly Vector2 DragLimitMax = new Vector2(0, 60);
	private readonly Vector2 DragLimitMin = new Vector2(-60, 0);
	private readonly float ImpulseMultiplier = 20.0f;
	private readonly float ImpulseMax = 1200.0f;
	
	private Vector2 Start = new Vector2(0, 0);
	private Vector2 DragStart = new Vector2(0, 0);
	private Vector2 DraggedVector = new Vector2(0, 0);
	private Vector2 LastDraggedVector = new Vector2(0, 0);
	private float ArrowScaleX = 0.0f;
	private int LastCollisionCount = 0;

	public enum AnimalState
	{
		Ready,
		Drag,
		Release
	}

	private AnimalState state = AnimalState.Ready;
	
	public override void _Ready()
	{
		ArrowScaleX = Arrow.Scale.Y;
		Arrow.Hide();
		SignalManager = GetNode<SignalManager>("/root/SignalManager");
		Start = Position;
	}

	public override void _PhysicsProcess(double delta)
	{
		Update((float) delta);
		label.Text = $"{state}\n";
		label.Text += string.Format("{0:F1},{1:F1}", DraggedVector.X, DraggedVector.Y);
		
	}

	private void PlayCollision()
	{
		if (LastCollisionCount == 0 &&
		    GetContactCount() > 0 &&
		    KickSound.Playing == false)
		{
			KickSound.Play();
		}
		LastCollisionCount = GetContactCount();
	}
	
	private void UpdateFlight()
	{
		PlayCollision();
	}
	
	public void Update(float delta)
	{
		switch (state)
		{
			case AnimalState.Drag:
				UpdateDrag();
				break;
			case AnimalState.Ready:
				break;
			case AnimalState.Release:
				UpdateFlight();
				break;
		}
	}

	private bool DetectRelease()
	{
		if (state == AnimalState.Drag)
		{
			if (Input.IsActionJustReleased("drag"))
			{
				SetNewState(AnimalState.Release);
				return true;
			}
		}

		return false;
	}

	private Vector2 GetImpulse()
	{
		return DraggedVector * -1 * ImpulseMultiplier;
	}

	private void SetRelease()
	{
		Arrow.Hide();
		Freeze = false;
		ApplyCentralImpulse(GetImpulse());
		LaunchSound.Play();
		SignalManager.EmitSignal(nameof(SignalManager.OnAttemptMade));
	}
	
	private void SetDrag()
	{
		DragStart = GetGlobalMousePosition();
		Arrow.Show();
	}
	
	private void SetNewState(AnimalState newState)
	{
		state = newState;
		if (state == AnimalState.Release)
		{
			SetRelease();
		}
		else if (state == AnimalState.Drag)
		{
			SetDrag();
		}
		
	}

	private void ScaleArrow()
	{
		float impulseLength = GetImpulse().Length();
		float percentage = impulseLength / ImpulseMax;
		Arrow.Scale = new Vector2(ArrowScaleX * percentage + ArrowScaleX, Arrow.Scale.Y);
		Arrow.Rotation = (Start - Position).Angle();
	}

	private void PlayStretchSound()
	{
		Vector2 newVector = LastDraggedVector - DraggedVector;
		if (newVector.Length() > 0)
		{
			if (StretchSound.Playing == false)
			{
				StretchSound.Play();
			}
		}
	}

	private Vector2 GetDraggedVector(Vector2 gmp)
	{
		return gmp - DragStart;
	}

	private void DragInLimits()
	{
		LastDraggedVector = DraggedVector;
		
		DraggedVector.X = SpecialMath.Clamp(DraggedVector.X,
			DragLimitMin.X, DragLimitMax.X);
		DraggedVector.Y = SpecialMath.Clamp(DraggedVector.Y,
			DragLimitMin.Y, DragLimitMax.Y);

		Position = Start + DraggedVector;
	}
	
	
	private void UpdateDrag()
	{
		if (DetectRelease())
			return;
		
		Vector2 gmp = GetGlobalMousePosition();
		DraggedVector = GetDraggedVector(gmp);
		PlayStretchSound();
		DragInLimits();
		ScaleArrow();
	}

	public void OnInputEvent(Node viewport, InputEvent @event, int shapeIdx)
	{
		if (state == AnimalState.Ready && @event.IsActionPressed("drag"))
		{
			SetNewState(AnimalState.Drag);
		}
	}
	
	public void OnScreenExited()
	{
		Die();
	}

	public void Die()
	{
		SignalManager.EmitSignal(nameof(SignalManager.OnAnimalDied));
		QueueFree();
	}

	private void OnSleepingStateChanged()
	{
		if (Sleeping == true)
		{
			var cb = GetCollidingBodies();
			if (cb.Count > 0)
			{
				try
				{
					cb[0].Call("Die");
				}
				catch (MissingMethodException)
				{
					GD.Print("SOMETHING WRONG");
				}
			}
			
			CallDeferred("Die");
		}
	}
}
