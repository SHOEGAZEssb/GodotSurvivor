using Godot;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.UI
{
	/// <summary>
	/// Bar for displaying player health.
	/// </summary>
	public partial class HealthBar : TextureProgressBar
	{
		private Label _label;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_label = GetNode<Label>("Label");

			var player = GetTree().CurrentScene.GetNode<PlayerController>("Player");
			player.Ready += Player_Ready;
		}

		private void Player_Ready()
		{
			MinValue = 0;
			MaxValue = Stats.CurrentStats.MaxHP;
			Stats.CurrentStats.CurrentHPChanged += OnPlayerHPChanged;
			Stats.CurrentStats.MaxHPChanged += OnPlayerMaxHPChanged;
			UpdateLabel();
		}

		private void OnPlayerHPChanged()
		{
			Value = Stats.CurrentStats.CurrentHP;
			UpdateLabel();
		}

		private void OnPlayerMaxHPChanged()
		{
			MaxValue = Stats.CurrentStats.MaxHP;
			UpdateLabel();
		}

		private void UpdateLabel()
		{
			_label.Text = $"{Stats.CurrentStats.CurrentHP} / {Stats.CurrentStats.MaxHP}";
		}
	}
}
