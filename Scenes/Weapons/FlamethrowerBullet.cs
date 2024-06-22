using Godot;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Player;
using GodotSurvivor.Scenes.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotSurvivor.Scenes.Weapons
{
	public partial class FlamethrowerBullet : Area2D, ICanApplyStatuses
	{
		[Export]
		public float InitialSpeed = 200f;
		private float _currentSpeed;

		[Export]
		public float SpeedDropoff = 100f;

		[Export]
		public int StillFramesBeforeRemove = 30;

		[Export]
		public int ContactDamage = 3;

		public IDictionary<string, (PackedScene statusScene, float chance)> ApplyableStatuses => _applyableStatuses;
		private readonly IDictionary<string, (PackedScene statusScene, float chance)> _applyableStatuses = new Dictionary<string, (PackedScene statusScene, float chance)>();

		private int _stillFrames = 0;

		public override void _Ready()
		{
			base._Ready();
			_currentSpeed = InitialSpeed;
			this.AddApplyableStatus(nameof(Burning), 0.1f, Burning.CreateCustomPackedScene(1, 1f));
		}

		public override void _PhysicsProcess(double delta)
		{
			if (_currentSpeed > 0)
			{
				Position += Transform.X * _currentSpeed * (float)delta;
				_currentSpeed -= SpeedDropoff * (float)delta;
			}
			else if (++_stillFrames >= StillFramesBeforeRemove)
				QueueFree();
		}

		private void OnBodyEntered(Node2D body)
		{
			if (body is IDamageableByPlayer e)
			{
				var (damage, crit) = DamageHelper.CalculateCrit(ContactDamage, Stats.CurrentStats.CritRate);
				e.TakeDamage(new DamageInfo(damage, crit, DamageSource.Weapon, e as Node2D, this));
				DamageHelper.ApplyStatuses(e as Node, ApplyableStatuses);
			}
		}
	}
}
