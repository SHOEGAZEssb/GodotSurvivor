using GodotSurvivor.Scenes.Collectibles;

namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// Interface for items (abilities, trinkets).
	/// </summary>
	public interface IItem : ICollectibleStatSource
	{
		/// <summary>
		/// Info about this item.
		/// </summary>
		public ItemData Metadata { get; }
	}
}
