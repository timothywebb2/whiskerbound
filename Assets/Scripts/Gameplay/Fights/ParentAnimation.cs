using UnityEngine;

public class ParentAnimation : MonoBehaviour
{
    // this script exists to trigger animation events that exist in other objects...
    // for example, when the VFX animation finishes, the enemy continues their turn

    public GameObject parent; // the script to be affected by the animation event
    public void AnimationDone()
    {
        if(PlayerPrefs.GetInt("Enemy") == 2) //squirrel
            parent.GetComponent<SquirrelEnemy>().AnimationDone();
        /*else if(PlayerPrefs.GetInt("Enemy") == 3) // tiger
            parent.GetComponent<TigerBoss>().AnimationDone();*/
        else // all other enemies
            parent.GetComponent<DemoEnemy>().AnimationDone();
    }

    public void AnimationHit() // this is for when there are more than one enemy, since the animator is not on the same object as the script
    {
        parent.GetComponent<SquirrelEnemy>().AnimationHit();
    }
}
