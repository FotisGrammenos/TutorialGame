using Godot;
using System;

public partial class Collectable : Area2D
{

	GameManager _gameManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>("/root/Node/GameManager");
	}

	private void OnBodyEntered(CharacterBody2d body)
	{
		_gameManager.AddPoint(1);
		this.QueueFree();
	}
}
