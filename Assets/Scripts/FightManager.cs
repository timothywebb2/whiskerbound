using UnityEngine;


public class FightManager : MonoBehaviour
{
    //forest enemies
   public GameObject ferretEnemy;
   public GameObject squirrelEnemy;
   public GameObject tigerBoss;

    //desert enemies
   public GameObject tazEnemy;
   public GameObject meerkatEnemy;
   public GameObject kangarooBoss;

    //cave enemies
    public GameObject batEnemy;
    public GameObject lionEnemy;
    public GameObject bearBoss;

    //arctic
    public GameObject walrusBoss;

    void Awake()
   {
      DontDestroyOnLoad(this.gameObject);

      ferretEnemy = GameObject.FindGameObjectWithTag("FerretBossParent");
      squirrelEnemy = GameObject.FindGameObjectWithTag("SquirrelBossParent");
      tigerBoss = GameObject.FindGameObjectWithTag("TigerBossParent");
      tazEnemy = GameObject.FindGameObjectWithTag("TazBossParent");
      meerkatEnemy = GameObject.FindGameObjectWithTag("MeerkatBossParent");
      kangarooBoss = GameObject.FindGameObjectWithTag("KangarooBossParent");
   }

   public void BattleComplete()
   {
      //note: add specific code for when specific enemies are beaten ie. might upgrades
      Debug.Log("Battle complete");
      if(PlayerPrefs.GetInt("Enemy", 0) == 6)
         PlayerPrefs.SetInt("ArcticKey", 0);
   }

   public void BattleBegin()
   {
      ferretEnemy = GameObject.FindGameObjectWithTag("FerretBossParent");
      squirrelEnemy = GameObject.FindGameObjectWithTag("SquirrelBossParent");
      tigerBoss = GameObject.FindGameObjectWithTag("TigerBossParent");
      tazEnemy = GameObject.FindGameObjectWithTag("TazBossParent");
      meerkatEnemy = GameObject.FindGameObjectWithTag("MeerkatBossParent");
      kangarooBoss = GameObject.FindGameObjectWithTag("KangarooBossParent");

      ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(false);
      tazEnemy.SetActive(false);
      meerkatEnemy.SetActive(false);
      kangarooBoss.SetActive(false);

      switch(PlayerPrefs.GetInt("Enemy"))
      {
         case 1:
            ferretEnemy.SetActive(true);
            break;
         case 2:
            squirrelEnemy.SetActive(true);
            break;
         case 3:
            tigerBoss.SetActive(true);
            break;
         case 4:
            tazEnemy.SetActive(true);
            break;
         case 5:
            meerkatEnemy.SetActive(true);
            break;
         case 6:
            kangarooBoss.SetActive(true);
            break;
      }

      PlayerPrefs.SetInt("Enemy", 0);
   }
}
