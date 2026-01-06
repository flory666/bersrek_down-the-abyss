using System.Diagnostics;
using System.Runtime;
using UnityEngine;

public class gutadinberbec : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    private Vector3 move;
    private Vector3 actionMove;
    private Vector2 moveInput;
    private CharacterController controls;
    public Controls Inputs;
    private Animator anim;
    [SerializeField]
    private bool movable = true;
    [SerializeField]
    private bool canBlock = true;
    [SerializeField]
    private bool rememberAttack = false;
    [SerializeField]
    private int AttackCount;
    [SerializeField]
    private bool stepping = false;
    private float attackDistance = 3f;
    private string animationName = "";
    private string currentAnimation = "";
    private dragon_slayer dragon_slayer;
    private Vector3 characterDirection;
    private int CannonAmmo = 1;
    [SerializeField]
    private MovementState movementState = 0;
    enum MovementState
    {
        idle,
        running,
        attacking,
        attackingIdle,
        blocking,
        dodging,
        armcannon
    }

    private void Awake()
    {
        Inputs = new Controls();
        anim = GetComponent<Animator>();
        controls = GetComponent<CharacterController>();
        dragon_slayer = FindObjectOfType<dragon_slayer>();
        Inputs.guts.attack.performed += ctx => attack();
        Inputs.guts.block.performed += ctx => block_performed();
        Inputs.guts.block.canceled += ctx => block_canceled();
        Inputs.guts.armcannon.performed += ctx => armcannon();
        Inputs.guts.dodge.performed += ctx => dodge();
    }

    private void Update()
    {
        switch (movementState)
        {
            case MovementState.idle:
                movementInput();
                CharacterRotaion();
                anim.Play("idle");
                break;
            case MovementState.running:
                movementInput();
                CharacterRotaion();
                break;
            case MovementState.attacking:
                actionMovement();
                break;
            case MovementState.attackingIdle:
                afterAttackIdle();
                break;
            case MovementState.blocking:
                // Blocking logic if needed
                break;
            case MovementState.dodging:
                actionMovement();
                break;
            case MovementState.armcannon:
                // Arm cannon logic if needed
                break;
        }
        // if (movable)
        // {
        //     movementInput();
        //     CharacterRotaion();
        // }
        // if (stepping)
        // actionMovement();
    }
    private void movementInput()
    {
        moveInput = Inputs.guts.move_character.ReadValue<Vector2>();
        move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        if (move != Vector3.zero && movable)
        {
            movementState = MovementState.running;
            controls.Move(move * speed * Time.fixedDeltaTime);
            anim.Play("run");
            AttackCount = 0;
        }
        else
        {
            movementState = MovementState.idle;
        }
    }
    private void actionMovement()
    {
        moveInput = Inputs.guts.move_character.ReadValue<Vector2>();
        move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        switch (currentAnimation)
        {
            case "attack1":
                CharacterRotaion();
                actionMove = transform.TransformDirection(Vector3.forward);
                controls.Move(actionMove * speed * 0.2f * Time.fixedDeltaTime);
                break;
            case "attack2":
                CharacterRotaion();
                actionMove = transform.TransformDirection(Vector3.forward);
                controls.Move(actionMove * speed * 0.2f * Time.fixedDeltaTime);
                break;
            case "attack3":
                CharacterRotaion();
                actionMove = transform.TransformDirection(Vector3.forward);
                controls.Move(actionMove * speed * 0.2f * Time.fixedDeltaTime);
                break;
            case "dodge":
                actionMove = transform.TransformDirection(Vector3.forward);
                controls.Move(actionMove * speed * 1f * Time.fixedDeltaTime);
                break;
            case "armcannon":
                // No movement during arm cannon animation
                break;
            case "staggered":
                // No movement during staggered animation
                break;
        }


    }
    private void animation_player(string animationName)
    {
        if (currentAnimation != animationName)
        {
            anim.Play(animationName);
            currentAnimation = animationName;
        }
    }
    private void attack()
    {
        if (movementState == MovementState.attacking)
        {
            rememberAttack = true;
            return;
        }
        switch (++AttackCount)
        {
            case 1:
                movementState = MovementState.attacking;
                animation_player("attack1");
                dragon_slayer.EnableSwordCollider();
                movable = false;
                canBlock = false;
                break;
            case 2:
                movementState = MovementState.attacking;
                animation_player("attack2");
                dragon_slayer.EnableSwordCollider();
                movable = false;
                canBlock = false;
                break;
            case 3:
                movementState = MovementState.attacking;
                animation_player("attack3");
                dragon_slayer.EnableSwordCollider();
                movable = false;
                canBlock = false;
                break;
        }
        if (AttackCount == 3)
        {
            AttackCount = 0;
        }
    }
    private void attackEnd()
    {
        dragon_slayer.DisableSwordCollider();
        movementState = MovementState.attackingIdle;
        movable = true;
        canBlock = true;
        OnEnable();
    }
    private void afterAttackIdle()
    {
        if (rememberAttack)
        {
            attack();
            rememberAttack = false;
            return;
        }
        else
        {
            moveInput = Inputs.guts.move_character.ReadValue<Vector2>();
            move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            if (move != Vector3.zero && movable)
            {
                movementState = MovementState.running;
                controls.Move(move * speed * Time.fixedDeltaTime);
                anim.Play("run");
                AttackCount = 0;
            }
        }
    }
    private void block_performed()
    {
        if (!canBlock) return;
        UnityEngine.Debug.Log("Blocking");
        movementState = MovementState.blocking;
        AttackCount = 0;
        movable = false;
        animation_player("block");
    }
    private void block_canceled()
    {
        if (!canBlock) return;
        movementState = MovementState.idle;
        movable = true;
        animation_player("idle");
    }
    private void armcannon()
    {
        if (CannonAmmo != 0)
        {
            anim.Play("armcannon");
            CannonAmmo--;
            AttackCount = 0;
        }
    }
    private void dodge()
    {
        if (!movable) return;
        moveInput = Inputs.guts.move_character.ReadValue<Vector2>();
        move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        characterDirection = Vector3.forward * moveInput.y + Vector3.right * moveInput.x;
        if (characterDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(characterDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.fixedDeltaTime);
        }
        movementState = MovementState.dodging;
        movable = false;
        canBlock = false;
        animation_player("dodge");
        OnDisable();
        AttackCount = 0;
    }
    private void dodgeEnd()
    {
        movable = true;
        canBlock = true;
        animation_player("idle");
        OnEnable();
        movementState = MovementState.idle;
    }
    private void CharacterRotaion()
    {
        characterDirection = Vector3.forward * moveInput.y + Vector3.right * moveInput.x;
        if (characterDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(characterDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.fixedDeltaTime);
        }
    }
    private void OnEnable()
    {
        Inputs.Enable();
    }

    private void OnDisable()
    {
        Inputs.Disable();
    }
}
