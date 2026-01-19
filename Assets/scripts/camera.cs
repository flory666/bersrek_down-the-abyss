using UnityEngine;
using UnityEngine.InputSystem;

public class camera : MonoBehaviour
{
    [Header("Input")]
    public Controls Inputs;

    [Header("Settings")]
    public float rotationSpeed = 120f;
    public float deadzone = 0.15f;

    private float currentYaw;
    private Vector2 lookInput;
    public GameObject player;
    private Vector3 pos;

    void Awake()
    {
        Inputs = new Controls();
    }

    void OnEnable()
    {
        Inputs.guts.Enable();
        pos.y=24f;
    }

    void OnDisable()
    {
        Inputs.guts.Disable();
    }

    void Update()
    {
        pos.x = player.transform.position.x;
        pos.z = player.transform.position.z;
        transform.position = pos;
        lookInput = Inputs.guts.move_camera.ReadValue<Vector2>();
        float x = lookInput.x;

        if (Mathf.Abs(x) < deadzone)
            return;

        currentYaw += x * rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            currentYaw,
            transform.rotation.eulerAngles.z
        );
    }
}
