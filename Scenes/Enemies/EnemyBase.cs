using Godot;
using GodotSurvivor.Scenes.Pickups;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.Enemies
{
	/// <summary>
	/// Base class for most, if not all enemies.
	/// </summary>
	public partial class EnemyBase : CharacterBody2D, IDamageableByPlayer
	{
		/// <summary>
		/// Base movement speed.
		/// </summary>
		[Export]
		public float SpeedBase = 70f;

		[Export]
		public float SpeedMultiplier = 1f;

		public float Speed => SpeedBase * SpeedMultiplier;

		/// <summary>
		/// Current hp.
		/// </summary>
		[Export]
		public int HP = 20;

		[Export]
		public int BaseTouchDamage = 5;

		/// <summary>
		/// Amount of experience this enemy drops on death.
		/// </summary>
		[Export]
		public int ExperienceWorth = 1;

		/// <summary>
		/// Signal name of the <see cref="OnTakeDamageEventHandler"/>.
		/// (Workaround for interfaces not having events)
		/// </summary>
		public string OnTakeDamageSignalName => SignalName.OnTakeDamage;

		/// <summary>
		/// Event that is fired when this enemy takes damage.
		/// </summary>
		[Signal]
		public delegate void OnTakeDamageEventHandler();

		protected PlayerController _player;
		protected AnimatedSprite2D _sprite;
		protected DamageTextManager _damageTextManager;

		private PackedScene _expCrystalScene;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_player = GetTree().CurrentScene.GetNode<PlayerController>("Player");
			_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			_damageTextManager = GetNode<DamageTextManager>("DamageTextManager");
			_expCrystalScene = ResourceLoader.Load<PackedScene>("res://Scenes/Pickups/ExpCrystal.tscn");
		}

		/// <summary>
		/// Damage this enemy.
		/// </summary>
		/// <param name="damageInfo">Info of the damage.</param>
		public void TakeDamage(DamageInfo damageInfo)
		{
			HP -= damageInfo.Damage;
			_damageTextManager.ShowFloatingText(damageInfo.Damage, Position, damageInfo.Crit);
			Stats.CurrentStats.OnEnemyDamaged(damageInfo);
			EmitSignal(SignalName.OnTakeDamage);
			if (HP <= 0)
			{
				OnDeath(damageInfo);
				QueueFree();
			}
		}

		/// <summary>
		/// Drops experience when this enemy is killed.
		/// </summary>
		/// <param name="damageInfo">Damage info that lead to the death.</param>
		protected virtual void OnDeath(DamageInfo damageInfo)
		{
			Stats.CurrentStats.OnEnemyKilled(damageInfo);
			var expCrystal = _expCrystalScene.Instantiate<ExpCrystal>();
			expCrystal.Experience = ExperienceWorth; // ExpGainMultiplier will be added in the Stats
			GetTree().CurrentScene.CallDeferred("add_child", expCrystal);
			expCrystal.Position = GlobalPosition;
		}

		protected virtual void OnAreaEntered(Area2D other)
		{
			if (other is PlayerController player)
				player.TakeDamage(BaseTouchDamage);
		}
	}
}
