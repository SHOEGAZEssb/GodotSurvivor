using Godot;

namespace GodotSurvivor.Scenes.Helper
{
	internal static class RandomHelper
	{
		public static bool HitRandomChance(float percentage)
		{
			return GD.Randf() <= percentage;
		}
	}
}