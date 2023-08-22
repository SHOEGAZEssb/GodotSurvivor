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
		/// If the upgrade is unique, meaning it can only be picked once.
		/// </summary>
		public bool Unique { get; }

		/// <summary>
		/// If this upgrade can be applied, meaning it can appear
		/// in the level up screen.
		/// </summary>
		public bool IsApplicable => _isApplicable?.Invoke() ?? true;
		private readonly Func<bool> _isApplicable;

		private readonly Action _upgradeAction;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name of the upgrade.</param>
		/// <param name="description">Description of the upgrade.</param>
		/// <param name="type">Type of the upgrade.</param>
		/// <param name="upgradeAction">The effect of the upgrade.</param>
		/// <param name="isApplicable">If this upgrade can be applied, meaning it can appear in the level up screen.
		/// Pass null if no check is needed.</param>
		/// <param name="unique">If the upgrade is unique, meaning it can only be picked once.</param>
		public Upgrade(string name, string description, UpgradeType type, Action upgradeAction, Func<bool> isApplicable = null, bool unique = false)
		{
			Name = name;
			Description = description;
			Type = type;
			_upgradeAction = upgradeAction;
			_isApplicable = isApplicable;
			Unique = unique;
		}

		/// <summary>
		/// Applies the upgrade.
		/// </summary>
		public void OnChosen()
		{
			_upgradeAction.Invoke();
			Stats.CurrentStats.ChosenUpgrades.Add(this);
		}
	}
}
