using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DemoEnemy : MonoBehaviour
{

    public int curHealth;
    public int damageType;
    public GameObject knightPlayer;
        public TextMeshProUGUI HealthText;
        public GameObject VictoryText;
        public float timePassed = 0.0f;
        public bool VictoryAchieved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        knightPlayer = GameObject.FindGameObjectWithTag("KnightBattle");
        curHealth = 75;
        damageType = 2; // 1 = PHYS, 2 = MYS, 3 = SPR
        VictoryText.SetActive(false);
        VictoryAchieved = false;
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        if (VictoryAchieved == true) {
        timePassed += Time.deltaTime;
        if (timePassed > 3.0f)
{
Debug.Log("Change scene");
SceneManager.LoadScene(2);
}
        }
    }

    public void TakeDamage(int amount) {
        curHealth -= amount;
        UpdateHUD();
        if (curHealth <= 0) {
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
        Debug.Log("Damaged player by 2");
        knightPlayer.GetComponent<KnightMoveset>().TakeDamage(2);
    }

    void UpdateHUD()
    {
        HealthText.text = "HP: " + curHealth;
    }

public void Victory() {
VictoryAchieved = true;
VictoryText.SetActive(true);
Debug.Log("Victory achieved!");

}

}
