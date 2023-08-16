using Godot;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.Pickups
{
	public partial class ExpCrystal : ItemPickupBase
	{
		[Export]
		public int Experience = 1;

		protected override void OnPickup(PlayerController player)
		{
			player.PlayerStats.CurrentExperience += Experience;
			QueueFree();
		}
	}
}
