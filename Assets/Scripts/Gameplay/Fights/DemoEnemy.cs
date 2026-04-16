using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;


public class DemoEnemy : MonoBehaviour
{
    [Header("Player")]
    public GameObject knightPlayer;
    public GameObject sorcererPlayer;

    [Header("Enemy Values")]
    public int curHealth;
    public int damageType;
    public int selectingMove;
    public int selectingTarget;
    public int damageOutput;

    [Header("Fight Management")]
    public GameObject battlePhase;
    public GameObject fightManager;

    [Header("UI/Audio")]
    public TextMeshProUGUI HealthText;
    public GameObject VictoryText;
    public float timePassed = 0.0f;
    public bool VictoryAchieved;
    public Slider EnemyHealthBar;
    public AudioClip damageSound;
    private AudioSource audioSource;

    [Header("Animation")]
    private Animator animator;
    public GameObject enemyVFX;
    private Animator VFXanimation;
    private bool animationHit;
    private bool animationDone;

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
        VFXanimation = enemyVFX.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        battlePhase.GetComponent<BattlePhase>().NumberedFight(1);
      
        if (VictoryAchieved == true)
        {
            timePassed += Time.deltaTime;
          
            if (timePassed > 3.0f)
            {
Debug.Log("Change scene");
                if(PlayerPrefs.GetInt("Enemy", 0) == 10)
                    SceneManager.LoadScene("MAIN MENU");
                
                else
                    SceneManager.LoadScene(PlayerPrefs.GetString("LastScene", "forestVillage"));
           }
       }
   }

   public void TakeDamage(int amount)
   {
        //DETERMINE WHAT ANIMATION TO PLAY WHEN TAKING DAMAGE
        curHealth -= amount;
        if (damageSound != null)     
            audioSource.PlayOneShot(damageSound);

        EnemyHealthBar.value -= amount;
Debug.Log("Enemy took " + amount + " damage and has " + curHealth + " health");
        UpdateHUD();
        if (curHealth <= 0)
            Victory();
   }

    public void gotGoaded()
    {
        // Here is where the code will be for the enemy when they're goaded once allies are added
    }

    public void gotStunned()
    {
        // Here is where the code will be for the enemy when they're stunned
    }

    //public void BeginTurn()
    public IEnumerator BeginTurn()
    {
Debug.Log("Enemy has started attacking!");
        selectingMove = Random.Range(1, 3);
        selectingTarget = Random.Range(1, 3);
        
        if (selectingMove == 1) 
        {
            damageOutput = Random.Range(1, 7) + 1;
            
            animator.SetBool("isAttacking", true); // start attack animation
            yield return new WaitUntil(() => animationHit); // wait for animation to show enemy hitting
            animationHit = false; // reset animation
            
            // attack sound effect is attached to player

            if (selectingTarget == 1)
            {
Debug.Log("Lash is used on the Knight!");
                knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
            }
            
            if (selectingTarget == 2)
            {
Debug.Log("Lash is used on the Sorcerer!");
                sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
            }

            yield return new WaitUntil(() => animationDone); // wait for rest of animation to finish
            animationDone = false;
            animator.SetBool("isAttacking", false);
        }
        
        else if (selectingMove == 2)
        {
Debug.Log("Recuperate is used!");
            enemyVFX.SetActive(true);
            VFXanimation.SetBool("isAttacking", false);
            //ADD HEAL SFX
            damageOutput = Random.Range(1, 5) + Random.Range(1, 5) + 1;
            curHealth += damageOutput;
            yield return new WaitForSeconds(2);
            enemyVFX.SetActive(false);
        }
        
Debug.Log("Enemy has finished attacking!");
    }

    void UpdateHUD()
    {
        // HealthText.text = "HP: " + curHealth;
    }

    public void Victory()
    {
        fightManager.GetComponent<FightManager>().BattleComplete();
        VictoryAchieved = true;
        VictoryText.SetActive(true);
Debug.Log("Victory achieved!");
    }

    public void AnimationHit()
    {
        animationHit = true;
    }

    public void AnimationDone()
    {
        animationDone = true;
    }
}
