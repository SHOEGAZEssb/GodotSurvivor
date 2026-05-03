using Godot;
using GodotSurvivor.Scenes.Collectibles;
using GodotSurvivor.Scenes.Enemies;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// Trinket that makes enemies that are killed by the <see cref="Burning"/>
	/// status drop double experience.
	/// </summary>
	public partial class GlowingCoal : Node, IItem
	{
		public string CollectibleId => CollectibleIds.GlowingCoal;

		public ItemData Metadata => new("Glowing Coal", "Enemies killed by Burning status drop double exp", "res://Sprites/Placeholder.png");

		public override void _Ready()
		{
			var player = GetTree().CurrentScene.GetNode<PlayerController>("Player");
			GetParent().RemoveChild(this);
			player.AddChild(this);
			player.PlayerStats.Items.Add(this);
			CollectibleStatTracker.RecordUnlock(CollectibleId);

			Stats.CurrentStats.EnemyKilledEventHandler += CurrentStats_EnemyKilledEventHandler;
		}

		private void CurrentStats_EnemyKilledEventHandler(object sender, DamageInfo e)
		{
			if (e.DamageSourceType == DamageSource.Burning)
			{
				if (e.Target is EnemyBase enemy)
				{
					enemy.ExperienceWorth *= 2;
					CollectibleStatTracker.RecordUse(CollectibleId);
					CollectibleStatTracker.RecordCustom(CollectibleId, CollectibleStatKeys.BurningKillsBoosted);
					CollectibleStatTracker.GainExperience(CollectibleId, 1);
				}
			}
		}
	}
}
