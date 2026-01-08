using Godot;
using System;
using System.ComponentModel;

public partial class Maniquin : AnimatedSprite2D
{
	CharacterBody2d characterBody {get; set;}

	protected enum AnimationStatesEnum { Default, Running, Jumping, Falling };
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        characterBody = GetParent<CharacterBody2d>();

		characterBody.CharaterVelocityUpdated += SetMovimentForAnimation;
		
		SetAnimation(AnimationStatesEnum.Default);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		characterBody.HandleFlip(this);
	}

	protected void SetMovimentForAnimation(object seder, Vector2 velocity)
	{
		var isOnFloor = characterBody.IsOnFloor();

		var nextAnimation = AnimationStatesEnum.Default;
		if(isOnFloor && Math.Abs(velocity.X) > 0)
		{
			nextAnimation = AnimationStatesEnum.Running;

		}

		if (!isOnFloor)
		{
			nextAnimation = velocity.Y < 0 ? AnimationStatesEnum.Jumping : AnimationStatesEnum.Falling;
		}		
		
		SetAnimation(nextAnimation);
	}

	protected void SetAnimation(AnimationStatesEnum state)
	{
		Play(state.ToString());
	}
}
