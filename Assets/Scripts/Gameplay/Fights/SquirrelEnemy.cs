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
    public GameObject battlePhase;
    public int selectingMove;
    public int selectingTarget;
    public int damageOutput;
    public int attackedEnemy;
    public int multiHitting;
    private bool isProvoked1;
    private bool isProvoked2;

    [Header("Fight Management")]
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
    private AudioSource audioSource;
    public Animator animator1;
    public Animator animator2;
    public GameObject enemyVFX1;
    public GameObject enemyVFX2;
    public Animator VFXanimation1;
    public Animator VFXanimation2;
    private bool animationDone;


   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
       knightPlayer = GameObject.FindGameObjectWithTag("KnightBattle");
       sorcererPlayer = GameObject.FindGameObjectWithTag("SorcererBattle");
       battlePhase = GameObject.FindGameObjectWithTag("BattleController");
       fightManager = GameObject.FindGameObjectWithTag("FightManager");
       curHealth1 = 30;
       curHealth2 = 30;
       maxHealth1 = curHealth1;
       maxHealth2 = curHealth2;
       multiHitting = 1;
       attackedEnemy = 1;
       damageType = 1; // 1 = PHYS, 2 = MYS, 3 = SPR
       selectingMove = 1;
       selectingTarget = 1;
       squirrelOneDown = false;
       squirrelTwoDown = false;
       squirrelCoordination = true;
       VictoryText.SetActive(false);
       VictoryAchieved = false;
       UpdateHUD();
       audioSource = GetComponent<AudioSource>();

       animator1 = this.transform.GetChild(0).GetComponent<Animator>();
       animator2 = this.transform.GetChild(1).GetComponent<Animator>();

       VFXanimation1 = enemyVFX1.GetComponent<Animator>();
       VFXanimation2 = enemyVFX2.GetComponent<Animator>();
   }

   // Update is called once per frame
   void Update()
   {
        battlePhase.GetComponent<BattlePhase>().NumberedFight(2);

        if (VictoryAchieved)
        {
            PlayerPrefs.SetInt("BeatSquirrel", 1);
            timePassed += Time.deltaTime;
            if (timePassed > 3.0f)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("LastScene", "forestVillage"));
            }
        }
    }

    public IEnumerator TakeDamage(int amount, bool isFire)
    {
Debug.Log("SquirrelEnemy/TakeDamage: isFire is " + isFire);
        if (damageSound != null)     
            audioSource.PlayOneShot(damageSound);

        if (squirrelOneDown == false && squirrelTwoDown == false) 
            attackedEnemy = Random.Range(1, 3);
       
        if (squirrelOneDown == true && squirrelTwoDown == false) 
            attackedEnemy = 2;
       
        if (squirrelOneDown == false && squirrelTwoDown == true)
            attackedEnemy = 1;
       
        if (multiHitting == 1)
        {
            if (attackedEnemy == 1)
            {
                curHealth1 -= amount;
                if(curHealth1 < 0)
                    curHealth1 = 0;
                EnemyHealthBar1.value = curHealth1;
                UpdateHUD();

                // set VFX to damaged animation
                if(isFire)
                {
                    VFXanimation1.SetBool("isFireAttack", true);
Debug.Log("SquirrelEnemy/TakeDamage: Pausing to play fire animation");
                    yield return new WaitForSeconds(1.5f);
                }

                else
                {
                    VFXanimation1.SetBool("isAttacked", true);
Debug.Log("SquirrelEnemy/TakeDamage: Playing slash animation");
                    yield return new WaitUntil(() => animationDone);
                    animationDone = false;
                }

                VFXanimation1.SetBool("isAttacked", false); // reset VFX
                VFXanimation1.SetBool("isFireAttack", false);

Debug.Log("Squirrel 1 took " + amount + " damage and has " + curHealth1 + " health");
                if (curHealth1 <= 0)
                    squirrelOneDown = true;
            }

            else if (attackedEnemy == 2)
            {
                curHealth2 -= amount;
                if(curHealth2 < 0)
                    curHealth2 = 0;
                EnemyHealthBar2.value = curHealth2;
                UpdateHUD();
                // set VFX to damaged animation
                if(isFire)
                {
                    VFXanimation2.SetBool("isFireAttack", true);
Debug.Log("SquirrelEnemy/TakeDamage: Pausing to play fire animation");
                    yield return new WaitForSeconds(1.5f);
                }

                else
                {
                    VFXanimation2.SetBool("isAttacked", true);
Debug.Log("SquirrelEnemy/TakeDamage: Playing slash animation");
                    yield return new WaitUntil(() => animationDone);
                    animationDone = false;
                }

                VFXanimation2.SetBool("isAttacked", false); // reset VFX
                VFXanimation2.SetBool("isFireAttack", false);

Debug.Log("Squirrel 2 took " + amount + " damage and has " + curHealth2 + " health");
                if (curHealth2 <= 0) 
                    squirrelTwoDown = true;
            }
        }
    
        if (multiHitting == 2)
        {
            curHealth1 -= amount;
            curHealth2 -= amount;
            EnemyHealthBar1.value = curHealth1;
            EnemyHealthBar2.value = curHealth2;
            UpdateHUD();

Debug.Log("Both enemies took " + amount + " damage");
Debug.Log("Squrrel 1 has " + curHealth1 + " and Squirrel 2 has " + curHealth2);

            if (curHealth1 <= 0)
                squirrelOneDown = true;

            if (curHealth2 <= 0)
                squirrelTwoDown = true;
            
            multiHitting = 1;
        }

        if (squirrelOneDown == true || squirrelTwoDown == true)
            squirrelCoordination = false;

        if (squirrelOneDown == true && squirrelTwoDown == true) 
            Victory();   
    }

    // overload method to apply status effects
    public IEnumerator TakeDamage(int target, int amount, bool isFire)
    {
Debug.Log("SquirrelEnemy/TakeDamage: isFire is " + isFire);
        if (damageSound != null)     
            audioSource.PlayOneShot(damageSound);

        attackedEnemy = target;
       
        if (squirrelOneDown == true && squirrelTwoDown == false) 
            attackedEnemy = 2;
       
        if (squirrelOneDown == false && squirrelTwoDown == true)
            attackedEnemy = 1;
       
        if (multiHitting == 1)
        {
            if (attackedEnemy == 1)
            {
                curHealth1 -= amount;
                if(curHealth1 < 0)
                    curHealth1 = 0;
                EnemyHealthBar1.value = curHealth1;
                UpdateHUD();

                // set VFX to damaged animation
                if(isFire)
                {
                    VFXanimation1.SetBool("isFireAttack", true);
Debug.Log("SquirrelEnemy/TakeDamage: Pausing to play fire animation");
                    yield return new WaitForSeconds(1.5f);
                }

                else
                {
                    VFXanimation1.SetBool("isAttacked", true);
Debug.Log("SquirrelEnemy/TakeDamage: Playing slash animation");
                    yield return new WaitUntil(() => animationDone);
                    animationDone = false;
                }

                VFXanimation1.SetBool("isAttacked", false); // reset VFX
                VFXanimation1.SetBool("isFireAttack", false);

Debug.Log("Squirrel 1 took " + amount + " damage and has " + curHealth1 + " health");
                if (curHealth1 <= 0)
                    squirrelOneDown = true;
            }

            else if (attackedEnemy == 2)
            {
                curHealth2 -= amount;
                if(curHealth2 < 0)
                    curHealth2 = 0;
                EnemyHealthBar2.value = curHealth2;
                UpdateHUD();
                // set VFX to damaged animation
                if(isFire)
                {
                    VFXanimation2.SetBool("isFireAttack", true);
Debug.Log("SquirrelEnemy/TakeDamage: Pausing to play fire animation");
                    yield return new WaitForSeconds(1.5f);
                }

                else
                {
                    VFXanimation2.SetBool("isAttacked", true);
Debug.Log("SquirrelEnemy/TakeDamage: Playing slash animation");
                    yield return new WaitUntil(() => animationDone);
                    animationDone = false;
                }

                VFXanimation2.SetBool("isAttacked", false); // reset VFX
                VFXanimation2.SetBool("isFireAttack", false);

Debug.Log("Squirrel 2 took " + amount + " damage and has " + curHealth2 + " health");
                if (curHealth2 <= 0) 
                    squirrelTwoDown = true;
            }
        }
    
        if (multiHitting == 2)
        {
            curHealth1 -= amount;
            curHealth2 -= amount;
            EnemyHealthBar1.value = curHealth1;
            EnemyHealthBar2.value = curHealth2;
            UpdateHUD();

Debug.Log("Both enemies took " + amount + " damage");
Debug.Log("Squrrel 1 has " + curHealth1 + " and Squirrel 2 has " + curHealth2);

            if (curHealth1 <= 0)
                squirrelOneDown = true;

            if (curHealth2 <= 0)
                squirrelTwoDown = true;
            
            multiHitting = 1;
        }

        if (squirrelOneDown == true || squirrelTwoDown == true)
            squirrelCoordination = false;

        if (squirrelOneDown == true && squirrelTwoDown == true) 
            Victory();   
    }

    public void gotGoaded(int target)
    {
        if(target == 1)
            isProvoked1 = true;
        else if (target == 2)
            isProvoked2 = true;
    }

    public void gotStunned()
    {
        //Here is where the code will be for the enemy when they're stunned
    }

    public void BeginTurn()
    {
        if (squirrelOneDown == false)
        {
Debug.Log("Squirrel 1 has started attacking!");
            if(isProvoked1)
            {
Debug.Log("Squirrel 1 is provoked! Will only attack Knight!");
                selectingMove = 1;
                selectingTarget = 1;
            }
            
            else
            {
                selectingMove = Random.Range(1, 3);
                selectingTarget = Random.Range(1, 3);
            }

            animator1.SetBool("isAttacking", true);

            if (selectingMove == 1)
            {
                damageOutput = Random.Range(1, 7) + 1;
                if(squirrelCoordination)
                    damageOutput+= Random.Range(1, 7);
                    
                if(selectingTarget == 1)
                {
Debug.Log("Lash is used on the Knight!");
                    knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
                }
                else if(selectingTarget == 2)
                {
Debug.Log("Lash is used on the Sorcerer");
                    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
                }
            }
            
            else if (selectingMove == 2)
            {
Debug.Log("Recuperate is used!");
                damageOutput = Random.Range(1, 5) + 1;
                curHealth1 += damageOutput;
                EnemyHealthBar1.value = curHealth1;
            }  

            animator1.SetBool("isAttacking", false);
        }

        isProvoked1 = false;
Debug.Log("Squirrel 1 has finished attacking!");
        BeginTurn2();
    }

    public void BeginTurn2()
    {
        if (squirrelTwoDown == false)
        {
Debug.Log("Squirrel 2 has started attacking!");

            if(isProvoked2)
            {
Debug.Log("Squirrel 1 is provoked! Will only attack Knight!");
                selectingMove = 1;
                selectingTarget = 1;
            }

            else
            {
                selectingMove = Random.Range(1, 3);
                selectingTarget = Random.Range(1, 3);
            }

            animator2.SetBool("isAttacking", true);
        
            if (selectingMove == 1)
            {
                damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
                if(squirrelCoordination)
                    damageOutput+= Random.Range(1, 7);
                    
                if(selectingTarget == 1)
                {
Debug.Log("Lash is used on the Knight!");
                    knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
                }
                else if(selectingTarget == 2)
                {
Debug.Log("Lash is used on the Sorcerer!");
                    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
                }
            }

            else if (selectingMove == 2)
            {
Debug.Log("Recuperate is used!");
                damageOutput = Random.Range(1, 5) + 1;
                curHealth2 += damageOutput;
                EnemyHealthBar2.value = curHealth2;
            }
Debug.Log("Squirrel 2 has finished attacking!");
            isProvoked2 = false;
            animator2.SetBool("isAttacking", false);
        }
    }

    public void multiHit()
    {
        multiHitting = 2;
    }

    void UpdateHUD()
    {
        HealthText1.text = curHealth1 + "/" + maxHealth1;
        HealthText2.text = curHealth2 + "/" + maxHealth2;
    }

    public void Victory()
    {
        fightManager.GetComponent<FightManager>().BattleComplete();
        VictoryAchieved = true;
        VictoryText.SetActive(true);
    
Debug.Log("Victory achieved!");
    }

    /*public void AnimationHit()
    {
        animationHit = true;
    }*/

    public void AnimationDone()
    {
        animationDone = true;
    }
}