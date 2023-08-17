using Godot;
using GodotSurvivor.Scenes.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GodotSurvivor.Scenes.Player
{
	/// <summary>
	/// The stats of a run.
	/// </summary>
	public partial class Stats : Node
	{
		public static Stats CurrentStats { get; set; }

		#region Player Stats

		#region Movement Speed

		[Export]
		public float MovementSpeedBase = 100;

		public float MovementSpeed => (float)Math.Ceiling(MovementSpeedBase * MovementSpeedMultiplier);

		public float MovementSpeedMultiplier = 1f;

		#endregion Movement Speed

		#region Pickup Radius

		[Export]
		public float PickupRadiusBase = 50f;

		public float PickupRadius => (float)Math.Ceiling(PickupRadiusBase * PickupRadiusMultiplier);

		public float PickupRadiusMultiplier
		{
			get => _pickupRadiusMultiplier;
			set
			{
				_pickupRadiusMultiplier = value;
				EmitSignal(SignalName.PickupRadiusChanged);
			}
		}
		private float _pickupRadiusMultiplier = 1f;

		[Signal]
		public delegate void PickupRadiusChangedEventHandler();

		#endregion Pickup Radius

		#region HP

		[Export]
		public int MaxHPBase;

		public int MaxHP => (int)Math.Ceiling(MaxHPBase * MaxHPMultiplier);

		public float MaxHPMultiplier
		{
			get => _maxHPMultiplier;
			set
			{
				_maxHPMultiplier = value;
				EmitSignal(SignalName.MaxHPChanged);
				if (MaxHP < CurrentHP)
					CurrentHP = MaxHP;
			}
		}
		private float _maxHPMultiplier = 1f;

		[Signal]
		public delegate void MaxHPChangedEventHandler();

		[Export]
		public int CurrentHP
		{
			get => _currentHP;
			set
			{
				_currentHP = value;
				EmitSignal(SignalName.CurrentHPChanged);
			}
		}
		private int _currentHP;

		[Signal]
		public delegate void CurrentHPChangedEventHandler();

		#endregion HP

		#region Defense

		[Export]
		public int DefenseBase;

		public int Defense => (int)Math.Round(DefenseBase * DefenseMultiplier);

		[Export]
		public float DefenseMultiplier
		{
			get => _defenseMultiplier;
			set
			{
				_defenseMultiplier = value;
			}
		}
		private float _defenseMultiplier;

		#endregion Defense

		#region Crit Rate

		public float CritRateBase { get; }

		public float CritRateBonus
		{
			get => _critRateBonus;
			set
			{
				_critRateBonus = value;
			}
		}
		private float _critRateBonus = 0f;

		public float CritRate => CritRateBase + CritRateBonus;

		#endregion Crit Rate

		#region Experience / Level

		[Export]
		public int CurrentExperience
		{
			get => _currentExperience;
			set
			{
				_currentExperience = (int)Math.Ceiling(value * ExperienceGainMultiplier);
				EmitSignal(SignalName.ExpGained);
				while (CurrentExperience >= ExperienceToNextLevel)
				{
					ExperienceToNextLevel = (int)Math.Ceiling(ExperienceToNextLevel * 1.3);
					Level += 1;
				}
			}
		}
		private int _currentExperience = 0;

		[Export]
		public float ExperienceGainMultiplier
		{
			get => _experienceGainMultiplier;
			set
			{
				_experienceGainMultiplier = value;
			}
		}
		private float _experienceGainMultiplier = 1f;

		[Signal]
		public delegate void ExpGainedEventHandler();

		[Export]
		public int ExperienceToNextLevel = 10;

		[Export]
		public int Level
		{
			get => _level;
			set
			{
				_level = value;
				EmitSignal(SignalName.LevelChanged);
			}
		}
		private int _level = 1;

		[Signal]
		public delegate void LevelChangedEventHandler();

		#endregion Experience / Level

		#region Upgrades

		public List<Upgrade> AvailableUpgrades
		{
			get
			{
				IEnumerable<Upgrade> list = _availablePlayerUpgrades;
				foreach (var ability in Items.OfType<IAbility>())
					list = list.Concat(ability.AvailableUpgrades);

				return list.Where(u => u.IsApplicable && (!u.Unique || !ChosenUpgrades.Contains(u))).ToList();
			}
		}
		private readonly List<Upgrade> _availablePlayerUpgrades;

		public List<Upgrade> ChosenUpgrades { get; } = new List<Upgrade>();

		#endregion Upgrades

		#endregion Player Stats

		#region Items

		public List<IItem> Items { get; } = new List<IItem>();

		public List<PackedScene> ItemPool { get; }

		#endregion Items

		#region Run Stats

		public int NumKilledEnemies
		{
			get => _numKilledEnemies;
			set
			{
				_numKilledEnemies = value;
				EmitSignal(SignalName.NumKilledEnemiesChanged);
			}
		}
		private int _numKilledEnemies = 0;

		[Signal]
		public delegate void NumKilledEnemiesChangedEventHandler();

		#endregion Run Stats

		#region Construction

		public Stats(float movementSpeedBase, float pickupRadiusBase, int maxHPBase, float critRateBase, float experienceGainMultiplier = 1f)
		{
			MovementSpeedBase = movementSpeedBase;
			PickupRadiusBase = pickupRadiusBase;
			MaxHPBase = maxHPBase;
			CurrentHP = MaxHPBase;
			CritRateBase = critRateBase;
			ExperienceGainMultiplier = experienceGainMultiplier;

			_availablePlayerUpgrades = CreateUpgrades();
			ItemPool = CreateItemPool();
		}

		#endregion Construction

		#region Events

		public event EventHandler<DamageInfo> EnemyDamagedEventHandler;

		public void OnEnemyDamaged(DamageInfo damageInfo)
		{
			EnemyDamagedEventHandler?.Invoke(this, damageInfo);
		}

		public event EventHandler<DamageInfo> EnemyKilledEventHandler;

		public void OnEnemyKilled(DamageInfo damageInfo)
		{
			NumKilledEnemies++;
			EnemyKilledEventHandler?.Invoke(this, damageInfo);
		}

		#endregion Events

		private List<Upgrade> CreateUpgrades()
		{
			return new List<Upgrade>()
			{
				new Upgrade("", "+10% Max HP", UpgradeType.Player, new Action(() => MaxHPMultiplier += 0.1f)),
				new Upgrade("", "+10% Pickup Range", UpgradeType.Player, new Action(() => PickupRadiusMultiplier += 0.5f))
			};
		}

		private static List<PackedScene> CreateItemPool()
		{
			return new List<PackedScene>()
			{
				ResourceLoader.Load<PackedScene>("res://Scenes/Items/MagicShield.tscn"),
				ResourceLoader.Load<PackedScene>("res://Scenes/Items/Sawblade.tscn"),
				ResourceLoader.Load<PackedScene>("res://Scenes/Items/GlowingCoal.tscn")
			};
		}
	}
}
