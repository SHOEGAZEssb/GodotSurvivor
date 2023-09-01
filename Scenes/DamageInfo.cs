using Godot;

namespace GodotSurvivor.Scenes
{
	/// <summary>
	/// Source of damage.
	/// </summary>
	public enum DamageSource
	{
		/// <summary>
		/// Damage was caused by a weapon.
		/// </summary>
		Weapon,

		/// <summary>
		/// Damage was caused by an ability.
		/// </summary>
		Ability,

		/// <summary>
		/// Damage was caused by the Burning status.
		/// </summary>
		Burning,

		/// <summary>
		/// Damage was caused by something else.
		/// </summary>
		Other
	}

	/// <summary>
	/// Info about damage that was caused.
	/// </summary>
	public readonly struct DamageInfo
	{
		/// <summary>
		/// Amount of damage.
		/// </summary>
		public int Damage { get; }

		/// <summary>
		/// If the attack was a crit or not.
		/// </summary>
		public bool Crit { get; }

		/// <summary>
		/// Type of the damage source.
		/// </summary>
		public DamageSource DamageSourceType { get; }

		/// <summary>
		/// Receiver of the damage.
		/// </summary>
		public Node2D Target { get; }

		/// <summary>
		/// Source of the damage.
		/// </summary>
		public Node Source { get; }

		/// <summary>
		///
		/// </summary>
		/// <param name="damage">Amount of damage.</param>
		/// <param name="crit">If the attack was a crit or not.</param>
		/// <param name="damageSource">Type of the damage source.</param>
		/// <param name="target">Receiver of the damage.</param>
		/// <param name="source">Source of the damage.</param>
		public DamageInfo(int damage, bool crit, DamageSource damageSource, Node2D target, Node source)
		{
			Damage = damage;
			Crit = crit;
			DamageSourceType = damageSource;
			Target = target;
			Source = source;
		}
	}
}