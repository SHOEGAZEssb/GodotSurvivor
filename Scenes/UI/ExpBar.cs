using Godot;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.UI
{
	public partial class ExpBar : TextureProgressBar
	{
		private Label _label;
		private PackedScene _levelUpScreenScene;
		private CanvasLayer _parentCanvas;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_label = GetNode<Label>("Label");
			_levelUpScreenScene = ResourceLoader.Load<PackedScene>("res://Scenes/UI/LevelUpScreen.tscn");
			_parentCanvas = (CanvasLayer)FindParent("CanvasLayer");

			var player = GetTree().CurrentScene.GetNode<PlayerController>("Player");
			player.Ready += Player_Ready;

		}

		private void Player_Ready()
		{
			MinValue = 0;
			MaxValue = Stats.CurrentStats.ExperienceToNextLevel;
			Value = Stats.CurrentStats.CurrentExperience;
			Stats.CurrentStats.ExpGained += OnPlayerExpGained;
			Stats.CurrentStats.LevelChanged += OnPlayerLevelGained;

			UpdateLabel();
		}

		private void OnPlayerExpGained()
		{
			Value = Stats.CurrentStats.CurrentExperience;
			UpdateLabel();
		}

		private void OnPlayerLevelGained()
		{
			MinValue = Stats.CurrentStats.CurrentExperience;
			MaxValue = Stats.CurrentStats.ExperienceToNextLevel;

			_parentCanvas.AddChild(_levelUpScreenScene.Instantiate());
			UpdateLabel();
		}

		private void UpdateLabel()
		{
			_label.Text = $"{Stats.CurrentStats.CurrentExperience} / {Stats.CurrentStats.ExperienceToNextLevel}";
		}
	}
}
