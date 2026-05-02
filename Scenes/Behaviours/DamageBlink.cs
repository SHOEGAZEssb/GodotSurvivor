using Godot;

namespace GodotSurvivor.Scenes.Behaviours
{
	/// <summary>
	/// Makes its IDamageable parent blink red when taking damage.
	/// </summary>
	public partial class DamageBlink : Node2D
	{
		private AnimatedSprite2D _sprite;
		private Tween _tween;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var parent = GetParent();
			parent.Connect((parent as IDamageable).OnTakeDamageSignalName, Callable.From(() => OnTakeDamage()));
			_sprite = parent.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		}

		/// <summary>
		/// Make the <see cref="_sprite"/> blink red once.
		/// </summary>
		private void OnTakeDamage()
		{
			_tween?.Kill();

			_tween = CreateTween();
			_tween.TweenProperty(_sprite, "modulate", Colors.Red, 0.0f);
			_tween.TweenCallback(Callable.From(() => _sprite.Modulate = new Color(1, 1, 1, 1))).SetDelay(0.1f);
		}
	}
}
