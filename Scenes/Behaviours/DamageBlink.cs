using Godot;
using GodotSurvivor.Scenes;

public partial class DamageBlink : Node2D
{
	private AnimatedSprite2D _sprite;
	private Tween _tween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var parent = GetParent();
		var bla = parent.Connect((parent as IDamageable).OnTakeDamageSignalName, Callable.From(() => OnTakeDamage()));
		_sprite = parent.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	private void OnTakeDamage()
	{
		_tween?.Kill();

		var tween = CreateTween();
		tween.TweenProperty(_sprite, "modulate", Colors.Red, 0.0f);
		tween.TweenCallback(Callable.From(() => _sprite.Modulate = new Color(1, 1, 1, 1))).SetDelay(0.1f);
	}
}
