using Godot;
using System;

public partial class DamageTextManager : Node2D
{
	[Export]
	public Vector2 Travel = new(0, -80);

	[Export]
	public float Duration = 0.7f;

	[Export]
	public double Spread = Math.PI / 2;

	private PackedScene _damageText;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_damageText = ResourceLoader.Load<PackedScene>("res://Scenes/DamageText.tscn");
	}

	public void ShowFloatingText(int value, Vector2 position, bool crit)
	{
		var dt = _damageText.Instantiate<DamageText>();
		GetTree().Root.AddChild(dt);
		//AddChild(dt);
		dt.ShowValue(value.ToString(), position, Travel, Duration, Spread, crit);
	}
}
