namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// Interface for items (abilities, trinkets).
	/// </summary>
	public interface IItem
	{
		/// <summary>
		/// Info about this item.
		/// </summary>
		public ItemData Metadata { get; }
	}
}
