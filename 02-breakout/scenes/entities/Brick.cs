using Godot;
using System;

public partial class Brick : StaticBody2D
{
	[Signal] public delegate void BrickDestroyedEventHandler(int score);
	[Export] public int _brickScore = 5;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.AddToGroup("Bricks");
	}

	public void Hit()
	{
		EmitSignal(SignalName.BrickDestroyed, _brickScore);
		this.QueueFree();
	}
}
