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
    private bool fireAttack;

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
        curHealth = 200;
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

   public IEnumerator TakeDamage(int amount, bool isFire)
   {
Debug.Log("DemoEnemy/TakeDamage: isFire is " + isFire);
        fireAttack = isFire;
        // set VFX to damaged animation
        if(isFire)
            VFXanimation.SetBool("isFireAttack", true);
        else
            VFXanimation.SetBool("isAttacked", true);

        yield return new WaitForSeconds(1); // give time for animation to start
        
        curHealth -= amount; // take damage
        EnemyHealthBar.value -= amount;

        if (damageSound != null) // play damage sound 
            audioSource.PlayOneShot(damageSound);

        if(isFire)
        {
Debug.Log("DemoEnemy/TakeDamage: Pausing to play fire animation");
            yield return new WaitForSeconds(1.5f);
        }
        
        else
        {   
Debug.Log("DemoEnemy/TakeDamage: Playing slash animation");
            yield return new WaitUntil(() => animationDone);
            animationDone = false;
        }
        
        VFXanimation.SetBool("isAttacked", false); // reset VFX
        VFXanimation.SetBool("isFireAttack", false);

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
Debug.Log("DemoEnemy/BeginTurn: fireAttack is " + fireAttack);
        // wait for player attacks to finish
        if(fireAttack)
        {
Debug.Log("DemoEnemy/BeginTurn: Playing fire attack");
            VFXanimation.SetBool("isFireAttack", true);
            yield return new WaitForSeconds(2);
            VFXanimation.SetBool("isFireAttack", false);
        }
        else
        {
Debug.Log("DemoEnemy/BeginTurn: Playing normal attack");
            VFXanimation.SetBool("isAttacked", true);
            yield return new WaitUntil(() => animationDone);
            animationDone = false;
            VFXanimation.SetBool("isAttacked", false);
        }

Debug.Log("Enemy has started attacking!");

        selectingMove = Random.Range(1, 3);
        selectingTarget = Random.Range(1, 3);
        
        if (selectingMove == 1) 
        {
            damageOutput = Random.Range(1, 7) + 1;
            
            animator.SetBool("isAttacking", true); // start attack animation
Debug.Log("DemoEnemy/BeginTurn: Coroutine is pausing until animation event");
            yield return new WaitUntil(() => animationHit); // wait for animation to show enemy hitting
            animationHit = false; // reset animation
            
            // attack sound effect is attached to player

            // ADD PROVOKE FUNCTIONALITY
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
Debug.Log("DemoEnemy/BeginTurn: Coroutine is pausing until animation is done");
            yield return new WaitUntil(() => animationDone); // wait for rest of animation to finish
            animationDone = false;
            animator.SetBool("isAttacking", false);
        }
        
        else if (selectingMove == 2)
        {
Debug.Log("Recuperate is used!");
            VFXanimation.SetBool("isHealing", true);
            //ADD HEAL SFX
            damageOutput = Random.Range(1, 5) + Random.Range(1, 5) + 1;
            curHealth += damageOutput;
Debug.Log("DemoEnemy/TakeDamage: Coroutine is pausing for 2 seconds");
            yield return new WaitForSeconds(2);
            VFXanimation.SetBool("isHealing", false);
        }

        
Debug.Log("Enemy has finished attacking!");
    }

    void UpdateHUD()
    {
        HealthText.text = curHealth + "/200"; 
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
