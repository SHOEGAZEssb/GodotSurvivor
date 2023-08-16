using Godot;
using GodotSurvivor.Scenes.Statuses;
using System.Collections.Generic;
using System.Linq;

namespace GodotSurvivor.Scenes.Helper
{
	public static class ListExtensions
	{
		public static IEnumerable<T> GetRandomListItems<T>(this IEnumerable<T> list, int num)
		{
			var randomItems = new List<T>();

			// create new list to be able to remove items from it
			var pool = list.ToList();
			if (num > pool.Count)
				num = pool.Count;

			for (int i = 0; i < num; i++)
			{
				int index = (int)(GD.Randi() % pool.Count);
				var item = pool[index];
				pool.Remove(item);
				randomItems.Add(item);
			}

			return randomItems;
		}
	}

	public static class ICanApplyStatusesExtensions
	{
		private static IDictionary<string, PackedScene> _statusScenes;

		static ICanApplyStatusesExtensions()
		{
			_statusScenes = new Dictionary<string, PackedScene>()
			{
				{ "Burning", ResourceLoader.Load<PackedScene>("res://Scenes/Statuses/Burning.tscn") }
			};
		}

		public static void AddApplyableStatusByName(this ICanApplyStatuses node, string name, float chance)
		{
			// if status already exists, increment chance
			if (node.ApplyableStatuses.ContainsKey(name))
		}
	}
}