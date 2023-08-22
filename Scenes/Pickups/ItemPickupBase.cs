using Godot;
using GodotSurvivor.Scenes.Player;

namespace GodotSurvivor.Scenes.Pickups
{
	/// <summary>
	/// Base class for items that can be picked up by the
	/// players pickup range.
	/// </summary>
	public partial class ItemPickupBase : Area2D
	{
		/// <summary>
		/// Speed that this item pickup floats to the <see cref="Target"/>.
		/// </summary>
		[Export]
		public float PickupSpeed = 150f;

		/// <summary>
		/// Target to float to.
		/// </summary>
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
