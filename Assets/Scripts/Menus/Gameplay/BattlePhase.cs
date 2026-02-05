using UnityEngine;

public class BattlePhase : MonoBehaviour
{

public int battlePhaseTurn;
        public int squirrelFight;
            public GameObject firstEnemy;
            public GameObject KnightIcon;
            public GameObject SorcererIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battlePhaseTurn  = 0;
        squirrelFight = 1;
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
        KnightIcon = GameObject.FindGameObjectWithTag("KnightIcon");
        SorcererIcon = GameObject.FindGameObjectWithTag("SorcererIcon");
    }

    // Update is called once per frame
    void Update()
    {
        if (battlePhaseTurn == 2) {
            // Temporary
            if (squirrelFight == 1) {
        firstEnemy.GetComponent<DemoEnemy>().BeginTurn();
}
else if (squirrelFight == 2) {
        firstEnemy.GetComponent<SquirrelEnemy>().BeginTurn();
}
else if (squirrelFight == 3) {
        firstEnemy.GetComponent<TigerBoss>().BeginTurn();
        }
        KnightIcon.SetActive(true);
        SorcererIcon.SetActive(true);
        battlePhaseTurn = 0;
    }
    }

    public void NumberedFight(int amount) {
squirrelFight = amount;
    }

    public void ActionInputted() {
        battlePhaseTurn += 1;

     /*   if (battlePhaseTurn == 2) {
            // Temporary
            if (squirrelFight == 1) {
        firstEnemy.GetComponent<DemoEnemy>().BeginTurn();
}
else if (squirrelFight == 2) {
        firstEnemy.GetComponent<SquirrelEnemy>().BeginTurn();
}
else if (squirrelFight == 3) {
        firstEnemy.GetComponent<TigerBoss>().BeginTurn();
        }
        KnightIcon.SetActive(true);
        SorcererIcon.SetActive(true);
        battlePhaseTurn = 0;
    }
    */
}
}