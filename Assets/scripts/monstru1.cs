using UnityEngine;

public class monstru1 : MonoBehaviour, IDamageable
{
    [Header("References")]
    public Transform player;
    private CharacterController controller;
    private Animator anim;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 360f;

    [Header("Combat")]
    public float attackRange = 2f;
    private float distance;
    public GameObject rightHand;
    public GameObject leftHand;
    private int health = 30;
    public float gravity = -9.81f;
    public float groundedForce = -2f; // îl ține lipit de sol
    private Vector3 velocity;
    private States state= States.chasing;
    enum States
    {
        idle,
        chasing,
        attacking,
        staggered,
        death
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        rightHand.SetActive(false);
        leftHand.SetActive(false);
        anim = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {

        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = groundedForce;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
        switch (state)
        {
            case States.idle:
                anim.Play("monster1_idle");
                break;
            case States.chasing:
                ChasePlayer();
                break;
            case States.attacking:
                AttackPlayer();
                AttackMovement();
                break;
            case States.staggered:
                break;
            case States.death:
                break;
        }
    }

    private void ChasePlayer()
    {
        distance = Vector3.Distance(transform.position, player.position);
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (dir == Vector3.zero) return;

        // rotație
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);

        // mișcare
        controller.Move(dir.normalized * moveSpeed * Time.deltaTime);

        anim.Play("monster1_run");
        if (distance <= attackRange)
        {
            state = States.attacking;
        }
    }

    private void AttackPlayer()
    {
        anim.Play("monster1_attack");
    }

    private void AttackMovement()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;

        float stepDistance = 3f;

        controller.Move(forward.normalized * stepDistance * Time.deltaTime);
    }

    private void attackEnd()
    {
        rightHand.SetActive(false);
        leftHand.SetActive(false);
        state = States.chasing;
    }
    private void attackDirection()
    {
        if (rightHand.activeSelf == false)
        {
            rightHand.SetActive(true);
            leftHand.SetActive(false);
        }
        else
        {
            leftHand.SetActive(true);
            rightHand.SetActive(false);
        }

        Vector3 dirToPlayer = player.position - transform.position;
        dirToPlayer.y = 0;
        if (dirToPlayer == Vector3.zero) return;
        Quaternion targetRot = Quaternion.LookRotation(dirToPlayer);
        float deltaY = Mathf.DeltaAngle(transform.eulerAngles.y, targetRot.eulerAngles.y);
        float clampedDeltaY = Mathf.Clamp(deltaY, -45f, 45f);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + clampedDeltaY, 0f);
    }
    public void TakeDamage(int damage)
    {
        if (state == States.death) return;
        health -= damage;
        anim.Play("monster1_stagger");
        if (health <= 0)
            Death();
        else
            state = States.staggered;

    }
    private void Staggered()
    {
        rightHand.SetActive(false);
        leftHand.SetActive(false);
        anim.Play("monster1_stagger");
    }
    private void staggeredEnd()
    {
        state = States.chasing;
    }
    private void Death()
    {
        if(state == States.death)
            return;
        state = States.death;
        rightHand.SetActive(false);
        leftHand.SetActive(false);
        anim.Play("monster1_death");
        GameEvents.OnEnemyKilled?.Invoke();
        Destroy(gameObject, 10f);
    }
    private void OnEnable()
    {
        GameEvents.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= OnPlayerDied;
    }
    private void OnPlayerDied()
    {
        if(state != States.death)
        state = States.idle;
    }
}
