using Godot;
using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Helper
{
	/// <summary>
	/// Helper class for damage related functions.
	/// </summary>
	internal static class DamageHelper
	{
		/// <summary>
		/// Calculates crit damage based on the given values.
		/// </summary>
		/// <param name="baseDamage">Damage without crit.</param>
		/// <param name="percentage">Chance to crit.</param>
		/// <returns>New damage (unchanged if no crit) and crit info.</returns>
		public static (int damage, bool crit) CalculateCrit(int baseDamage, float percentage)
		{
			return RandomHelper.HitRandomChance(percentage) ? (baseDamage * 2, true) : (baseDamage, false);
		}

		/// <summary>
		/// Goes through the possible statuses and applies
		/// them to the node when their chance is hit.
		/// </summary>
		/// <param name="node">Node to apply statuses to.</param>
		/// <param name="statuses">Statuses info.</param>
		public static void ApplyStatuses(Node node, IDictionary<string, (PackedScene statusScene, float chance)> statuses)
		{
			if (statuses == null)
				return;

			foreach (var (statusScene, chance) in statuses.Values)
			{
				if (RandomHelper.HitRandomChance(chance))
					node.AddChild(statusScene.Instantiate());
			}
		}
	}
}