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

		private TextureRect _targetIcon;
		private Label _targetLabel;
		private Label _nameLabel;
		private Label _descriptionLabel;
		private Label _stackLabel;

		public override void _Ready()
		{
			_targetIcon = GetNode<TextureRect>("%TargetIcon");
			_targetLabel = GetNode<Label>("%TargetLabel");
			_nameLabel = GetNode<Label>("%NameLabel");
			_descriptionLabel = GetNode<Label>("%DescriptionLabel");
			_stackLabel = GetNode<Label>("%StackLabel");

			Text = string.Empty;
			_targetLabel.Text = Upgrade.TargetName;
			_nameLabel.Text = Upgrade.Name;
			_descriptionLabel.Text = Upgrade.Description;

			if (!string.IsNullOrWhiteSpace(Upgrade.TargetTexturePath) && ResourceLoader.Exists(Upgrade.TargetTexturePath))
				_targetIcon.Texture = ResourceLoader.Load<Texture2D>(Upgrade.TargetTexturePath);

			int currentStacks = Stats.CurrentStats.GetUpgradeStackCount(Upgrade.Id);
			_stackLabel.Text = Upgrade.MaxStacks == int.MaxValue
				? $"Stack {currentStacks + 1}"
				: $"{currentStacks + 1} / {Upgrade.MaxStacks}";
		}

		private void OnButtonPressed()
		{
			Upgrade.OnChosen();
			EmitSignal(SignalName.UpgradeApplied);
		}
	}
}
