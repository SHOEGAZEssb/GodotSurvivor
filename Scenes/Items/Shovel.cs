using Godot;
using GodotSurvivor.Scenes.Collectibles;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Pickups;
using GodotSurvivor.Scenes.Player;
using GodotSurvivor.Scenes.Statuses;
using System;
using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// Ability that digs holes that damages enemies and can dig up treasure.
	/// </summary>
	public partial class Shovel : Node, IAbility
	{
		#region Properties

		#region Delay

		/// <summary>
		/// Base time between each ticks in seconds.
		/// </summary>
		public float DelayBase = 1f;

		/// <summary>
		/// Delay multiplier.
		/// </summary>
		public float DelayMultiplier
		{
			get => _delayMultiplier;
			set
			{
				_delayMultiplier = Math.Max(0.1f, value);
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

		#region Size

		/// <summary>
		/// Base size of the shovel hole.
		/// </summary>
		public float SizeBase = 0.5f;

		/// <summary>
		/// Shield size multiplier.
		/// </summary>
		public float SizeMultiplier = 1f;

		public Vector2 Size => new(SizeBase * SizeMultiplier, SizeBase * SizeMultiplier);

		#endregion Size

		#region Damage

		/// <summary>
		/// Base damage of the shovel hole.
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

		#region Treasure

		/// <summary>
		/// Chance to dig up an <see cref="ExpCrystal"/>.
		/// </summary>
		public float ExpCrystalChance = 0.1f;

		/// <summary>
		/// Max experience of a dug up <see cref="ExpCrystal"/>.
		/// </summary>
		public int MaxExpCrystalValue = 5;

		/// <summary>
		/// Chance to dig up a <see cref="Chest"/>.
		/// </summary>
		public float ChestChance = 0.01f;

		private PackedScene _expCrystalScene;
		private PackedScene _chestScene;

		#endregion Treasure

		public List<Upgrade> AvailableUpgrades { get; private set; }

		public string CollectibleId => CollectibleIds.Shovel;

		public ItemData Metadata => new("Shovel", "Can dig up treasure and damage enemies", "res://Sprites/Items/Shovel.png");

		public IDictionary<string, (PackedScene statusScene, float chance)> ApplyableStatuses => _applyableStatuses;
		private readonly IDictionary<string, (PackedScene statusScene, float chance)> _applyableStatuses = new Dictionary<string, (PackedScene statusScene, float chance)>();

		private PackedScene _shovelHoleScene;
		private PlayerController _player;

		#endregion Properties


		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_player = GetTree().CurrentScene.GetNode<PlayerController>("Player");
			GetParent().RemoveChild(this);
			_player.AddChild(this);
			_player.PlayerStats.Items.Add(this);
			CollectibleStatTracker.RecordUnlock(CollectibleId);

			AvailableUpgrades = CreateUpgrades();

			_delayTimer = GetNode<Timer>("DelayTimer");
			_delayTimer.WaitTime = Delay;

			_shovelHoleScene = ResourceLoader.Load<PackedScene>("res://Scenes/Items/ShovelHole.tscn");
			_expCrystalScene = ResourceLoader.Load<PackedScene>("res://Scenes/Pickups/ExpCrystal.tscn");
			_chestScene = ResourceLoader.Load<PackedScene>("res://Scenes/Pickups/Chest.tscn");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (_delayTimer.IsStopped())
			{
				CollectibleStatTracker.RecordUse(CollectibleId);
				var randomPos = GetRandomPosition();

				var hole = _shovelHoleScene.Instantiate<ShovelHole>();
				hole.Position = randomPos;
				hole.Scale = Size;
				hole.Damage = Damage;
				hole.ApplyableStatuses = ApplyableStatuses;
				hole.Treasure = GetTreasure();
				hole.Source = this;

				var sprite = new Sprite2D
				{
					Texture = ResourceLoader.Load<Texture2D>("res://Sprites/Items/Shovel.png"),
					Position = randomPos,
					Scale = new Vector2(0.05f, 0.05f),
					ZIndex = 10
				};
				GetTree().CurrentScene.AddChild(sprite);
				var tween = sprite.CreateTween();
				tween.TweenProperty(sprite, "rotation_degrees", 90, 0f).SetDelay(1f);
				tween.TweenCallback(Callable.From(() => GetTree().CurrentScene.AddChild(hole)));
				tween.TweenCallback(Callable.From(() => sprite.QueueFree())).SetDelay(0.2f);

				_delayTimer.Start();
			}
		}

		private List<Upgrade> CreateUpgrades()
		{
			return new List<Upgrade>()
			{
				new("shovel.slow", "Slow", "50% chance to apply Slow", UpgradeType.Ability, Metadata.Name, new Action(() => this.AddApplyableStatus(nameof(Slow), 0.5f, Slow.CreateCustomPackedScene(0.3f, 5f))), null, 1, Metadata.TexturePath)
			};
		}

		private Vector2 GetRandomPosition()
		{
			var screenSize = GetViewport().GetVisibleRect().Size;
			var camera = GetViewport().GetCamera2D();
			var zoom = camera != null ? camera.Zoom : Vector2.One;
			var worldSize = screenSize / zoom;
			var offset = new Vector2(
				(float)GD.RandRange(-worldSize.X / 2, worldSize.X / 2),
				(float)GD.RandRange(-worldSize.Y / 2, worldSize.Y / 2));

			return _player.GlobalPosition + offset;
		}

		private Node2D GetTreasure()
		{
			// chest has priority
			if (RandomHelper.HitRandomChance(ChestChance))
				return _chestScene.Instantiate<Chest>();
			else if (RandomHelper.HitRandomChance(ExpCrystalChance))
			{
				var crystal = _expCrystalScene.Instantiate<ExpCrystal>();
				crystal.Experience = (int)(GD.Randi() % MaxExpCrystalValue) + 1;
				return crystal;
			}
			else
				return null;
		}
	}
}
