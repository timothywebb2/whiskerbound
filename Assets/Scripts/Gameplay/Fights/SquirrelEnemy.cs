using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class SquirrelEnemy : MonoBehaviour
{
    [Header("Player and Enemies")]
    public GameObject knightPlayer;
    public GameObject sorcererPlayer;
    public GameObject clericPlayer;

    public GameObject squirrelOne;
    public GameObject squirrelTwo;

    [Header("Enemy Values")]
    public int curHealth1;
    public int curHealth2;
    private int maxHealth1;
    private int maxHealth2;
    public int damageType;
    public bool squirrelCoordination;
    public bool squirrelOneDown;
    public bool squirrelTwoDown;
    public int selectingMove;
    public int selectingTarget;
    public int damageOutput;
    public int attackedEnemy;
    public int multiHitting;
    private bool enemy1Attacking;
    private bool isProvoked1;
    private bool isProvoked2;
    private bool isStunned1;
    private bool isStunned2;

    [Header("Fight Management")]
    public GameObject battlePhase;
    public GameObject fightManager;

    [Header("UI/Audio")]
    public TextMeshProUGUI HealthText1;
    public TextMeshProUGUI HealthText2;
    public GameObject VictoryText;
    public float timePassed = 0.0f;
    public bool VictoryAchieved;
    public Slider EnemyHealthBar1;
    public Slider EnemyHealthBar2;
    public AudioClip damageSound;
    public AudioClip healSound;
    public TextMeshProUGUI currentAction;
    public string enemyName1;
    public string enemyName2;
    private AudioSource audioSource;

    [Header("Animation")]
    public Animator animator1;
    public Animator animator2;
    public Animator VFXanimation1;
    public Animator VFXanimation2;
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

        curHealth1 = 30;
        curHealth2 = 30;
        maxHealth1 = curHealth1;
        maxHealth2 = curHealth2;
        EnemyHealthBar1.maxValue = maxHealth1;
        EnemyHealthBar2.maxValue = maxHealth2;
        
        multiHitting = 1;
        damageType = 1; // 1 = PHYS, 2 = MYS, 3 = SPR

        squirrelOneDown = false;
        squirrelTwoDown = false;
        squirrelCoordination = true;
        VictoryText.SetActive(false);
        VictoryAchieved = false;
        UpdateHUD();
        audioSource = GetComponent<AudioSource>();

        animator1 = this.transform.GetChild(0).GetComponent<Animator>();
        animator2 = this.transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isKnightAttacking = knightPlayer.GetComponent<Animator>().GetBool("isAttacking");
        isSorcererAttacking = sorcererPlayer.GetComponent<Animator>().GetBool("isAttacking");
        isClericAttacking = clericPlayer.GetComponent<Animator>().GetBool("isAttacking");

        battlePhase.GetComponent<BattlePhase>().NumberedFight(2);

        if (VictoryAchieved)
        {
            PlayerPrefs.SetInt("BeatSquirrel", 1);
            timePassed += Time.deltaTime;
            if (timePassed > 3.0f)
                SceneManager.LoadScene(PlayerPrefs.GetString("LastScene", "forestVillage"));
        }
    }

    public IEnumerator TakeDamage(int amount, bool isFire)
    {
Debug.Log("SquirrelEnemy/TakeDamage: isFire is " + isFire);
        // check which enemies are still alive
        //ALLOW PLAYER TO PICK
        attackedEnemy = Random.Range(1, 3);
       
        if (squirrelOneDown)
        {   
            multiHitting = 1;
            attackedEnemy = 2;
        }      
        else if (squirrelTwoDown)
        {
            multiHitting = 1;
            attackedEnemy = 1;
        }
       
        if (multiHitting == 1) // only 1 enemy is taking damage
        {
            if (attackedEnemy == 1)
            {
                // wait for previous attack animations to finish, especially for volcanic hex
                yield return new WaitUntil(() => VFXanimation1.GetBool("isAttacked") == false &&
                VFXanimation1.GetBool("isFireAttack") == false);
                // set VFX to damaged animation
                if(isFire)
                    VFXanimation1.SetBool("isFireAttack", true);
                else
                    VFXanimation1.SetBool("isAttacked", true);

                curHealth1 -= amount;
                if(curHealth1 < 0)
                    curHealth1 = 0;
                EnemyHealthBar1.value = curHealth1;
                UpdateHUD();

                if (damageSound != null)     
                    audioSource.PlayOneShot(damageSound);

                yield return new WaitForSeconds(1.5f);

                VFXanimation1.SetBool("isAttacked", false); // reset VFX
                VFXanimation1.SetBool("isFireAttack", false);

Debug.Log("Squirrel 1 took " + amount + " damage and has " + curHealth1 + " health");
                if (curHealth1 <= 0)
                    squirrelOneDown = true;
            }

            else if (attackedEnemy == 2)
            {
                // wait for previous attack animations to finish, especially for volcanic hex
                yield return new WaitUntil(() => VFXanimation2.GetBool("isAttacked") == false &&
                VFXanimation2.GetBool("isFireAttack") == false);
                // set VFX to damaged animation
                if(isFire)
                    VFXanimation2.SetBool("isFireAttack", true);
                else
                    VFXanimation2.SetBool("isAttacked", true);

                curHealth2 -= amount;
                if(curHealth2 < 0)
                    curHealth2 = 0;
                EnemyHealthBar2.value = curHealth2;
                UpdateHUD();

                if (damageSound != null)     
                    audioSource.PlayOneShot(damageSound);

                yield return new WaitForSeconds(1.5f); // let animation play

                VFXanimation2.SetBool("isAttacked", false); // reset VFX
                VFXanimation2.SetBool("isFireAttack", false);

Debug.Log("Squirrel 2 took " + amount + " damage and has " + curHealth2 + " health");
                if (curHealth2 <= 0) 
                    squirrelTwoDown = true;
            }
        }
    
        if (multiHitting == 2) // both enemies are taking damage
        {
            // wait for previous attack animations to finish, especially for volcanic hex
            yield return new WaitUntil(() => VFXanimation1.GetBool("isAttacked") == false &&
            VFXanimation1.GetBool("isFireAttack") == false &&
            VFXanimation2.GetBool("isAttacked") == false &&
            VFXanimation2.GetBool("isFireAttack") == false);
            // set VFX to damaged animation
            if(isFire)
            {
                VFXanimation1.SetBool("isFireAttack", true);
                VFXanimation2.SetBool("isFireAttack", true);
            }
            else
            {
                VFXanimation1.SetBool("isAttacked", true);
                VFXanimation2.SetBool("isAttacked", true);
            }

            curHealth1 -= amount;
            curHealth2 -= amount;
            EnemyHealthBar1.value = curHealth1;
            EnemyHealthBar2.value = curHealth2;
            UpdateHUD();

            if (damageSound != null)     
                audioSource.PlayOneShot(damageSound);
            yield return new WaitForSeconds(1.5f); // let animation play

            VFXanimation1.SetBool("isAttacked", false); // reset VFX
            VFXanimation1.SetBool("isFireAttack", false);
            VFXanimation2.SetBool("isAttacked", false);
            VFXanimation2.SetBool("isFireAttack", false);

Debug.Log("Both enemies took " + amount + " damage");
Debug.Log("Squrrel 1 has " + curHealth1 + " and Squirrel 2 has " + curHealth2);

            if (curHealth1 <= 0)
            {
                squirrelOneDown = true;
                animator1.SetBool("isDefeated", true);
                StartCoroutine("DisableEnemy1");
            }

            if (curHealth2 <= 0)
            {
                squirrelTwoDown = true;
                animator2.SetBool("isDefeated", true);
                StartCoroutine("DisableEnemy2");
            }
            
            multiHitting = 1;
        }

        if (squirrelOneDown || squirrelTwoDown)
            squirrelCoordination = false;

        if (squirrelOneDown && squirrelTwoDown) 
            Victory();   
    }

    // overload method to apply status effects
    public IEnumerator TakeDamage(int target, int amount, bool isFire)
    {
Debug.Log("SquirrelEnemy/TakeDamage: isFire is " + isFire);
        // check which enemies are still alive
        attackedEnemy = target;
       
        if (squirrelOneDown) 
            attackedEnemy = 2;      
        else if (squirrelTwoDown)
            attackedEnemy = 1;
       
        if (multiHitting == 1) // only 1 enemy is taking damage
        {
            if (attackedEnemy == 1)
            {
                // set VFX to damaged animation
                if(isFire)
                    VFXanimation1.SetBool("isFireAttack", true);
                else
                    VFXanimation1.SetBool("isAttacked", true);

                curHealth1 -= amount;
                if(curHealth1 < 0)
                    curHealth1 = 0;
                EnemyHealthBar1.value = curHealth1;
                UpdateHUD();

                if (damageSound != null)     
                    audioSource.PlayOneShot(damageSound);

                yield return new WaitForSeconds(1.5f);

                VFXanimation1.SetBool("isAttacked", false); // reset VFX
                VFXanimation1.SetBool("isFireAttack", false);

Debug.Log("Squirrel 1 took " + amount + " damage and has " + curHealth1 + " health");
                if (curHealth1 <= 0)
                    squirrelOneDown = true;
            }

            else if (attackedEnemy == 2)
            {
                // set VFX to damaged animation
                if(isFire)
                    VFXanimation2.SetBool("isFireAttack", true);
                else
                    VFXanimation2.SetBool("isAttacked", true);

                curHealth2 -= amount;
                if(curHealth2 < 0)
                    curHealth2 = 0;
                EnemyHealthBar2.value = curHealth2;
                UpdateHUD();

                if (damageSound != null)     
                    audioSource.PlayOneShot(damageSound);

                yield return new WaitForSeconds(1.5f); 

                VFXanimation2.SetBool("isAttacked", false); // reset VFX
                VFXanimation2.SetBool("isFireAttack", false);

Debug.Log("Squirrel 2 took " + amount + " damage and has " + curHealth2 + " health");
                if (curHealth2 <= 0) 
                    squirrelTwoDown = true;
            }
        }
    
        if (multiHitting == 2) // both enemies are taking damage
        {
            // set VFX to damaged animation
            if(isFire)
            {
                VFXanimation1.SetBool("isFireAttack", true);
                VFXanimation2.SetBool("isFireAttack", true);
            }
            else
            {
                VFXanimation1.SetBool("isAttacked", true);
                VFXanimation2.SetBool("isAttacked", true);
            }

            curHealth1 -= amount;
            curHealth2 -= amount;
            if(curHealth1 < 0)
                    curHealth1 = 0;
            if(curHealth2 < 0)
                    curHealth2 = 0;
            EnemyHealthBar1.value = curHealth1;
            EnemyHealthBar2.value = curHealth2;
            UpdateHUD();

            if (damageSound != null)     
                audioSource.PlayOneShot(damageSound);

            VFXanimation1.SetBool("isAttacked", false); // reset VFX
            VFXanimation1.SetBool("isFireAttack", false);
            VFXanimation2.SetBool("isAttacked", false);
            VFXanimation2.SetBool("isFireAttack", false);

Debug.Log("Both enemies took " + amount + " damage");
Debug.Log("Squrrel 1 has " + curHealth1 + " and Squirrel 2 has " + curHealth2);

            if (curHealth1 <= 0)
                squirrelOneDown = true;

            if (curHealth2 <= 0)
                squirrelTwoDown = true;
            
            multiHitting = 1;
        }

        if (squirrelOneDown || squirrelTwoDown)
            squirrelCoordination = false;

        if (squirrelOneDown && squirrelTwoDown) 
            Victory();   
    }

    public void gotGoaded(int target)
    {
        if(target == 1)
            isProvoked1 = true;
        else if (target == 2)
            isProvoked2 = true;
    }

    public void gotStunned(int target)
    {
        if(target == 1)
            isStunned1 = true;
        else if (target == 2)
            isStunned2 = true;
    }

    public IEnumerator BeginTurn()
    {
Debug.Log("Reached begin turn, isKnightAttacking is " + isKnightAttacking + " and isSorcererAttacking is " + isSorcererAttacking);
        // wait for player to finish attacking
        yield return new WaitUntil(() => !isKnightAttacking);
        yield return new WaitUntil(() => !isSorcererAttacking);
        yield return new WaitUntil(() => !isClericAttacking);

        if (squirrelOneDown == false)
        {
Debug.Log("SquirrelEnemy/BeginTurn: Squirrel 1 has started attacking!");
            enemy1Attacking = true;

            int stun = 4;
            if(isStunned1)
                stun = Random.Range(1, 5); //generates number 1-4, 1 means the enemy skips turn

            if(stun == 1)
            {
Debug.Log("SquirrelEnemy/BeginTurn: Enemy is stunned! Skipping turn!");
                StartCoroutine(printCurrentAction(enemyName1 + " is stunned and can't attack!", 0f));
            }

            else if(curHealth1 > 0)
            {
                // if enemy is at full health, they can only attack (move 1)
                if(curHealth1 >= maxHealth1)
                {
Debug.Log("SquirrelEnemy/BeginTurn: Enemy is at max health! Attacking!");
                    selectingMove = 1;
                }
                // if enemy is provoked and knight isnt downed, they have to target knight
                if(isProvoked1 && knightPlayer.GetComponent<KnightMoveset>().curHealth > 0)
                {
Debug.Log("SquirrelEnemy/BeginTurn: Squirrel 1 is provoked! Will only attack Knight!");
                    selectingMove = 1;
                    selectingTarget = 1;
                    StartCoroutine(printCurrentAction(enemyName1 + " is provoked! It will only attack the Knight!", 0f));
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
                    damageOutput = Random.Range(1, 7) + 1;

                    animator1.SetBool("isAttacking", true);
                    yield return new WaitUntil(() => animationHit); // wait for animation to show enemy hitting
                    animationHit = false; // reset animation

                    if(squirrelCoordination)
                        damageOutput+= Random.Range(1, 7);
                    
                    // attack sound effect is attached to the player
                    if(selectingTarget == 1)
                    {
Debug.Log("SquirrelEnemy/BeginTurn: Lash is used on the Knight!");
                        knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
                    }
                    else if(selectingTarget == 2)
                    {
Debug.Log("SquirrelEnemy/BeginTurn: Lash is used on the Sorcerer");
                        sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
                    }
                    else if(selectingTarget == 3)
                    {
Debug.Log("SquirrelEnemy/BeginTurn: Lash is used on the Cleric!");
                        clericPlayer.GetComponent<ClericMoveset>().TakeDamage(damageOutput);
                    }
                    
                    yield return new WaitUntil(() => animationDone); // wait for rest of animation to finish
                    animationDone = false;
                    animator1.SetBool("isAttacking", false);
                }
                
                else if (selectingMove == 2)
                {
                    VFXanimation1.SetBool("isHealing", true);
                    damageOutput = Random.Range(1, 5) + 1;

                    if (damageSound != null) // play damage sound 
                        audioSource.PlayOneShot(healSound);

                    if((curHealth1 + damageOutput) > maxHealth1)
                        curHealth1 = maxHealth1;
                    else
                        curHealth1 += damageOutput;
                    
                    EnemyHealthBar1.value = curHealth1;
                    UpdateHUD();
                    StartCoroutine(printCurrentAction(enemyName1 + " healed for " + damageOutput + "!", 0f));
Debug.Log("SquirrelEnemy/BeginTurn: Recuperate is used! Healed for " + damageOutput + " to " + curHealth1 + " health.");
                    yield return new WaitForSeconds(2);
                    VFXanimation1.SetBool("isHealing", false);
                }  
            }
        }

        isProvoked1 = false;
        isStunned1 = false;
        enemy1Attacking = false;
Debug.Log("SquirrelEnemy/BeginTurn: Squirrel 1 has finished attacking!");
        StartCoroutine(BeginTurn2());
    }

    public IEnumerator BeginTurn2()
    {
        if (squirrelTwoDown == false)
        {
Debug.Log("SquirrelEnemy/BeginTurn2: Squirrel 2 has started attacking!");
            yield return new WaitUntil(() => !enemy1Attacking);
            int stun = 4;
            if(isStunned2)
                stun = Random.Range(1, 5); //generates number 1-4, 1 means the enemy skips turn

            if(stun == 1)
            {
Debug.Log("SquirrelEnemy/BeginTurn2: Enemy is stunned! Skipping turn!");
                StartCoroutine(printCurrentAction(enemyName1 + " is stunned and can't attack!", 0f));
            }

            else if (curHealth2 > 0)
            {
                if(curHealth2 >= maxHealth2)
                {
Debug.Log("SquirrelEnemy/BeginTurn2: Enemy 2 is at max health! Attacking!");
                    selectingMove = 1;
                }

                // if enemy is provoked and knight isnt downed, they have to target knight
                if(isProvoked2 && knightPlayer.GetComponent<KnightMoveset>().curHealth > 0)
                {
Debug.Log("SquirrelEnemy/BeginTurn2: Enemy 2 is provoked! Will only attack Knight!");
                    selectingMove = 1;
                    selectingTarget = 1;
                    StartCoroutine(printCurrentAction(enemyName2 + " is provoked! It will only attack the Knight!", 0f));
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
Debug.Log("SquirrelEnemy/BeginTurn2: Running target while loop!");
                            selectingTarget = Random.Range(1, partySize + 1); // can only select party members unlocked

                            switch(selectingTarget)
                            {
                                case 1: //targeting knight
                                    if(knightPlayer.GetComponent<KnightMoveset>().curHealth > 0)
                                    {
Debug.Log("SquirrelEnemy/BeginTurn2: Knight is not downed! Selecting knight!");
                                        goodTarget = true;
                                    }
                                    break;

                                case 2: // targeting sorcerer
                                    if(sorcererPlayer.GetComponent<SorcererMoveset>().curHealth > 0)
                                    {
Debug.Log("SquirrelEnemy/BeginTurn2: Sorcerer is not downed! Selecting sorcerer!");
                                        goodTarget = true;
                                    }
                                    break;
                                case 3: // targeting cleric
                                    if(clericPlayer.GetComponent<ClericMoveset>().curHealth > 0)
                                    {
Debug.Log("SquirrelEnemy/BeginTurn2: Cleric is not downed! Selecting cleric!");
                                        goodTarget = true;
                                    }
                                    break;
                            }
                        }
                    }
                }
            
                if (selectingMove == 1)
                {
                    animator2.SetBool("isAttacking", true);
                    yield return new WaitUntil(() => animationHit); // wait for animation to show enemy hitting
                    animationHit = false; // reset animation

                    damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
                    if(squirrelCoordination)
                        damageOutput+= Random.Range(1, 7);
                        
                    if(selectingTarget == 1)
                    {
Debug.Log("SquirrelEnemy/BeginTurn2: Lash is used on the Knight!");
                        knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
                    }
                    else if(selectingTarget == 2)
                    {
Debug.Log("SquirrelEnemy/BeginTurn2: Lash is used on the Sorcerer!");
                        sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
                    }
                    else if(selectingTarget == 3)
                    {
Debug.Log("SquirrelEnemy/BeginTurn2: Lash is used on the Cleric!");
                        clericPlayer.GetComponent<ClericMoveset>().TakeDamage(damageOutput);
                    }

                //Debug.Log("SquirrelEnemy/BeginTurn: Coroutine is pausing until animation is done");
                    yield return new WaitUntil(() => animationDone); // wait for rest of animation to finish
                    animationDone = false;
                    animator2.SetBool("isAttacking", false);
                }

                else if (selectingMove == 2 && curHealth2 < maxHealth2)
                {
                    VFXanimation2.SetBool("isHealing", true);
                    damageOutput = Random.Range(1, 5) + 1;
                    
                    if (damageSound != null) // play damage sound 
                        audioSource.PlayOneShot(healSound);

                    if((curHealth2 + damageOutput) > maxHealth2)
                        damageOutput = maxHealth2 - curHealth2;
                    curHealth2 += damageOutput;

                    EnemyHealthBar2.value = curHealth2;
                    UpdateHUD();
                    StartCoroutine(printCurrentAction(enemyName2 + " healed for " + damageOutput + "!", 0f));
Debug.Log("SquirrelEnemy/BeginTurn2: Recuperate is used! Healed for " + damageOutput + " to " + curHealth2 + " health.");
                    yield return new WaitForSeconds(2);
                    VFXanimation2.SetBool("isHealing", false);
                }
            }
Debug.Log("SquirrelEnemy/BeginTurn2: Squirrel 2 has finished attacking!");
            isProvoked2 = false;
            isStunned2 = false;
        }
    }

    public void multiHit()
    {
        multiHitting = 2;
    }

    void UpdateHUD()
    {
        int health1Text = curHealth1;
        if(health1Text < 0)
            health1Text = 0;
        int health2Text = curHealth2;
        if(health2Text < 0)
            health1Text = 0;

        HealthText1.text = health1Text + "/" + maxHealth1;
        HealthText2.text = health2Text + "/" + maxHealth2;
    }

    public void Victory()
    {
        fightManager.GetComponent<FightManager>().BattleComplete();
        //animator1.SetBool("isDefeated", true);
        //animator2.SetBool("isDefeated", true);
        VictoryAchieved = true;
        VictoryText.SetActive(true);
    }

    IEnumerator printCurrentAction(string toPrint, float delay)
    {
        yield return new WaitForSeconds(delay);
Debug.Log("SquirrelEnemy/printCurrentAction: Waiting for text to be blank");
        yield return new WaitUntil(() => currentAction.text == "");
Debug.Log("SquirrelEnemy/printCurrentAction: Printing current action");

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

    public IEnumerator DisableEnemy1()
    {
        yield return new WaitForSeconds(3);
        squirrelOne.GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator DisableEnemy2()
    {
        yield return new WaitForSeconds(3);
        squirrelTwo.GetComponent<SpriteRenderer>().enabled = false;
    }
}