using Godot;
using GodotSurvivor.Scenes.Collectibles;
using System.Linq;

namespace GodotSurvivor.Scenes.UI
{
	public partial class StatsScreen : Control
	{
		private GridContainer _itemGrid;
		private Label _emptyLabel;
		private TextureRect _selectedIcon;
		private Label _selectedName;
		private Label _selectedDescription;
		private VBoxContainer _statsList;

		public override void _Ready()
		{
			_itemGrid = GetNode<GridContainer>("%ItemGrid");
			_emptyLabel = GetNode<Label>("%EmptyLabel");
			_selectedIcon = GetNode<TextureRect>("%SelectedIcon");
			_selectedName = GetNode<Label>("%SelectedName");
			_selectedDescription = GetNode<Label>("%SelectedDescription");
			_statsList = GetNode<VBoxContainer>("%StatsList");

			GetNode<Button>("%BackButton").Pressed += OnBackPressed;
			BuildItemsTab();
		}

		private void BuildItemsTab()
		{
			var unlockedDefinitions = CollectibleStatTracker.GetUnlockedDefinitions().ToList();
			_emptyLabel.Visible = unlockedDefinitions.Count == 0;

			foreach (var definition in unlockedDefinitions)
			{
				var button = new Button
				{
					CustomMinimumSize = new Vector2(76, 76),
					Icon = ResourceLoader.Load<Texture2D>(definition.TexturePath),
					ExpandIcon = true,
					TooltipText = definition.Name
				};
				button.Pressed += () => ShowDetails(definition);
				_itemGrid.AddChild(button);
			}

			if (unlockedDefinitions.Count > 0)
				ShowDetails(unlockedDefinitions[0]);
			else
				ShowEmptyDetails();
		}

		private void ShowDetails(CollectibleDefinition definition)
		{
			var stats = CollectibleStatTracker.GetStats(definition.Id);
			_selectedIcon.Texture = ResourceLoader.Load<Texture2D>(definition.TexturePath);
			_selectedName.Text = definition.Name;
			_selectedDescription.Text = definition.Description;

			foreach (var child in _statsList.GetChildren())
				child.QueueFree();

			AddSectionHeader("Tracked Stats");

			foreach (var statKey in definition.SupportedStats)
			{
				var statDefinition = CollectibleCatalog.StatDefinitions.TryGetValue(statKey, out var value)
					? value
					: new StatDefinition(statKey, statKey);

				_statsList.AddChild(new Label
				{
					Text = $"{statDefinition.Label}: {stats.GetValue(statKey)}"
				});
			}

			AddSectionHeader($"Available Upgrades ({definition.AvailableUpgrades.Count})");

			if (definition.AvailableUpgrades.Count == 0)
			{
				_statsList.AddChild(new Label
				{
					Text = "No level-up upgrades.",
					AutowrapMode = TextServer.AutowrapMode.WordSmart
				});
				return;
			}

			foreach (var upgrade in definition.AvailableUpgrades)
			{
				bool discovered = stats.PickedUpgradeIds != null && stats.PickedUpgradeIds.Contains(upgrade.Id);
				string stackInfo = upgrade.MaxStacks == int.MaxValue
					? "Repeatable"
					: $"Max {upgrade.MaxStacks}";

				var upgradeLabel = new Label
				{
					Text = discovered
						? $"{upgrade.Name} ({stackInfo}): {upgrade.Description}"
						: "???",
					AutowrapMode = TextServer.AutowrapMode.WordSmart
				};
				_statsList.AddChild(upgradeLabel);
			}
		}

		private void ShowEmptyDetails()
		{
			_selectedIcon.Texture = null;
			_selectedName.Text = "No Items Used";
			_selectedDescription.Text = "Start a game and use a weapon or item to unlock it here.";

			foreach (var child in _statsList.GetChildren())
				child.QueueFree();
		}

		private void AddSectionHeader(string text)
		{
			var header = new Label
			{
				Text = text,
				CustomMinimumSize = new Vector2(0, 28)
			};
			header.AddThemeFontSizeOverride("font_size", 20);
			header.AddThemeColorOverride("font_color", new Color(0.9f, 0.78f, 0.42f));
			_statsList.AddChild(header);
		}

		private void OnBackPressed()
		{
			GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu.tscn");
		}
	}
}
