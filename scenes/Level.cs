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

		_characterAtThisLvl = GetNode<CharacterBody2D>("/root/Node/SceneObjects/CharacterBody2D");
		
		var currentCharacter = _gameManager.GetMainCharacter().Instantiate() as CharacterBody2D;
    
		currentCharacter.Position = _characterAtThisLvl.Position;
		
		AddChild(currentCharacter);
		
		_characterAtThisLvl = currentCharacter;
	}
}
