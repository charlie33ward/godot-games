using Godot;
using System;

public partial class level : Node2D
{
	private static PackedScene AnimalScene;
	private PackedScene MainScene;
	private SignalManager SignalManager;
	private Marker2D AnimalStart;
	
	
	public override void _Ready()
	{
		MainScene = (PackedScene)ResourceLoader.Load("res://scenes/main/main.tscn");
		AnimalScene = (PackedScene)ResourceLoader.Load("res://scenes/animal/animal.tscn");
		AnimalStart = GetNode<Marker2D>("AnimalStart");

		SignalManager = GetNode<SignalManager>("/root/SignalManager");
		SignalManager.OnAnimalDied += AnimalDied;
		
		InstantiateAnimal();
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			GetTree().ChangeSceneToPacked(MainScene);
		}
	}

	public void AnimalDied()
	{
		InstantiateAnimal();
	}

	public void InstantiateAnimal()
	{
		AnimalStart = GetNode<Marker2D>("AnimalStart");
		Node2D Animal = (Node2D)AnimalScene.Instantiate();
		Animal.Position = new Vector2(AnimalStart.Position.X, AnimalStart.Position.Y);
		AddChild(Animal);
	}

	private void OnAnimalStartTreeEntered()
	{
		GD.Print(AnimalStart);
		if (AnimalStart == null)
		{
			AnimalStart = GetNode<Marker2D>("AnimalStart");

		}
	}

	private void OnTreeExiting()
	{
		SignalManager.OnAnimalDied -= AnimalDied;
	}
}
