using UnityEngine;
using UnityEngine.SceneManagement;

public class menuTrigger : MonoBehaviour
{

    //scene settings
    public string sceneToLoad = "ShopUI"; //shop ui
    public bool isAdditive = true; // true for ui overlay on current scene
    private bool hasEntered = false; // prevents instant retriggering

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEntered)
        {
            hasEntered = true; // block reentry

            //disable movement
            ProtoMovement movement = other.GetComponent<ProtoMovement>();
            if (movement != null)
            {
                movement.enabled = false;
                //you would keep momentum from entering shop, so this should stop that
                other.GetComponent<CharacterController>()?.Move(Vector3.zero);
            }

            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        if (isAdditive)
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    public void UnloadShopScene(GameObject player)
    {
        SceneManager.UnloadSceneAsync(sceneToLoad);

        //enable movement
        ProtoMovement movement = player.GetComponent<ProtoMovement>();
        if(movement != null)
            movement.enabled = true;

        hasEntered = false;
    }

    public void ExitShop()
    {

        menuTrigger trigger = Object.FindFirstObjectByType<menuTrigger>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (trigger != null && player != null )
        {
            trigger.UnloadShopScene(player);
        }
        else
        {
            Debug.LogWarning("could not find trigger or player to exit shop properly.");
            SceneManager.UnloadSceneAsync(sceneToLoad);
        }
    }
}
