using Godot;

namespace GodotSurvivor.Scenes.Pickups
{
	/// <summary>
	/// Picks up <see cref="ItemPickupBase"/>s in its range.
	/// </summary>
	public partial class ItemPickupRange : Area2D
	{
		/// <summary>
		/// The radius of the pickup range.
		/// </summary>
		[Export]
		public float PickupRadius
		{
			get => _pickupCircle?.Radius ?? _pickupRadius;
			set
			{
				_pickupRadius = value;
				if (_pickupCircle != null)
					_pickupCircle.Radius = value;
			}
		}
		private float _pickupRadius;
		private CircleShape2D _pickupCircle;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_pickupCircle = GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
			if (_pickupRadius != 0)
				_pickupCircle.Radius = _pickupRadius;
		}
	}
}
