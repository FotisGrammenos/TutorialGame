using Godot;
using System;

public partial class Level : Node
{
	GameManager _gameManager;
	CharacterBody2D _characterAtThisLvl;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>("/root/GameManager");

		//logic for Co-Op
		int intex = 0;
		foreach(var player in GameManager.Players)
		{
			GD.Print($"Player load at level {player.Id}");
			
			PackedScene scene = GD.Load<PackedScene>(player.SelectedCharacterPath);
    		CharacterBody2d currentPlayer = scene.Instantiate<CharacterBody2d>();
			currentPlayer.Name = player.Id.ToString();
			AddChild(currentPlayer);

			var spawnPoints = GetTree().GetNodesInGroup("SpawnPoints");
			foreach(Node2D spawnPoint in spawnPoints)
			{
				if(int.Parse(spawnPoint.Name) == intex)
				{
					currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
				}
			}
			intex++;
		}

		//logic for single player
		//_characterAtThisLvl = GetNode<CharacterBody2D>("/root/Node/SceneObjects/CharacterBody2D");
		//var currentCharacter = _gameManager.GetMainCharacter().Instantiate() as CharacterBody2D;
		//currentCharacter.Position = _characterAtThisLvl.Position;
		//AddChild(currentCharacter);
		//_characterAtThisLvl = currentCharacter;
	}
}
