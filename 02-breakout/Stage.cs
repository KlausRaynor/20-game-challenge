using Godot;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices.Marshalling;

public partial class Stage : Node2D
{
	[Export] public PackedScene BallScene;
	[Export] public PackedScene PaddleScene;
	[Export] public PackedScene BrickScene;
	// Called when the node enters the scene tree for the first time.

	private Label scoreLabel;
	private int score;
	public override void _Ready()
	{
		
		const int Columns = 6;
		const int Rows = 8;
		const float Gap = 2f;
		const float TopMargin = 20f;

		Vector2 screenSize = GetViewportRect().Size;
		
		Node2D probe = (Node2D)BrickScene.Instantiate();
		scoreLabel = GetNode<Label>("Label");

		var probeShape = (RectangleShape2D)probe.GetNode<CollisionShape2D>("CollisionShape2D").Shape;
		float baseW = probeShape.Size.X;
		float baseH = probeShape.Size.Y;
		probe.Free();

		float cellWidth = screenSize.X / Columns;
		float scaleX = (cellWidth - Gap) / baseW;

		// spawn grid of bricks that are offset.
		// 8 rows, 14 columns. Offset by 20 pixels in x and y.
		for (int row = 0; row < Rows; row++)
		{
			for (int col = 0; col < Columns; col++)
			{
				Brick brick = (Brick)BrickScene.Instantiate();
				brick.Scale = new Vector2(scaleX, brick.Scale.Y);
				
				float x = col * cellWidth + cellWidth * 0.5f;
				float y = row * (baseH + Gap) + TopMargin;

				brick.Position = new Vector2(x, y);
				AddChild(brick);
				brick.BrickDestroyed += OnBrickDestroyed;
			}
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBrickDestroyed(int incScore)
	{
		score+= incScore;
		scoreLabel.Text = "Score: " + score;
	}
}
