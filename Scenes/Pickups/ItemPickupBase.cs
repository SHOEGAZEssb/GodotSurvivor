using Godot;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.Pickups
{
	public partial class ItemPickupBase : Area2D
	{
		[Export]
		public float PickupSpeed = 150f;

		public PlayerController Target;

		public override void _PhysicsProcess(double delta)
		{
			if (Target != null)
			{
				Position = Position.MoveToward(Target.Position, PickupSpeed * (float)delta);
				if (Position.DistanceTo(Target.Position) <= 1f)
					OnPickup(Target);
			}
		}

		private void OnAreaEntered(Area2D other)
		{
			if (other is ItemPickupRange ipr)
				Target = ipr.GetParent<PlayerController>();
		}

		protected virtual void OnPickup(PlayerController player)
		{
			GD.PrintErr("This method should never be called");
		}
	}
}
