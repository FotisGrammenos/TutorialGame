using Godot;
using System;

public partial class GameManager : Node
{

	private Label _scoreLabel { get; set; }
	int _points {get; set;} = 0;

	public void AddPoint(int value)
	{
		_points += value; 
		_scoreLabel.Text = $"Points : {_points}";
		GD.Print(_points);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("/root/Node/SceneObjects/UI/Panel/PointsLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
