using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class TigerBoss : MonoBehaviour
{
    public int curHealth;
    public int damageType;
    public GameObject knightPlayer;
    public GameObject sorcererPlayer;
    public int selectingMove;
    public int selectingTarget;
    public int blessTime;
    public int damageOutput;
    public GameObject battlePhase;
    public TextMeshProUGUI HealthText;
    public GameObject VictoryText;
    public float timePassed = 0.0f;
    public bool VictoryAchieved;
    public Slider EnemyHealthBar;
    public AudioClip damageSound;
    private AudioSource audioSource;
    private Animator animator;
    public GameObject fightManager;

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        knightPlayer = GameObject.FindGameObjectWithTag("KnightBattle");
        sorcererPlayer = GameObject.FindGameObjectWithTag("SorcererBattle");
        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
          fightManager = GameObject.FindGameObjectWithTag("FightManager");
        curHealth = 120;
        damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR
        selectingMove = 1;
        selectingTarget = 1;
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
        //knightPlayer.GetComponent<KnightMoveset>().NumberedFight(3);
        //sorcererPlayer.GetComponent<SorcererMoveset>().NumberedFight(3);
        battlePhase.GetComponent<BattlePhase>().NumberedFight(3);
      
        if (VictoryAchieved == true)
        {
            PlayerPrefs.SetInt("BeatTiger", 1);
            timePassed += Time.deltaTime;
            if (timePassed > 3.0f)
            {
Debug.Log("Change scene");
                SceneManager.LoadScene("forestOverworld");
            }
        }
    }


    public void TakeDamage(int amount)
    {
        curHealth -= amount;
        if (damageSound != null)
            audioSource.PlayOneShot(damageSound);

        UpdateHUD();

        if (curHealth <= 0) 
            Victory();
       
        EnemyHealthBar.value = curHealth;
    }


    public void gotGoaded()
    {
        //Here is where the code will be for the enemy when they're goaded once allies are added
    }


    public void gotStunned()
    {
        //Here is where the code will be for the enemy when they're stunned
    }


    public void BeginTurn()
    {
        if (blessTime > 0)
        {
            curHealth += Random.Range(1, 13);
            blessTime -= 1;
        }
       
        selectingMove = Random.Range(1, 5);
        selectingTarget = Random.Range(1, 3);

        animator.SetBool("isAttacking", true);

        if (selectingMove == 1)
        {
Debug.Log("Crush is used!");

            damageOutput = Random.Range(1, 13) + Random.Range(1, 13);

            if (selectingTarget == 1)
                knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
            
            else if (selectingTarget == 2)
                sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);  
        }

        else if (selectingMove == 2)
        {
Debug.Log("Sweep is used!");
            damageOutput = Random.Range(1, 7) + Random.Range(1, 7);
            knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
            sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
        }
        
        else if (selectingMove == 3)
        {
Debug.Log("Empower is used!");
            blessTime += 3;
        }

        else if (selectingMove == 4)
        {
Debug.Log("Tiger Ward is used!");
            curHealth += Random.Range(1, 5) + Random.Range(1, 5);
        }

        animator.SetBool("isAttacking", false);
    }

    void UpdateHUD()
    {
        //HealthText.text = "HP: " + curHealth;
    }

    public void Victory()
    {
        fightManager.GetComponent<FightManager>().BattleComplete();
        VictoryAchieved = true;
        VictoryText.SetActive(true);
Debug.Log("Victory achieved!");
    }
}
