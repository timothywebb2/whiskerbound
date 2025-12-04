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
    public bool squirrelCoordination;
     public int selectingMove;
          public int selectingTarget;
          public int damageOutput;
          public int attackedEnemy;
          public int multiHitting;
        public TextMeshProUGUI HealthText1;
        public TextMeshProUGUI HealthText2;
        public GameObject VictoryText;
        public float timePassed = 0.0f;
        public bool VictoryAchieved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        knightPlayer = GameObject.FindGameObjectWithTag("KnightBattle");
        sorcererPlayer = GameObject.FindGameObjectWithTag("SorcererBattle");
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
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {

        knightPlayer.GetComponent<KnightMoveset>().SquirrelFight();
        sorcererPlayer.GetComponent<SorcererMoveset>().SquirrelFight();

        if (VictoryAchieved == true)
        {
            PlayerPrefs.SetInt("BeatSquirrel", 1);
            timePassed += Time.deltaTime;
            if (timePassed > 3.0f)
            {
Debug.Log("Change scene");
                SceneManager.LoadScene("Overworld");
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
        UpdateHUD();
        if (curHealth1 <= 0) {
            squirrelOneDown = true;
        }
        }
        else if (attackedEnemy == 2) {
            curHealth2 -= amount;
        UpdateHUD();
        if (curHealth2 <= 0) {
            squirrelTwoDown = true;
        }
        }
        }
         if (multiHitting == 2) {
curHealth1 -= amount;
curHealth2 -= amount;
UpdateHUD();
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
}
    }
}

public void multiHit() {
multiHitting = 2;
}

    void UpdateHUD()
    {
        HealthText1.text = "HP: " + curHealth1;
        HealthText2.text = "HP: " + curHealth2;
    }

public void Victory() {
VictoryAchieved = true;
VictoryText.SetActive(true);
Debug.Log("Victory achieved!");

}

}