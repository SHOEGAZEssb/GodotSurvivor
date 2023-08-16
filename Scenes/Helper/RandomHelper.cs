using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotSurvivor.Scenes.Helper
{
	internal static class RandomHelper
	{
		public static bool HitRandomChance(float percentage)
		{
			return GD.Randf() <= percentage;
		}
	}
}