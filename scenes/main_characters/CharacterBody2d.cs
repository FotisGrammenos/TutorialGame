using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	[Export] public float Speed = 400.0f;
	[Export] public float JumpVelocity = -900.0f;
	[Export] public int AttackDamage = 1;
	  
	public float Gragvity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public event EventHandler<Vector2> CharaterVelocityUpdated;
	public event EventHandler<int> AttackHandler;
	public event EventHandler<int> GetHitHandler;
	
	bool _isAttacking {get; set;} = false;
	bool _isGettingHit {get; set;} = false;
	AnimationPlayer _animationPlayer {get; set;}
	Area2D _attackingArea {get; set;} 

    public override void _Ready()
    {
<<<<<<< HEAD
        GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer")
			.SetMultiplayerAuthority(int.Parse(Name));
    }

	public override void _PhysicsProcess(double delta)
	{  
		if(GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer")
			.GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
=======
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    	_animationPlayer.AnimationFinished += OnAnimationPlayerAnimationFinished;

		_attackingArea = GetNode<Area2D>("AttackArea");

	}

    public override void _PhysicsProcess(double delta)
	{  
		Attack();
		if (_isAttacking || _isGettingHit)
>>>>>>> 560aa9bde024cd65d8fd7e1e2e00051cd0fc0289
		{
			return;
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
		{
            sprite.FlipH = Math.Sign(Velocity.X) < 0 ;
			_attackingArea.Scale = new Vector2(
				Math.Sign(Velocity.X) < 0 ? -1 : 1, 1);
		}
    }

	public void Attack()
	{
		if (Input.IsActionJustPressed("attack"))
		{
			var itemsOverlap = _attackingArea.GetOverlappingAreas();

			foreach(var item in itemsOverlap)
			{
				var node = item.GetParent();
				GD.Print(item.Name);
				item.QueueFree();
				OnAnimationPlayerAnimationFinished("Hit");
			}

			_isAttacking = true;
			AttackHandler?.Invoke(this, AttackDamage);
		}
	}

	public void GetHit()
	{
		_isGettingHit = true;
		GetHitHandler?.Invoke(this, AttackDamage);
	}

	protected void EmitCharacterVelocity(Vector2 velocity)
	{
		CharaterVelocityUpdated?.Invoke(this, velocity);
	}

	public void OnAnimationPlayerAnimationFinished(StringName animName)
    {
        if(animName == "Attacking")
		{
			_isAttacking = false;
		}
		
		if (animName == "Hit")
		{
			_isGettingHit = false;
		}
    }
}
