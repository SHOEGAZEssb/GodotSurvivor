namespace GodotSurvivor.Scenes
{
	internal interface IDamageableByEnemy : IDamageable
	{
		void TakeDamage(int damage);
	}
}
