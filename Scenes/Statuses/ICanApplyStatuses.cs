using Godot;
using System.Collections.Generic;

namespace GodotSurvivor.Scenes.Statuses
{
	public interface ICanApplyStatuses
	{
		IDictionary<string, (PackedScene statusScene, float chance)> ApplyableStatuses { get; }
	}
}