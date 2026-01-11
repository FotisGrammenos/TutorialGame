using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{ 
	private Label _scoreLabel { get; set; }
	int _points {get; set;} = 0;
	int _totalPoints {get; set;}

	public void AddPoint(int value)
	{
		_points += value; 
		_scoreLabel.Text = $"Points : {_points}";
		GD.Print(_points);
	}

	public bool IsAllCollectablesCollected()
    {
        return _points == _totalPoints;
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("/root/Node/SceneObjects/UI/Panel/PointsLabel");
		InitCollectables();
	}

	public void InitCollectables()
	{
		_totalPoints = 4;

		//var level = GetTree().CurrentScene;

		//var list = new List<Node>();

        //foreach (Node child in level.GetChildren())
        //{
            // If the child is a Collectable script
            //if (child is Collectable)
            //{
            //    list.Add(child);
            //}
        //}

		//GD.Print($"Found {list.Count} collectables");

		//foreach (var n in list)
		//	GD.Print($" - {n.Name} ({n.GetType()})");

		//_totalPoints = list.Count;

		//GD.Print($"Total collectables: {_totalPoints}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
