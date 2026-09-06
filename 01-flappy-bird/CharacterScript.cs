using Godot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class CharacterScript : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	
	[Signal]
	public delegate void DiedEventHandler();
	private bool isDead = false;

	public override void _PhysicsProcess(double delta)
	{
		if(isDead)
			return;
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") || Input.IsActionPressed("click"))
		{
			velocity.Y = JumpVelocity;
		}

		Velocity = velocity;
		MoveAndSlide();

		if(GetSlideCollisionCount() > 0 || Position.Y >= GetViewportRect().Size.Y)
		{
			EmitSignal(SignalName.Died);
			isDead = true;
		}
	}
}
