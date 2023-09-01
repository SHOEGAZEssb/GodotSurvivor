using Godot;

namespace GodotSurvivor.Scenes.Statuses
{
	/// <summary>
	/// Burning status effect that deals damage over time.
	/// </summary>
	public partial class Burning : Node2D
	{
		/// <summary>
		/// Damage per tick.
		/// </summary>
		[Export]
		public int Damage = 1;

		/// <summary>
		/// Time between ticks in seconds.
		/// </summary>
		[Export]
		public float Delay = 1f;

		/// <summary>
		/// Lifetime of the status in seconds.
		/// </summary>
		[Export]
		public float Lifetime;

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
			if (Lifetime != 0)
			{
				_lifetimeTimer.WaitTime = Lifetime;
				_lifetimeTimer.Start();
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (_delayTimer.IsStopped())
			{
				_target.TakeDamage(new DamageInfo(Damage, false, DamageSource.Burning, _target as Node2D, this));
				_delayTimer.Start();
			}

			if (Lifetime != 0 && _lifetimeTimer.IsStopped())
				QueueFree();
		}

		/// <summary>
		/// Creates a new packed burning scene with the given values.
		/// </summary>
		/// <param name="damage">Damage per tick.</param>
		/// <param name="delay">Delay between each tick in seconds.</param>
		/// <param name="lifetime">Lifetime in seconds.</param>
		/// <returns>New custom packed Burning scene.</returns>
		public static PackedScene CreateCustomPackedScene(int damage, float delay, float lifetime = 0)
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
}
