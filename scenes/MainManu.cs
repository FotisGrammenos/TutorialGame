using Godot;
using System;
using System.Linq;

public partial class MainManu : Node
{
	Node2D _characterPreview;
    int _currentCharacterIndex;
	GameManager _gameManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameManager = GetNode<GameManager>("/root/GameManager");
		_characterPreview = GetNode<Node2D>("Node2D");

		_currentCharacterIndex = 0;
		_gameManager.InitIdleSceneOfCharacter(_currentCharacterIndex);

		LoadCharacter();
	}
	 
    private void PeerDisconnected(long id)
    {
		GD.Print($"Player disconected {id}");
    }

    private void PeerConnected(long id)
    {
		GD.Print($"Player connected {id}");
    }

	private void ConnectedToServer()
	{
		GD.Print("Connected to server");
		RpcId(1,
			"SentPlayerInforamtion",
			Multiplayer.GetUniqueId(),
			_gameManager.GetMainCharacter().ResourcePath);
	}

    private void ConnectionFailed()
    {
		GD.Print("Connected FAIL to server");
	}

    public void OnPressedButtonLevel1()
	{
		GetTree().ChangeSceneToFile("res://scenes/Level1.tscn");
		_gameManager.InitCollectables();

		_gameManager.InitMainCaracter(_currentCharacterIndex);
	}

	public void OnPressedButtonLevel2()
	{
		GetTree().ChangeSceneToFile("res://scenes/Level2.tscn");
		_gameManager.InitCollectables();

		_gameManager.InitMainCaracter(_currentCharacterIndex);
	}

	public void OnNextCharacterPressed()
	{
		_currentCharacterIndex++;
		if(_currentCharacterIndex >= _gameManager.GetIdleSceneCharactersCount())
		{
			_currentCharacterIndex = 0;
		}

		_gameManager.InitIdleSceneOfCharacter(_currentCharacterIndex);
		LoadCharacter();
	}

	private void LoadCharacter()
    {
        _characterPreview?.QueueFree();

		var currentCharacter = _gameManager.GetIdleSceneCharacter().Instantiate() as Node2D;
    
		AddChild(currentCharacter);

		currentCharacter.Position = _characterPreview.Position;

		_characterPreview = currentCharacter;

		var sprite2D =_characterPreview.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite2D.Play("Default");
    }

}
