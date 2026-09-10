using Godot;
using System;

public partial class Stage : Node2D
{
	[Export] public PackedScene BallScene;
	[Export] public PackedScene PaddleScene;
	[Export] public PackedScene BrickScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Node2D probe = (Node2D)BrickScene.Instantiate();
		var shape = (RectangleShape2D)probe.GetNode<CollisionShape2D>("CollisionShape2D").Shape;
		float brickW = shape.Size.X;
		float brickH = shape.Size.Y;
		probe.QueueFree();
		// spawn grid of bricks that are offset.
		// 8 rows, 14 columns. Offset by 20 pixels in x and y.
		for (int row = 0; row < 8; row++)
		{
			for (int col = 0; col < 14; col++)
			{
				Node2D brick = (Node2D)BrickScene.Instantiate();
				if(row % 2 == 0)
				{
					brick.Position = new Vector2(col * (brickW + 20) + 20, row * (brickH + 20) + 20);
				}
				else
				{
					brick.Position = new Vector2(col * (brickW + 20), row * (brickH + 20) + 20);
				}
				AddChild(brick);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
