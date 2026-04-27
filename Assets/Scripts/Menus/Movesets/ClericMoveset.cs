using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class ClericMoveset : MonoBehaviour
{
    [Header("Cleric Values")]
    public int maxHealth;
    public int curHealth;
    public int damageType;
    public int mightBonus;
    public int damageOutput;
    public int damageOutputBefore; // This is temporary
    public int shieldOutput;
    public int thornsOutput;
    public int healOutput;
        int hasThorns;
         public int rallyRandom; // This is temporary
    public int thornDamage;
    public bool rallyOrNot;
    public int volcanicTally;
    public bool intercedeOn;

    [Header("Allies")]
    public KnightMoveset knightAlly;
    public SorcererMoveset sorcererAlly;

    [Header("Fight Management")]
    public int squirrelFight;
    public GameObject battlePhase;
    public float timePassed = 0.0f;
    public GameObject firstEnemy;
    public bool loseCondition;

    [Header("UI/Audio")]
    public GameObject ClericSkills;
    public GameObject opacity;
    public GameObject closeButton;
    public TextMeshProUGUI HealthText;
    public GameObject LoseText;
    public Slider Clerichealthbar;
    public AudioClip damageSound;
    private AudioSource audioSource;
    public TextMeshProUGUI currentAction;
    public bool printing;

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
        maxHealth = 60;
        curHealth = maxHealth;
        Clerichealthbar.maxValue = maxHealth;
        Clerichealthbar.value = curHealth;
       
        intercedeOn = false;
        rallyOrNot = false;
        volcanicTally = 0;
              rallyRandom = 1;
            hasThorns = 0;
        thornDamage = 0;
        // damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR
        knightAlly = GameObject.FindGameObjectWithTag("KnightBattle").GetComponent<KnightMoveset>();
        sorcererAlly = GameObject.FindGameObjectWithTag("SorcererBattle").GetComponent<SorcererMoveset>();
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
        // intercedeOn = false;
        currentAction.enabled = false;
        loseCondition = false;
        squirrelFight = 1;
        LoseText.SetActive(false);
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
                SceneManager.LoadScene(2);
        }
    }

    public void TakeDamage(int amount)
    {
        if (intercedeOn == false)
        {
            int finalDamage = amount;
            if (damageReductionActive)
            {
                finalDamage = Mathf.RoundToInt(amount * damageReductionPercent);
                damageReductionActive = false; //consumes the effect

Debug.Log("ClericMoveset/TakeDamage: Damage reduce from " + amount + " to " + finalDamage);
            }

            curHealth -= finalDamage;
            Clerichealthbar.value = curHealth;

            VFXanimator.SetBool("isAttacked", true);

            if (damageSound != null)
            {
                audioSource.PlayOneShot(damageSound);
            }
           
            if(!printing)
                StartCoroutine(printCurrentAction("Cleric took " + finalDamage + " damage!", 1f));
                if (hasThorns > 0)
            {
Debug.Log("KnightMoveset/TakeDamage: Enemy took damage from thorns!");
                if (squirrelFight == 1) 
                    StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(thornDamage, false));
               
                else if (squirrelFight == 2)
                    StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(thornDamage, false));
               
                else if (squirrelFight == 3)
                    StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(thornDamage, false));

                VFXanimator.SetBool("isAttacked", false);
                hasThorns -= 1;
            }
            
            VFXanimator.SetBool("isAttacked", false);
       }
       
        else if (intercedeOn == true)
        {
Debug.Log("ClericMoveset/TakeDamage: Damage blocked!");
            if (!printing)
                StartCoroutine(printCurrentAction("Damage blocked from Cleric!", 1f));
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

    public IEnumerator HealPotion()
    {
        if (healUsedThisTurn)
        {
            if (!printing)
                StartCoroutine(printCurrentAction("Potion already used this turn!", 0f));
            //return;
        }

        if (!ItemManager.Instance.UseItem("HealPotion"))
        {
            StartCoroutine(printCurrentAction("No charges left!", 0f));
            //return;
        }

        if(!healUsedThisTurn && ItemManager.Instance.UseItem("HealPotion"))
        {
            healUsedThisTurn = true;

            int oldHealth = curHealth;
            int healAmount = Random.Range(2, 9);

            curHealth += healAmount;

            if (curHealth > maxHealth)
                curHealth = maxHealth;

            int actualHeal = curHealth - oldHealth;
            Clerichealthbar.value = curHealth;
            
            VFXanimator.SetBool("isHealing", true);
            //PUT HEAL SFX

Debug.Log("ClericMoveset/HealPotion: Healed for " + actualHeal);
            if (!printing)
                StartCoroutine(printCurrentAction("Healed for " + actualHeal + " HP!", 0f));
            
            yield return new WaitForSeconds(2);
            VFXanimator.SetBool("isHealing", false);
        }
    }

    public void DamageReductionPotion()
    {
        if (damageReductionUsedThisTurn)
        {
            if (!printing)
                StartCoroutine(printCurrentAction("Already used this turn!", 0f));
            return;
        }

        if (!ItemManager.Instance.UseItem("ArcaneEssence"))
        {
            StartCoroutine(printCurrentAction("No charges left!", 0f));
            return;
        }

        damageReductionUsedThisTurn = true;
        damageReductionActive = true;

Debug.Log("ClericMoveset/DamageReductionPotion: Damage reduction activated!");

        if (!printing)
            StartCoroutine(printCurrentAction("Damage taken reduced by 25% for next hit!", 0f));
    }

    public void Incinerate()
    {
        lastMove = Incinerate;
        ExecuteMove(() =>
        {

            damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + mightBonus;
            if (squirrelFight == 1)
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));
            
            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput, true));
            }
            else if (squirrelFight == 3)
                StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput, true));
            
            volcanicTally += damageOutput;
Debug.Log("ClericMoveset/Incinerate: Damaged enemy by " + damageOutput + " with Incinerate");
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

Debug.Log("ClericMoveset/Enervate: Damaged enemy by " + damageOutput);
            VolcanicHex();
            PassTurn();
        }, () => "Damaged enemy by " + damageOutput + " with Enervate!");
   }

    public void Ward()
    {
        lastMove = Ward;
        ExecuteMove(() =>
        {

Debug.Log("ClericMoveset/Ward: Ward activated!");
            shieldOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7) + mightBonus;
            knightAlly.gotShielded(shieldOutput);
            sorcererAlly.gotShielded(shieldOutput);
            PassTurn();
        }, () => "Ward used on Knight!");
    }

    public void Rally() {
        lastMove = Rally;
        ExecuteMove(() =>
        {

            rallyRandom = Random.Range(1, 3);
            if (rallyRandom == 1)
            {
                sorcererAlly.RallyIncinerate();
            }
            else if (rallyRandom == 2)
            {
                sorcererAlly.RallyEnervate();
            }
Debug.Log("Rally being used!");
            PassTurn();
        }, () => "Rally used on Sorcerer!");
   }

   public void Devotion()
    {
        lastMove = Devotion;
        ExecuteMove(() =>
        {

            damageOutput = Random.Range(1, 5) + Random.Range(1, 5) + mightBonus;
            if (squirrelFight == 1)
            {
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));
                healOutput = Random.Range(1, 13);
                knightAlly.GetComponent<KnightMoveset>().GotHealed(healOutput);
                sorcererAlly.GetComponent<SorcererMoveset>().GotHealed(healOutput);
            }
            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput, true));
                healOutput = Random.Range(1, 13);
                knightAlly.GetComponent<KnightMoveset>().GotHealed(healOutput);
                sorcererAlly.GetComponent<SorcererMoveset>().GotHealed(healOutput);
            }
            else if (squirrelFight == 3)
            {
                StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput, true));
                healOutput = Random.Range(1, 13);
                knightAlly.GetComponent<KnightMoveset>().GotHealed(healOutput);
                sorcererAlly.GetComponent<SorcererMoveset>().GotHealed(healOutput);
            }
Debug.Log("ClericMoveset/Devotion: Damaged enemy by " + damageOutput + " with Devotion");
            VolcanicHex();
            PassTurn();
        }, () => "Damaged enemy by " + damageOutput + " with Devotion!");
   }

    public void Scourge()
    {
        lastMove = Scourge;
        ExecuteMove(() =>
        {

Debug.Log("ClericMoveset/Scourge: Scourge activated!");
            thornsOutput = Random.Range(1, 7) + mightBonus;
            knightAlly.gotThorns(thornsOutput);
            sorcererAlly.gotThorns(thornsOutput);
            PassTurn();
        }, () => "Scourge used on Knight!");
    }


    public void VolcanicHex()
    {
        if (volcanicTally >= 30)
        {
            damageOutput = Random.Range(1, 13) + mightBonus;

            if (squirrelFight == 1) 
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));

            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput, true));
            }
            
            else if (squirrelFight == 3) 
                StartCoroutine(firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput, true));

            volcanicTally = 0;
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

Debug.Log("ClericMoveset/RallyIncinerate: Damaged enemy by " + damageOutput + " with Incinerate");
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

Debug.Log("ClericMoveset/RallyEnervate: Damaged enemy by " + damageOutput);
    }

    public void IntercedeCleric()
    {
Debug.Log("ClericMoveset/IntercedeCleric: Intercede on Cleric!");
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

    void UpdateHUD()
    {
        HealthText.text = curHealth + "/" + maxHealth;
    }

     public void OpenClericSkills()
    {   
        // if the log isnt printing, and if no party members are in the middle of an attack or being attacked/healed
        // ADD CLERIC
        if (!printing && animator.GetBool("isAttacking") == false && knightAlly.animator.GetBool("isAttacking") == false && sorcererAlly.animator.GetBool("isAttacking") == false)
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
            /*else if (squirrelFight == 3)
            {
                TigerBoss enemy = firstEnemy.GetComponent<TigerBoss>();
                enemyIsAttacking = enemy.animator.GetBool("isAttacking");

                var stateInfo = enemy.VFXanimation.GetCurrentAnimatorStateInfo(0);
                if(stateInfo.IsTag("Neutral"))
                    enemyVFX = false;
            }*/
            if(!enemyIsAttacking && !enemyIsAttacking2 && !enemyVFX && !enemyVFX2)
            {
                ClericSkills.SetActive(true);
                opacity.SetActive(true);
                closeButton.SetActive(true);
            }
            else
Debug.Log("ClericMoveset/OpenClericSkills: Can't open menu! Enemy is attacking or being attacked/healing!");
        }

        else
Debug.Log("ClericMoveset/OpenClericSkills: Can't open menu! Log is printing or player is attacking or being attacked/healing!");
    }


    public void Lose()
    {
        loseCondition = true;
        LoseText.SetActive(true);
Debug.Log("ClericMoveset/Lose: You lose!");
    }

    void ExecuteMove(System.Action move, System.Func<string> getMessage)
    {
        StartCoroutine(ExecuteMoveRoutine(move, getMessage));
    }

    IEnumerator ExecuteMoveRoutine(System.Action move, System.Func<string> getMessage)
    {
        animator.SetBool("isAttacking", true);
        move.Invoke();
//Debug.Log("ClericMoveset/ExecuteMoveRoutine: Coroutine is pausing to run printCurrentAction");
        yield return StartCoroutine(printCurrentAction(getMessage(), 0f));

        if (doubleCastActive && Random.value <= 1f)
        {
//Debug.Log("ClericMoveset/ExecuteMoveRoutine: Double cast triggered!");

            move.Invoke();
//Debug.Log("ClericMoveset/ExecuteMoveRoutine: Coroutine is pausing to run printCurrentAction");
            yield return StartCoroutine(printCurrentAction(getMessage() + " (Double Cast!)", 0f));
        }

        animator.SetBool("isAttacking", false);
    }

    public void ActivateDoubleCast()
    {
        if (doubleCastActive)
        {
            if (!printing)
                StartCoroutine(printCurrentAction("Double Cast already active!", 0f));
            return;
        }

        if (!ItemManager.Instance.UseItem("Adrenaline"))
        {
            StartCoroutine(printCurrentAction("No charges left!", 0f));
            return;
        }

        doubleCastActive = true;

//Debug.Log("ClericMoveset/ActivateDoubleCast: Double Cast ACTIVATED!");

        if (!printing)
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
//Debug.Log("ClericMoveset/printCurrentAction: Coroutine is pausing for " + delay + " seconds");
        yield return new WaitForSeconds(delay);
//Debug.Log("ClericMoveset/printCurrentAction: Coroutine is pausing until printing is false");
        yield return new WaitUntil(() => !printing);

        printing = true;

        currentAction.enabled = true;
        currentAction.text = toPrint;

//Debug.Log("ClericMoveset/printCurrentAction: Coroutine is pausing for 3 seconds");
        yield return new WaitForSeconds(3);

        printing = false;
        currentAction.enabled = false;
    }
}