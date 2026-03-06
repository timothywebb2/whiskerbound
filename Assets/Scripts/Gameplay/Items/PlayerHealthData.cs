using UnityEngine;

public class PlayerHealthData : MonoBehaviour, IHasHealth
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;

    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0, MaxHealth);
    }

    public int MaxHealth => maxHealth;
}