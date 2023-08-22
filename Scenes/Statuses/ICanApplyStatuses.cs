using Godot;
using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Statuses
{
	/// <summary>
	/// Interface for abilities and weapons that can apply status effects.
	/// </summary>
	public interface ICanApplyStatuses
	{
		/// <summary>
		/// Currently applyable statuses.
		/// </summary>
		IDictionary<string, (PackedScene statusScene, float chance)> ApplyableStatuses { get; }
	}
}