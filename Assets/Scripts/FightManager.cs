using UnityEngine;


public class FightManager : MonoBehaviour
{


public int CurrentEncounter;
public GameObject ferretEnemy;
public GameObject squirrelEnemy;
public GameObject tigerBoss;
public GameObject tazEnemy;
public GameObject meerkatEnemy;
public GameObject kangarooBoss;


   void Awake() {
      // CurrentEncounter += 1;
       DontDestroyOnLoad(this.gameObject);

       ferretEnemy = GameObject.FindGameObjectWithTag("FerretBossParent");
       squirrelEnemy = GameObject.FindGameObjectWithTag("SquirrelBossParent");
       tigerBoss = GameObject.FindGameObjectWithTag("TigerBossParent");
       tazEnemy = GameObject.FindGameObjectWithTag("TazBossParent");
       meerkatEnemy = GameObject.FindGameObjectWithTag("MeerkatBossParent");
       kangarooBoss = GameObject.FindGameObjectWithTag("KangarooBossParent");

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
       tazEnemy = GameObject.FindGameObjectWithTag("TazBossParent");
       meerkatEnemy = GameObject.FindGameObjectWithTag("MeerkatBossParent");
       kangarooBoss = GameObject.FindGameObjectWithTag("KangarooBossParent");

      if (CurrentEncounter == 1) {
      //ferret
      ferretEnemy.SetActive(true);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(false);
      tazEnemy.SetActive(false);
      meerkatEnemy.SetActive(false);
      kangarooBoss.SetActive(false);
      PlayerPrefs.SetInt("SpawnPoint", 1);
       }
       if (CurrentEncounter == 2) {
       //squirrel
       ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(true);
      tigerBoss.SetActive(false);
      tazEnemy.SetActive(false);
      meerkatEnemy.SetActive(false);
      kangarooBoss.SetActive(false);
      PlayerPrefs.SetInt("SpawnPoint", 2);
       }
       if (CurrentEncounter == 3) {
      //tiger
      ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(true);
      tazEnemy.SetActive(false);
      meerkatEnemy.SetActive(false);
      kangarooBoss.SetActive(false);
      PlayerPrefs.SetInt("SpawnPoint", 3);
       }
       if (CurrentEncounter == 4) {
      //  ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
      ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(false);
      tazEnemy.SetActive(true);
      meerkatEnemy.SetActive(false);
      kangarooBoss.SetActive(false);
      PlayerPrefs.SetInt("SpawnPoint", 1);
       }
       if (CurrentEncounter == 5) {
       // ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
       ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(false);
      tazEnemy.SetActive(false);
      meerkatEnemy.SetActive(true);
      kangarooBoss.SetActive(false);
      PlayerPrefs.SetInt("SpawnPoint", 2);
       }
       if (CurrentEncounter == 6) {
      //  ferretEnemy.GetComponent<DemoEnemy>().NotFerretFight();
      ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(false);
      tazEnemy.SetActive(false);
      meerkatEnemy.SetActive(false);
      kangarooBoss.SetActive(true);
      PlayerPrefs.SetInt("SpawnPoint", 3);
       }
   }

}
