using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class KnightMoveset : MonoBehaviour
{

    public int maxHealth;
    public int curHealth;
    public int damageType;
    public int mightBonus;
    public int damageOutput;
    public int healOutput;
    public GameObject firstEnemy;
    public bool intercedeOn;
    public TextMeshProUGUI HealthText;
    public GameObject KnightSkills;

    public TextMeshProUGUI currentAction;
    private bool printing;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 50;
        damageType = 1; // 1 = PHYS, 2 = MYS, 3 = SPR
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
        intercedeOn = false;
        currentAction.enabled = false;
        UpdateHUD();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount) {
        if (intercedeOn == false) {
            curHealth -= amount;
            if(!printing)
                StartCoroutine(printCurrentAction("Took " + amount + " damage!", 0.5f));
        }
        else if (intercedeOn == true) {
            Debug.Log("Damage blocked!");
            if (!printing)
                StartCoroutine(printCurrentAction("Damage blocked!", 0.5f));
            intercedeOn = false;
        }
        UpdateHUD();
    }

    public void Provoke() {

damageOutput = Random.Range(1, 13) + Random.Range(1, 13) + mightBonus;
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
firstEnemy.GetComponent<DemoEnemy>().gotGoaded();
Debug.Log("Damaged enemy by " + damageOutput + " with Provoke");
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Provoke!", 0f));
PassTurn();

    }

    public void Cleave() {

damageOutput = Random.Range(1, 13) + mightBonus;
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
Debug.Log("Damaged enemy by " + damageOutput);
        if (!printing)
            StartCoroutine(printCurrentAction("Damaged enemy by " + damageOutput + " with Cleave!", 0f));
PassTurn();

    }

    public void Intercede() {

intercedeOn = true;
Debug.Log("Intercede on!");
        if (!printing)
            StartCoroutine(printCurrentAction("Intercede on!", 0f));
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
    
    public void OpenKnightSkills()
    {
        if (!printing)
            KnightSkills.SetActive(true);
    }
}
