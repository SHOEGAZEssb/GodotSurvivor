using Godot;
using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Helper
{
	internal static class DamageHelper
	{
		public static (int damage, bool crit) CalculateCrit(int baseDamage, float percentage)
		{
			return RandomHelper.HitRandomChance(percentage) ? (baseDamage * 2, true) : (baseDamage, false);
		}

    public static void ApplyStatuses(Node node, IDictionary<string, (PackedScene statusScene, float chance)> statuses)
    {
      foreach (var (statusScene, chance) in statuses.Values)
      {
        if (RandomHelper.HitRandomChance(chance))
          node.AddChild(statusScene.Instantiate());
      }
    }
  }
}