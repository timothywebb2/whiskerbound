using UnityEngine;


public class BattlePhase : MonoBehaviour
{
    public int battlePhaseTurn;
    public int squirrelFight;
    public GameObject firstEnemy;
    public GameObject KnightIcon;
    public GameObject SorcererIcon;
    public GameObject ClericIcon;
    public GameObject KnightBattle;
    public GameObject SorcererBattle;
    public GameObject ClericBattle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battlePhaseTurn  = 0;
        squirrelFight = 1;
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");

        KnightIcon = GameObject.FindGameObjectWithTag("KnightIcon");
        SorcererIcon = GameObject.FindGameObjectWithTag("SorcererIcon");
        ClericIcon = GameObject.FindGameObjectWithTag("ClericIcon");

        KnightBattle = GameObject.FindGameObjectWithTag("KnightBattle");
        SorcererBattle = GameObject.FindGameObjectWithTag("SorcererBattle");
        ClericBattle = GameObject.FindGameObjectWithTag("ClericBattle");
    }

    // Update is called once per frame
    void Update()
    {
        //CHECK WHAT CHARACTERS ARE UNLOCKED
        //IF JUST KNIGHT, if(battlePhaseTurn == 1)
        //IF KNIGHT AND SORCERER, if(battlePhaseTurn == 2)
        if (battlePhaseTurn == 3)
        {
            // Temporary
            if (squirrelFight == 1)
            {
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().BeginTurn());
                battlePhaseTurn = 0;
            }
            
            else if (squirrelFight == 2)
            {
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().BeginTurn());
                battlePhaseTurn = 0;
            }

            else if (squirrelFight == 3)
            {
                StartCoroutine(firstEnemy.GetComponent<TigerBoss>().BeginTurn());
                battlePhaseTurn = 0;
            }

            KnightIcon.SetActive(true);
            SorcererIcon.SetActive(true);
            ClericIcon.SetActive(true);
        }
    }

    public void NumberedFight(int amount)
    {
        squirrelFight = amount;
        KnightBattle.GetComponent<KnightMoveset>().NumberedFight(squirrelFight);
        SorcererBattle.GetComponent<SorcererMoveset>().NumberedFight(squirrelFight);
        ClericBattle.GetComponent<ClericMoveset>().NumberedFight(squirrelFight);
    }

    public void ActionInputted()
    {
       battlePhaseTurn += 1;
    }
}
