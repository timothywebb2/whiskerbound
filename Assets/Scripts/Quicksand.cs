using UnityEngine;

public class Quicksand : MonoBehaviour
{
    public Transform player;
    public CharacterController control;
    public Transform dragPoint;
    public float speed;
    //private bool drag;
    public void OnTriggerStay(Collider WhatIHit)
    {
        if(WhatIHit.tag == "Player")
        {
            //float step = speed * Time.deltaTime;
            Vector3 dir = dragPoint.position - player.position;
            Vector3 movement = dir.normalized * speed * Time.deltaTime;
            if(movement.magnitude > dir.magnitude)
                movement = dir;
            control.Move(movement);
            //player.position = Vector3.Lerp(player.position, dragPoint.position, step);
            //drag = true;
        }
    }

    /*public void OnTriggerExit(Collider WhatIHit)
    {
        if(WhatIHit.tag == "Player")   
            drag = false;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        if(drag)
        {
            player.position = Vector3.Lerp(player.position, dragPoint.position, step);
        }
    }*/
}
