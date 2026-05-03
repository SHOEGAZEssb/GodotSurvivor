using Godot;
using GodotSurvivor.Scenes.Collectibles;

namespace GodotSurvivor.Scenes.Weapons
{
	public partial class Pistol : WeaponBase
	{
		public override string CollectibleId => CollectibleIds.Pistol;

		private PackedScene _bulletScene;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			base._Ready();
			_bulletScene = ResourceLoader.Load<PackedScene>("res://Scenes/Weapons/GunBullet.tscn");
		}

		protected override void Shoot()
		{
			var bullet = _bulletScene.Instantiate<GunBullet>();
			bullet.CollectibleId = CollectibleId;
			Owner.AddChild(bullet);
			bullet.Transform = GlobalTransform;
		}
	}
}
