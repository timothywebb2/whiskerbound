using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SorcererMoveset : MonoBehaviour
{

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
        public int squirrelFight;
        public bool intercedeOn;
            public float timePassed = 0.0f;
    public GameObject knightAlly;
    public GameObject firstEnemy;
        public bool loseCondition;
    //  public bool intercedeOn;
    public TextMeshProUGUI HealthText;
    public GameObject SorcererSkills;
     public GameObject LoseText;

    public TextMeshProUGUI currentAction;
    public bool printing;

    // testing

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 30;
         intercedeOn = false;
         rallyOrNot = false;
         volcanicTally = 0;
        //   damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR
        knightAlly = GameObject.FindGameObjectWithTag("KnightBattle");
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
     //   intercedeOn = false;
        currentAction.enabled = false;
         loseCondition = false;
        squirrelFight = 1;
          LoseText.SetActive(false);
        UpdateHUD();

    }

    // Update is called once per frame
    void Update()
    {

        if (loseCondition == true) {
        timePassed += Time.deltaTime;
        if (timePassed > 3.0f)
{
Debug.Log("Change scene");
SceneManager.LoadScene(2);
}
        }

    

        /*

 if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            if (SorcererSkills.activeSelf == true) {
SorcererSkills.SetActive(false);
            }
            else {
                        if (!printing) {
SorcererSkills.SetActive(true);
                        }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (SorcererSkills.activeSelf == true) {
Provoke();
SorcererSkills.SetActive(false);
            }
        }

         if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SorcererSkills.activeSelf == true) {
Cleave();
SorcererSkills.SetActive(false);
            }
        }

         if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (SorcererSkills.activeSelf == true) {
Intercede();
SorcererSkills.SetActive(false);
            }
        }

         if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (SorcererSkills.activeSelf == true) {
Rally();
SorcererSkills.SetActive(false);
            }
        }

         */
         
    }

    public void TakeDamage(int amount) {
       if (intercedeOn == false) {
            curHealth -= amount;
            if(!printing)
                StartCoroutine(printCurrentAction("Sorcerer took " + amount + " damage!", 1f));
        }
        else if (intercedeOn == true) {
            Debug.Log("Damage blocked!");
            if (!printing)
                StartCoroutine(printCurrentAction("Damage blocked from Sorcerer!", 1f));
            intercedeOn = false;
        }
        if (curHealth >= 25) {
            knightAlly.GetComponent<KnightMoveset>().UnLastStand();
        }
           else if (curHealth < 25) {
            knightAlly.GetComponent<KnightMoveset>().LastStand();
        }
         if (curHealth <= 0) {
            Lose();
        }
        UpdateHUD();
    }

    public void Incinerate() {

damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + mightBonus;
if (squirrelFight == 1) {
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 2) {
    firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 3) {
firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
}
volcanicTally += damageOutput;
Debug.Log("Damaged enemy by " + damageOutput + " with Incinerate");
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Incinerate!", 0f));
VolcanicHex();
PassTurn();

    }

    public void Enervate() {

damageOutput = Random.Range(1, 7) + mightBonus;
if (squirrelFight == 1) {
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 2) {
    firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 3) {
firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
}
volcanicTally += damageOutput;
if (squirrelFight == 1) {
        firstEnemy.GetComponent<DemoEnemy>().gotStunned();
}
else if (squirrelFight == 2) {
        firstEnemy.GetComponent<SquirrelEnemy>().gotStunned();
}
else if (squirrelFight == 3) {
firstEnemy.GetComponent<TigerBoss>().gotStunned();
}
        Debug.Log("Damaged enemy by " + damageOutput);
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Enervate!", 0f));
VolcanicHex();
PassTurn();

    }

    public void Ward() {

Debug.Log("Ward activated!");
        shieldOutput = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7) + mightBonus;
        knightAlly.GetComponent<KnightMoveset>().gotShielded(shieldOutput);
        if (!printing)
            StartCoroutine(printCurrentAction("Ward used on ally!", 0f));
PassTurn();

    }

    public void Scourge()
    {

        Debug.Log("Scourge activated!");
        thornsOutput = Random.Range(1, 7) + mightBonus;
        knightAlly.GetComponent<KnightMoveset>().gotThorns(thornsOutput);
        if (!printing)
            StartCoroutine(printCurrentAction("Thorns used on ally!", 0f));
        PassTurn();

    }

    public void VolcanicHex() {
        if (volcanicTally >= 30) {
damageOutput = Random.Range(1, 13) + mightBonus;
if (squirrelFight == 1) {
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 2) {
    firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 3) {
firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
}
volcanicTally = 0;
        }

    }

public void RallyIncinerate() {
damageOutput = Random.Range(1, 4) + Random.Range(1, 4) + mightBonus;
if (squirrelFight == 1) {
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 2) {
    firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 3) {
firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
}
Debug.Log("Damaged enemy by " + damageOutput + " with Incinerate");
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Incinerate!", 0f));
    }

    public void RallyEnervate() {
damageOutput = Random.Range(1, 4) + mightBonus;
if (squirrelFight == 1) {
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 2) {
    firstEnemy.GetComponent<SquirrelEnemy>().multiHit();
firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
}
else if (squirrelFight == 3) {
firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
}
if (squirrelFight == 1) {
        firstEnemy.GetComponent<DemoEnemy>().gotStunned();
}
else if (squirrelFight == 2) {
         firstEnemy.GetComponent<SquirrelEnemy>().gotStunned();
}
else if (squirrelFight == 3) {
firstEnemy.GetComponent<TigerBoss>().gotStunned();
}
        Debug.Log("Damaged enemy by " + damageOutput);
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Enervate!", 0f));
    }

    public void IntercedeSorcerer() {
        Debug.Log("Intercede on Sorcerer!");
        intercedeOn = true;
    }

        public void NotSquirrelFight() {
squirrelFight = 1;
    }

    public void SquirrelFight() {
squirrelFight = 2;
    }

        public void NumberedFight(int amount) {
squirrelFight = amount;
    }

    void UpdateHUD()
    {
        HealthText.text = "HP: " + curHealth;
    }

    public void PassTurn()
    {

if (squirrelFight == 1) {
        firstEnemy.GetComponent<DemoEnemy>().BeginTurn();
}
else if (squirrelFight == 2) {
        firstEnemy.GetComponent<SquirrelEnemy>().BeginTurn();
}
else if (squirrelFight == 3) {
        firstEnemy.GetComponent<TigerBoss>().BeginTurn();
}

    }

    IEnumerator printCurrentAction(string toPrint, float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return new WaitUntil(() => !printing);

        printing = true;

        currentAction.enabled = true;
        currentAction.text = toPrint;

        yield return new WaitForSeconds(2);

        printing = false;
        currentAction.enabled = false;
    }
    
    public void OpenSorcererSkills()
    {
        if (!printing)
            SorcererSkills.SetActive(true);
    }

    public void Lose() {
loseCondition = true;
LoseText.SetActive(true);
Debug.Log("You lose!");
}
}
