using UnityEngine;
using UnityEngine.UI;

public class gutadinberbec : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float speed = 10f;
    private int health = 50;
    private Vector3 move;
    private Vector3 actionMove;
    private Vector2 moveInput;
    private CharacterController controls;
    public Controls Inputs;
    public Transform cam;
    private Animator anim;
    private bool movable = true;
    private bool canBlock = true;
    private bool rememberAttack = false;
    private int AttackCount = 0;
    private string animationName = "idle";
    private string currentAnimation = "";
    private dragon_slayer dragon_slayer;
    private Vector3 characterDirection;
    private int CannonAmmo = 1;
    public float gravity = -9.81f;
    public float groundedForce = -2f; // îl ține lipit de sol
    private Vector3 velocity;
    private bool canTakeDamage = true;
    public audioMaster audioMaster;
    public Slider slider;
    private MovementState movementState = 0;
    enum MovementState
    {
        idle,
        running,
        attacking,
        attackingIdle,
        blocking,
        dodging,
        armcannon,
        staggered,
        death
    }

    private void Awake()
    {
        Inputs = new Controls();
        anim = GetComponent<Animator>();
        controls = GetComponent<CharacterController>();
        dragon_slayer = FindAnyObjectByType<dragon_slayer>();
        Inputs.guts.attack.performed += ctx => attack();
        Inputs.guts.block.performed += ctx => block_performed();
        Inputs.guts.block.canceled += ctx => block_canceled();
        Inputs.guts.armcannon.performed += ctx => armcannon();
        Inputs.guts.dodge.performed += ctx => dodge();
        audioMaster=GameObject.FindGameObjectWithTag("audio").GetComponent<audioMaster>();
        slider.maxValue = health;
        slider.value = health;
        slider.interactable = false;
    }

    private void Update()
    {
        if (controls.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = groundedForce;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controls.Move(velocity * Time.deltaTime);
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
            case MovementState.staggered:
                // Staggered logic if needed
                break;
            case MovementState.death:
                // Death logic if needed
                break;
        }
    }
    private void movementInput()
    {
        moveInput = Inputs.guts.move_character.ReadValue<Vector2>();
        move =Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * new Vector3(moveInput.x, 0, moveInput.y).normalized;
        if (move != Vector3.zero && movable)
        {
            movementState = MovementState.running;
            controls.Move(move * speed * Time.deltaTime);
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
                controls.Move(actionMove * speed * 0.4f * Time.deltaTime);
                break;
            case "attack2":
                CharacterRotaion();
                actionMove = transform.TransformDirection(Vector3.forward);
                controls.Move(actionMove * speed * 0.4f * Time.deltaTime);
                break;
            case "attack3":
                CharacterRotaion();
                actionMove = transform.TransformDirection(Vector3.forward);
                controls.Move(actionMove * speed * 0.4f * Time.deltaTime);
                break;
            case "dodge":
                actionMove = transform.TransformDirection(Vector3.forward);
                controls.Move(actionMove * speed * 1.5f * Time.deltaTime);
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
    private void playSteps()
    {audioMaster.playSound(audioMaster.steps);}
    private void attack()
    {
        if (movementState == MovementState.attacking)
        {
            rememberAttack = true;
            return;
        }
        audioMaster.playSound(audioMaster.sword);
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
                controls.Move(move * speed * Time.deltaTime);
                anim.Play("run");
                AttackCount = 0;
            }
        }
    }
    private void block_performed()
    {
        if (!canBlock) return;
        canTakeDamage = false;
        moveInput = Inputs.guts.move_character.ReadValue<Vector2>();
        CharacterRotaion();
        movementState = MovementState.blocking;
        AttackCount = 0;
        movable = false;
        animation_player("block");
    }
    private void block_canceled()
    {
        if (!canBlock) return;
        canTakeDamage = true;
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
        if (movementState == MovementState.dodging || movementState == MovementState.attacking || movementState == MovementState.blocking) return;
        canTakeDamage = false;
        CharacterRotaion();
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
        canTakeDamage = true;
        movementState = MovementState.idle;
    }
    private void CharacterRotaion()
    {
        characterDirection =Quaternion.Euler(0f, cam.eulerAngles.y, 0f)* (Vector3.forward * moveInput.y + Vector3.right * moveInput.x);
        if (characterDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(characterDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Mathf.Infinity);
        }
    }
    public void TakeDamage(int damage)
    {
        if (!canTakeDamage) return;
        health -= damage;
        slider.value -= damage;
        if (health <= 0)
        {
            Death();
        }
        else
        {
            Stagger();
        }
        movementState = MovementState.staggered;
    }
    private void Stagger()
    {
        anim.Play("stagger");
        dragon_slayer.DisableSwordCollider();
        movable = false;
        canBlock = false;
        OnDisable();
        canTakeDamage = false;
    }
    private void StaggerEnd()
    {
        movable = true;
        canBlock = true;
        animation_player("idle");
        OnEnable();
        canTakeDamage = true;
        movementState = MovementState.idle;
    }
    private void Death()
    {
        canTakeDamage = false;
        movementState = MovementState.death;
        anim.Play("death");
        movable = false;
        canBlock = false;
        OnDisable();
        GameEvents.OnPlayerDied?.Invoke();
    }

    public void OnEnable()
    {
        Inputs.Enable();
    }

    public void OnDisable()
    {
        Inputs.Disable();
    }
}
