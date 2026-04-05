using UnityEngine;


public class FightManager : MonoBehaviour
{


public int CurrentEncounter;
public GameObject ferretEnemy;
public GameObject squirrelEnemy;
public GameObject tigerBoss;


   void Awake() {
      // CurrentEncounter += 1;
       DontDestroyOnLoad(this.gameObject);

       ferretEnemy = GameObject.FindGameObjectWithTag("FerretBossParent");
       squirrelEnemy = GameObject.FindGameObjectWithTag("SquirrelBossParent");
       tigerBoss = GameObject.FindGameObjectWithTag("TigerBossParent");

     /*  if (CurrentEncounter == 1) {
      //  ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
      ferretEnemy.SetActive(true);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(false);
       }
       if (CurrentEncounter == 2) {
       // ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
       ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(true);
      tigerBoss.SetActive(false);
       }
       if (CurrentEncounter == 3) {
      //  ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
      ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(true);
       }

       */
   }
  
   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {


   }


   // Update is called once per frame
   void Update()
   {
      
   }

   public void BattleComplete()
   {
    CurrentEncounter += 1;
   }

   public void BattleBegin()
   {

ferretEnemy = GameObject.FindGameObjectWithTag("FerretBossParent");
       squirrelEnemy = GameObject.FindGameObjectWithTag("SquirrelBossParent");
       tigerBoss = GameObject.FindGameObjectWithTag("TigerBossParent");

      if (CurrentEncounter == 1) {
      //  ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
      ferretEnemy.SetActive(true);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(false);
       }
       if (CurrentEncounter == 2) {
       // ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
       ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(true);
      tigerBoss.SetActive(false);
       }
       if (CurrentEncounter == 3) {
      //  ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
      ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(true);
       }
   }

}
