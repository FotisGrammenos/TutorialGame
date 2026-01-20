using Godot;
using System.Collections.Generic;

public partial class FinishCheckPoint : Area2D
{
	[Export]
    public string NextScenePath = "";
	GameManager _gameManager;

	public void OnEnterFinishCheckPoint(CharacterBody2d body)
	{

		//_gameManager.ResetScore();
		if (_gameManager.IsAllCollectablesCollected())
		{
			GetTree().ChangeSceneToFile(NextScenePath);
			if(NextScenePath == "res://scenes/MainManu.tscn")
			{
				GameManager.Players = new List<PlayerInfo>();
			}
		}
		else
		{
			GetTree().ChangeSceneToFile(GetTree().CurrentScene.SceneFilePath);
		}

		_gameManager.InitCollectables();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>("/root/GameManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
