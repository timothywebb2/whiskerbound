using UnityEngine;

[CreateAssetMenu(menuName = "Items/Adrenaline")]

public class AdrenalineItem : ItemData
{
    public int mightBonus = 3;

    public override void Use(GameObject player)
    {
        var might = player.GetComponent<Might>();
        if (might != null)
            might.AddTemporaryMight(mightBonus);
    }
}