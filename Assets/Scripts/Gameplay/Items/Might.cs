using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Might : MonoBehaviour
{
    public int BaseMight { get; set; } = 1;
    public int TemporaryMight { get; set; } = 0;

    public int CurrentMight => BaseMight + TemporaryMight;


    public void AddTemporaryMight(int amount)
    {
        TemporaryMight += amount;
    }

    public void ClearTemporaryMight()
    {
        TemporaryMight = 0;
    }

    public interface IUsableItem
    {
        void Use(GameObject player);
    }

    [CreateAssetMenu(menuName = "Items/Adrenaline")]
    public class AdrenalineItem : ScriptableObject, IUsableItem
    {
        public int mightBonus = 3;

        public void Use(GameObject player)
        {
            Might might = player.GetComponent<Might>();
            if (might != null)
            {
                might.AddTemporaryMight(mightBonus);
            }
        }
    }
}