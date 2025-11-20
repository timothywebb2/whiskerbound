using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class SorcererMoveset : MonoBehaviour
{

    public int maxHealth;
    public int curHealth;
    public int damageType;
    public int mightBonus;
    public int damageOutput;
    public int shieldOutput;
    public int thornsOutput;
    public int healOutput;
    public GameObject knightAlly;
    public GameObject firstEnemy;
    //  public bool intercedeOn;
    public TextMeshProUGUI HealthText;
    public GameObject SorcererSkills;

    public TextMeshProUGUI currentAction;
    public bool printing;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 50;
        //   damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR
        knightAlly = GameObject.FindGameObjectWithTag("KnightBattle");
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
     //   intercedeOn = false;
        currentAction.enabled = false;
        UpdateHUD();

    }

    // Update is called once per frame
    void Update()
    {

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
        curHealth -= amount;
            if(!printing)
                StartCoroutine(printCurrentAction("Took " + amount + " damage!", 0.5f));
        UpdateHUD();
    }

    public void Incinerate() {

damageOutput = Random.Range(1, 7) + Random.Range(1, 7) + mightBonus;
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
Debug.Log("Damaged enemy by " + damageOutput + " with Incinerate");
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Incinerate!", 0f));
PassTurn();

    }

    public void Enervate() {

damageOutput = Random.Range(1, 7) + mightBonus;
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
        firstEnemy.GetComponent<DemoEnemy>().gotStunned();
        Debug.Log("Damaged enemy by " + damageOutput);
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Enervate!", 0f));
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

    public void Rally() {

        healOutput = Random.Range(1, 13);
        if (curHealth + healOutput >= 50)
            curHealth = 50;
        else
            curHealth += healOutput;

        Debug.Log("Healing self by " + healOutput);
        if (!printing)
            StartCoroutine(printCurrentAction("Healing self by " + healOutput + " with Rally!", 0f));
PassTurn();

    }

    void UpdateHUD()
    {
        HealthText.text = "HP: " + curHealth;
    }

    public void PassTurn()
    {

        firstEnemy.GetComponent<DemoEnemy>().BeginTurn();

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
}
