using Godot;
using System;

public partial class Ball : CharacterBody2D
{

	[Export] public float Speed = 400.0f;

	private Vector2 _direction = new Vector2(0.4f, 1).Normalized();

	public override void _PhysicsProcess(double delta)
	{
		Velocity = _direction * Speed;

		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

		if (collision != null)
		{
			_direction = _direction.Bounce(collision.GetNormal());
		}
	}
}
