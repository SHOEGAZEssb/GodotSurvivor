using Godot;
using GodotSurvivor.Scenes.Pickups;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.Enemies
{
	public partial class EnemyBase : CharacterBody2D, IDamageableByPlayer
	{
		[Export]
		public int Speed = 80;

		[Export]
		public int HP = 20;

		[Export]
		public int BaseTouchDamage = 5;

		[Export]
		public int ExperienceWorth = 1;

		public string OnTakeDamageSignalName => SignalName.OnTakeDamage;

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

		protected virtual void OnDeath(DamageInfo damageInfo)
		{
			Stats.CurrentStats.OnEnemyKilled(damageInfo);
			var expCrystal = _expCrystalScene.Instantiate<ExpCrystal>();
			expCrystal.Experience = ExperienceWorth; // ExpGainMultiplier will be added in the Stats
			GetTree().CurrentScene.AddChild(expCrystal);
			expCrystal.Position = GlobalPosition;
		}

		protected virtual void OnAreaEntered(Area2D other)
		{
			if (other is PlayerController player)
				player.TakeDamage(BaseTouchDamage);
		}
	}
}
