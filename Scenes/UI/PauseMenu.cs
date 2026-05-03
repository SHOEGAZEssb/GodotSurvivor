using Godot;

namespace GodotSurvivor.Scenes.UI
{
	public partial class PauseMenu : Control
	{
		public override void _Ready()
		{
			Visible = false;
			GetNode<Button>("%ResumeButton").Pressed += OnResumePressed;
			GetNode<Button>("%MainMenuButton").Pressed += OnMainMenuPressed;
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (IsPauseInput(@event))
			{
				TogglePause();
				GetViewport().SetInputAsHandled();
			}
		}

		private static bool IsPauseInput(InputEvent @event)
		{
			if (InputMap.HasAction("pause") && @event.IsActionPressed("pause"))
				return true;

			return @event is InputEventKey { Pressed: true, Echo: false, Keycode: Key.Escape };
		}

		private void TogglePause()
		{
			SetPaused(!GetTree().Paused);
		}

		private void SetPaused(bool paused)
		{
			GetTree().Paused = paused;
			Visible = paused;
		}

		private void OnResumePressed()
		{
			SetPaused(false);
		}

		private void OnMainMenuPressed()
		{
			SetPaused(false);
			GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
		}
	}
}
