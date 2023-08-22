using Godot;
using System;

namespace GodotSurvivor.Scenes.Items
{
	/// <summary>
	/// Single rotating part of the <see cref="Sawblade"/>.
	/// </summary>
	public partial class SawbladePart : Area2D
	{
		/// <summary>
		/// Event that is fired when this sawblade part hits an enemy.
		/// </summary>
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
