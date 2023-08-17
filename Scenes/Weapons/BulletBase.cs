using Godot;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.Weapons
{
	public partial class BulletBase : Area2D
	{
		[Export]
		public int BaseDamage = 10;

		private void OnBodyEntered(Node2D body)
		{
			if (body is IDamageableByPlayer e)
			{
				var (damage, crit) = DamageHelper.CalculateCrit(BaseDamage, Stats.CurrentStats.CritRate);
				e.TakeDamage(new DamageInfo(damage, crit, DamageSource.Weapon, e as Node2D, this));
				QueueFree();
			}
		}
	}
}
