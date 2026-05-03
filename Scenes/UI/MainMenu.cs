using Godot;

namespace GodotSurvivor.Scenes.UI
{
	public partial class MainMenu : Control
	{
		public override void _Ready()
		{
			GetNode<Button>("%NewGameButton").Pressed += OnNewGamePressed;
			GetNode<Button>("%StatsButton").Pressed += OnStatsPressed;
			GetNode<Button>("%QuitButton").Pressed += OnQuitPressed;
		}

		private void OnNewGamePressed()
		{
			GetTree().ChangeSceneToFile("res://Scenes/UI/NewGameScreen.tscn");
		}

		private void OnStatsPressed()
		{
			GetTree().ChangeSceneToFile("res://Scenes/UI/StatsScreen.tscn");
		}

		private void OnQuitPressed()
		{
			GetTree().Quit();
		}
	}
}
