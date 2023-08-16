using System;

namespace GodotSurvivor.Scenes.Player
{
	public enum UpgradeType
	{
		Player,
		Weapon,
		Ability
	}

	public class Upgrade
	{
		public string Name { get; }
		public string Description { get; }
		public UpgradeType Type { get; }
		public bool Unique { get; }
		public bool IsApplicable => _isApplicable?.Invoke() ?? true;
		private readonly Func<bool> _isApplicable;

		private readonly Action _upgradeAction;

		public Upgrade(string name, string description, UpgradeType type, Action upgradeAction, Func<bool> isApplicable = null, bool unique = false)
		{
			Name = name;
			Description = description;
			Type = type;
			_upgradeAction = upgradeAction;
			_isApplicable = isApplicable;
			Unique = unique;
		}

		public void OnChosen()
		{
			_upgradeAction.Invoke();
			Stats.CurrentStats.ChosenUpgrades.Add(this);
		}
	}
}
