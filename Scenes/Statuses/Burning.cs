using Godot;
using GodotSurvivor.Scenes;
using System;

public partial class Burning : Node2D
{
	public int Damage;
	public float Delay;
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

		if (_lifetimeTimer.IsStopped())
			QueueFree();
	}
}
