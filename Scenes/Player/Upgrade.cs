using System;

namespace GodotSurvivor.Scenes.Player
{
	/// <summary>
	/// Type of the upgrade.
	/// </summary>
	public enum UpgradeType
	{
		/// <summary>
		/// Upgrades player stats.
		/// </summary>
		Player,

		/// <summary>
		/// Upgrades weapon stats.
		/// </summary>
		Weapon,

		/// <summary>
		/// Upgrades single ability stats.
		/// </summary>
		Ability
	}

	/// <summary>
	/// An upgrade for an item, the player, or a weapon.
	/// </summary>
	public class Upgrade
	{
		/// <summary>
		/// Stable identifier for tracking chosen stacks.
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// Name of the upgrade.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Description of the upgrade.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Type of the upgrade.
		/// </summary>
		public UpgradeType Type { get; }

		/// <summary>
		/// Name of the player, item, or weapon affected by this upgrade.
		/// </summary>
		public string TargetName { get; }

		/// <summary>
		/// Collectible id of the item or weapon affected by this upgrade.
		/// </summary>
		public string TargetCollectibleId { get; }

		/// <summary>
		/// Optional texture path for the affected target.
		/// </summary>
		public string TargetTexturePath { get; }

		/// <summary>
		/// If the upgrade is unique, meaning it can only be picked once.
		/// </summary>
		public bool Unique => MaxStacks == 1;

		/// <summary>
		/// Maximum amount of times this upgrade can be chosen.
		/// </summary>
		public int MaxStacks { get; }

		/// <summary>
		/// If this upgrade can be applied, meaning it can appear
		/// in the level up screen.
		/// </summary>
		public bool IsApplicable => (_isApplicable?.Invoke() ?? true) && Stats.CurrentStats.GetUpgradeStackCount(Id) < MaxStacks;
		private readonly Func<bool> _isApplicable;

		private readonly Action _upgradeAction;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name of the upgrade.</param>
		/// <param name="description">Description of the upgrade.</param>
		/// <param name="type">Type of the upgrade.</param>
		/// <param name="targetName">Name of the player, item, or weapon affected by this upgrade.</param>
		/// <param name="upgradeAction">The effect of the upgrade.</param>
		/// <param name="isApplicable">If this upgrade can be applied, meaning it can appear in the level up screen.
		/// Pass null if no check is needed.</param>
		/// <param name="maxStacks">Maximum amount of times this upgrade can be chosen.</param>
		/// <param name="targetTexturePath">Optional texture path for the affected target.</param>
		/// <param name="targetCollectibleId">Collectible id of the item or weapon affected by this upgrade.</param>
		public Upgrade(string id, string name, string description, UpgradeType type, string targetName, Action upgradeAction, Func<bool> isApplicable = null, int maxStacks = int.MaxValue, string targetTexturePath = null, string targetCollectibleId = null)
		{
			Id = id;
			Name = name;
			Description = description;
			Type = type;
			TargetName = targetName;
			TargetCollectibleId = targetCollectibleId;
			TargetTexturePath = targetTexturePath;
			_upgradeAction = upgradeAction;
			_isApplicable = isApplicable;
			MaxStacks = maxStacks;
		}

		/// <summary>
		/// Applies the upgrade.
		/// </summary>
		public void OnChosen()
		{
			_upgradeAction.Invoke();
			Stats.CurrentStats.RecordChosenUpgrade(this);
		}
	}
}
