using Godot;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Player;
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

		public float SizeBase = 0.5f;

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

		public int DamageBase = 5;

		public float DamageMultiplier = 1f;

		public int Damage => (int)Math.Round(DamageBase * DamageMultiplier);

		#endregion Damage

		#region Delay

		public float DelayBase = 3f;

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
					enemy.TakeDamage(DamageHelper.CalculateCrit(Damage, Stats.CurrentStats.CritRate));
				_delayTimer.Start();
			}
		}

		private List<Upgrade> CreateUpgrades()
		{
			return new List<Upgrade>()
			{
				new Upgrade("", "-10% Attack Delay", UpgradeType.Ability, () => DelayMultiplier -= 0.1f),
				new Upgrade("", "+20% Damage", UpgradeType.Ability, () => DamageMultiplier += 0.2f),
				new Upgrade("", "+15% Size", UpgradeType.Ability, () => SizeMultiplier += 0.15f)
			};
		}
	}
}
