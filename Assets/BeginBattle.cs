using UnityEngine;

public class BeginBattle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject fightManager;

    void Awake() {
        fightManager = GameObject.FindGameObjectWithTag("FightManager");
        fightManager.GetComponent<FightManager>().BattleBegin();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
