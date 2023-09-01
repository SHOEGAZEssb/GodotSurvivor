using Godot;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Player;
using GodotSurvivor.Scenes.Statuses;
using System.Collections.Generic;
using System.Linq;

namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// The hole the <see cref="Shovel"/> digs.
	/// </summary>
	internal partial class ShovelHole : Area2D, ICanApplyStatuses
	{
		#region Properties

		/// <summary>
		/// Damage dealt to enemies hit by the hole.
		/// </summary>
		public int Damage { get; set; }

		/// <summary>
		/// The treasure that will be dug out.
		/// </summary>
		public Node2D Treasure { get; set; }

		public IDictionary<string, (PackedScene statusScene, float chance)> ApplyableStatuses { get; set; }

		/// <summary>
		/// The parent shovel.
		/// </summary>
		public Shovel Parent { get; set; }

		/// <summary>
		/// We need this counter because <see cref="Area2D.GetOverlappingBodies"/>
		/// returns the results of the LAST physics step.
		/// So we need to wait one frame before checking the overlaps.
		/// </summary>
		private int _numPhysicsSteps = 0;

		#endregion Properties

		public override void _PhysicsProcess(double delta)
		{
			if (_numPhysicsSteps == 1)
			{
				// spawn treasure
				if (Treasure != null)
				{
					Treasure.Position = GlobalPosition;
					GetTree().CurrentScene.AddChild(Treasure);
				}

				// damage enemies
				var enemies = GetOverlappingBodies().OfType<IDamageableByPlayer>();
				foreach (var enemy in enemies)
				{
					DamageHelper.ApplyStatuses(enemy as Node, ApplyableStatuses);
					var (damage, crit) = DamageHelper.CalculateCrit(Damage, Stats.CurrentStats.CritRate);
					enemy.TakeDamage(new DamageInfo(damage, crit, DamageSource.Ability, enemy as Node2D, Parent));
				}

				// fade out and remove object
				var tween = GetTree().CreateTween();
				tween.TweenProperty(this, "modulate:a", 0.1, 1f);//.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
				tween.TweenCallback(Callable.From(() => QueueFree()));
			}

			_numPhysicsSteps++;
		}
	}
}