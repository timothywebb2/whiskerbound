using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class KnightMoveset : MonoBehaviour
{

    public int maxHealth;
    public int curHealth;
    public int damageType;
    public int mightBonus;
    public int damageOutput;
    public int healOutput;
    int hasThorns;
    public int thornDamage;
    public int rallyRandom; // This is temporary
    public GameObject sorcererAlly;
    public bool sorcererLastStand;
    public GameObject firstEnemy;
    public int squirrelFight;
    public float timePassed = 0.0f;
    public bool intercedeOn;
    public bool loseCondition;
    public TextMeshProUGUI HealthText;
    public GameObject KnightSkills;
    public GameObject LoseText;

    public TextMeshProUGUI currentAction;
    public bool printing;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 50;
        damageType = 1; // 1 = PHYS, 2 = MYS, 3 = SPR
        sorcererAlly = GameObject.FindGameObjectWithTag("SorcererBattle");
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
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

            if (KnightSkills.activeSelf == true) {
KnightSkills.SetActive(false);
            }
            else {
                        if (!printing) {
KnightSkills.SetActive(true);
                        }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (KnightSkills.activeSelf == true) {
Provoke();
KnightSkills.SetActive(false);
            }
        }

         if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (KnightSkills.activeSelf == true) {
Cleave();
KnightSkills.SetActive(false);
            }
        }

         if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (KnightSkills.activeSelf == true) {
Intercede();
KnightSkills.SetActive(false);
            }
        }

         if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (KnightSkills.activeSelf == true) {
Rally();
KnightSkills.SetActive(false);
            }
        }
        */
    }

    public void TakeDamage(int amount) {
        if (intercedeOn == false) {
            curHealth -= amount;
            if(!printing)
                StartCoroutine(printCurrentAction("Knight took " + amount + " damage!", 1f));
            if (hasThorns > 0) {
                if (squirrelFight == 1) {
                firstEnemy.GetComponent<DemoEnemy>().TakeDamage(thornDamage);
                }
                else if (squirrelFight == 2) {
                firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(thornDamage);
                }
                else if (squirrelFight == 3) {
                firstEnemy.GetComponent<TigerBoss>().TakeDamage(thornDamage);
                }
                hasThorns -= 1;
            }
        }
        else if (intercedeOn == true) {
            Debug.Log("Damage blocked!");
            if (!printing)
                StartCoroutine(printCurrentAction("Damage blocked!", 1f));
            intercedeOn = false;
        }
        if (curHealth <= 0) {
            Lose();
        }
        UpdateHUD();
    }

    public void Provoke() {

damageOutput = Random.Range(1, 13) + Random.Range(1, 13) + mightBonus;
if (squirrelFight == 1) {
    firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
    firstEnemy.GetComponent<DemoEnemy>().gotGoaded();
}
else if (squirrelFight == 2) {
firstEnemy.GetComponent<SquirrelEnemy>().TakeDamage(damageOutput);
firstEnemy.GetComponent<SquirrelEnemy>().gotGoaded();
}
else if (squirrelFight == 3) {
firstEnemy.GetComponent<TigerBoss>().TakeDamage(damageOutput);
firstEnemy.GetComponent<TigerBoss>().gotGoaded();
}
Debug.Log("Damaged enemy by " + damageOutput + " with Provoke");
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Provoke!", 0f));
PassTurn();

    }

    public void Cleave() {

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
Debug.Log("Damaged enemy by " + damageOutput);
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Cleave!", 0f));
PassTurn();

    }

    public void Intercede() {

// intercedeOn = true;
sorcererAlly.GetComponent<SorcererMoveset>().IntercedeSorcerer();
Debug.Log("Intercede on Sorcerer!");
        if (!printing)
            StartCoroutine(printCurrentAction("Intercede on Sorcerer!", 0f));
PassTurn();

    }

    public void Rally() {

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
if (rallyRandom == 1) {
    sorcererAlly.GetComponent<SorcererMoveset>().RallyIncinerate();
}
else if (rallyRandom == 2) {
    sorcererAlly.GetComponent<SorcererMoveset>().RallyEnervate();
}
Debug.Log("Rally being used!");
PassTurn();
    }

     public void LastStand() {
        if (sorcererLastStand == false) {
mightBonus += 2;
sorcererLastStand = true;
        }
    }

    public void UnLastStand() {
if (sorcererLastStand == true) {
mightBonus -= 2;
sorcererLastStand = false;
        }
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
        HealthText.text = "HP: " + curHealth;
    }

    public void PassTurn()
    {

      //  firstEnemy.GetComponent<DemoEnemy>().BeginTurn();

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
    
    public void OpenKnightSkills()
    {
        if (!printing)
            KnightSkills.SetActive(true);
    }

    public void Lose() {
loseCondition = true;
LoseText.SetActive(true);
Debug.Log("You lose!");
}
}