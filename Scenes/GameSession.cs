namespace GodotSurvivor.Scenes
{
	public static class GameSession
	{
		public const string DefaultWeaponScenePath = "res://Scenes/Weapons/Flamethrower.tscn";
		public const string PistolScenePath = "res://Scenes/Weapons/Pistol.tscn";
		public const string FlamethrowerScenePath = "res://Scenes/Weapons/Flamethrower.tscn";

		public static string SelectedWeaponScenePath { get; set; } = DefaultWeaponScenePath;
	}
}
