using Godot;
using GodotSurvivor.Scenes.Statuses;
using System.Collections.Generic;
using System.Linq;

namespace GodotSurvivor.Scenes.Helper
{
	/// <summary>
	/// IEnumerable extensions.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Gets <paramref name="num"/> amount random unique items
		/// form this list.
		/// </summary>
		/// <typeparam name="T">Type of the items to get.</typeparam>
		/// <param name="list">List to get items from.</param>
		/// <param name="num">Amount of items to get.
		/// If the list contains less items than <paramref name="num"/>, only
		/// so many are returned.</param>
		/// <returns>Random unique items.</returns>
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

	/// <summary>
	/// Extensions for the <see cref="ICanApplyStatuses"/> interface.
	/// </summary>
	public static class ICanApplyStatusesExtensions
	{
		/// <summary>
		/// Adds a new status to the list of applyable statuses of a node.
		/// </summary>
		/// <param name="node">Node to add status to.</param>
		/// <param name="name">Name of the status.</param>
		/// <param name="chance">Chance to trigger status apply.
		/// If the status already exists, the chance will be incremented.</param>
		/// <param name="statusScene">Scene of the status.</param>
		public static void AddApplyableStatus(this ICanApplyStatuses node, string name, float chance, PackedScene statusScene)
		{
			// if status already exists, increment chance
			if (node.ApplyableStatuses.ContainsKey(name))
				node.ApplyableStatuses[name] = (statusScene, node.ApplyableStatuses[name].chance + chance);
			else
				node.ApplyableStatuses.Add(name, (statusScene, chance));
		}
	}
}