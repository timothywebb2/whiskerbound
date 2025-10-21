using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
public class ProtoMovement : MonoBehaviour
{
    public InputActionAsset InputActions;
    public CinemachineCamera frontCamera;
    private InputAction m_move;
    private Vector2 m_moveAmt;
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
    }
    
    void Update()
    {
        m_moveAmt = m_move.ReadValue<Vector2>() / sensitivity;
        this.transform.Translate(m_moveAmt.y, 0, -m_moveAmt.x);
    }
}
