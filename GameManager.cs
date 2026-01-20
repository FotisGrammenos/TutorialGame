using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{ 
	PackedScene[] _mainCharactersScenes {get; set;}
	PackedScene[] _idleCharactersScenes {get; set;}
	PackedScene _idleSceneOfCharacter {get; set;}
	PackedScene _mainSceneOfCharacter {get; set;} 
	Label _scoreLabel { get; set; }
	int _points {get; set;} = 0;
	int _totalPoints {get; set;}

	public static List<PlayerInfo> Players = new List<PlayerInfo>(); 

	public GameManager()
	{
	 	_mainCharactersScenes = new PackedScene[]
        {
            GD.Load<PackedScene>("res://scenes/main_characters/YellowCharacter.tscn"),
            GD.Load<PackedScene>("res://scenes/main_characters/BlueCharacter.tscn"),
            GD.Load<PackedScene>("res://scenes/main_characters/PinkCharacter.tscn"),
            GD.Load<PackedScene>("res://scenes/main_characters/GreenCharacter.tscn")
        };

		_idleCharactersScenes = new PackedScene[]
        {
            GD.Load<PackedScene>("res://scenes/main_characters/SelectYellow.tscn"),
            GD.Load<PackedScene>("res://scenes/main_characters/SelectBlue.tscn"),
            GD.Load<PackedScene>("res://scenes/main_characters/SelectPink.tscn"),
            GD.Load<PackedScene>("res://scenes/main_characters/SelectGreen.tscn")
        };
	}


	public void AddPoint(int value)
	{
		_scoreLabel = GetNode<Label>("/root/Node/SceneObjects/UI/Panel/PointsLabel");
		_points += value; 
		_scoreLabel.Text = $"Points : {_points}";
		//GD.Print(_points);
	}

	public bool IsAllCollectablesCollected()
    {
        return _points == _totalPoints;
    }

	public void InitCollectables()
	{
		_totalPoints = 4;
		_points = 0;
	}

	public void InitMainCaracter(int choosenCharacter)
	{
		_mainSceneOfCharacter = _mainCharactersScenes[choosenCharacter];
	}

	public PackedScene GetMainCharacter()
	{
		return _mainSceneOfCharacter;
	}

	public void InitIdleSceneOfCharacter(int choosenCharacter)
	{
		_idleSceneOfCharacter = _idleCharactersScenes[choosenCharacter];
	}

	public PackedScene GetIdleSceneCharacter()
	{
		return _idleSceneOfCharacter;	
	}

	public int GetIdleSceneCharactersCount()
	{
		return _idleCharactersScenes.Count();
	}

}
