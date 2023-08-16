namespace GodotSurvivor.Scenes
{
    public interface IDamageable
    {
		// Workaround for not being able to put signal delegates in interfaces.
		string OnTakeDamageSignalName { get; }
    }
}