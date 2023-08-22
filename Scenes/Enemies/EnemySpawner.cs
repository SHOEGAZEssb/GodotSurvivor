using Godot;
using GodotSurvivor.Scenes.Player;
using System;

namespace GodotSurvivor.Scenes.Enemies
{
	/// <summary>
	/// Handles the spawning of enemies and enemy waves.
	/// </summary>
	public partial class EnemySpawner : Node2D
	{
		private PackedScene _weakEnemyScene;
		private PlayerController _player;
		private Timer _timer;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_weakEnemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Enemies/WeakEnemy.tscn");
			_player = GetTree().CurrentScene.GetNode<PlayerController>("Player");
			_timer = GetNode<Timer>("Timer");
		}

		private void TimerFinished()
		{
			SpawnWeakEnemy(5);
			_timer.Start();
		}

		private void SpawnWeakEnemy(int amount)
		{
			var currentScene = GetTree().CurrentScene;
			for (int i = 0; i < amount; i++)
			{
				var enemy = _weakEnemyScene.Instantiate<WeakEnemy>();
				enemy.Position = _player.Position + new Vector2(300, 0).Rotated((float)GD.RandRange(0, 2 * Math.PI));
				currentScene.AddChild(enemy);
			}
		}
	}
}