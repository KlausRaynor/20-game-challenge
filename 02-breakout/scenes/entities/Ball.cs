using Godot;

public partial class Ball : CharacterBody2D
{
	private float _speed = 600;
	[Export] public float Speed
	{
		get => _speed;
		set
		{
			if (Mathf.IsEqualApprox(_speed, value)) return;
			_speed = value;
			EmitSignal(SignalName.SpeedChanged, _speed);
		}
	};
	[Export] public float BounceSteepness = 2.0f;
	[Signal] public delegate void SpeedChangedEventHandler(float _speed);
	private Vector2 _direction = new Vector2(GD.Randf() * 2f - 1f, 1).Normalized();
	private Vector2 startingPos = new Vector2(317.0f, 498.0f);
	private int defaultSpeed = 600;
	private int speedBounce = 20;



	public override void _PhysicsProcess(double delta)
	{
		Velocity = _direction * Speed;

		// checks for OOB bottom of screen and resets position
		if (GlobalPosition.Y > 1200)
		{
			GlobalPosition = startingPos;
			Speed = defaultSpeed;
			_direction = new Vector2(GD.Randf() * 2f - 1f, 1).Normalized();
		}

		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

		

		if (collision == null)
			return;

		if (collision.GetCollider() is Node2D body)
		{
			if (body.IsInGroup("Paddle"))
			{
				float halfWidth = GetPaddleHalfWidth(body);
				float offsetX = collision.GetPosition().X - body.GlobalPosition.X;
				float factor = Mathf.Clamp(offsetX / halfWidth, -1f, 1f);
				_direction = new Vector2(factor, -BounceSteepness).Normalized();
				Speed = defaultSpeed;
			}
			else
			{
				if (body is Brick brick)
				{
					brick.Hit();
				}
				Speed += speedBounce;
				_direction = _direction.Bounce(collision.GetNormal());
			}

		}
	}

	private float GetPaddleHalfWidth(Node2D paddle)
	{
		CollisionShape2D shapeNode = paddle.GetNode<CollisionShape2D>("paddleCollider");
		RectangleShape2D rect = (RectangleShape2D)shapeNode.Shape;
		return rect.Size.X * 0.5f * paddle.GlobalScale.X;
	}
}