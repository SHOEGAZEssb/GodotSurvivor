using Godot;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.UI
{
	/// <summary>
	/// Button for choosing an upgrade in the <see cref="LevelUpScreen"/>.
	/// </summary>
	public partial class UpgradeButton : Button
	{
		/// <summary>
		/// Event that is fired when the upgrade of this button is applied.
		/// </summary>
		[Signal]
		public delegate void UpgradeAppliedEventHandler();

		/// <summary>
		/// The upgrade to apply when this button is clicked.
		/// </summary>
		public Upgrade Upgrade;

		public override void _Ready()
		{
			Text = Upgrade.Description;
		}

		private void OnButtonPressed()
		{
			Upgrade.OnChosen();
			EmitSignal(SignalName.UpgradeApplied);
		}
	}
}
