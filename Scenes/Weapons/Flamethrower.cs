using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotSurvivor.Scenes.Weapons
{
	public partial class Flamethrower : WeaponBase
	{
		private PackedScene _bulletScene;

		public override void _Ready()
		{
			base._Ready();
			_bulletScene = ResourceLoader.Load<PackedScene>("res://Scenes/Weapons/FlamethrowerBullet.tscn");
		}

		protected override void Shoot()
		{
			var bullet = _bulletScene.Instantiate<FlamethrowerBullet>();
			Owner.AddChild(bullet);
			bullet.GlobalPosition = GlobalPosition;
			bullet.GlobalRotation = GlobalRotation;
		}
	}
}
