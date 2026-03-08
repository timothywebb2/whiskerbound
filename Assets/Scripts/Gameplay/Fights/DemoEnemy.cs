using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class DemoEnemy : MonoBehaviour
{


   public int curHealth;
   public int damageType;
   public GameObject knightPlayer;
    public GameObject sorcererPlayer;
    public int selectingMove;
         public int selectingTarget;
         public int damageOutput;
                   public GameObject battlePhase;
public GameObject fightManager;
       public TextMeshProUGUI HealthText;
       public GameObject VictoryText;
       public float timePassed = 0.0f;
       public bool VictoryAchieved;
       public Slider EnemyHealthBar;
       public AudioClip damageSound;
       private AudioSource audioSource;
       private Animator animator;


   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
       knightPlayer = GameObject.FindGameObjectWithTag("KnightBattle");
       sorcererPlayer = GameObject.FindGameObjectWithTag("SorcererBattle");
        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
        fightManager = GameObject.FindGameObjectWithTag("FightManager");
       curHealth = 20;
       damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR
       selectingMove = 1;
       selectingTarget = 1;
       VictoryText.SetActive(false);
       VictoryAchieved = false;
       UpdateHUD();
       audioSource = GetComponent<AudioSource>();
       animator = this.GetComponent<Animator>();
   }


   // Update is called once per frame
   void Update()
   {
        battlePhase.GetComponent<BattlePhase>().NumberedFight(1);
      
       if (VictoryAchieved == true)
       {
           PlayerPrefs.SetInt("BeatFerret", 1);
           timePassed += Time.deltaTime;
          
           if (timePassed > 3.0f)
           {
Debug.Log("Change scene");
               SceneManager.LoadScene("forestOverworld");
           }
       }
   }


   public void TakeDamage(int amount) {
       curHealth -= amount;
       if (damageSound != null)
{
    audioSource.PlayOneShot(damageSound);
}
       EnemyHealthBar.value -= amount;
       UpdateHUD();
       if (curHealth <= 0) {
           Victory();
       }
   }


   public void gotGoaded() {
       // Here is where the code will be for the enemy when they're goaded once allies are added
   }


   public void gotStunned()
   {
       // Here is where the code will be for the enemy when they're stunned
   }


   public void BeginTurn() {
    animator.SetBool("isAttacking", true);
       selectingMove = Random.Range(1, 3);
       selectingTarget = Random.Range(1, 3);
if (selectingMove == 1) {
   if (selectingTarget == 1) {
   Debug.Log("Lash is used!");
   damageOutput = Random.Range(1, 7) + 1;
   knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
   }
   if (selectingTarget == 2) {
   Debug.Log("Lash is used!");
   damageOutput = Random.Range(1, 7) + 1;
   sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
   }
}
else if (selectingMove == 2) {
   Debug.Log("Recuperate is used!");
   damageOutput = Random.Range(1, 5) + Random.Range(1, 5) + 1;
   curHealth += damageOutput;
}
animator.SetBool("isAttacking", false);
   }


   void UpdateHUD()
   {
    //   HealthText.text = "HP: " + curHealth;
   }

  /* void NotFerretFight() {
    gameObject.active = false;
   } */


public void Victory() {
fightManager.GetComponent<FightManager>().BattleComplete();
VictoryAchieved = true;
VictoryText.SetActive(true);
Debug.Log("Victory achieved!");


}


}
