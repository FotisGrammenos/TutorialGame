using Godot;
using System;

public partial class PigEnemy : Area2D
{
	private void OnBodyEntered(CharacterBody2d body)
	{
		body.GetHit();
	}
}
