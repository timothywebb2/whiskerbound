using UnityEngine;

public class Health : MonoBehaviour
{
    private IHasHealth data;

    void Awake()
    {
        data = GetComponent<IHasHealth>();
        if (data == null)
        {
            Debug.LogError("Health requires IHasHealth on the same GameObject");
        }
    }

    public void Heal(int amount)
    {
        if (data == null) return;

        data.CurrentHealth = Mathf.Min(
            data.CurrentHealth + amount,
            data.MaxHealth
        );
    }

    public void TakeDamage(int amount)
    {
        if (data == null) return;

        data.CurrentHealth -= amount;
    }
}