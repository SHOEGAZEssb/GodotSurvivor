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

		public CollectibleDefinition(
			string id,
			string name,
			string description,
			string texturePath,
			CollectibleCategory category,
			IReadOnlyList<string> supportedStats)
		{
			Id = id;
			Name = name;
			Description = description;
			TexturePath = texturePath;
			Category = category;
			SupportedStats = supportedStats;
		}
	}
}
