using Godot;
using GodotSurvivor.Scenes.Pickups;
using GodotSurvivor.Scenes.Weapons;

namespace GodotSurvivor.Scenes.Player
{
	public partial class PlayerController : Area2D, IDamageableByEnemy
	{
		#region Properties

		[Export]
		public Stats PlayerStats;

		public WeaponBase CurrentWeapon => _pistol;

		public string OnTakeDamageSignalName => SignalName.OnTakeDamage;

		#endregion Properties

		[Signal]
		public delegate void ExpGainedEventHandler();

		[Signal]
		public delegate void LevelGainedEventHandler();

		[Signal]
		public delegate void OnTakeDamageEventHandler();

		private AnimatedSprite2D _sprite;
		private Node2D _weaponPosition;
		private PackedScene _pistolScene;
		private WeaponBase _pistol;
		private ItemPickupRange _pickupRange;
		private Timer _invincibilityTimer;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			PlayerStats = new Stats(movementSpeedBase: 100f, pickupRadiusBase: 50f, maxHPBase: 30, critRateBase: 0.05f);
			Stats.CurrentStats = PlayerStats;

			// get weapons
			_weaponPosition = GetNode<Node2D>("Weapon");
			_pistolScene = ResourceLoader.Load<PackedScene>("res://Scenes/Weapons/Pistol.tscn");

			_pistol = _pistolScene.Instantiate<WeaponBase>();
			_pistol.Position = _weaponPosition.Position;
			AddChild(_pistol);
			_pistol.Owner = Owner;

			_invincibilityTimer = GetNode<Timer>("InvincibilityTimer");

			_pickupRange = GetNode<ItemPickupRange>("ItemPickupRange");
			_pickupRange.PickupRadius = PlayerStats.PickupRadius;
		}

		public override void _PhysicsProcess(double delta)
		{
			if (Input.IsKeyPressed(Key.Shift))
				PlayerStats.CurrentExperience = PlayerStats.ExperienceToNextLevel;

			var velocity = GetInput();
			AnimatePlayer(velocity);
			Position += velocity * (float)delta;
		}

		public void GainExperience(int experience)
		{
			PlayerStats.CurrentExperience += experience;
		}

		public void TakeDamage(int damage)
		{
			if (_invincibilityTimer.IsStopped())
			{
				_invincibilityTimer.Start();
				PlayerStats.CurrentHP -= damage;
				EmitSignal(SignalName.OnTakeDamage);
				//if (CurrentHP <= 0)
				//	QueueFree();
			}
		}

		private Vector2 GetInput()
		{
			var velocity = new Vector2();

			if (Input.IsActionPressed("right"))
				velocity.X += 1;
			if (Input.IsActionPressed("left"))
				velocity.X -= 1;
			if (Input.IsActionPressed("down"))
				velocity.Y += 1;
			if (Input.IsActionPressed("up"))
				velocity.Y -= 1;

			return velocity.Normalized() * PlayerStats.MovementSpeed;
		}

		private void AnimatePlayer(Vector2 velocity)
		{
			// right and left
			if (velocity.X >= 1)
				_sprite.Play("walk_right");
			else if (velocity.X <= -1)
				_sprite.Play("walk_left");
			else if (velocity.Y != 0)
			{
				// up and down
				// dependant on last walk
				if (velocity.Y != 0)
				{
					if (_sprite.Animation == "walk_right" || _sprite.Animation == "walk_left")
						return;
					else if (_sprite.Animation == "idle_right")
						_sprite.Play("walk_right");
					else
						_sprite.Play("walk_left");
				}
			}
			else
			{
				// idle
				if (_sprite.Animation == "walk_right")
					_sprite.Play("idle_right");
				else if (_sprite.Animation == "walk_left")
					_sprite.Play("idle_left");
			}
		}
	}
}
