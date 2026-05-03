namespace GodotSurvivor.Scenes.Collectibles
{
	public sealed class StatDefinition
	{
		public string Id { get; }
		public string Label { get; }

		public StatDefinition(string id, string label)
		{
			Id = id;
			Label = label;
		}
	}
}
