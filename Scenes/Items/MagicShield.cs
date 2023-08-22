using Godot;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Player;
using GodotSurvivor.Scenes.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GodotSurvivor.Scenes.Items
{
	public partial class MagicShield : Area2D, IAbility
	{
		#region Properties

		public ItemData Metadata => new("Magic Shield", "Periodically damages enemies in its range", "res://Sprites/Items/MagicShieldIcon.png");

		#region Size

		/// <summary>
		/// Base size of the shield.
		/// </summary>
		public float SizeBase = 0.5f;

		/// <summary>
		/// Shield size multiplier.
		/// </summary>
		public float SizeMultiplier
		{
			get => _sizeMultiplier;
			set
			{
				_sizeMultiplier = value;
				Scale = new Vector2(SizeBase * SizeMultiplier, SizeBase * SizeMultiplier);
			}
		}
		private float _sizeMultiplier = 1f;

		#endregion Size

		#region Damage

		/// <summary>
		/// Base damage of the shield.
		/// </summary>
		public int DamageBase = 5;

		/// <summary>
		/// Damage multiplier.
		/// </summary>
		public float DamageMultiplier = 1f;

		/// <summary>
		/// Damage per tick.
		/// </summary>
		public int Damage => (int)Math.Round(DamageBase * DamageMultiplier);

		#endregion Damage

		#region Delay

		/// <summary>
		/// Base time between each ticks in seconds.
		/// </summary>
		public float DelayBase = 3f;

		/// <summary>
		/// Delay multiplier.
		/// </summary>
		public float DelayMultiplier
		{
			get => _delayMultiplier;
			set
			{
				_delayMultiplier = value;
				_delayTimer.WaitTime = Delay;
			}
		}
		private float _delayMultiplier = 1f;

		/// <summary>
		/// Time between each ticks in seconds.
		/// </summary>
		public float Delay => DelayBase * DelayMultiplier;

		private Timer _delayTimer;

		#endregion Delay

		public List<Upgrade> AvailableUpgrades { get; private set; }

		public IDictionary<string, (PackedScene statusScene, float chance)> ApplyableStatuses => _applyableStatuses;
		private readonly IDictionary<string, (PackedScene statusScene, float chance)> _applyableStatuses = new Dictionary<string, (PackedScene statusScene, float chance)>();

		#endregion Properties

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var player = GetTree().CurrentScene.GetNode<PlayerController>("Player");
			GetParent().RemoveChild(this);
			player.AddChild(this);
			player.PlayerStats.Items.Add(this);

			AvailableUpgrades = CreateUpgrades();
			_delayTimer = GetNode<Timer>("DelayTimer");
			_delayTimer.WaitTime = Delay;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (_delayTimer.IsStopped())
			{
				var enemies = GetOverlappingBodies().OfType<IDamageableByPlayer>();
				foreach (var enemy in enemies)
				{
					DamageHelper.ApplyStatuses(enemy as Node, ApplyableStatuses);
					var (damage, crit) = DamageHelper.CalculateCrit(Damage, Stats.CurrentStats.CritRate);
					enemy.TakeDamage(new DamageInfo(damage, crit, DamageSource.Ability, enemy as Node2D, this));
				}
				_delayTimer.Start();
			}
		}

		private List<Upgrade> CreateUpgrades()
		{
			return new List<Upgrade>()
			{
				new Upgrade("", "-10% Attack Delay", UpgradeType.Ability, () => DelayMultiplier -= 0.1f),
				new Upgrade("", "+20% Damage", UpgradeType.Ability, () => DamageMultiplier += 0.2f),
				new Upgrade("", "+15% Size", UpgradeType.Ability, () => SizeMultiplier += 0.15f),
				new Upgrade("", "50% Chance to apply Burning", UpgradeType.Ability, () => this.AddApplyableStatus("Burning", 0.5f, Burning.CreateCustomPackedScene(1, 1f)), null, true)
			};
		}
	}
}
