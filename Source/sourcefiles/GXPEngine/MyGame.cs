using System;
using System.Drawing;
using GXPEngine;

public class MyGame : Game //MyGame is a Game
{
	public StartMenu start;

	public int ScoreRed;
	public int ScoreBlue;
	public LevelLoader loader;
	public Sprite background;
	public int seconds = 4;

	public bool toggleMusicOn = true;

	public bool scoredBlue = false;
	public bool scoredRed = false;
	public int shakeCounter = 0;
	public bool startLock = true;

	int timeGet;
	bool getTime1, getTime2;
	Random rand = new Random();
	float shakeAmount = 3.0f;
	int rumble1, rumble2, rumble3, rumble4, rumble5, rumble6;

	public float mainScaleX;
	public float mainScaleY;
	//public Sprite extraPuckReflection;

	public Sound backgroundMusic;
	Sound alarmSound;
	public Sound skating;

	public Sound addedPuck;
	public Sound reverseControls;
	public Sound boomShot;
	public Sound goFast;
	public Sound scoreYell;
	public Sound countDown;
	public Sound pickUp;

	public SoundChannel channelOne;
	public SoundChannel channelTwo;

	//initialize game here
	public MyGame() : base(1600, 1200, false)
	{
		start = new StartMenu();
		AddChild(start);

		mainScaleX = game.width / 800.0f;
		mainScaleY = game.height / 600.0f;


		//Music
		backgroundMusic = new Sound("assets/sound/audience.mp3", true, true);

		alarmSound = new Sound("assets/sound/buzzernew.wav", false, false);
		skating = new Sound("assets/sound/skating.ogg", true, false);

		addedPuck = new Sound("assets/sound/whistle.mp3", false, false);
		reverseControls = new Sound("assets/sound/reversed.mp3", false, false);
		boomShot = new Sound("assets/sound/powershot.mp3", false, false);
		goFast = new Sound("assets/sound/speedboost.mp3", false, false);
		countDown = new Sound("assets/sound/countdown.mp3", false, false);
		pickUp = new Sound("assets/sound/pickup.mp3", false, false);

		scoreYell = new Sound("assets/sound/audience_score.mp3", false, true);
	}

	public void Resetti()
	{
		loader.LevelReset();
	}


	void ShakeItUp()
	{
		if (shakeCounter > 0 && getTime2 == false)
		{
			rumble1 = rand.Next(-2, 3);
			rumble2 = rand.Next(-2, 3);
			rumble3 = rand.Next(-2, 3);
			rumble4 = rand.Next(-2, 3);
			rumble5 = rand.Next(-2, 3);
			rumble6 = rand.Next(-2, 3);
			Console.WriteLine("{0} {1} {2} {3}", rumble1, rumble2, rumble3, rumble4);

			Console.WriteLine("CallCheck 1");
			if (shakeCounter > 0 && background != null)
			{
				background.x += shakeAmount * rumble1;
				background.y += shakeAmount * rumble2;
				loader.x += shakeAmount * rumble3;
				loader.y += shakeAmount * rumble4;
				loader.scoreBoard.x += shakeAmount * rumble5;
				loader.scoreBoard.y += shakeAmount * rumble6;
				timeGet = (Time.now / 10) + 3;
				getTime1 = true;
				getTime2 = true;
			}
		}

		if (timeGet < Time.now / 10 && getTime1 == true)
		{
			if (shakeCounter > 0 && background != null)
			{
				background.x -= shakeAmount * 2 * rumble1;
				background.y -= shakeAmount * 2 * rumble2;
				loader.x -= shakeAmount * 2 * rumble3;
				loader.y -= shakeAmount * 2 * rumble4;
				loader.scoreBoard.x -= shakeAmount * 2 * rumble5;
				loader.scoreBoard.y -= shakeAmount * 2 * rumble6;
				Console.WriteLine("CallCheck 3");
				timeGet = (Time.now / 10) + 3;
				getTime1 = false;
				//timeGet = (Time.now / 1000) + 1;
			}
		}

		if (timeGet < Time.now / 10 && getTime2 == true)
		{
			if (shakeCounter > 0 && background != null)
			{
				background.x += shakeAmount * rumble1;
				background.y += shakeAmount * rumble2;
				loader.x += shakeAmount * rumble3;
				loader.y += shakeAmount * rumble4;
				loader.scoreBoard.x += shakeAmount * rumble5;
				loader.scoreBoard.y += shakeAmount * rumble6;
				Console.WriteLine("CallCheck 3");
				getTime2 = false;
				scoredBlue = false;
				scoredRed = false;
				shakeCounter -= 1;
				//timeGet = (Time.now / 1000) + 1;
			}
		}
	}

	//update game here
	void Update()
	{
		ShakeItUp();

		if (scoredBlue == true || scoredRed == true)
		{
			alarmSound.Play();
			scoredBlue = false;
			scoredRed = false;
		}

		//Console.WriteLine(timeGet);

		if (Input.GetKeyDown(Key.TWO) || Input.GetKeyDown(Key.FIVE))
		{
			Resetti();
		}
	}

	//system starts here
	static void Main()
	{
		new MyGame().Start();

	}
}
