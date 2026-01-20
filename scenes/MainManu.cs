using Godot;
using System;
using System.Linq;

public partial class MainManu : Node
{
	[Export] 
	private int port = 8910;

	[Export]
	private string adress = "127.0.0.1";

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
		_gameManager.InitMainCaracter(_currentCharacterIndex);

		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
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

		//_gameManager.InitMainCaracter(_currentCharacterIndex);
	}

	public void OnPressedButtonLevel2()
	{
		GetTree().ChangeSceneToFile("res://scenes/Level2.tscn");
		_gameManager.InitCollectables();

		//_gameManager.InitMainCaracter(_currentCharacterIndex);
	}

	public void OnNextCharacterPressed()
	{
		_currentCharacterIndex++;
		if(_currentCharacterIndex >= _gameManager.GetIdleSceneCharactersCount())
		{
			_currentCharacterIndex = 0;
		}

		_gameManager.InitIdleSceneOfCharacter(_currentCharacterIndex);
		_gameManager.InitMainCaracter(_currentCharacterIndex);
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

	public void OnHostButtonDown()
	{
		ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
		var error = peer.CreateServer(port, 2);
		if(error != Error.Ok)
		{
			GD.Print($"Error :{error}");
			return;
		}

		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);

		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Waitning For Players");
		SentPlayerInforamtion(1, _gameManager.GetMainCharacter().ResourcePath);
	}

	public void OnJoinButtonDown()
	{
		ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
		peer.CreateClient(adress, port);
		
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);

		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Joining Game");
	}

	public void OnStartCoOpGamePressed()
	{
		Rpc("GameCoOpStart");
	}
	
	[Rpc(
		MultiplayerApi.RpcMode.AnyPeer, 
		CallLocal = true, 
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)
	]
	private void GameCoOpStart()
	{
		foreach(var player in GameManager.Players)
		{
			GD.Print($"Playing  {player.Id}");
		}

		//var scene = ResourceLoader
		//	.Load<PackedScene>("res://scenes/LevelCoOp.tscn")
		//	.Instantiate<Level>();
		
		//GetTree().ChangeSceneToFile(scene);

		//_gameManager.InitCollectables();

		//_gameManager.InitMainCaracter(_currentCharacterIndex);
		
		GetTree().ChangeSceneToFile("res://scenes/LevelCoOp.tscn");

		//GetTree().Root.AddChild(scene);
		
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SentPlayerInforamtion(int id, string scene)
	{
		GD.Print($"MPIKA SentPlayerInforamtion {id}");

		PlayerInfo playerInfo = new PlayerInfo()
		{
			Id = id,
			Name = $"player{id}",
			SelectedCharacterPath = scene
		};
		if(GameManager.Players.FirstOrDefault(p => p.Id == playerInfo.Id) is null)
		{
			GameManager.Players.Add(playerInfo);
		}

		if (Multiplayer.IsServer())
		{
			foreach(var item in GameManager.Players)
			{
				GD.Print("ForeachAt SentPlayerInforamtion");
				Rpc("SentPlayerInforamtion", item.Id, item.SelectedCharacterPath);
			}
		}

	}
}
