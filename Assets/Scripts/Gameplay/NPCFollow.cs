using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class NPCFollow : MonoBehaviour
{
    public enum FollowerType
    {
        Sorcerer, Cleric
    };
    public FollowerType followerType;

    public Transform knightTransform;

    public Transform followCharacter; // character this NPC will follow
    public float distanceFromCharacter; // how far this character should be by default from the person they're following
    public List<Vector3> followCharacterPositions = new List<Vector3>(); // past positions of character for this NPC to pathfind to
    public List<Vector2> playerInputs = new List<Vector2>(); // inputs from player at each sampled position; determines animation
    public float allowableSampleDistance; // max distance from character before creating a new sample
    public float sampleTimeDifference; // max time between samples
    float sampleTime; // time since last sample
    float followSpeed; // current speed of NPC
    public float baseSpeed; // base NPC walking speed
    public float fastSpeed; // fast NPC walking speed to catch up
    public float fastDistance; // minimum distance character needs to be from follower to activate fastSpeed
    public float removeDistance; // margin of error when NPC travels to sample (smaller number = more accurate)
    private Vector3 oldPosition;
    private CinemachineCamera frontCamera;
    private Animator animator;

    void Start()
    {
        int partySize = PlayerPrefs.GetInt("PartySize", 1);

        frontCamera = knightTransform.GetComponent<ProtoMovement>().frontCamera;

        if(followerType == FollowerType.Sorcerer && partySize < 2)
            this.gameObject.SetActive(false);
        else if(followerType == FollowerType.Cleric && partySize < 3)
            this.gameObject.SetActive(false);
        
        sampleTime = Time.time;
        followSpeed = fastSpeed;

        oldPosition = transform.position;

        transform.position = knightTransform.position;
        followCharacterPositions.Add(knightTransform.position);
        playerInputs.Add(new Vector2(0, 0));

        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        float speed = Vector3.Distance(transform.position, oldPosition) * 100f;
        oldPosition = transform.position;

        if(speed == 0)
        {
            animator.SetInteger("XDirection", 0);
            animator.SetInteger("YDirection", 0);
        }

        // sample player position
        if(Time.time > sampleTime)
        {
            sampleTime = Time.time + sampleTimeDifference;

            if(Vector3.Distance(transform.position, followCharacter.position) > distanceFromCharacter &&
                Vector3.Distance(followCharacter.position, followCharacterPositions[followCharacterPositions.Count - 1]) > allowableSampleDistance)
            {
                followCharacterPositions.Add(followCharacter.position); // sample players current position and add to list

                if(followerType == FollowerType.Sorcerer)
                    playerInputs.Add(knightTransform.GetComponent<ProtoMovement>().moveInput);
                else if(followerType == FollowerType.Cleric)
                    playerInputs.Add(followCharacter.GetComponent<NPCFollow>().playerInputs[0]);

            }
        }

        // move to player position
        if((Vector3.Distance(transform.position, followCharacter.position) > fastDistance) || // if follower is too far away
        knightTransform.gameObject.GetComponent<ProtoMovement>().isSprinting) // OR player is sprinting
            followSpeed = fastSpeed; // then increase speed
        else
            followSpeed = baseSpeed;

        if(Vector3.Distance(transform.position, followCharacter.position) > distanceFromCharacter) // if the current distance between NPC and character is above max
        {
            Vector3 cameraForward = GetCameraForward();
            Vector3 cameraRight = GetCameraRight();

            Vector3 moveDirection = (cameraForward * playerInputs[0].y + cameraRight * playerInputs[0].x);

            float deadZone = 0.1f;
            Vector3 worldMove = moveDirection.normalized;

            int xDir = 0;
            int yDir = 0;

            if (worldMove.x > deadZone) xDir = 1;
            else if (worldMove.x < -deadZone) xDir = -1;
            else xDir = 0;

            if (worldMove.z > deadZone) yDir = 1;
            else if (worldMove.z < -deadZone) yDir = -1;
            else yDir = 0;

            int rotatedX = -yDir;
            int rotatedY = xDir;

            animator.SetInteger("XDirection", rotatedX);
            animator.SetInteger("YDirection", rotatedY);

            transform.position = Vector3.MoveTowards(transform.position, followCharacterPositions[0], Time.deltaTime * followSpeed);

            if(Vector3.Distance(transform.position, followCharacterPositions[0]) < removeDistance && followCharacterPositions.Count > 1)
            {
                followCharacterPositions.RemoveAt(0);
                playerInputs.RemoveAt(0);
            } 
        }
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = frontCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = frontCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
}
