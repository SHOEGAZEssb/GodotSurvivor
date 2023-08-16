using GodotSurvivor.Scenes.Player;
using GodotSurvivor.Scenes.Statuses;
using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Items
{
	public interface IAbility : IItem, ICanApplyStatuses
	{
		List<Upgrade> AvailableUpgrades { get; }
	}
}
