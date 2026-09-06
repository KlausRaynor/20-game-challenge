using Godot;

public partial class Stage : Node2D
{
	[Export]
	public PackedScene PipeScene;

	private double waitTime;
	private Godot.Timer timer;
	private int score = 0;
	private Label scoreLabel;
	private Label GameOverLabel;

	private Button btnPlayAgain;
	private Button btnQuit;
	private CharacterScript bird;
	private Control gameOverUI;
	private Sprite2D randomBackground;
	private readonly string[] _backgroundPaths = new string[]
	{
		"res://_art/skybox-alien.png",
		"res://_art/skybox-day.png",
		"res://_art/skybox-morning.png",
		"res://_art/skybox-night.png",
		"res://_art/skybox-space.png",
	};
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Godot.Timer>("SpawnTimer");
		waitTime = timer.WaitTime;
		timer.Timeout += OnTimeout;

		scoreLabel = GetNode<Label>("Score");
		GameOverLabel = GetNode<Label>("%FinalScoreLabel");

		gameOverUI = GetNode<Control>("%GameOver");


		btnPlayAgain = GetNode<Button>("%btnPlayAgain");
		btnQuit = GetNode<Button>("%btnQuit");

		btnPlayAgain.Pressed += OnPlayAgain;
		btnQuit.Pressed += OnQuit;

		bird = GetNode<CharacterScript>("CharacterBody2D");
		bird.Died += OnBirdDied;

		Sprite2D skybox = GetNode<Sprite2D>("Skybox");
		int randomIndex = GD.RandRange(0, _backgroundPaths.Length - 1);
		string chosenPath = _backgroundPaths[randomIndex];

		Texture2D randomTexture = GD.Load<Texture2D>(chosenPath);
		skybox.Texture = randomTexture;
	}

	private void OnTimeout()
	{
		float newY = GD.Randf()*100;
		waitTime -= 0.1;
		if (waitTime <= 0.3)
			waitTime = 0.3;
		timer.WaitTime = waitTime;
		GD.Print(newY);
		GD.Print(waitTime);
		PipePair pipe = PipeScene.Instantiate<PipePair>();
		pipe.Scored += OnPipeScored;
		pipe.Position = new Vector2(GetViewportRect().Size.X, newY);
		AddChild(pipe);
	}

	private void OnPipeScored()
	{
		score++;
		scoreLabel.Text = "SCORE: " + score;
	}

	private void OnPlayAgain()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}

	private void OnBirdDied()
	{
		gameOverUI.Visible = true;
		GameOverLabel.Text = "SCORE: " + score;
		GetTree().Paused = true;
	}
}
