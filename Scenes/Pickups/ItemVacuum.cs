using GodotSurvivor.Scenes.Player;
using System.Linq;

namespace GodotSurvivor.Scenes.Pickups
{
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
