using Godot;
using System;

public partial class MainManu : Node
{
	GameManager _gameManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>("/root/Node/GameManager");
	}
	
    public void OnPressedButtonLevel1()
	{
		GetTree().ChangeSceneToFile("res://scenes/Level1.tscn");
		_gameManager.InitCollectables();
	}

	public void OnPressedButtonLevel2()
	{
		GetTree().ChangeSceneToFile("res://scenes/Level2.tscn");
		_gameManager.InitCollectables();
	}

}
