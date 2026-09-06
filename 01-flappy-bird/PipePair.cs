using Godot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class PipePair : Node2D
{

	[Export]
	public float Speed = 200;
	[Export]
	public float BirdX = 293;

	[Signal]
	public delegate void ScoredEventHandler();
	// Called when the node enters the scene tree for the first time.

	private bool hasScored = false;
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 newPos = Position;
		newPos.X -= Speed * (float)delta;
		Position = newPos;

		if (!hasScored && Position.X < BirdX)
		{
			hasScored = true;
			EmitSignal(SignalName.Scored);
			GD.Print("scored");

		}

		if (Position.X <= -20)
			QueueFree();

	}
}
