using Godot;
using GodotSurvivor.Scenes;

namespace GodotSurvivor.Scenes.UI
{
	public partial class NewGameScreen : Control
	{
		private Button _startButton;
		private string _selectedWeaponScenePath = GameSession.DefaultWeaponScenePath;

		public override void _Ready()
		{
			_startButton = GetNode<Button>("%StartButton");

			GetNode<Button>("%FlamethrowerButton").Pressed += () => SelectWeapon(
				GameSession.FlamethrowerScenePath,
				"Start with Flamethrower");
			GetNode<Button>("%PistolButton").Pressed += () => SelectWeapon(
				GameSession.PistolScenePath,
				"Start with Pistol");
			GetNode<Button>("%BackButton").Pressed += OnBackPressed;
			_startButton.Pressed += OnStartPressed;
		}

		private void SelectWeapon(string weaponScenePath, string startButtonText)
		{
			_selectedWeaponScenePath = weaponScenePath;
			_startButton.Text = startButtonText;
		}

		private void OnStartPressed()
		{
			GameSession.SelectedWeaponScenePath = _selectedWeaponScenePath;
			GetTree().ChangeSceneToFile("res://Scenes/Ingame.tscn");
		}

		private void OnBackPressed()
		{
			GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
		}
	}
}
