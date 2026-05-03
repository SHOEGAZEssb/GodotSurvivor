using Godot;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.UI
{
	/// <summary>
	/// Screen that is shown when the player levels up.
	/// Presents different <see cref="Upgrade"/> choices.
	/// </summary>
	public partial class LevelUpScreen : Control
	{
		[Signal]
		public delegate void UpgradeChosenEventHandler();

		private VBoxContainer _buttonContainer;
		private PackedScene _buttonScene;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GetTree().Paused = true;
			_buttonContainer = GetNode<VBoxContainer>("%ChoiceList");
			_buttonScene = ResourceLoader.Load<PackedScene>("res://Scenes/UI/UpgradeButton.tscn");

			var upgrades = Stats.CurrentStats.AvailableUpgrades.GetRandomListItems(5);
			foreach (var upgrade in upgrades)
			{
				var button = _buttonScene.Instantiate<UpgradeButton>();
				button.Upgrade = upgrade;
				button.UpgradeApplied += OnUpgradeApplied;
				_buttonContainer.AddChild(button);
			}
		}

		private void OnUpgradeApplied()
		{
			EmitSignal(SignalName.UpgradeChosen);
			QueueFree();
		}
	}
}
