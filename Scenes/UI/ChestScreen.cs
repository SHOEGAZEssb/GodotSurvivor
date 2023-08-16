using Godot;
using GodotSurvivor.Scenes.Helper;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.UI
{
	public partial class ChestScreen : CenterContainer
	{
		[Signal]
		public delegate void ItemChosenEventHandler();

		private HBoxContainer _itemButtonContainer;

		public override void _Ready()
		{
			GetTree().Paused = true;
			_itemButtonContainer = GetNode<HBoxContainer>("ItemButtonContainer");
			var itemButtonScene = ResourceLoader.Load<PackedScene>("res://Scenes/UI/ItemButton.tscn");
			var items = Stats.CurrentStats.ItemPool.GetRandomListItems(3);
			foreach (var item in items)
			{
				var itemButton = itemButtonScene.Instantiate<ItemButton>();
				itemButton.ItemChosen += ItemButton_ItemChosen;
				itemButton.ItemScene = item;
				_itemButtonContainer.AddChild(itemButton);
			}
		}

		private void ItemButton_ItemChosen(PackedScene itemScene)
		{
			var item = itemScene.Instantiate<Node>();
			GetTree().CurrentScene.AddChild(item);
			GetTree().Paused = false;
			EmitSignal(SignalName.ItemChosen);
			Stats.CurrentStats.ItemPool.Remove(itemScene);
			QueueFree();
		}
	}
}
