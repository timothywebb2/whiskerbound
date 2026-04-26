using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class KnightMoveset : MonoBehaviour
{
    [Header("Knight Values")]
    public int maxHealth;
    public int curHealth;
    public int damageType;
    public int CurrentMight;
    public int damageOutput;
    public int healOutput;
    int hasThorns;
    public int thornDamage;
    public int rallyRandom; // This is temporary
    public bool intercedeOn;

    [Header("Allies")]
    public SorcererMoveset sorcererAlly;
    public bool sorcererLastStand;
    public ClericMoveset clericAlly;

    [Header("Fight Management")]
    public GameObject battlePhase;
    public GameObject firstEnemy;
    public int squirrelFight;
    public float timePassed = 0.0f;
    public bool loseCondition;

    [Header("UI/Audio")]
    public TextMeshProUGUI HealthText;
    public GameObject KnightSkills;
    public GameObject opacity;
    public GameObject closeButton;
    public GameObject LoseText;
    public Slider Knighthealthbar;
    public AudioClip damageSound;
    private AudioSource audioSource;
    public TextMeshProUGUI currentAction;
    public bool printing;
    public Animator animator;
    public GameObject VFXObject;
    private Animator VFXanimator;

    [Header("Items")]
    //ITEM booleans
    //tracks if the buff is applied this fight
    public bool doubleCastActive = false;
    //tracks the last move made
    private System.Action lastMove;
    //tracks if heal is used so you dont spam
    public bool healUsedThisTurn = false;
    //essence of arcanum tracking
    public bool damageReductionActive = false;
    public bool damageReductionUsedThisTurn = false;
    //25% reduction on damage, lower the .75 to increase the reduction
    public float damageReductionPercent = 0.75f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 100;
        curHealth = maxHealth;
        Knighthealthbar.maxValue = maxHealth;
        Knighthealthbar.value = curHealth;
       
        damageType = 1; // 1 = PHYS, 2 = MYS, 3 = SPR
        sorcererAlly = GameObject.FindGameObjectWithTag("SorcererBattle").GetComponent<SorcererMoveset>();
        clericAlly = GameObject.FindGameObjectWithTag("ClericBattle").GetComponent<ClericMoveset>();
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
        rallyRandom = 1;
        hasThorns = 0;
        thornDamage = 0;
        intercedeOn = false;
        sorcererLastStand = false;
        loseCondition = false;
        currentAction.enabled = false;
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
            {
                SceneManager.LoadScene(2);
            }
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

Debug.Log("KnightMoveset/TakeDamage: Damage reduce from " + amount + " to " + finalDamage);
            }

            curHealth -= finalDamage;
            Knighthealthbar.value = curHealth;
           
            VFXanimator.SetBool("isAttacked", true);
           
            if (damageSound != null)
                audioSource.PlayOneShot(damageSound);
           
            if(!printing)
                StartCoroutine(printCurrentAction("Knight took " + finalDamage + " damage!", 1f));
            if (hasThorns > 0)
            {
Debug.Log("KnightMoveset/TakeDamage: Enemy took damage from thorns!");
                if (squirrelFight == 1) 
                    StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(thornDamage, false));
               
                else if (squirrelFight == 2)
                    StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(thornDamage, false));
               
                else if (squirrelFight == 3)
                    firstEnemy.GetComponent<TigerBoss>().TakeDamage(thornDamage);

                VFXanimator.SetBool("isAttacked", false);
                hasThorns -= 1;
            }
       }

       else if (intercedeOn == true)
       {
Debug.Log("KnightMoveset/TakeDamage: Damage blocked!");
            if (!printing)
                StartCoroutine(printCurrentAction("Damage blocked!", 1f));
            intercedeOn = false;
        }

        if (curHealth <= 0) 
            Lose();
       
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
            int healAmount = Random.Range(2, 9); // 2d4, can easily change the range if we want to

            curHealth += healAmount;

            //clamp to max
            if (curHealth > maxHealth)
                curHealth = maxHealth;

            int actualHeal = curHealth - oldHealth;
            VFXanimator.SetBool("isHealing", true);
            //PUT HEAL SFX

    Debug.Log("KnightMoveset/HealPotion: Healed for " + actualHeal);
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

Debug.Log("KnightMoveset/DamageReductionPotion: Damage reduction activated!");

        if (!printing)
            StartCoroutine(printCurrentAction("Damage taken reduced by 25% for next hit!", 0f));
    }

    public void Provoke() {
        //set last move, if the 50% goes off, execute the move again
        lastMove = Provoke;
        ExecuteMove(() =>
        {

            damageOutput = Random.Range(1, 13) + Random.Range(1, 13) + CurrentMight;
            if (squirrelFight == 1)
            {
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, false));
                firstEnemy.GetComponent<DemoEnemy>().gotGoaded();
            }
            else if (squirrelFight == 2)
            {
                int attackedEnemy = Random.Range(1, 3);
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(attackedEnemy, damageOutput, false));
                firstEnemy.GetComponent<SquirrelEnemy>().gotGoaded(attackedEnemy);
            }
            else if (squirrelFight == 3)
            {
                firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
                firstEnemy.GetComponent<TigerBoss>().gotGoaded();
            }
Debug.Log("KnightMoveset/Provoke: Damaged enemy by " + damageOutput + " with Provoke");
            PassTurn();
        }, () => "Damaged enemy by " + damageOutput + " with Provoke!");

   }


   public void Cleave() {
        lastMove = Cleave;
        ExecuteMove(() =>
        {

            damageOutput = Random.Range(1, 13) + CurrentMight;
            if (squirrelFight == 1)
            {
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput, false));
            }
            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput, false));
            }
            else if (squirrelFight == 3)
            {
                firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
            }
    Debug.Log("KnightMoveset/Cleave: Damaged enemy by " + damageOutput + " with Cleave");
            PassTurn();
        }, () => "Damaged enemy by " + damageOutput + " with Cleave!");
   }

   public void Intercede() {
        lastMove = Intercede;
        ExecuteMove(() =>
        {

            // intercedeOn = true;
            sorcererAlly.IntercedeSorcerer();
            clericAlly.IntercedeCleric();
Debug.Log("KnightMoveset/Intercede: Intercede on Sorcerer and Cleric!");
            PassTurn();
        }, () => "Intercede on Sorceror and Cleric!");
   }

   public void Rally() {
        lastMove = Rally;
        ExecuteMove(() =>
        {

            /*   healOutput = Random.Range(1, 13);
               if (curHealth + healOutput >= 50)
                   curHealth = 50;
               else
                   curHealth += healOutput;


               Debug.Log("Healing Knight by " + healOutput);
               if (!printing)
                   StartCoroutine(printCurrentAction("Healing Knight by " + healOutput + " with Rally!", 0f));
        PassTurn();
        */


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
        }, () => "Rally used on Sorcerer and Cleric!");
   }

    public void LastStand()
    {
        if (sorcererLastStand == false)
        {
            CurrentMight += 2;
            sorcererLastStand = true;
        }
    }

    public void UnLastStand()
    {
        if (sorcererLastStand == true)
        {
            CurrentMight -= 2;
            sorcererLastStand = false;
        }
    }


public void GotHealed(int amount) {
if (curHealth + amount >= 100)
                   curHealth = 100;
               else
                   curHealth += amount;
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

    public void PassTurn()
    {
        //reset each turn
        healUsedThisTurn = false;
        damageReductionUsedThisTurn = false;

        battlePhase.GetComponent<BattlePhase>().ActionInputted();
    }

    IEnumerator printCurrentAction(string toPrint, float delay)
    {
//Debug.Log("KnightMoveset/printCurrentAction: Coroutine is pausing for " + delay + " seconds");
        yield return new WaitForSeconds(delay);
        
        if(printing)
        {
//Debug.Log("KnightMoveset/printCurrentAction: Coroutine is pausing until printing is false");
            yield return new WaitUntil(() => !printing);
        }

        printing = true;
//Debug.Log("KnightMoveset/printCurrentAction: Current action enabled");
        currentAction.enabled = true;
        currentAction.text = toPrint;
//Debug.Log("KnightMoveset/printCurrentAction: Coroutine is pausing for 5 seconds");
        yield return new WaitForSeconds(3);

        printing = false;
        currentAction.enabled = false;
    }
  
    public void OpenKnightSkills()
    {   
        // if the log isnt printing, and if no party members are in the middle of an attack or being attacked/healed
        // ADD CLERIC
        if (!printing && animator.GetBool("isAttacking") == false && sorcererAlly.animator.GetBool("isAttacking") == false && clericAlly.animator.GetBool("isAttacking") == false)
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
                KnightSkills.SetActive(true);
                opacity.SetActive(true);
                closeButton.SetActive(true);
            }
            else
Debug.Log("KnightMoveset/OpenKnightSkills: Can't open menu! Enemy is attacking or being attacked/healing!");
        }

        else
Debug.Log("KnightMoveset/OpenKnightSkills: Can't open menu! Log is printing or player is attacking or being attacked/healing!");
    }

    void ExecuteMove(System.Action move, System.Func<string> getMessage)
    {
        StartCoroutine(ExecuteMoveRoutine(move, getMessage));
    }

    IEnumerator ExecuteMoveRoutine(System.Action move, System.Func<string> getMessage)
    {
        animator.SetBool("isAttacking", true);
        move.Invoke();
//Debug.Log("KnightMoveset/ExecuteMoveRoutine: Coroutine is pausing to run printCurrentAction");
        yield return StartCoroutine(printCurrentAction(getMessage(), 0f));

        if (doubleCastActive && Random.value <= 1f)
        {
//Debug.Log("KnightMoveset/ExecuteMoveRoutine: Double cast triggered!");

            move.Invoke();
//Debug.Log("KnightMoveset/ExecuteMoveRoutine: Coroutine is pausing to run printCurrentAction");
            yield return StartCoroutine(printCurrentAction(getMessage + " (Double Cast!)", 0f));
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

//Debug.Log("KnightMoveset/ActivateDoubleCast: Double Cast ACTIVATED!");

        if (!printing)
            StartCoroutine(printCurrentAction("Double Cast activated!", 0f));
    }

    public void Lose()
    {
        loseCondition = true;
        LoseText.SetActive(true);
Debug.Log("KnightMoveset/Lose: You lose!");
    }
}