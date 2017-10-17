/// <summary>
/// Apply this interface to anything that can take damage.
/// </summary>
public interface IDamageable {
    #region Properties
    int Health { get; set; }
    int MaxHealth { get; set; }
    #endregion

    #region Methods
    void Damage(int amount);
    void Heal(int amount);
    void RestoreHealth();
    #endregion
}
