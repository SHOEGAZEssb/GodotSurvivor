using GodotSurvivor.Scenes.Player;
using GodotSurvivor.Scenes.Statuses;
using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// Interface for an ability item.
	/// </summary>
	public interface IAbility : IItem, ICanApplyStatuses
	{
		/// <summary>
		/// List of applyable upgrades.
		/// </summary>
		List<Upgrade> AvailableUpgrades { get; }
	}
}
