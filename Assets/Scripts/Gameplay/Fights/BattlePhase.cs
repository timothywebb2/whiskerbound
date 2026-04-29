using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI movesLeft;
    private int partySize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battlePhaseTurn  = 0;
        squirrelFight = 1;
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");

        partySize = PlayerPrefs.GetInt("PartySize", 1);
        movesLeft.text = "Moves Left: " + partySize.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (battlePhaseTurn == partySize)
        {
            // Temporary
            if (squirrelFight == 1)
            {
                StartCoroutine(firstEnemy.GetComponent<DemoEnemy>().BeginTurn());
                battlePhaseTurn = 0;
                movesLeft.text = "Moves Left: " + partySize.ToString();
            }
            
            else if (squirrelFight == 2)
            {
                StartCoroutine(firstEnemy.GetComponent<SquirrelEnemy>().BeginTurn());
                battlePhaseTurn = 0;
                movesLeft.text = "Moves Left: " + partySize.ToString();
            }

            else if (squirrelFight == 3)
            {
                StartCoroutine(firstEnemy.GetComponent<TigerBoss>().BeginTurn());
                battlePhaseTurn = 0;
                movesLeft.text = "Moves Left: " + partySize.ToString();
            }

            KnightIcon.SetActive(true);
            if(partySize >= 2)
                SorcererIcon.SetActive(true);
            if(partySize >= 3)
            ClericIcon.SetActive(true);
        }
    }

    public void NumberedFight(int amount)
    {
        squirrelFight = amount;
        KnightBattle.GetComponent<KnightMoveset>().NumberedFight(squirrelFight);

        if(partySize >= 2)
            SorcererBattle.GetComponent<SorcererMoveset>().NumberedFight(squirrelFight);
        if(partySize >= 3)
            ClericBattle.GetComponent<ClericMoveset>().NumberedFight(squirrelFight);
    }

    public void ActionInputted()
    {
        battlePhaseTurn += 1;
        movesLeft.text = "Moves Left: " + (partySize - battlePhaseTurn).ToString();
    }
}
