namespace GodotSurvivor.Scenes
{
	/// <summary>
	/// Interface for entities that can be damaged by enemies.
	/// </summary>
	internal interface IDamageableByEnemy : IDamageable
	{
		/// <summary>
		/// Damages the entity.
		/// </summary>
		/// <param name="damage">Damage to take.</param>
		void TakeDamage(int damage);
	}
}