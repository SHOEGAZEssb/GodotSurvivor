using System.Collections.Generic;
using System.Linq;

namespace GodotSurvivor.Scenes.Collectibles
{
	public static class CollectibleCatalog
	{
		public static IReadOnlyList<CollectibleDefinition> Definitions { get; } = new List<CollectibleDefinition>
		{
			new(
				CollectibleIds.Flamethrower,
				"Flamethrower",
				"A close-range stream of fire.",
				"res://Sprites/Weapons/Flamethrower.png",
				CollectibleCategory.Weapon,
				new[]
				{
					CollectibleStatKeys.TimesUsed,
					CollectibleStatKeys.DamageDone,
					CollectibleStatKeys.EnemiesKilled,
					CollectibleStatKeys.Experience,
					CollectibleStatKeys.Level
				}),
			new(
				CollectibleIds.Pistol,
				"Pistol",
				"A precise ranged shot.",
				"res://Sprites/Weapons/gun.png",
				CollectibleCategory.Weapon,
				new[]
				{
					CollectibleStatKeys.TimesUsed,
					CollectibleStatKeys.DamageDone,
					CollectibleStatKeys.EnemiesKilled,
					CollectibleStatKeys.Experience,
					CollectibleStatKeys.Level
				}),
			new(
				CollectibleIds.MagicShield,
				"Magic Shield",
				"Periodically damages enemies in its range.",
				"res://Sprites/Items/MagicShieldIcon.png",
				CollectibleCategory.Ability,
				new[]
				{
					CollectibleStatKeys.TimesUsed,
					CollectibleStatKeys.DamageDone,
					CollectibleStatKeys.EnemiesKilled,
					CollectibleStatKeys.Experience,
					CollectibleStatKeys.Level
				}),
			new(
				CollectibleIds.Sawblade,
				"Sawblade",
				"Rotates around the player and damages enemies.",
				"res://Sprites/Placeholder.png",
				CollectibleCategory.Ability,
				new[]
				{
					CollectibleStatKeys.TimesUsed,
					CollectibleStatKeys.DamageDone,
					CollectibleStatKeys.EnemiesKilled,
					CollectibleStatKeys.Experience,
					CollectibleStatKeys.Level
				}),
			new(
				CollectibleIds.GlowingCoal,
				"Glowing Coal",
				"Enemies killed by Burning status drop double exp.",
				"res://Sprites/Placeholder.png",
				CollectibleCategory.Trinket,
				new[]
				{
					CollectibleStatKeys.TimesUsed,
					CollectibleStatKeys.BurningKillsBoosted,
					CollectibleStatKeys.Experience,
					CollectibleStatKeys.Level
				}),
			new(
				CollectibleIds.Shovel,
				"Shovel",
				"Can dig up treasure and damage enemies.",
				"res://Sprites/Items/Shovel.png",
				CollectibleCategory.Ability,
				new[]
				{
					CollectibleStatKeys.TimesUsed,
					CollectibleStatKeys.DamageDone,
					CollectibleStatKeys.EnemiesKilled,
					CollectibleStatKeys.TreasureDugUp,
					CollectibleStatKeys.Experience,
					CollectibleStatKeys.Level
				})
		};

		public static IReadOnlyDictionary<string, StatDefinition> StatDefinitions { get; } =
			new Dictionary<string, StatDefinition>
			{
				[CollectibleStatKeys.TimesUsed] = new(CollectibleStatKeys.TimesUsed, "Times Used"),
				[CollectibleStatKeys.DamageDone] = new(CollectibleStatKeys.DamageDone, "Damage Done"),
				[CollectibleStatKeys.EnemiesKilled] = new(CollectibleStatKeys.EnemiesKilled, "Enemies Killed"),
				[CollectibleStatKeys.Experience] = new(CollectibleStatKeys.Experience, "Experience"),
				[CollectibleStatKeys.Level] = new(CollectibleStatKeys.Level, "Level"),
				[CollectibleStatKeys.BurningKillsBoosted] = new(CollectibleStatKeys.BurningKillsBoosted, "Burning Kills Boosted"),
				[CollectibleStatKeys.TreasureDugUp] = new(CollectibleStatKeys.TreasureDugUp, "Treasure Dug Up"),
				[CollectibleStatKeys.HealingDone] = new(CollectibleStatKeys.HealingDone, "Healing Done")
			};

		public static CollectibleDefinition GetDefinition(string id)
		{
			return Definitions.FirstOrDefault(definition => definition.Id == id);
		}
	}
}
