using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GodotSurvivor.Scenes.Collectibles
{
	public static class CollectibleStatTracker
	{
		private const string SavePath = "user://collectible_stats.json";
		private static readonly Dictionary<string, CollectibleStats> _statsByCollectibleId = new();
		private static bool _loaded;

		public static IReadOnlyDictionary<string, CollectibleStats> StatsByCollectibleId
		{
			get
			{
				EnsureLoaded();
				return _statsByCollectibleId;
			}
		}

		public static CollectibleStats GetStats(string collectibleId)
		{
			EnsureLoaded();
			if (!_statsByCollectibleId.TryGetValue(collectibleId, out var stats))
			{
				stats = new CollectibleStats();
				_statsByCollectibleId[collectibleId] = stats;
			}

			return stats;
		}

		public static IEnumerable<CollectibleDefinition> GetUnlockedDefinitions()
		{
			EnsureLoaded();
			return CollectibleCatalog.Definitions
				.Where(definition => GetStats(definition.Id).Unlocked || GetStats(definition.Id).TimesUsed > 0)
				.OrderBy(definition => definition.Category)
				.ThenBy(definition => definition.Name);
		}

		public static void RecordUnlock(string collectibleId)
		{
			var stats = GetStats(collectibleId);
			stats.Unlocked = true;
			Save();
		}

		public static void RecordUse(string collectibleId, long amount = 1)
		{
			var stats = GetStats(collectibleId);
			stats.Unlocked = true;
			stats.TimesUsed += amount;
			Save();
		}

		public static void RecordDamage(DamageInfo damageInfo)
		{
			var collectibleId = ResolveCollectibleId(damageInfo.Source);
			if (string.IsNullOrEmpty(collectibleId))
				return;

			var stats = GetStats(collectibleId);
			stats.Unlocked = true;
			stats.DamageDone += damageInfo.Damage;
			Save();
		}

		public static void RecordKill(DamageInfo damageInfo)
		{
			var collectibleId = ResolveCollectibleId(damageInfo.Source);
			if (string.IsNullOrEmpty(collectibleId))
				return;

			var stats = GetStats(collectibleId);
			stats.Unlocked = true;
			stats.EnemiesKilled++;
			Save();
		}

		public static void RecordCustom(string collectibleId, string statKey, long amount = 1)
		{
			var stats = GetStats(collectibleId);
			stats.Unlocked = true;
			stats.CustomStats[statKey] = stats.CustomStats.GetValueOrDefault(statKey) + amount;
			Save();
		}

		public static void RecordUpgradeChosen(string collectibleId, string upgradeId)
		{
			if (string.IsNullOrWhiteSpace(collectibleId) || string.IsNullOrWhiteSpace(upgradeId))
				return;

			var stats = GetStats(collectibleId);
			stats.Unlocked = true;
			stats.PickedUpgradeIds ??= new List<string>();

			if (!stats.PickedUpgradeIds.Contains(upgradeId))
				stats.PickedUpgradeIds.Add(upgradeId);

			Save();
		}

		public static void RecordHealing(string collectibleId, long amount)
		{
			RecordCustom(collectibleId, CollectibleStatKeys.HealingDone, amount);
		}

		public static void GainExperience(string collectibleId, long amount)
		{
			var stats = GetStats(collectibleId);
			stats.Unlocked = true;
			stats.Experience += amount;
			while (stats.Experience >= ExperienceRequiredForLevel(stats.Level + 1))
				stats.Level++;
			Save();
		}

		private static string ResolveCollectibleId(Node source)
		{
			while (source != null)
			{
				if (source is ICollectibleStatSource statSource)
					return statSource.CollectibleId;

				source = source.GetParent();
			}

			return null;
		}

		private static long ExperienceRequiredForLevel(int level)
		{
			return (level - 1) * 10L;
		}

		private static void EnsureLoaded()
		{
			if (_loaded)
				return;

			_loaded = true;
			var globalPath = ProjectSettings.GlobalizePath(SavePath);
			if (!File.Exists(globalPath))
				return;

			var json = File.ReadAllText(globalPath);
			var loadedStats = JsonSerializer.Deserialize<Dictionary<string, CollectibleStats>>(json);
			if (loadedStats == null)
				return;

			_statsByCollectibleId.Clear();
			foreach (var (key, value) in loadedStats)
				_statsByCollectibleId[key] = value;
		}

		private static void Save()
		{
			EnsureLoaded();
			var globalPath = ProjectSettings.GlobalizePath(SavePath);
			var directory = Path.GetDirectoryName(globalPath);
			if (!string.IsNullOrEmpty(directory))
				Directory.CreateDirectory(directory);

			var json = JsonSerializer.Serialize(_statsByCollectibleId, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(globalPath, json);
		}
	}
}
