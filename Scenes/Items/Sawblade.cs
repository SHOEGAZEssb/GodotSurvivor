using Godot;
using GodotSurvivor.Scenes.Enemies;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Player;
using System;
using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// Ability item that rotates around the player
	/// and damages enemies it hits.
	/// </summary>
	public partial class Sawblade : Node2D, IAbility
	{
		#region Properties

		public ItemData Metadata => new("Sawblade", "Rotates around the player and damages enemies", "res://Sprites/Placeholder.png");

		public List<Upgrade> AvailableUpgrades { get; private set; }
		private bool _dividerUpgradeActive = false;

		private PackedScene _sawbladePartScene;

		#region Damage

		/// <summary>
		/// Base damage.
		/// </summary>
		public int DamageBase = 3;

		/// <summary>
		/// Damage multiplier.
		/// </summary>
		public float DamageMultiplier = 1f;

		/// <summary>
		/// Damage the sawblade parts do when hitting an enemy.
		/// </summary>
		public int Damage => (int)Math.Round(DamageBase * DamageMultiplier);

		#endregion Damage

		#region Amount

		/// <summary>
		/// Amount of rotating sawblade parts.
		/// </summary>
		public int Amount
		{
			get => _amount;
			private set
			{
				_amount = value;
				UpdateSawbladeParts();
			}
		}
		private int _amount = 1;

		#endregion Amount

		#region Distance

		/// <summary>
		/// Base distance of the sawblade parts to the player.
		/// </summary>
		public float DistanceBase = 100f;

		/// <summary>
		/// Distance multiplier.
		/// </summary>
		public float DistanceMultiplier = 1f;

		/// <summary>
		/// Distance of the sawblade parts to the player.
		/// </summary>
		public float Distance => DistanceBase * DistanceMultiplier;

		#endregion Distance

		#region Rotation

		/// <summary>
		/// Base sawblade part rotation speed around the player.
		/// </summary>
		public float RotationSpeedBase = 1f;

		/// <summary>
		/// Rotation speed multiplier.
		/// </summary>
		public float RotationSpeedMultiplier = 1f;

		/// <summary>
		/// Sawblade part rotation speed around the player.
		/// </summary>
		public float RotationSpeed => RotationSpeedBase * RotationSpeedMultiplier;

		private float CurrentRotation
		{
			get => _currentRotation;
			set
			{
				if (value > 360)
					value = 0;
				_currentRotation = value;
			}
		}
		private float _currentRotation = 0;

		#endregion Rotation

		#region Size

		/// <summary>
		/// Base size of a sawblade part.
		/// </summary>
		public float SizeBase = 0.2f;

		/// <summary>
		/// Sawblade part size multiplier.
		/// </summary>
		public float SizeMultiplier
		{
			get => _sizeMultiplier;
			set
			{
				_sizeMultiplier = value;
				foreach (var part in _parts)
					part.Scale = new Vector2(SizeBase * SizeMultiplier, SizeBase * SizeMultiplier);
			}
		}
		private float _sizeMultiplier = 1f;

		#endregion Size

		public IDictionary<string, (PackedScene statusScene, float chance)> ApplyableStatuses { get; } = new Dictionary<string, (PackedScene statusScene, float chance)>();

		private readonly List<SawbladePart> _parts = new();

		#endregion Properties

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var player = GetTree().CurrentScene.GetNode<PlayerController>("Player");
			GetParent().RemoveChild(this);
			player.AddChild(this);
			player.PlayerStats.Items.Add(this);

			_sawbladePartScene = ResourceLoader.Load<PackedScene>("res://Scenes/Items/SawbladePart.tscn");
			UpdateSawbladeParts();
			AvailableUpgrades = CreateUpgrades();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			var separation = 360 / _parts.Count;
			for (int i = 0; i < _parts.Count; i++)
			{
				var part = _parts[i];
				part.Position = Position + new Vector2((float)Math.Cos(DegreesToRadians(CurrentRotation + i * separation)),
														 (float)Math.Sin(DegreesToRadians(CurrentRotation + i * separation))) * Distance;
			}

			CurrentRotation += RotationSpeed;
		}

		private void UpdateSawbladeParts()
		{
			int numNewParts = Amount - _parts.Count;
			for (int i = 0; i < numNewParts; i++)
			{
				var part = _sawbladePartScene.Instantiate<SawbladePart>();
				part.OnEnemyHit += Part_OnEnemyHit;
				part.Scale = new Vector2(SizeBase * SizeMultiplier, SizeBase * SizeMultiplier);
				_parts.Add(part);
				AddChild(part);
			}
		}

		private static float DegreesToRadians(float degrees)
		{
			return degrees * (float)Math.PI / 180f;
		}

		private List<Upgrade> CreateUpgrades()
		{
			return new List<Upgrade>()
			{
				new("", "+10% Damage", UpgradeType.Ability, new Action(() => DamageMultiplier += 0.1f)),
				new("", "+1 Sawblade", UpgradeType.Ability, new Action(() => Amount += 1)),
				new("", "+15% Rotation Speed", UpgradeType.Ability, new Action(() => RotationSpeedMultiplier += 0.15f)),
				new("", "+10% Size", UpgradeType.Ability, new Action(() => SizeMultiplier += 0.1f)),
				new("", "Low chance to split enemies in two", UpgradeType.Ability, new Action(() => _dividerUpgradeActive = true), null, true),
			};
		}

		private void Part_OnEnemyHit(object sender, IDamageableByPlayer e)
		{
			if (_dividerUpgradeActive && RandomHelper.HitRandomChance(0.01f) && e is EnemyBase enemy)
			{
				// scale enemy down
				enemy.Scale *= 0.75f;
				enemy.HP /= 2;
				enemy.ExperienceWorth /= 2;
				enemy.BaseTouchDamage /= 2;
				// pack enemy as scene
				var enemyScene = new PackedScene();
				enemyScene.Pack(enemy);

				// instantiate the two new enemies
				var e1 = enemyScene.Instantiate<EnemyBase>();
				var e2 = enemyScene.Instantiate<EnemyBase>();
				e2.Position = new Vector2(e2.Position.X + 1, e2.Position.Y + 1);
				GetTree().CurrentScene.AddChild(e1);
				GetTree().CurrentScene.AddChild(e2);
				enemy.QueueFree();
			}
			else
			{
				DamageHelper.ApplyStatuses(e as Node, ApplyableStatuses);
				var (damage, crit) = DamageHelper.CalculateCrit(Damage, Stats.CurrentStats.CritRate);
				e.TakeDamage(new DamageInfo(damage, crit, DamageSource.Ability, e as Node2D, this));
			}
		}
	}
}
