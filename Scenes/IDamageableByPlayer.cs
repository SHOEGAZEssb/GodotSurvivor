namespace GodotSurvivor.Scenes
{
	public interface IDamageableByPlayer : IDamageable
	{
		void TakeDamage(DamageInfo damageInfo);
	}
}