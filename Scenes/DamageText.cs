using Godot;

namespace GodotSurvivor.Scenes
{
	/// <summary>
	/// Damage fly text.
	/// </summary>
	public partial class DamageText : Label
	{
		/// <summary>
		/// Shows the damage fly text.
		/// </summary>
		/// <param name="value">Value to show.</param>
		/// <param name="initialPosition">The initial position of the fly text.</param>
		/// <param name="travel">Approximate direction.</param>
		/// <param name="duration">Time before object is removed.</param>
		/// <param name="spread">Spread for the movement.</param>
		/// <param name="crit">Makes the fly text red if true.</param>
		public void ShowValue(string value, Vector2 initialPosition, Vector2 travel, float duration, double spread, bool crit)
		{
			Text = value;
			var movement = travel.Rotated((float)GD.RandRange(-spread / 2, spread / 2));
			PivotOffset = Size / 2;

			var tween = GetTree().CreateTween().SetParallel(true);
			tween.TweenProperty(this, "position", initialPosition + movement, duration).From(initialPosition).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
			tween.TweenProperty(this, "modulate:a", 0.1, duration).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
			if (crit)
			{
				Modulate = new Color(1, 0, 0);
				tween.TweenProperty(this, "scale", Scale, 0.4f).From(Scale * 1.5f).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.In);
			}

			tween.Chain().TweenCallback(Callable.From(() => QueueFree()));
		}
	}
}
