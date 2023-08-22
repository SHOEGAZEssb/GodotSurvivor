using Godot;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.Pickups
{
	/// <summary>
	/// Grants the user experience when picked up.
	/// </summary>
	public partial class ExpCrystal : ItemPickupBase
	{
		/// <summary>
		/// Amount of experience this crystal is worth.
		/// </summary>
		[Export]
		public int Experience = 1;

		protected override void OnPickup(PlayerController player)
		{
			player.PlayerStats.CurrentExperience += Experience;
			QueueFree();
		}
	}
}
