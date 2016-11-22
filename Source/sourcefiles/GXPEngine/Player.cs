﻿using System;
using GXPEngine;

namespace GXPEngine
{
	public class Player : Sprite
	{
		public float SpeedX, SpeedY;
		public float SpeedMultiplierX = 0.4f;
		public float SpeedMultiplierY = 0.4f;
		float Friction = 0.9f;
		public float speedLimit = 12.0f;
		public float ForceMultiplier = 1.0f;
		string colour;
		//bool hittingGoal = false;
		public bool touchingGoal = false;
		public bool InversedControls = false;
		public float StartX, StartY;
		Random puckRange = new Random();

		int left, right, up, down;

		public Player(int rLeft, int rRight, int rUp, int rDown, string rColour) : base("colors.png")
		{
			SetOrigin(width / 2, height / 2);

			this.left = rLeft;
			this.right = rRight;
			this.up = rUp;
			this.down = rDown;

			colour = rColour;

			Sprite reflection = new Sprite("colors.png");
			this.AddChild(reflection);
			reflection.SetOrigin(width / 2, height / 2);
			if (colour == "blue"){
				reflection.color = 0x4040FF;
			}
			if (colour == "red"){
				reflection.color = 0xFF4040;
			}
			reflection.scaleY = -0.75f;
			reflection.alpha = 0.25f;
			reflection.SetXY(0, 56);

		}

		public void Reset()
		{
			SpeedX = 0.0f;
			SpeedY = 0.0f;
		}

		public void Impulse(float ImpulseX, float ImpulseY)
		{
			SpeedX = ImpulseX;
			SpeedY = ImpulseY;
		}

		void Update()
		{
			if (InversedControls == false)
			{
				if (Input.GetKey(left))
				{ SpeedX -= SpeedMultiplierX; }
				if (Input.GetKey(right))
				{ SpeedX += SpeedMultiplierX; }
				if (Input.GetKey(up))
				{ SpeedY -= SpeedMultiplierY; }
				if (Input.GetKey(down))
				{ SpeedY += SpeedMultiplierY; }
			}
			else {
				if (Input.GetKey(right))
				{ SpeedX -= SpeedMultiplierX; }
				if (Input.GetKey(left))
				{ SpeedX += SpeedMultiplierX; }
				if (Input.GetKey(down))
				{ SpeedY -= SpeedMultiplierY; }
				if (Input.GetKey(up))
				{ SpeedY += SpeedMultiplierY; }
			}

			if (this.x - width / 2 <= 0){
				SpeedX = 1.0f + Mathf.Abs(SpeedX / 2.0f);
			}
			if (this.x + width/2 >= game.width){
				SpeedX = -1.0f -Mathf.Abs(SpeedX/2.0f);
			}
			if (this.y -height/2 <= 0 + (game.height * 0.16f)){
				SpeedY = 1.0f + Mathf.Abs(SpeedY/2.0f);
			}
			if (this.y + height/2 >= game.height){
				SpeedY = -1.0f -Mathf.Abs(SpeedY/2.0f);
			}

			if (SpeedX > speedLimit){
				SpeedX = speedLimit;
			}
			if (SpeedX < -speedLimit){
				SpeedX = -speedLimit;
			}

			if (SpeedY > speedLimit){
				SpeedY = speedLimit;
			}

			if (SpeedY < -speedLimit){
				SpeedY = -speedLimit;
			}

			x += SpeedX;
			y += SpeedY;

			SpeedX *= Friction;
			SpeedY *= Friction;

			if (SpeedX < 0.1f && SpeedX > -0.1f){
				SpeedX = 0.0f;
			}

			if (SpeedY < 0.1f && SpeedY > -0.1f){
				SpeedY = 0.0f;
			}

			this.SpeedMultiplierX = 0.4f;
			this.SpeedMultiplierY = 0.4f;
			Friction = 0.97f;
			this.touchingGoal = false;

			Console.WriteLine(ForceMultiplier);

			foreach (GameObject other in GetCollisions())
			{
				if (other is Puck)
				{
					//int _puckRange = puckRange.Next(-2, 2);

					Puck puck = other as Puck;
					puck.Impulse((this.SpeedX * ForceMultiplier) - this.Friction, (this.SpeedY * ForceMultiplier) - this.Friction);
					puck.x += this.SpeedX;
					puck.y += this.SpeedY;
					this.ForceMultiplier = 1.0f;
					//puck.SpeedMultiplierX *= -0.1f;
					//.SpeedMultiplierY *= -0.1f;
				}

				if (other is AntiGoal)
				{
					AntiGoal notGoal = other as AntiGoal;
					this.SpeedX -= SpeedX * 2.0f;
					this.SpeedY -= SpeedY * 2.0f;
					this.SpeedMultiplierX = -0.1f;
					this.SpeedMultiplierY = -0.1f;
					this.Friction = 0.8f;
					this.touchingGoal = true;
					//goal.Impulse(this.SpeedX, this.SpeedY);
					//goal.x += this.SpeedX * 1.1f;
					//goal.y += this.SpeedY * 1.1f;


					//this.Impulse(-(this.SpeedX*1.1f), -(this.SpeedY*1.1f));
					//this.x -= this.SpeedX * 10.0f;
					//this.y -= this.SpeedY * 10.0f;
				}

				if (other is Player)
				{
					Player player = other as Player;
					Console.WriteLine(!this.matrix.Equals(player));

					if (player.touchingGoal == false)
					{
						player.Impulse(this.SpeedX, this.SpeedY);
						player.x += this.SpeedX;
						player.y += this.SpeedY;
						this.SpeedMultiplierX *= -0.1f;
						this.SpeedMultiplierY *= -0.1f;
						this.SpeedX -= SpeedX * 2.5f;
						this.SpeedY -= SpeedY * 2.5f;
						//this.Impulse(-player.SpeedX, -player.SpeedY);
					}
					else {
						player.Impulse(-this.SpeedX * 0.5f, -this.SpeedY* 0.5f);
						this.SpeedMultiplierX = 0.1f;
						this.SpeedMultiplierY = 0.1f;
						this.SpeedX -= SpeedX * 25.5f;
						this.SpeedY -= SpeedY * 25.5f;
						this.Impulse(-this.SpeedX * 43, -this.SpeedY*44);
					}

				}
			}

			//Console.WriteLine("{0} {1} {2}", SpeedX, SpeedY, Math.Pow(2.0, 3.0));
			//Console.WriteLine(this.matrix.Equals(player));
		}
	}
}
