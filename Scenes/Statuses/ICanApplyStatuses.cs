using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotSurvivor.Scenes.Statuses
{
	public interface ICanApplyStatuses
	{
		IDictionary<string, (PackedScene statusScene, float chance)> ApplyableStatuses { get; }
	}
}