namespace GodotSurvivor.Scenes.Items
{
	public readonly struct ItemData
	{
		public string Name { get; }
		public string Description { get; }
		public string TexturePath { get; }

		public ItemData(string name, string description, string texturePath)
		{
			Name = name;
			Description = description;
			TexturePath = texturePath;
		}
	}
}