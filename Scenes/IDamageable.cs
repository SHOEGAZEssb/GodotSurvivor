namespace GodotSurvivor.Scenes
{
	/// <summary>
	/// Interface for all damageable entities.
	/// </summary>
	public interface IDamageable
	{
		/// <summary>
		/// Name of the OnTakeDamage event.
		/// Workaround for not being able to put signal delegates in interfaces.
		/// </summary>
		string OnTakeDamageSignalName { get; }
	}
}