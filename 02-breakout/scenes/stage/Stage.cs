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
	private Label statsLabel;
	private int score;
	private Ball ball;
	public override void _Ready()
	{
		
		const int Columns = 6;
		const int Rows = 8;
		const float Gap = 2f;
		const float TopMargin = 150f;

		Vector2 screenSize = GetViewportRect().Size;
		
		Node2D probe = (Node2D)BrickScene.Instantiate();
		
		scoreLabel = GetNode<Label>("Label");
		statsLabel = GetNode<Label>("Stats");
		ball = (Ball)GetNode<CharacterBody2D>("Ball");
		var probeShape = (RectangleShape2D)probe.GetNode<CollisionShape2D>("CollisionShape2D").Shape;
		float baseW = probeShape.Size.X;
		float baseH = probeShape.Size.Y;
		probe.Free();

		float cellWidth = (screenSize.X - TopMargin) / Columns;
		float cellWidthOffset = cellWidth / 2;
		float scaleX = (cellWidth - Gap) / baseW;

		// spawn grid of bricks that are offset.
		// 8 rows, 14 columns. Offset by 20 pixels in x and y.
		for (int row = 0; row < Rows; row++)
		{
			for (int col = 0; col < Columns; col++)
			{
				Brick brick = (Brick)BrickScene.Instantiate();
				brick.Scale = new Vector2(scaleX, brick.Scale.Y);
				float x;
				float y;
				if(row %2 == 0)
				{
					x = col * cellWidth + cellWidth/2 + TopMargin/2-cellWidthOffset/2;
				}
				else
				{
					x = col * cellWidth + cellWidthOffset + cellWidth /2 + TopMargin/2-cellWidthOffset/2;
				}
				y = row * (baseH + Gap) + TopMargin;

				brick.Position = new Vector2(x, y);
				AddChild(brick);
				brick.BrickDestroyed += OnBrickDestroyed;
			}
		}

		statsLabel.Text = "Current Ball Speed: " + ball.Speed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		statsLabel.Text = "Current Ball Speed: " + ball.Speed;
	}

	private void OnBrickDestroyed(int incScore)
	{
		score+= incScore;
		scoreLabel.Text = "Score: " + score;
	}
}
