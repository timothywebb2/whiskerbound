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
    public GameObject clericPlayer;

    [Header("Enemy Values")]
    public int curHealth;
    private int maxHealth;
    public int damageType;
    public int selectingMove;
    public int selectingTarget;
    public int damageOutput;
    private bool isProvoked; // checks if this enemy was hit with the knight's Provoke attack
    private bool isStunned; // checks if the enemy was stunned by sorcerer, if so rolls to skip turn

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
    public AudioClip healSound;
    public TextMeshProUGUI currentAction;
    public string enemyName;
    private AudioSource audioSource;

    [Header("Animation")]
    public Animator animator;
    public Animator VFXanimation;
    private bool animationHit;
    private bool animationDone;

    private bool isKnightAttacking;
    private bool isSorcererAttacking;
    private bool isClericAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        knightPlayer = GameObject.FindGameObjectWithTag("KnightBattle");
        sorcererPlayer = GameObject.FindGameObjectWithTag("SorcererBattle");
        clericPlayer = GameObject.FindGameObjectWithTag("ClericBattle");

        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
        fightManager = GameObject.FindGameObjectWithTag("FightManager");

        curHealth = 20;
        maxHealth = curHealth;
        EnemyHealthBar.maxValue = maxHealth;

        damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR

        VictoryText.SetActive(false);
        VictoryAchieved = false;
        UpdateHUD();
        audioSource = GetComponent<AudioSource>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isKnightAttacking = knightPlayer.GetComponent<Animator>().GetBool("isAttacking");
        isSorcererAttacking = sorcererPlayer.GetComponent<Animator>().GetBool("isAttacking");
        isClericAttacking = clericPlayer.GetComponent<Animator>().GetBool("isAttacking");

        battlePhase.GetComponent<BattlePhase>().NumberedFight(1);
      
        if (VictoryAchieved == true)
        {
            timePassed += Time.deltaTime;
          
            if (timePassed > 3.0f)
            {
Debug.Log("DemoEnemy/Update: Change scene");
                if(PlayerPrefs.GetInt("Enemy", 0) == 10)
                    SceneManager.LoadScene("MAIN MENU");
                
                else
                    SceneManager.LoadScene(PlayerPrefs.GetString("LastScene", "forestVillage")); 
           }
       }
   }

   public IEnumerator TakeDamage(int amount, bool isFire)
   {
        // wait for previous attack animations to finish, especially for volcanic hex
        yield return new WaitUntil(() => VFXanimation.GetBool("isAttacked") == false &&
        VFXanimation.GetBool("isFireAttack") == false);

Debug.Log("DemoEnemy/TakeDamage: isFire is " + isFire);
        // set VFX to damaged animation
        if(isFire)
            VFXanimation.SetBool("isFireAttack", true);
        else
            VFXanimation.SetBool("isAttacked", true);
        
        curHealth -= amount; // take damage
        if(curHealth < 0)
            curHealth = 0;
        EnemyHealthBar.value -= amount;
        UpdateHUD();

        if (damageSound != null) // play damage sound 
            audioSource.PlayOneShot(damageSound);

        yield return new WaitForSeconds(1.5f);
        
        VFXanimation.SetBool("isAttacked", false); // reset VFX
        VFXanimation.SetBool("isFireAttack", false);

Debug.Log("DemoEnemy/TakeDamage: Enemy took " + amount + " damage and has " + curHealth + " health");
        if (curHealth <= 0)
            Victory();
   }

    public void gotGoaded()
    {
        isProvoked = true;
    }

    public void gotStunned()
    {
        isStunned = true;
    }

    public IEnumerator BeginTurn()
    {
        // wait for player attacks to finish
        yield return new WaitUntil(() => !isKnightAttacking);
        yield return new WaitUntil(() => !isSorcererAttacking);
        yield return new WaitUntil(() => !isClericAttacking);

Debug.Log("DemoEnemy/BeginTurn: Enemy has started attacking!");

        int stun = 4;
        if(isStunned)
            stun = Random.Range(1, 5); //generates number 1-4, 1 means the enemy skips turn
        
        if(stun == 1)
        {
Debug.Log("DemoEnemy/BeginTurn: Enemy is stunned! Skipping turn!");
            StartCoroutine(printCurrentAction(enemyName + " is stunned and can't attack!", 0f));
        }

        else if(curHealth > 0)
        {
            // if enemy is at full health, they can only attack (move 1)
            if(curHealth >= maxHealth)
            {
Debug.Log("DemoEnemy/BeginTurn: Enemy is at max health! Attacking!");
                selectingMove = 1;
            }
            // if enemy is provoked and knight isnt downed, they have to target knight
            if(isProvoked && knightPlayer.GetComponent<KnightMoveset>().curHealth > 0)
            {
Debug.Log("DemoEnemy/BeginTurn: Enemy is provoked! Will only attack Knight!");
                selectingMove = 1;
                selectingTarget = 1;
                StartCoroutine(printCurrentAction(enemyName + " is provoked! It will only attack the Knight!", 0f));
            }

            else
            {
                int partySize = PlayerPrefs.GetInt("PartySize", 1);
                selectingMove = Random.Range(1, 3);

                bool goodTarget = false;
                if(selectingMove == 1) // if attacking, check if the chosen target has remaining HP
                {
                    while(!goodTarget)
                    {
Debug.Log("DemoEnemy/BeginTurn: Running target while loop!");
                        selectingTarget = Random.Range(1, partySize + 1); // can only select party members unlocked

                        switch(selectingTarget)
                        {
                            case 1: //targeting knight
                                if(knightPlayer.GetComponent<KnightMoveset>().curHealth > 0)
                                {
Debug.Log("DemoEnemy/BeginTurn: Knight is not downed! Selecting knight!");
                                    goodTarget = true;
                                }
                                break;

                            case 2: // targeting sorcerer
                                if(sorcererPlayer.GetComponent<SorcererMoveset>().curHealth > 0)
                                {
Debug.Log("DemoEnemy/BeginTurn: Sorcerer is not downed! Selecting sorcerer!");
                                    goodTarget = true;
                                }
                                break;
                            case 3: // targeting cleric
                                if(clericPlayer.GetComponent<ClericMoveset>().curHealth > 0)
                                {
Debug.Log("DemoEnemy/BeginTurn: Cleric is not downed! Selecting cleric!");
                                    goodTarget = true;
                                }
                                break;
                        }
                    }
                }
            }
            
            // log printing attached to player
            if (selectingMove == 1) 
            {
                damageOutput = Random.Range(1, 7) + 1;
                
                animator.SetBool("isAttacking", true); // start attack animation
//Debug.Log("DemoEnemy/BeginTurn: Coroutine is pausing until animation event");
                yield return new WaitUntil(() => animationHit); // wait for animation to show enemy hitting
                animationHit = false; // reset animation
                
                // attack sound effect is attached to player
                if (selectingTarget == 1)
                {
Debug.Log("DemoEnemy/BeginTurn: Lash is used on the Knight!");
                    knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
                }
                else if (selectingTarget == 2)
                {
Debug.Log("DemoEnemy/BeginTurn: Lash is used on the Sorcerer!");
                    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
                }
                else if (selectingTarget == 3)
                {
Debug.Log("DemoEnemy/BeginTurn: Lash is used on the Cleric!");
                    clericPlayer.GetComponent<ClericMoveset>().TakeDamage(damageOutput);
                }
                
//Debug.Log("DemoEnemy/BeginTurn: Coroutine is pausing until animation is done");
                yield return new WaitUntil(() => animationDone); // wait for rest of animation to finish
                animationDone = false;
                animator.SetBool("isAttacking", false);
            }
            
            else if (selectingMove == 2)
            {
                VFXanimation.SetBool("isHealing", true);

                if (damageSound != null) // play damage sound 
                    audioSource.PlayOneShot(healSound);

                damageOutput = Random.Range(1, 5) + Random.Range(1, 5) + 1;

                if((curHealth + damageOutput) > maxHealth)
                    damageOutput = maxHealth - curHealth;
                curHealth += damageOutput;
                
                EnemyHealthBar.value = curHealth;
                UpdateHUD();
                StartCoroutine(printCurrentAction(enemyName + " healed for " + damageOutput + "!", 0f));
Debug.Log("DemoEnemy/BeginTurn: Recuperate is used! Healed for " + damageOutput + " to " + curHealth + " health.");
                yield return new WaitForSeconds(2);
                VFXanimation.SetBool("isHealing", false);
            }
        }

        isProvoked = false;
        isStunned = false;
Debug.Log("DemoEnemy/BeginTurn: Enemy has finished attacking!");
    }

    void UpdateHUD()
    {
        HealthText.text = curHealth + "/" + maxHealth; 
    }

    public void Victory()
    {
        fightManager.GetComponent<FightManager>().BattleComplete();
        VictoryAchieved = true;
        VictoryText.SetActive(true);
        animator.SetBool("isDefeated", true);
    }

    IEnumerator printCurrentAction(string toPrint, float delay)
    {
        yield return new WaitForSeconds(delay);
Debug.Log("DemoEnemy/printCurrentAction: Waiting for text to be blank");
        yield return new WaitUntil(() => currentAction.text == "");
Debug.Log("DemoEnemy/printCurrentAction: Printing current action");
        currentAction.enabled = true;
        currentAction.text = toPrint;

        yield return new WaitForSeconds(3);
        currentAction.text = "";

        currentAction.enabled = false;
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
