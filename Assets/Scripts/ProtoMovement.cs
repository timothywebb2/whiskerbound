using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Cinemachine;
public class ProtoMovement : MonoBehaviour
{
    public InputActionAsset InputActions;
    public TMP_Text cameraType;
    public CinemachineCamera isoCamera;
    public CinemachineCamera frontCamera;
    private InputAction m_move;
    private Vector2 m_moveAmt;
    private InputAction m_switch;
    private bool isIsometric = true;
    private float sensitivity = 50f;
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }
    
    private void Awake()
    {
        m_move = InputSystem.actions.FindAction("Move");
        m_switch = InputSystem.actions.FindAction("Jump");

        m_switch.performed += _ =>
        {
            if (isIsometric)
            {
                isoCamera.Priority = 10;
                frontCamera.Priority = 0;
                cameraType.text = "Isometric";
            }
            else
            {
                isoCamera.Priority = 0;
                frontCamera.Priority = 10;
                cameraType.text = "Front";
            }
            isIsometric = !isIsometric;
        };
    }
    
    void Update()
    {
        m_moveAmt = m_move.ReadValue<Vector2>() / sensitivity;
        this.transform.Translate(m_moveAmt.y, 0, -m_moveAmt.x);
    }
}
