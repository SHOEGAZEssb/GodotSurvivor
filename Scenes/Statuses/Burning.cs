using Godot;
using GodotSurvivor.Scenes;
using System;

public partial class Burning : Node2D
{
	public int Damage = 1;
	public float Delay = 1f;
	public float? Lifetime;

	private IDamageableByPlayer _target;
	private Timer _delayTimer;
	private Timer _lifetimeTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_target = GetParent() as IDamageableByPlayer;
		_delayTimer = GetNode<Timer>("DelayTimer");
		_delayTimer.WaitTime = Delay;

		_lifetimeTimer = GetNode<Timer>("LifetimeTimer");
		if (Lifetime.HasValue)
		{
			_lifetimeTimer.WaitTime = Lifetime.Value;
			_lifetimeTimer.Start();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_delayTimer.IsStopped())
		{
			_target.TakeDamage((Damage, false));
			_delayTimer.Start();
		}

		if (Lifetime.HasValue && _lifetimeTimer.IsStopped())
			QueueFree();
	}

	public static PackedScene CreateCustomPackedScene(int damage, float delay, float? lifetime = null)
	{
		var burning = ResourceLoader.Load<PackedScene>("res://Scenes/Statuses/Burning.tscn").Instantiate<Burning>();
		burning.Damage = damage;
		burning.Delay = delay;
		burning.Lifetime = lifetime;

		var newScene = new PackedScene();
		newScene.Pack(burning);
		return newScene;
  }
}
