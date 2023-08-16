namespace GodotSurvivor.Scenes
{
	public interface IDamageableByPlayer : IDamageable
	{
		void TakeDamage((int damage, bool crit) damageInfo);
	}
}