using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 50;
        damageType = 1; // 1 = PHYS, 2 = MYS, 3 = SPR
        firstEnemy = GameObject.FindGameObjectWithTag("Enemy1");
        intercedeOn = false;
        UpdateHUD();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount) {
        if (intercedeOn == false) {
        curHealth -= amount;
        }
        else if (intercedeOn == true) {
            Debug.Log("Damage blocked!");
            intercedeOn = false;
        }
        UpdateHUD();
    }

    public void Provoke() {

damageOutput = Random.Range(1, 13) + Random.Range(1, 13) + mightBonus;
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
firstEnemy.GetComponent<DemoEnemy>().gotGoaded();
Debug.Log("Damaged enemy by " + damageOutput);
PassTurn();

    }

    public void Cleave() {

damageOutput = Random.Range(1, 13) + mightBonus;
firstEnemy.GetComponent<DemoEnemy>().TakeDamage(damageOutput);
Debug.Log("Damaged enemy by " + damageOutput);
PassTurn();

    }

    public void Intercede() {

intercedeOn = true;
Debug.Log("Intercede on!");
PassTurn();

    }

    public void Rally() {

healOutput = Random.Range(1, 13);
curHealth += healOutput;
Debug.Log("Healing self by " + healOutput);
PassTurn();

    }

    void UpdateHUD()
    {
        HealthText.text = "HP: " + curHealth;
    }

    public void PassTurn() {

firstEnemy.GetComponent<DemoEnemy>().BeginTurn();

    }
}
