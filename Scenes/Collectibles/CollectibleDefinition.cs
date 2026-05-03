using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Collectibles
{
	public sealed class CollectibleDefinition
	{
		public string Id { get; }
		public string Name { get; }
		public string Description { get; }
		public string TexturePath { get; }
		public CollectibleCategory Category { get; }
		public IReadOnlyList<string> SupportedStats { get; }
		public IReadOnlyList<CollectibleUpgradeDefinition> AvailableUpgrades { get; }

		public CollectibleDefinition(
			string id,
			string name,
			string description,
			string texturePath,
			CollectibleCategory category,
			IReadOnlyList<string> supportedStats,
			IReadOnlyList<CollectibleUpgradeDefinition> availableUpgrades = null)
		{
			Id = id;
			Name = name;
			Description = description;
			TexturePath = texturePath;
			Category = category;
			SupportedStats = supportedStats;
			AvailableUpgrades = availableUpgrades ?? new List<CollectibleUpgradeDefinition>();
		}
	}
}
