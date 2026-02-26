using UnityEngine;
using System.Collections.Generic;

public class NPCFollow : MonoBehaviour
{
    public Transform followCharacter; // character this NPC will follow
    public float distanceFromCharacter;
    public List<Vector3> followCharacterPositions = new List<Vector3>(); // past positions of character for this NPC to pathfind to
    public float allowableSampleDistance; // max distance from character before creating a new sample
    public float sampleTimeDifference; // max time between samples
    float sampleTime; // time since last sample
    float followSpeed; // current speed of NPC
    public float baseSpeed; // base NPC walking speed
    public float fastSpeed; // fast NPC walking speed to catch up
    public float fastDistance;
    public float removeDistance; // margin of error when NPC travels to sample (smaller number = more accurate)

    void Start()
    {
        sampleTime = Time.time;
        followCharacterPositions.Add(followCharacter.position);
        followSpeed = fastSpeed;
    }

    void Update()
    {
        if(Time.time > sampleTime)
        {
            sampleTime = Time.time + sampleTimeDifference;

            if(Vector3.Distance(transform.position, followCharacter.position) > distanceFromCharacter &&  
                Vector3.Distance(followCharacter.position, followCharacterPositions[followCharacterPositions.Count - 1]) > allowableSampleDistance)
            {
                followCharacterPositions.Add(followCharacter.position); // sample players current position and add to list
            }
        }

        if(Vector3.Distance(transform.position, followCharacter.position) > fastDistance)
            followSpeed = fastSpeed;
        else
            followSpeed = baseSpeed;
        
        if(Vector3.Distance(transform.position, followCharacter.position) > distanceFromCharacter) // if the current distance between NPC and character is above max
        {
            transform.position = Vector3.MoveTowards(transform.position, followCharacterPositions[0], Time.deltaTime * followSpeed);

            if(Vector3.Distance(transform.position, followCharacterPositions[0]) < removeDistance && followCharacterPositions.Count > 1)
            {
                followCharacterPositions.RemoveAt(0);
            } 
        }
    }
}
