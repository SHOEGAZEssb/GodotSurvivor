namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// Infos about an <see cref="IItem"/>.
	/// </summary>
	public readonly struct ItemData
	{
		/// <summary>
		/// Name of the item.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Description of the item.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Path to the texture of the item.
		/// </summary>
		public string TexturePath { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name of the item.</param>
		/// <param name="description">Description of the item.</param>
		/// <param name="texturePath">Path to the texture of the item.</param>
		public ItemData(string name, string description, string texturePath)
		{
			Name = name;
			Description = description;
			TexturePath = texturePath;
		}
	}
}