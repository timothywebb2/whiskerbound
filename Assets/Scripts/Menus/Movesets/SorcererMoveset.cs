using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class SorcererMoveset : MonoBehaviour
{
    [Header("Sorcerer Values")]
    public GameObject SorcererSkills;
    public int maxHealth;
    public int curHealth;
    public int damageType;
    public int mightBonus;
    public int damageOutput;
    public int damageOutputBefore; // This is temporary
    public int shieldOutput;
    public int thornsOutput;
    public int healOutput;
    public bool rallyOrNot;
    public int volcanicTally;
    public bool intercedeOn;

    [Header("Allies")]
    public GameObject knightAlly;

    [Header("Fight Management")]
    public int squirrelFight;
    public GameObject battlePhase;
    public float timePassed = 0.0f;
    public GameObject firstEnemy;
    public bool loseCondition;

    [Header("UI/Audio")]
    public TextMeshProUGUI HealthText;
    public GameObject LoseText;
    public Slider Sorcererhealthbar;
    public AudioClip damageSound;
    private AudioSource audioSource;
    public TextMeshProUGUI currentAction;
    public bool printing;
    private Animator animator;

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
        Sorcererhealthbar.maxValue = maxHealth;
        Sorcererhealthbar.value = curHealth;
       
        intercedeOn = false;
        rallyOrNot = false;
        volcanicTally = 0;
        // damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR
        knightAlly = GameObject.FindGameObjectWithTag("KnightBattle");
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

Debug.Log("Damage reduce from " + amount + " to " + finalDamage);
            }

            curHealth -= finalDamage;
            Sorcererhealthbar.value = curHealth;
           
            if (damageSound != null)
            {
                audioSource.PlayOneShot(damageSound);
            }
           
            if(!printing)
                StartCoroutine(printCurrentAction("Sorcerer took " + finalDamage + " damage!", 1f));
       }
       
        else if (intercedeOn == true)
        {
Debug.Log("Damage blocked!");
            if (!printing)
                StartCoroutine(printCurrentAction("Damage blocked from Sorcerer!", 1f));
            intercedeOn = false;
        }

        if (curHealth >= 25)
        {
            knightAlly.GetComponent<KnightMoveset>().UnLastStand();
        }
        
        else if (curHealth < 25)
        {
            knightAlly.GetComponent<KnightMoveset>().LastStand();
        }
        
        if (curHealth <= 0)
        {
            Lose();
        }

        UpdateHUD();
    }

    public void HealPotion()
    {
        if (healUsedThisTurn)
        {
            if (!printing)
                StartCoroutine(printCurrentAction("Potion already used this turn!", 0f));
            return;
        }

        if (!ItemManager.Instance.UseItem("HealPotion"))
        {
            StartCoroutine(printCurrentAction("No charges left!", 0f));
            return;
        }

        healUsedThisTurn = true;

        int oldHealth = curHealth;
        int healAmount = Random.Range(2, 9);

        curHealth += healAmount;

        if (curHealth > maxHealth)
            curHealth = maxHealth;

        int actualHeal = curHealth - oldHealth;
        Sorcererhealthbar.value = curHealth;
        
        //PUT HEAL ANIMTION
        //PUT HEAL SFX

Debug.Log("Healed for " + actualHeal);
        if (!printing)
            StartCoroutine(printCurrentAction("Healed for " + actualHeal + " HP!", 0f));
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

Debug.Log("Damage reduction activated!");

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
            {
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, true));
            }
            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
            }
            else if (squirrelFight == 3)
            {
                firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
            }
            volcanicTally += damageOutput;
            Debug.Log("Damaged enemy by " + damageOutput + " with Incinerate");
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
            }
            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
            }
            else if (squirrelFight == 3)
            {
                firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
            }
            volcanicTally += damageOutput;
            if (squirrelFight == 1)
            {
                firstEnemy.GetComponent<DemoEnemy>().gotStunned();
            }
            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().gotStunned();
            }
            else if (squirrelFight == 3)
            {
                firstEnemy.GetComponent<TigerBoss>().gotStunned();
            }
            Debug.Log("Damaged enemy by " + damageOutput);
            VolcanicHex();
            PassTurn();
        }, () => "Damaged enemy by " + damageOutput + " with Enervate!");
   }

    public void Ward()
    {
        lastMove = Ward;
        ExecuteMove(() =>
        {

            Debug.Log("Ward activated!");
            shieldOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7) + mightBonus;
            knightAlly.GetComponent<KnightMoveset>().gotShielded(shieldOutput);
            PassTurn();
        }, () => "Ward used on Knight!");
    }

    public void Scourge()
    {
        lastMove = Scourge;
        ExecuteMove(() =>
        {

            Debug.Log("Scourge activated!");
            thornsOutput = Random.Range(1, 7) + mightBonus;
            knightAlly.GetComponent<KnightMoveset>().gotThorns(thornsOutput);
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
                firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
            }
            
            else if (squirrelFight == 3) 
                firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);

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
            firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
        }

        else if (squirrelFight == 3)
            firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);

Debug.Log("Damaged enemy by " + damageOutput + " with Incinerate");
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
            firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
            firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
            firstEnemy.GetComponent<SquirrelEnemy>().gotStunned();
        }

        else if (squirrelFight == 3)   
        {
            firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
            firstEnemy.GetComponent<TigerBoss>().gotStunned();
        }

Debug.Log("Damaged enemy by " + damageOutput);
    }

    public void IntercedeSorcerer()
    {
Debug.Log("Intercede on Sorcerer!");
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

    void UpdateHUD()
    {
        HealthText.text = curHealth + "/" + maxHealth;
    }

    public void OpenSorcererSkills()
    {
        if (!printing)
            SorcererSkills.SetActive(true);
    }

    public void Lose()
    {
        loseCondition = true;
        LoseText.SetActive(true);
Debug.Log("You lose!");
    }

    void ExecuteMove(System.Action move, System.Func<string> getMessage)
    {
        StartCoroutine(ExecuteMoveRoutine(move, getMessage));
    }

    IEnumerator ExecuteMoveRoutine(System.Action move, System.Func<string> getMessage)
    {
        animator.SetBool("isAttacking", true);
        move.Invoke();
Debug.Log("SorcererMoveset/ExecuteMoveRoutine: Coroutine is pausing to run printCurrentAction");
        yield return StartCoroutine(printCurrentAction(getMessage(), 0f));

        if (doubleCastActive && Random.value <= 1f)
        {
            Debug.Log("Double cast triggered!");

            move.Invoke();
Debug.Log("SorcererMoveset/ExecuteMoveRoutine: Coroutine is pausing to run printCurrentAction");
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

        Debug.Log("Double Cast ACTIVATED!");

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
Debug.Log("SorcererMoveset/printCurrentAction: Coroutine is pausing for " + delay + " seconds");
        yield return new WaitForSeconds(delay);
Debug.Log("SorcererMoveset/printCurrentAction: Coroutine is pausing until printing is false");
        yield return new WaitUntil(() => !printing);

        printing = true;

        currentAction.enabled = true;
        currentAction.text = toPrint;

Debug.Log("SorcererMoveset/printCurrentAction: Coroutine is pausing for 5 seconds");
        yield return new WaitForSeconds(5);

        printing = false;
        currentAction.enabled = false;
    }
}