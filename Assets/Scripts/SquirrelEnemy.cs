using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SquirrelEnemy : MonoBehaviour
{

    public int curHealth1;
    public int curHealth2;
    public int damageType;
    public GameObject knightPlayer;
     public GameObject sorcererPlayer;
     public GameObject squirrelOne;
     public GameObject squirrelTwo;
     public bool squirrelOneDown;
    public bool squirrelTwoDown;
                        public GameObject battlePhase;
    public bool squirrelCoordination;
     public int selectingMove;
          public int selectingTarget;
          public int damageOutput;
          public int attackedEnemy;
          public int multiHitting;
        
        public GameObject VictoryText;
        public float timePassed = 0.0f;
        public bool VictoryAchieved;
    public Slider EnemyHealthBar1;
    public Slider EnemyHealthBar2;

    
     

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        knightPlayer = GameObject.FindGameObjectWithTag("KnightBattle");
        sorcererPlayer = GameObject.FindGameObjectWithTag("SorcererBattle");
        battlePhase = GameObject.FindGameObjectWithTag("BattleController");
        curHealth1 = 30;
        curHealth2 = 30;
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
        EnemyHealthBar1.maxValue = curHealth1;
        EnemyHealthBar1.value = curHealth1;

        EnemyHealthBar2.maxValue = curHealth2;
        EnemyHealthBar2.value = curHealth2;

        
    }

    // Update is called once per frame
    void Update()
    {

      //  knightPlayer.GetComponent<KnightMoveset>().SquirrelFight();
     //   sorcererPlayer.GetComponent<SorcererMoveset>().SquirrelFight();
        battlePhase.GetComponent<BattlePhase>().NumberedFight(2);

        if (VictoryAchieved == true)
        {
            PlayerPrefs.SetInt("BeatSquirrel", 1);
            timePassed += Time.deltaTime;
            if (timePassed > 3.0f)
            {
Debug.Log("Change scene");
                SceneManager.LoadScene("forestOverworld");
            }
        }
    }

    public void TakeDamage(int amount) {
        if (squirrelOneDown == false && squirrelTwoDown == false) {
        attackedEnemy = Random.Range(1, 3);
        }
        if (squirrelOneDown == true && squirrelTwoDown == false) {
        attackedEnemy = 2;
        }
        if (squirrelOneDown == false && squirrelTwoDown == true) {
        attackedEnemy = 1;
        }
        if (multiHitting == 1) {
        if (attackedEnemy == 1) {
                curHealth1 -= amount;
                EnemyHealthBar1.value = curHealth1;

                
                
        
        if (curHealth1 <= 0) {
            squirrelOneDown = true;
        }
        }
        else if (attackedEnemy == 2) {
            curHealth2 -= amount;
            EnemyHealthBar2.value = curHealth2;

        
        if (curHealth2 <= 0) {
            squirrelTwoDown = true;
        }
        }
        }
         if (multiHitting == 2) {
curHealth1 -= amount;
curHealth2 -= amount;
EnemyHealthBar1.value = curHealth1;
EnemyHealthBar2.value = curHealth2;


if (curHealth1 <= 0) {
            squirrelOneDown = true;
        }
if (curHealth2 <= 0) {
            squirrelTwoDown = true;
        }
multiHitting = 1;
         }

        if (squirrelOneDown == true || squirrelTwoDown == true) {
squirrelCoordination = false;
        }

        if (squirrelOneDown == true && squirrelTwoDown == true) {
            Victory();
        }
    }

    public void gotGoaded() {
        // Here is where the code will be for the enemy when they're goaded once allies are added
    }

    public void gotStunned()
    {
        // Here is where the code will be for the enemy when they're stunned
    }

    public void BeginTurn() {
        if (squirrelOneDown == false) {
        selectingMove = Random.Range(1, 3);
        selectingTarget = Random.Range(1, 3);
if (selectingMove == 1) {
    if (selectingTarget == 1) {
    Debug.Log("Lash is used!");
    if (squirrelCoordination == false) {
    damageOutput = Random.Range(1, 7) + 1;
    knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
    }
    else if (squirrelCoordination == true) {
    damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + 1;
    knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
    }
    }
    if (selectingTarget == 2) {
    Debug.Log("Lash is used!");
    if (squirrelCoordination == false) {
    damageOutput = Random.Range(1, 7) + 1;
    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
    }
    else if (squirrelCoordination == true) {
    damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + 1;
    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
    }
    }
}
else if (selectingMove == 2) {
    Debug.Log("Recuperate is used!");
    damageOutput = Random.Range(1, 5) + Random.Range(1, 5) + 1;
    curHealth1 += damageOutput;
    EnemyHealthBar1.value = curHealth1;
}
    }
    BeginTurn2();
}

public void BeginTurn2() {
    if (squirrelTwoDown == false) {
    selectingMove = Random.Range(1, 3);
        selectingTarget = Random.Range(1, 3);
if (selectingMove == 1) {
    if (selectingTarget == 1) {
    Debug.Log("Lash is used!");
    if (squirrelCoordination == false) {
    damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
    knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
    }
    else if (squirrelCoordination == true) {
    damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
    knightPlayer.GetComponent<KnightMoveset>().TakeDamage(damageOutput);
    }
    }
    if (selectingTarget == 2) {
    Debug.Log("Lash is used!");
    if (squirrelCoordination == false) {
    damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
    }
    else if (squirrelCoordination == true) {
    damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
    sorcererPlayer.GetComponent<SorcererMoveset>().TakeDamage(damageOutput);
    }
    }
}
else if (selectingMove == 2) {
    Debug.Log("Recuperate is used!");
    damageOutput = Random.Range(1, 5) + Random.Range(1, 5) + Random.Range(1, 5) + Random.Range(1, 5) + 1;
    curHealth2 += damageOutput;
    EnemyHealthBar2.value = curHealth2;
}
    }
}

public void multiHit() {
multiHitting = 2;
}

    

public void Victory() {
VictoryAchieved = true;
VictoryText.SetActive(true);
Debug.Log("Victory achieved!");

}

}