using Godot;
using GodotSurvivor.Scenes.Player;
using GodotSurvivor.Scenes.UI;

namespace GodotSurvivor.Scenes.Pickups
{
	/// <summary>
	/// Chest that grants random items from the <see cref="Stats.ItemPool"/>.
	/// </summary>
	public partial class Chest : Area2D
	{
		private PackedScene _chestScreenScene;
		private CanvasLayer _parentCanvas;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_chestScreenScene = ResourceLoader.Load<PackedScene>("res://Scenes/UI/ChestScreen.tscn");
			_parentCanvas = (CanvasLayer)GetTree().CurrentScene.FindChild("CanvasLayer");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		private void OnAreaEntered(Area2D area)
		{
			if (area is PlayerController player)
			{
				var chestScreen = _chestScreenScene.Instantiate<ChestScreen>();
				chestScreen.ItemChosen += ChestScreen_ItemChosen;
				_parentCanvas.AddChild(chestScreen);
			}
		}

		private void ChestScreen_ItemChosen()
		{
			QueueFree();
		}
	}
}
