using Godot;

namespace GodotSurvivor.Scenes.Pickups
{
	public partial class ItemPickupRange : Area2D
	{
		[Export]
		public float PickupRadius
		{
			get => _pickupCircle.Radius;
			set => _pickupCircle.Radius = value;
		}
		private CircleShape2D _pickupCircle;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_pickupCircle = GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
		}
	}
}
