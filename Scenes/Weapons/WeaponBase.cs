using Godot;

namespace GodotSurvivor.Scenes.Weapons
{
	public partial class WeaponBase : Sprite2D
	{
		[Export]
		public float BaseDelay = 3f;

		public float DelayMultiplier
		{
			get => _delayMultiplier;
			set
			{
				_delayMultiplier = value;
				_timer.WaitTime = (1000 / BaseDelay * DelayMultiplier) / 1000;
			}
		}
		private float _delayMultiplier = 1f;

		private Timer _timer;

		public override void _Ready()
		{
			_timer = GetNode<Timer>("Timer");
			_timer.WaitTime = 1000 / BaseDelay / 1000;
		}

		public override void _PhysicsProcess(double delta)
		{
			LookAt(GetGlobalMousePosition());
		}

		public override void _Process(double delta)
		{
			if (_timer.IsStopped() && Input.IsActionPressed("shoot"))
			{
				_timer.Start();
				Shoot();
			}
		}

		protected virtual void Shoot()
		{
			GD.PrintErr("This method should never be called");
		}
	}
}
