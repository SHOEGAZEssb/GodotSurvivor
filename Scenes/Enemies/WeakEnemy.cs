using Godot;

namespace GodotSurvivor.Scenes.Enemies
{
	public partial class WeakEnemy : EnemyBase
	{
		public override void _PhysicsProcess(double delta)
		{
			var moveVector = (_player.Position - Position).Normalized();
			MoveAndCollide(moveVector * Speed * (float)delta);
			Animate(moveVector);
		}

		private void Animate(Vector2 moveVector)
		{
			// right and left
			if (moveVector.X >= 0)
				_sprite.Play("walk_right");
			else if (moveVector.X <= 0)
				_sprite.Play("walk_left");
		}
	}
}
