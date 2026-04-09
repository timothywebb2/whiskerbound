using UnityEngine;
using UnityEngine.UI;


public class FightManager : MonoBehaviour
{
   public GameObject ferretEnemy;
   public GameObject squirrelEnemy;
   public GameObject tigerBoss;
   public GameObject tazEnemy;
   public GameObject meerkatEnemy;
   public GameObject kangarooBoss;
   public GameObject batEnemy;
   public GameObject lionEnemy;
   public GameObject bearBoss;
   public GameObject walrusBoss;

   public RawImage background;
   public Texture forestBackground;
   public Texture desertBackground;
   public Texture caveBackground;
   public Texture arcticBackground;

   public void Start()
   {
      DontDestroyOnLoad(this.gameObject);
   }
   public void BattleComplete()
   {
      //note: add specific code for when specific enemies are beaten ie. might upgrades
      Debug.Log("Battle complete");
      switch(PlayerPrefs.GetInt("Enemy", 0))
      {
         case 1:
            PlayerPrefs.SetInt("BeatFerret", 1);
            break;
         case 4:
            PlayerPrefs.SetInt("BeatTaz", 1);
            break;
         case 5:
            PlayerPrefs.SetInt("BeatMeerkat", 1);
            break;
         case 6:
            PlayerPrefs.SetInt("BeatKangaroo", 1);
            PlayerPrefs.SetInt("ArcticKey", 1);
            break;
         case 7:
            PlayerPrefs.SetInt("BeatBat", 1);
            break;
         case 8:
            PlayerPrefs.SetInt("BeatLion", 1);
            break;
         case 9:
            PlayerPrefs.SetInt("BeatBear", 1);
            break;
         default:
            Debug.Log("This enemy doesn't exist or is the final boss!");
            break;
      }
   }

   public void BattleBegin()
   {
      background = GameObject.FindGameObjectWithTag("Background").GetComponent<RawImage>();

      ferretEnemy = GameObject.FindGameObjectWithTag("FerretBossParent");
      squirrelEnemy = GameObject.FindGameObjectWithTag("SquirrelBossParent");
      tigerBoss = GameObject.FindGameObjectWithTag("TigerBossParent");
      tazEnemy = GameObject.FindGameObjectWithTag("TazBossParent");
      meerkatEnemy = GameObject.FindGameObjectWithTag("MeerkatBossParent");
      kangarooBoss = GameObject.FindGameObjectWithTag("KangarooBossParent");
      batEnemy = GameObject.FindGameObjectWithTag("BatBossParent");
      lionEnemy = GameObject.FindGameObjectWithTag("CaveLionBossParent");
      bearBoss = GameObject.FindGameObjectWithTag("GrizzlyBearParent");
      walrusBoss = GameObject.FindGameObjectWithTag("WalrusBossParent");


      ferretEnemy.SetActive(false);
      squirrelEnemy.SetActive(false);
      tigerBoss.SetActive(false);
      tazEnemy.SetActive(false);
      meerkatEnemy.SetActive(false);
      kangarooBoss.SetActive(false);
      batEnemy.SetActive(false);
      lionEnemy.SetActive(false);
      bearBoss.SetActive(false);
      walrusBoss.SetActive(false);

      switch(PlayerPrefs.GetInt("Enemy"))
      {
         case 1:
            ferretEnemy.SetActive(true);
            background.texture = forestBackground;
            break;
         case 2:
            squirrelEnemy.SetActive(true);
            background.texture = forestBackground;
            break;
         case 3:
            tigerBoss.SetActive(true);
            background.texture = forestBackground;
            break;
         case 4:
            tazEnemy.SetActive(true);
            background.texture = desertBackground;
            break;
         case 5:
            meerkatEnemy.SetActive(true);
            background.texture = desertBackground;
            break;
         case 6:
            kangarooBoss.SetActive(true);
            background.texture = desertBackground;
            break;
         case 7:
            batEnemy.SetActive(true);
            background.texture = caveBackground;
            break;
         case 8:
            lionEnemy.SetActive(true);
            background.texture = caveBackground;
            break;
         case 9:
            bearBoss.SetActive(true);
            background.texture = caveBackground;
            break;
         case 10:
            walrusBoss.SetActive(true);
            background.texture = arcticBackground;
            break;
      }
   }
}
