using Godot;
using System;

public partial class SignalManager : Node
{
	
	[Signal]
	public delegate void OnAnimalDiedEventHandler();
	[Signal]
	public delegate void OnCupDestroyedEventHandler();
	[Signal]
	public delegate void OnAttemptMadeEventHandler();
	
	
	

}
