using UnityEngine;

[CreateAssetMenu(menuName = "Items/Health Potion")]

public class HealthPotionItem : ItemData
{
    public int diceCount = 2;
    public int diceSides = 4;

    public override void Use(GameObject player)
    {
        var might = player.GetComponent<Might>();
        var health = player.GetComponent<Health>();

        if (might == null || health == null)
            return;

        int roll = 0;
        for (int i = 0; i < diceCount; i++)
            roll += Random.Range(1, diceSides + 1);

        health.Heal(roll * might.CurrentMight);
    }
}