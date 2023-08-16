namespace GodotSurvivor.Scenes.Helper
{
	internal static class DamageHelper
	{
		public static (int damage, bool crit) CalculateCrit(int baseDamage, float percentage)
		{
			return RandomHelper.HitRandomChance(percentage) ? (baseDamage * 2, true) : (baseDamage, false);
		}
	}
}