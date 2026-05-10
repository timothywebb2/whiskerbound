using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;


public class TigerBoss : MonoBehaviour
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
    public int blessTime;
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
    
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        knightPlayer = GameObject.FindGameObjectWithTag("KnightBattle");
        sorcererPlayer = GameObject.FindGameObjectWithTag("SorcererBattle");
        clericPlayer = GameObject.FindGameObjectWithTag("ClericBattle");

        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
        fightManager = GameObject.FindGameObjectWithTag("FightManager");

        curHealth = 40;
        maxHealth = curHealth;
        EnemyHealthBar.maxValue = maxHealth;

        damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR

        blessTime = 0;
        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
        VictoryText.SetActive(false);
        VictoryAchieved = false;
        UpdateHUD();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    //Update is called once per frame
    void Update()
    {
        isKnightAttacking = knightPlayer.GetComponent<Animator>().GetBool("isAttacking");
        isSorcererAttacking = sorcererPlayer.GetComponent<Animator>().GetBool("isAttacking");
        isClericAttacking = clericPlayer.GetComponent<Animator>().GetBool("isAttacking");

        battlePhase.GetComponent<BattlePhase>().NumberedFight(3);
      
        if (VictoryAchieved == true)
        {
            PlayerPrefs.SetInt("BeatTiger", 1);
            timePassed += Time.deltaTime;
            if (timePassed > 3.0f)
                SceneManager.LoadScene(PlayerPrefs.GetString("LastScene", "forestVillage"));
        }
    }

    public IEnumerator TakeDamage(int amount, bool isFire)
    {
        // wait for previous attack animations to finish, especially for volcanic hex
        yield return new WaitUntil(() => VFXanimation.GetBool("isAttacked") == false &&
        VFXanimation.GetBool("isFireAttack") == false);
        
Debug.Log("TigerBoss/TakeDamage: isFire is " + isFire);
        // set VFX to damaged animation
        if(isFire)
            VFXanimation.SetBool("isFireAttack", true);
        else
            VFXanimation.SetBool("isAttacked", true);
        
        yield return new WaitForSeconds(1); // give time for animation to start

        curHealth -= amount;
        if(curHealth < 0)
            curHealth = 0;
        EnemyHealthBar.value = curHealth;
        UpdateHUD();

        if (damageSound != null)
            audioSource.PlayOneShot(damageSound);
        
        yield return new WaitForSeconds(1.5f);

        VFXanimation.SetBool("isAttacked", false); // reset VFX
        VFXanimation.SetBool("isFireAttack", false);

Debug.Log("TigerBoss/TakeDamage: Enemy took " + amount + " damage and has " + curHealth + " health");
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

        if(curHealth > 0)
        {
            if (blessTime > 0)
            {
                damageOutput = Random.Range(1, 13);
                
                if((curHealth + damageOutput) > maxHealth)
                    damageOutput = maxHealth - curHealth;
                curHealth += damageOutput;

                EnemyHealthBar.value = curHealth;
                UpdateHUD();

                blessTime -= 1;
                StartCoroutine(printCurrentAction("Empower healed " + enemyName + " healed for " + damageOutput + "! " + blessTime + " turns of Empower left!", 0f));
Debug.Log("TigerBoss/BeginTurn: Bless healed boss up to " + curHealth + ". Boss has " + blessTime + " turns of Bless left.");
            }
        
            int stun = 4;
            if(isStunned)
                stun = Random.Range(1, 5); //generates number 1-4, 1 means the enemy skips turn
            
            if(stun == 1)
            {
Debug.Log("TigerBoss/BeginTurn: Enemy is stunned! Skipping turn!");
                StartCoroutine(printCurrentAction(enemyName + " is stunned and can't attack!", 0f));
            }
        }

        if (curHealth > 0)
        {   


            if(curHealth >= maxHealth)
            {
Debug.Log("TigerBoss/BeginTurn: Enemy is at max health!");
                selectingMove = Random.Range(1, 4);
            }
            // if enemy is provoked and knight isnt downed, they have to target knight
            if(isProvoked && knightPlayer.GetComponent<KnightMoveset>().curHealth > 0)
            {
Debug.Log("TigerBoss/BeginTurn: Enemy is provoked! Will only attack Knight!");
                selectingMove = 1;
                selectingTarget = 1;
                StartCoroutine(printCurrentAction(enemyName + " is provoked! It will only attack the Knight!", 0f));
            }

            else
            {
                int partySize = PlayerPrefs.GetInt("PartySize", 1);
                selectingMove = Random.Range(1, 5);
                
                bool goodTarget = false;
                if(selectingMove == 1) // if attacking, check if the chosen target has remaining HP
                {
                    while(!goodTarget)
                    {
Debug.Log("SquirrelEnemy/BeginTurn: Running target while loop!");
                        selectingTarget = Random.Range(1, partySize + 1); // can only select party members unlocked

                        switch(selectingTarget)
                        {
                            case 1: //targeting knight
                                if(knightPlayer.GetComponent<KnightMoveset>().curHealth > 0)
                                {
Debug.Log("SquirrelEnemy/BeginTurn: Knight is not downed! Selecting knight!");
                                    goodTarget = true;
                                }
                                break;

                            case 2: // targeting sorcerer
                                if(sorcererPlayer.GetComponent<SorcererMoveset>().curHealth > 0)
                                {
Debug.Log("SquirrelEnemy/BeginTurn: Sorcerer is not downed! Selecting sorcerer!");
                                    goodTarget = true;
                                }
                                break;
                            case 3: // targeting cleric
                                if(clericPlayer.GetComponent<ClericMoveset>().curHealth > 0)
                                {
Debug.Log("SquirrelEnemy/BeginTurn: Cleric is not downed! Selecting cleric!");
                                    goodTarget = true;
                                }
                                break;
                        }
                    }
                }
            }

            if (selectingMove == 1)
            {
                damageOutput = Random.Range(1, 13) + Random.Range(1, 13);
                
                animator.SetBool("isAttacking", true); // start attack animation
//Debug.Log("DemoEnemy/BeginTurn: Coroutine is pausing until animation event");
                yield return new WaitUntil(() => animationHit); // wait for animation to show enemy hitting
                animationHit = false; // reset animation

                // attack sound effect is attached to player
                if (selectingTarget == 1)
                {
Debug.Log("TigerBoss/BeginTurn: Crush is used on the Knight!");
                    knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
                }
                else if (selectingTarget == 2)
                {
Debug.Log("TigerBoss/BeginTurn: Crush is used on the Sorcerer!");
                    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
                }
                else if (selectingTarget == 3)
                {
Debug.Log("TigerBoss/BeginTurn: Crush is used on the Cleric!");
                    clericPlayer.GetComponent<ClericMoveset>().TakeDamage(damageOutput);
                }

//Debug.Log("DemoEnemy/BeginTurn: Coroutine is pausing until animation is done");
                yield return new WaitUntil(() => animationDone); // wait for rest of animation to finish
                animationDone = false;
                animator.SetBool("isAttacking", false);
            }

            else if (selectingMove == 2)
            {
                damageOutput = Random.Range(1, 7) + Random.Range(1, 7);

                animator.SetBool("isAttacking", true); // start attack animation
//Debug.Log("DemoEnemy/BeginTurn: Coroutine is pausing until animation event");
                yield return new WaitUntil(() => animationHit); // wait for animation to show enemy hitting
                animationHit = false; // reset animation

Debug.Log("TigerBoss/BeginTurn: Sweep is used!");
                knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
                if(PlayerPrefs.GetInt("PartySize", 1) > 1)
                {
                    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
                    if(PlayerPrefs.GetInt("PartySize", 1) > 2)
                        clericPlayer.GetComponent<ClericMoveset>().TakeDamage(damageOutput);
                }

                //Debug.Log("DemoEnemy/BeginTurn: Coroutine is pausing until animation is done");
                yield return new WaitUntil(() => animationDone); // wait for rest of animation to finish
                animationDone = false;
                animator.SetBool("isAttacking", false);
            }
            
            else if (selectingMove == 3)
            {
                //ADD VFX (MAYBE DIFFERENT FROM HEALING ONE?)
                StartCoroutine(printCurrentAction(enemyName + " used empower! Will passively heal for three turns!", 0f));
Debug.Log("TigerBoss/BeginTurn: Empower is used! Gained 3 turns of blessing!");
                blessTime += 3;
            }

            else if (selectingMove == 4)
            {
                VFXanimation.SetBool("isHealing", true);
                damageOutput = Random.Range(1, 5) + Random.Range(1, 5);

                if (damageSound != null) // play damage sound 
                    audioSource.PlayOneShot(healSound);

                if((curHealth + damageOutput) > maxHealth)
                    damageOutput = maxHealth - curHealth;
                curHealth += damageOutput;

                EnemyHealthBar.value = curHealth;
                UpdateHUD();
                StartCoroutine(printCurrentAction(enemyName + " healed for " + damageOutput + "!", 0f));
Debug.Log("TigerBoss/BeginTurn: Tiger Ward is used! Healed to " + curHealth);
                yield return new WaitForSeconds(2);
                VFXanimation.SetBool("isHealing", false);
            }
        }

        isProvoked = false;
        isStunned = false;
Debug.Log("TigerBoss/BeginTurn: Enemy has finished attacking!");
    }

    void UpdateHUD()
    {
        HealthText.text = curHealth + "/" + maxHealth;
    }

    public void Victory()
    {
        fightManager.GetComponent<FightManager>().BattleComplete();
        animator.SetBool("isDefeated", true);
        VictoryAchieved = true;
        VictoryText.SetActive(true);
Debug.Log("Victory achieved!");
    }
    IEnumerator printCurrentAction(string toPrint, float delay)
    {
        yield return new WaitForSeconds(delay);
Debug.Log("TigerBoss/printCurrentAction: Waiting for text to be blank");
        yield return new WaitUntil(() => currentAction.text == "");
Debug.Log("TigerBoss/printCurrentAction: Printing current action");

        currentAction.enabled = true;
        currentAction.text = toPrint;

        yield return new WaitForSeconds(3);

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