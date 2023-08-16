using Godot;
using System;

namespace GodotSurvivor.Scenes.Items
{
	public partial class SawbladePart : Area2D
	{
		public event EventHandler<IDamageableByPlayer> OnEnemyHit;

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			RotationDegrees += 1;
		}

		private void OnBodyEntered(Node2D body)
		{
			if (body is IDamageableByPlayer entity)
				OnEnemyHit?.Invoke(this, entity);
		}
	}
}
