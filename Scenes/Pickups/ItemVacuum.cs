using GodotSurvivor.Scenes.Player;
using System.Linq;

namespace GodotSurvivor.Scenes.Pickups
{
	/// <summary>
	/// Collects all other item pickups upon being picked up.
	/// </summary>
	internal partial class ItemVacuum : ItemPickupBase
	{
		protected override void OnPickup(PlayerController player)
		{
			var items = GetTree().CurrentScene.GetChildren().OfType<ExpCrystal>();
			foreach (var item in items)
			{
				item.Target = player;
			}

			QueueFree();
		}
	}
}
