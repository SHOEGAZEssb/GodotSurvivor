using Godot;

namespace GodotSurvivor.Scenes.Helper
{
	/// <summary>
	/// Helper class for rng related functions.
	/// </summary>
	internal static class RandomHelper
	{
		/// <summary>
		/// Rolls a random chance.
		/// </summary>
		/// <param name="percentage">Chance to hit.</param>
		/// <returns>True if the chance was hit, otherwise false.</returns>
		public static bool HitRandomChance(float percentage)
		{
			return GD.Randf() <= percentage;
		}
	}
}