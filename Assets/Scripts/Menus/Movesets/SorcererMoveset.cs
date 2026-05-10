using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class SorcererMoveset : MonoBehaviour
{
    [Header("Sorcerer Values")]
    public int maxHealth;
    public int curHealth;
    public int damageType;
    public int mightBonus;
    public int damageOutput;
    public int damageOutputBefore; // This is temporary
    public int shieldOutput;
    public int thornsOutput;
    int hasThorns;
    public int thornDamage;
    public int healOutput;
    public bool rallyOrNot;
    public int volcanicTally;
    public bool intercedeOn;

    [Header("Allies")]
    public KnightMoveset knightAlly;
    public ClericMoveset clericAlly;

    [Header("Fight Management")]
    public int squirrelFight;
    public GameObject battlePhase;
    public float timePassed = 0.0f;
    public GameObject firstEnemy;
    public bool loseCondition;

    [Header("UI/Audio")]
    public GameObject sorcererIcon;
    public GameObject SorcererSkills;
    public GameObject opacity;
    public GameObject closeButton;
    public TextMeshProUGUI HealthText;
    public GameObject LoseText;
    public Slider Sorcererhealthbar;
    public AudioClip damageSound;
    private AudioSource audioSource;
    public TextMeshProUGUI currentAction;
    public bool printing;
    public TextMeshProUGUI hexText;

    [Header("Animation")]
    public Animator animator;
    public GameObject VFXObject;
    private Animator VFXanimator;

    [Header("Items")]
    public bool doubleCastActive = false;
    private System.Action lastMove;
    public bool healUsedThisTurn = false;
    public bool damageReductionActive = false;
    public bool damageReductionUsedThisTurn = false;
    public float damageReductionPercent = 0.75f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PlayerPrefs.GetInt("PartySize", 1) < 2)
        {
            sorcererIcon.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        // KEEP BETWEEN BATTLES

        //curHealth = PlayerPrefs.GetInt("SorcererHealth", 60);
        curHealth = maxHealth;

        Sorcererhealthbar.maxValue = maxHealth;
        Sorcererhealthbar.value = curHealth;
       
        intercedeOn = false;
        rallyOrNot = false;
        hasThorns = 0;
        thornDamage = 0;
        volcanicTally = 0;
        // damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR

        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
        // intercedeOn = false;
        currentAction.enabled = false;
        loseCondition = false;
        squirrelFight = 1;
        LoseText.SetActive(false);
        hexText.text = "0/30";
        UpdateHUD();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        VFXanimator = VFXObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (loseCondition == true)
        {
            timePassed += Time.deltaTime;
            if (timePassed > 3.0f)
                SceneManager.LoadScene(PlayerPrefs.GetString("LastVillage", "forestVillage"));
        }
    }

    public void TakeDamage(int amount)
    {
        if (GameModeManager.Instance.IsGodMode())
        {
            curHealth = maxHealth;
            UpdateHUD();
            return;
        }

        if (intercedeOn == false)
        {
            int finalDamage = amount;
            if (damageReductionActive)
            {
                finalDamage = Mathf.RoundToInt(amount * damageReductionPercent);
                damageReductionActive = false; //consumes the effect

Debug.Log("SorcererMoveset/TakeDamage: Damage reduce from " + amount + " to " + finalDamage);
            }

            curHealth -= finalDamage;
            if(curHealth < 0)
                curHealth = 0;
            Sorcererhealthbar.value = curHealth;

            VFXanimator.SetBool("isAttacked", true);

            if (damageSound != null)
                audioSource.PlayOneShot(damageSound);
           
            //if(!printing)
                StartCoroutine(printCurrentAction("Sorcerer took " + finalDamage + " damage!", 1f));
            
            VFXanimator.SetBool("isAttacked", false);
       }
       
        else if (intercedeOn == true)
        {
Debug.Log("SorcererMoveset/TakeDamage: Damage blocked!");
            //if (!printing)
                StartCoroutine(printCurrentAction("Damage blocked from Sorcerer!", 1f));
            intercedeOn = false;
        }

        if (curHealth >= 25)
        {
            knightAlly.UnLastStand();
        }
        
        else if (curHealth < 25)
        {
            knightAlly.LastStand();
        }
        
        if (curHealth <= 0)
        {
            Lose();
        }

        UpdateHUD();
    }

    public void UseHealPotion()
    {
        StartCoroutine(HealPotion());
    }

    public IEnumerator HealPotion()
    {
        if (!GameModeManager.Instance.IsInfiniteCoins() && healUsedThisTurn)
        {
            //if (!printing)
                StartCoroutine(printCurrentAction("Potion already used this turn!", 0f));
            //return;
        }

        bool usedItem = ItemManager.Instance.UseItem("HealPotion");

        if (!GameModeManager.Instance.IsInfiniteCoins() && !usedItem)
        {
            StartCoroutine(printCurrentAction("No charges left!", 0f));
            //return;
        }

            healUsedThisTurn = true;

            int oldHealth = curHealth;
            int healAmount = Random.Range(2, 9);

            curHealth += healAmount;

            if (curHealth > maxHealth)
                curHealth = maxHealth;

            int actualHeal = curHealth - oldHealth;
            Sorcererhealthbar.value = curHealth;
            
            VFXanimator.SetBool("isHealing", true);
            //PUT HEAL SFX

Debug.Log("SorcererMoveset/HealPotion: Healed for " + actualHeal);
            //if (!printing)
                StartCoroutine(printCurrentAction("Healed for " + actualHeal + " HP!", 0f));
            
            yield return new WaitForSeconds(2);
            VFXanimator.SetBool("isHealing", false);
    }

    public void DamageReductionPotion()
    {
        if (!GameModeManager.Instance.IsInfiniteCoins() && damageReductionUsedThisTurn)
        {
            //if (!printing)
                StartCoroutine(printCurrentAction("Already used this turn!", 0f));
            return;
        }

        if (!GameModeManager.Instance.IsInfiniteCoins() && !ItemManager.Instance.UseItem("ArcaneEssence"))
        {
            StartCoroutine(printCurrentAction("No charges left!", 0f));
            return;
        }

        damageReductionUsedThisTurn = true;
        damageReductionActive = true;

Debug.Log("SorcererMoveset/DamageReductionPotion: Damage reduction activated!");

        //if (!printing)
            StartCoroutine(printCurrentAction("Damage taken reduced by 25% for next hit!", 0f));
    }

    public void Incinerate()
    {
        lastMove = Incinerate;
        ExecuteMove(() =>
        {

            damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + mightBonus;
            if (squirrelFight == 1)
            {
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));
            }
            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput, true));
            }
            else if (squirrelFight == 3)
            {
                StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput, true));
            }
            volcanicTally += damageOutput;
            hexText.text = volcanicTally + "/30";
Debug.Log("SorcererMoveset/Incinerate: Damaged enemy by " + damageOutput + " with Incinerate");
            VolcanicHex();
            PassTurn();
        }, () => "Damaged enemy by " + damageOutput + " with Incinerate!");
   }

   public void Enervate()
   {
        lastMove = Enervate;
        ExecuteMove(() =>
        {

            damageOutput = Random.Range(1, 7) + mightBonus;
            if (squirrelFight == 1)
            {
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));
                firstEnemy.GetComponent<DemoEnemy>().gotStunned();
            }
            else if (squirrelFight == 2)
            {
                //firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                int attackedEnemy = Random.Range(1, 3);
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(attackedEnemy, damageOutput, true));
                firstEnemy.GetComponent<SquirrelEnemy>().gotStunned(attackedEnemy);
            }
            else if (squirrelFight == 3)
            {
                StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput, true));
                firstEnemy.GetComponent<TigerBoss>().gotStunned();
            }
            volcanicTally += damageOutput;
            hexText.text = volcanicTally + "/30";

Debug.Log("SorcererMoveset/Enervate: Damaged enemy by " + damageOutput);
            VolcanicHex();
            PassTurn();
        }, () => "Damaged enemy by " + damageOutput + " with Enervate!");
   }

    public void Ward()
    {
        lastMove = Ward;
        ExecuteMove(() =>
        {

Debug.Log("SorcererMoveset/Ward: Ward activated!");
            shieldOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7) + mightBonus;
            knightAlly.gotShielded(shieldOutput);
            clericAlly.gotShielded(shieldOutput);
            PassTurn();
        }, () => "Ward used on Knight and Cleric!");
    }

    public void Scourge()
    {
        lastMove = Scourge;
        ExecuteMove(() =>
        {
            //SELECT ALLY
Debug.Log("SorcererMoveset/Scourge: Scourge activated!");
            thornsOutput = Random.Range(1, 7) + mightBonus;
            knightAlly.gotThorns(thornsOutput);
            clericAlly.gotThorns(thornsOutput);
            PassTurn();
        }, () => "Scourge used on Knight and Cleric!");
    }


    public void VolcanicHex()
    {
        if (volcanicTally >= 30)
        {
            damageOutput = Random.Range(1, 13) + mightBonus;
            
            if (squirrelFight == 1)
            { 
                int health = firstEnemy.GetComponent<DemoEnemy>().curHealth;
                if(health > 0)
                {
                    StartCoroutine(printCurrentAction("Volcanic Hex unleashed on all enemies!", 0f));
                    StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));
                }
            }

            else if (squirrelFight == 2)
            {
                int health1 = firstEnemy.GetComponent<SquirrelEnemy>().curHealth1;
                int health2 = firstEnemy.GetComponent<SquirrelEnemy>().curHealth2;
                if(health1 > 0 || health2 > 0)
                {
                    firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                    StartCoroutine(printCurrentAction("Volcanic Hex unleashed on all enemies!", 0f));
                    StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput, true));
                }
            }
            
            else if (squirrelFight == 3)
            {
                int health = firstEnemy.GetComponent<TigerBoss>().curHealth;
                if(health > 0)
                {
                    StartCoroutine(printCurrentAction("Volcanic Hex unleashed on all enemies!", 0f));
                    StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput, true));
                }
            }
            volcanicTally = 0;
            hexText.text = "0/30";
        }
    }

    public void RallyIncinerate()
    {
        damageOutput = Random.Range(1, 4) + Random.Range(1, 4) + mightBonus;

        if (squirrelFight == 1)
            StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));

        else if (squirrelFight == 2)
        {
            firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
            StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput, true));
        }

        else if (squirrelFight == 3)
            StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput, true));

Debug.Log("SorcererMoveset/RallyIncinerate: Damaged enemy by " + damageOutput + " with Incinerate");
    }

    public void RallyEnervate()
    {
        damageOutput = Random.Range(1, 4) + mightBonus;

        if (squirrelFight == 1)
        {
            StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));
            firstEnemy.GetComponent<DemoEnemy>().gotStunned();
        }

        else if (squirrelFight == 2)
        {
            int attackedEnemy = Random.Range(1, 3);
            StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput, true));
            firstEnemy.GetComponent<SquirrelEnemy>().gotStunned(attackedEnemy);
        }

        else if (squirrelFight == 3)   
        {
            StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput, true));
            firstEnemy.GetComponent<TigerBoss>().gotStunned();
        }

Debug.Log("SorcererMoveset/RallyEnervate: Damaged enemy by " + damageOutput);
    }

    public void IntercedeSorcerer()
    {
Debug.Log("SorcererMoveset/IntercedeSorcerer: Intercede on Sorcerer!");
        intercedeOn = true;
    }

    public void NotSquirrelFight()
    {
        squirrelFight = 1;
    }

    public void SquirrelFight()
    {
        squirrelFight = 2;
    }

    public void NumberedFight(int amount)
    {
        squirrelFight = amount;
    }

    public void gotShielded(int amount)
    {
        curHealth += amount;
    }

    public void gotThorns(int amount)
    {
        hasThorns += 2;
        thornDamage = amount;
    }

    public void GotHealed(int amount)
    {
        if (curHealth + amount >= maxHealth)
            curHealth = maxHealth;
        else
            curHealth += amount;
    }

    void UpdateHUD()
    {
        HealthText.text = curHealth + "/" + maxHealth;
    }

     public void OpenSorcererSkills()
    {   
        int partySize = PlayerPrefs.GetInt("PartySize", 1);

        if (!printing &&// if the log isnt printing...
        animator.GetBool("isAttacking") == false && // AND if no party members are in the middle of an attack or being attacked/healed..
        knightAlly.animator.GetBool("isAttacking") == false &&
        (clericAlly.animator.GetBool("isAttacking") == false || partySize < 3) &&  // (animation check skips cleric if not unlocked)
        curHealth > 0) //... AND sorcerer isnt downed
        {
            // check if enemies are in middle of attack or being attacked/healed
            bool enemyIsAttacking = true;
            bool enemyIsAttacking2 = false;

            bool enemyVFX = true;
            bool enemyVFX2 = false;
            if (squirrelFight == 1)
            {
                DemoEnemy enemy = firstEnemy.GetComponent<DemoEnemy>();
                enemyIsAttacking = enemy.animator.GetBool("isAttacking");

                var stateInfo = enemy.VFXanimation.GetCurrentAnimatorStateInfo(0);
                if(stateInfo.IsTag("Neutral"))
                    enemyVFX = false;
            }
            else if (squirrelFight == 2)
            {
                SquirrelEnemy enemy = firstEnemy.GetComponent<SquirrelEnemy>();
                enemyIsAttacking = enemy.animator1.GetBool("isAttacking");
                enemyIsAttacking2 = enemy.animator2.GetBool("isAttacking");

                var stateInfo = enemy.VFXanimation1.GetCurrentAnimatorStateInfo(0);
                var stateInfo2 = enemy.VFXanimation2.GetCurrentAnimatorStateInfo(0);
        
                if(stateInfo.IsTag("Neutral"))
                    enemyVFX = false;
                if(stateInfo2.IsTag("Neutral"))
                    enemyVFX2 = false;

Debug.Log("enemyVFX1 is " + enemyVFX);
Debug.Log("enemyVFX2 is " + enemyVFX2);
            }
            else if (squirrelFight == 3)
            {
                TigerBoss enemy = firstEnemy.GetComponent<TigerBoss>();
                enemyIsAttacking = enemy.animator.GetBool("isAttacking");

                var stateInfo = enemy.VFXanimation.GetCurrentAnimatorStateInfo(0);
                if(stateInfo.IsTag("Neutral"))
                    enemyVFX = false;
            }
            if(!enemyIsAttacking && !enemyIsAttacking2 && !enemyVFX && !enemyVFX2)
            {
                SorcererSkills.SetActive(true);
                opacity.SetActive(true);
                closeButton.SetActive(true);
            }
            else
Debug.Log("SorcererMoveset/OpenSorcererSkills: Can't open menu! Enemy is attacking or being attacked/healing!");
        }

        else
Debug.Log("SorcererMoveset/OpenSorcererSkills: Can't open menu! Log is printing or player is attacking or being attacked/healing!");
    }


    public void Lose()
    {
        // player has to have knight if they have sorcerer, check if knight is downed
        if(knightAlly.GetComponent<KnightMoveset>().curHealth <= 0)
        {
            // if knight is downed, then player loses if cleric is not unlocked OR cleric is down
            if(PlayerPrefs.GetInt("PartySize", 1) <= 2 || clericAlly.GetComponent<ClericMoveset>().curHealth <= 0)
            {
                loseCondition = true;
                LoseText.SetActive(true);
                PlayerPrefs.SetInt("SpawnPoint", 0);
Debug.Log("SorcererMoveset/Lose: You lose!");
            }
        }
    }

    void ExecuteMove(System.Action move, System.Func<string> getMessage)
    {
        StartCoroutine(ExecuteMoveRoutine(move, getMessage));
    }

    IEnumerator ExecuteMoveRoutine(System.Action move, System.Func<string> getMessage)
    {
        animator.SetBool("isAttacking", true);
        move.Invoke();
//Debug.Log("SorcererMoveset/ExecuteMoveRoutine: Coroutine is pausing to run printCurrentAction");
        yield return StartCoroutine(printCurrentAction(getMessage(), 0f));

        if (doubleCastActive && Random.value <= 0.5f)
        {
//Debug.Log("SorcererMoveset/ExecuteMoveRoutine: Double cast triggered!");

            move.Invoke();
//Debug.Log("SorcererMoveset/ExecuteMoveRoutine: Coroutine is pausing to run printCurrentAction");
            yield return StartCoroutine(printCurrentAction(getMessage() + " (Double Cast!)", 0f));
        }

        animator.SetBool("isAttacking", false);
    }

    public void ActivateDoubleCast()
    {
        if (doubleCastActive)
        {
            //if (!printing)
                StartCoroutine(printCurrentAction("Double Cast already active!", 0f));
            return;
        }

        if (!ItemManager.Instance.UseItem("Adrenaline"))
        {
            StartCoroutine(printCurrentAction("No charges left!", 0f));
            return;
        }

        doubleCastActive = true;

//Debug.Log("SorcererMoveset/ActivateDoubleCast: Double Cast ACTIVATED!");

        //if (!printing)
            StartCoroutine(printCurrentAction("Double Cast activated!", 0f));
    }

    public void PassTurn()
    {
        healUsedThisTurn = false;
        damageReductionUsedThisTurn = false;

        if (loseCondition)
            return;

        if (squirrelFight == 1) 
            firstEnemy.GetComponent<DemoEnemy>().BeginTurn();

        else if (squirrelFight == 2) 
            firstEnemy.GetComponent<SquirrelEnemy>().BeginTurn();

        else if (squirrelFight == 3) 
            firstEnemy.GetComponent<TigerBoss>().BeginTurn();

        battlePhase.GetComponent<BattlePhase>().ActionInputted();
    }

    IEnumerator printCurrentAction(string toPrint, float delay)
    {
//Debug.Log("SorcererMoveset/printCurrentAction: Coroutine is pausing for " + delay + " seconds");
        yield return new WaitForSeconds(delay);
//Debug.Log("SorcererMoveset/printCurrentAction: Coroutine is pausing until printing is false");
Debug.Log("SorcererMoveset/printCurrentAction: Waiting until text is blank");
        yield return new WaitUntil(() => currentAction.text == "");
Debug.Log("SorcererMoveset/printCurrentAction: Printing current action");
        printing = true;

        currentAction.enabled = true;
        currentAction.text = toPrint;

//Debug.Log("SorcererMoveset/printCurrentAction: Coroutine is pausing for 3 seconds");
        yield return new WaitForSeconds(3);
        currentAction.text = "";
        
        printing = false;
        currentAction.enabled = false;
    }
}