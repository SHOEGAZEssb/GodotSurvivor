using Godot;

namespace GodotSurvivor.Scenes.Weapons
{
	public partial class GunBullet : BulletBase
	{
		[Export]
		public float Speed = 500f;

		public override void _PhysicsProcess(double delta)
		{
			Position += Transform.X * Speed * (float)delta;
		}
	}
}
