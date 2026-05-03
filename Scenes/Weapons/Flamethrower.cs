using Godot;
using GodotSurvivor.Scenes.Collectibles;

namespace GodotSurvivor.Scenes.Weapons
{
	public partial class Flamethrower : WeaponBase
	{
		public override string CollectibleId => CollectibleIds.Flamethrower;

		private PackedScene _bulletScene;

		public override void _Ready()
		{
			base._Ready();
			_bulletScene = ResourceLoader.Load<PackedScene>("res://Scenes/Weapons/FlamethrowerBullet.tscn");
		}

		protected override void Shoot()
		{
			var bullet = _bulletScene.Instantiate<FlamethrowerBullet>();
			bullet.CollectibleId = CollectibleId;
			Owner.AddChild(bullet);
			bullet.GlobalPosition = GlobalPosition;
			bullet.GlobalRotation = GlobalRotation;
		}
	}
}
