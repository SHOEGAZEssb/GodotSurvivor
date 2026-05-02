using Godot;
using GodotSurvivor.Scenes.Enemies;
using System;

namespace GodotSurvivor.Scenes.Statuses
{
	/// <summary>
	/// Slow status effect that slows down enemies.
	/// </summary>
	public partial class Slow : Node2D
	{
		#region Properties

		/// <summary>
		/// Percent by which to slow enemy down.
		/// </summary>
		[Export]
		public float SlowPercentage = 0.3f;

		/// <summary>
		/// Lifetime of the status in seconds.
		/// </summary>
		[Export]
		public float Lifetime;

		private Timer _lifetimeTimer;

		private EnemyBase _target;
		private float _appliedSlow;

		#endregion Properties

		public override void _Ready()
		{
			_target = GetParentOrNull<EnemyBase>();
			if (_target == null)
				QueueFree();
			else
			{
				_lifetimeTimer = GetNode<Timer>("LifetimeTimer");
				if (Lifetime != 0)
				{
					_lifetimeTimer.WaitTime = Lifetime;
					_lifetimeTimer.Start();
				}

				_appliedSlow = Math.Min(SlowPercentage, _target.SpeedMultiplier);
				_target.SpeedMultiplier -= _appliedSlow;
			}
		}

		public override void _Process(double delta)
		{
			if (Lifetime != 0 && _lifetimeTimer.IsStopped())
			{
				_target.SpeedMultiplier += _appliedSlow;
				QueueFree();
			}
		}

		/// <summary>
		/// Creates a new packed Slow scene with the given values.
		/// </summary>
		/// <param name="slowPercentage">Percent by which to slow enemy down.</param>
		/// <param name="lifetime">Lifetime of the status in seconds.</param>
		/// <returns>new packed Slow scene.</returns>
		public static PackedScene CreateCustomPackedScene(float slowPercentage, float lifetime = 0)
		{
			var slow = ResourceLoader.Load<PackedScene>("res://Scenes/Statuses/Slow.tscn").Instantiate<Slow>();
			slow.SlowPercentage = slowPercentage;
			slow.Lifetime = lifetime;

			var newScene = new PackedScene();
			newScene.Pack(slow);
			return newScene;
		}
	}
}
