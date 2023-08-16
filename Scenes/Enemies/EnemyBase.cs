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

		public void TakeDamage((int damage, bool crit) damageInfo)
		{
			HP -= damageInfo.damage;
			_damageTextManager.ShowFloatingText(damageInfo.damage, Position, damageInfo.crit);
			EmitSignal(SignalName.OnTakeDamage);
			if (HP <= 0)
			{
				OnDeath();
				QueueFree();
			}
		}

		protected virtual void OnDeath()
		{
			var expCrystal = _expCrystalScene.Instantiate<ExpCrystal>();
			expCrystal.Experience = ExperienceWorth; // ExpGainMultiplier will be added in the Stats
			GetTree().CurrentScene.AddChild(expCrystal);
			expCrystal.Position = GlobalPosition;
			Stats.CurrentStats.NumKilledEnemies += 1;
		}

		protected virtual void OnAreaEntered(Area2D other)
		{
			if (other is PlayerController player)
				player.TakeDamage(BaseTouchDamage);
		}
	}
}
