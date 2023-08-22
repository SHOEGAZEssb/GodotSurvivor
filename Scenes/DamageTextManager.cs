using Godot;
using System;

namespace GodotSurvivor.Scenes
{
	/// <summary>
	/// Manager object for displaying <see cref="DamageText"/>s.
	/// </summary>
	public partial class DamageTextManager : Node2D
	{
		/// <summary>
		/// Approximate direction.
		/// </summary>
		[Export]
		public Vector2 Travel = new(0, -80);

		/// <summary>
		/// Length of animation in seconds.
		/// </summary>
		[Export]
		public float Duration = 0.7f;

		/// <summary>
		/// Spread for the travel.
		/// </summary>
		[Export]
		public double Spread = Math.PI / 2;

		private PackedScene _damageText;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_damageText = ResourceLoader.Load<PackedScene>("res://Scenes/DamageText.tscn");
		}

		/// <summary>
		/// Creates and shows a new <see cref="DamageText"/>.
		/// </summary>
		/// <param name="value">Value to show.</param>
		/// <param name="position">Initial position.</param>
		/// <param name="crit">Crit or not.</param>
		public void ShowFloatingText(int value, Vector2 position, bool crit)
		{
			var dt = _damageText.Instantiate<DamageText>();
			GetTree().Root.AddChild(dt);
			dt.ShowValue(value.ToString(), position, Travel, Duration, Spread, crit);
		}
	}
}