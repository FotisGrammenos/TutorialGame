using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	[Export] public float Speed = 400.0f;
	[Export] public float JumpVelocity = -900.0f;

	//get the gravity 
	public float Gragvity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public event EventHandler<Vector2> CharaterVelocityUpdated;

    public override void _Ready()
    {
		if(GameManager.CoOpMode) 
        	GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer")
				.SetMultiplayerAuthority(int.Parse(Name));
    }

	public override void _PhysicsProcess(double delta)
	{  
		if(GameManager.CoOpMode)
		{
			if(GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer")
				.GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
			{
				return;
			}
		}

		CheckHorizondalInput(delta);
		CheckVerticalInput(delta);
		EmitCharacterVelocity(Velocity);

    	MoveAndSlide();
	}

	void CheckHorizondalInput(double delta)
	{
		Vector2 velocity = Velocity;

		float verticalDirection = Input.GetAxis("left", "right");
		if (verticalDirection != 0)
		{
			velocity.X = verticalDirection * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		Velocity = velocity;
	}

	void CheckVerticalInput(double delta)
	{
		Vector2 velocity = Velocity;
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += Gragvity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Velocity = velocity;
	}

	public virtual void HandleFlip(AnimatedSprite2D sprite)
    {
        if (Math.Abs(Velocity.X) > 0)
            sprite.FlipH = Math.Sign(Velocity.X) < 0 ;
    }
	protected void EmitCharacterVelocity(Vector2 velocity)
	{
		CharaterVelocityUpdated?.Invoke(this, velocity);
	}
}
