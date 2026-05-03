using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Collectibles
{
	public sealed class CollectibleStats
	{
		public bool Unlocked { get; set; }
		public long TimesUsed { get; set; }
		public long DamageDone { get; set; }
		public long EnemiesKilled { get; set; }
		public long Experience { get; set; }
		public int Level { get; set; } = 1;
		public List<string> PickedUpgradeIds { get; set; } = new();
		public Dictionary<string, long> CustomStats { get; set; } = new();

		public long GetValue(string statKey)
		{
			return statKey switch
			{
				CollectibleStatKeys.TimesUsed => TimesUsed,
				CollectibleStatKeys.DamageDone => DamageDone,
				CollectibleStatKeys.EnemiesKilled => EnemiesKilled,
				CollectibleStatKeys.Experience => Experience,
				CollectibleStatKeys.Level => Level,
				_ => CustomStats.TryGetValue(statKey, out var value) ? value : 0
			};
		}
	}
}
