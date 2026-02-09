using UnityEngine;


public class BattlePhase : MonoBehaviour
{


public int battlePhaseTurn;
       public int squirrelFight;
           public GameObject firstEnemy;
           public GameObject KnightIcon;
           public GameObject SorcererIcon;
           public GameObject KnightBattle;
           public GameObject SorcererBattle;


   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
       battlePhaseTurn  = 0;
       squirrelFight = 1;
       firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
       KnightIcon = GameObject.FindGameObjectWithTag("KnightIcon");
       SorcererIcon = GameObject.FindGameObjectWithTag("SorcererIcon");
       KnightBattle = GameObject.FindGameObjectWithTag("KnightBattle");
       SorcererBattle = GameObject.FindGameObjectWithTag("SorcererBattle");
   }


   // Update is called once per frame
   void Update()
   {
       if (battlePhaseTurn == 2) {
           // Temporary
           if (squirrelFight == 1) {
       firstEnemy.GetComponent<DemoEnemy>().BeginTurn();
       battlePhaseTurn = 0;
}
else if (squirrelFight == 2) {
       firstEnemy.GetComponent<SquirrelEnemy>().BeginTurn();
       battlePhaseTurn = 0;
}
else if (squirrelFight == 3) {
       firstEnemy.GetComponent<TigerBoss>().BeginTurn();
       battlePhaseTurn = 0;
       }
       KnightIcon.SetActive(true);
       SorcererIcon.SetActive(true);
       Debug.Log("Test");
   }
   }


   public void NumberedFight(int amount) {
squirrelFight = amount;
KnightBattle.GetComponent<KnightMoveset>().NumberedFight(squirrelFight);
SorcererBattle.GetComponent<SorcererMoveset>().NumberedFight(squirrelFight);
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
