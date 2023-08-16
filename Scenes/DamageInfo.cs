using Godot;

namespace GodotSurvivor.Scenes
{
	public enum DamageSource
	{
		Weapon,
		Ability,
		Burning,
		Other
	}

	public readonly struct DamageInfo
	{
		public int Damage { get; }
		public bool Crit { get; }
		public DamageSource DamageSource { get; }
		public Node2D Target { get; }
		public Node2D Source { get; }

		public DamageInfo(int damage, bool crit, DamageSource damageSource, Node2D target, Node2D source)
		{
			Damage = damage;
			Crit = crit;
			DamageSource = damageSource;
			Target = target;
			Source = source;
		}
	}
}