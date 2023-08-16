using Godot;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.UI
{
	public partial class UpgradeButton : Button
	{
		[Signal]
		public delegate void UpgradeAppliedEventHandler();

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
