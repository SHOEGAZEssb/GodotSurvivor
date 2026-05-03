namespace GodotSurvivor.Scenes.Collectibles
{
	public sealed class CollectibleUpgradeDefinition
	{
		public string Id { get; }
		public string Name { get; }
		public string Description { get; }
		public int MaxStacks { get; }

		public CollectibleUpgradeDefinition(string id, string name, string description, int maxStacks = int.MaxValue)
		{
			Id = id;
			Name = name;
			Description = description;
			MaxStacks = maxStacks;
		}
	}
}
