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
    public GameObject sorcererAlly;
    public bool sorcererLastStand;

    [Header("Fight Management")]
    public GameObject battlePhase;
    public GameObject firstEnemy;
    public int squirrelFight;
    public float timePassed = 0.0f;
    public bool loseCondition;

    [Header("UI/Audio")]
    public TextMeshProUGUI HealthText;
    public GameObject KnightSkills;
    public GameObject LoseText;
    public Slider Knighthealthbar;
    public AudioClip damageSound;
    private AudioSource audioSource;
    public TextMeshProUGUI currentAction;
    public bool printing;

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
        sorcererAlly = GameObject.FindGameObjectWithTag("SorcererBattle");
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

Debug.Log("Damage reduce from " + amount + " to " + finalDamage);
            }

            curHealth -= finalDamage;
            Knighthealthbar.value = curHealth;
           
           
            if (damageSound != null)
                audioSource.PlayOneShot(damageSound);
           
            if(!printing)
                StartCoroutine(printCurrentAction("Knight took " + finalDamage + " damage!", 1f));
            if (hasThorns > 0)
            {
                if (squirrelFight == 1) 
                    firstEnemy.GetComponent<DemoEnemy>().TakeDamage(thornDamage);
               
                else if (squirrelFight == 2)
                    firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(thornDamage);
               
                else if (squirrelFight == 3)
                    firstEnemy.GetComponent<TigerBoss>().TakeDamage(thornDamage);
               
                hasThorns -= 1;
            }
       }

       else if (intercedeOn == true)
       {
Debug.Log("Damage blocked!");
            if (!printing)
                StartCoroutine(printCurrentAction("Damage blocked!", 1f));
            intercedeOn = false;
        }

        if (curHealth <= 0) 
            Lose();
       
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
        int healAmount = Random.Range(2, 9); // 2d4, can easily change the range if we want to

        curHealth += healAmount;

        //clamp to max
        if (curHealth > maxHealth)
            curHealth = maxHealth;

        int actualHeal = curHealth - oldHealth;
        Knighthealthbar.value = curHealth;
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

    public void Provoke() {
        //set last move, if the 50% goes off, execute the move again
        lastMove = Provoke;
        ExecuteMove(() =>
        {

            damageOutput = Random.Range(1, 13) + Random.Range(1, 13) + CurrentMight;
            if (squirrelFight == 1)
            {
                firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
                firstEnemy.GetComponent<DemoEnemy>().gotGoaded();
            }
            else if (squirrelFight == 2)
            {
                firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
                firstEnemy.GetComponent<SquirrelEnemy>().gotGoaded();
            }
            else if (squirrelFight == 3)
            {
                firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
                firstEnemy.GetComponent<TigerBoss>().gotGoaded();
            }
            Debug.Log("Damaged enemy by " + damageOutput + " with Provoke");
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
                firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
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
            Debug.Log("Damaged enemy by " + damageOutput);
            PassTurn();
        }, () => "Damaged enemy by " + damageOutput + " with Cleave!");
   }

   public void Intercede() {
        lastMove = Intercede;
        ExecuteMove(() =>
        {

            // intercedeOn = true;
            sorcererAlly.GetComponent<SorcererMoveset>().IntercedeSorcerer();
            Debug.Log("Intercede on Sorcerer!");
            PassTurn();
        }, () => "Intercede on Sorceror!");
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
                sorcererAlly.GetComponent<SorcererMoveset>().RallyIncinerate();
            }
            else if (rallyRandom == 2)
            {
                sorcererAlly.GetComponent<SorcererMoveset>().RallyEnervate();
            }
            Debug.Log("Rally being used!");
            PassTurn();
        }, () => "Rally used on Sorcerer!");
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
        //  HealthText.text = "HP: " + curHealth;
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
        yield return new WaitForSeconds(delay);
        yield return new WaitUntil(() => !printing);

        printing = true;

        currentAction.enabled = true;
        currentAction.text = toPrint;

        yield return new WaitForSeconds(5);

        printing = false;
        currentAction.enabled = false;
    }
  
    public void OpenKnightSkills()
    {
        if (!printing)
            KnightSkills.SetActive(true);
    }

    void ExecuteMove(System.Action move, System.Func<string> getMessage)
    {
        StartCoroutine(ExecuteMoveRoutine(move, getMessage));
    }

    IEnumerator ExecuteMoveRoutine(System.Action move, System.Func<string> getMessage)
    {
        move.Invoke();
        yield return StartCoroutine(printCurrentAction(getMessage(), 0f));

        if (doubleCastActive && Random.value <= 1f)
        {
            Debug.Log("Double cast triggered!");

            move.Invoke();
            yield return StartCoroutine(printCurrentAction(getMessage + " (Double Cast!)", 0f));
        }
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

    public void Lose()
    {
        loseCondition = true;
        LoseText.SetActive(true);
Debug.Log("You lose!");
    }
}