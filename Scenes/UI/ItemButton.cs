using Godot;
using GodotSurvivor.Scenes.Items;

namespace GodotSurvivor.Scenes.UI
{
	public partial class ItemButton : VBoxContainer
	{
		public PackedScene ItemScene;
		private TextureRect _itemImageRect;
		private Label _nameLabel;
		private Label _descriptionLabel;

		[Signal]
		public delegate void ItemChosenEventHandler(PackedScene itemScene);

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_itemImageRect = GetNode<TextureRect>("ItemImageRect");
			_nameLabel = GetNode<Label>("NameLabel");
			_descriptionLabel = GetNode<Label>("DescriptionLabel");

			// temporarily instanciate item to get metadata
			var item = ItemScene.Instantiate<Node2D>();
			var itemInfo = (item as IItem).Metadata;
			_itemImageRect.Texture = ResourceLoader.Load<Texture2D>(itemInfo.TexturePath);
			_nameLabel.Text = itemInfo.Name;
			_descriptionLabel.Text = itemInfo.Description;
			item.QueueFree();
		}

		private void OnButtonPressed()
		{
			EmitSignal(SignalName.ItemChosen, ItemScene);
		}
	}
}
