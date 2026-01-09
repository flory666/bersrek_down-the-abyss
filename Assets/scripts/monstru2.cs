using UnityEngine;

public class monstru2 : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private CharacterController controller;
    private Animator anim;
    private Vector3 dir;
    private float distance;
    private bool isPlayerDead = false;

    [Header("Movement")]
    [SerializeField]
    public float moveSpeed = 3f;
    [SerializeField]
    public float rotationSpeed = 45f;
    [SerializeField]
    private float attackRotationSpeed = 2f;

    [Header("Combat")]
    public GameObject hitBox;
    public GameObject attackbox;
    [SerializeField]
    private int health = 50;
    public float gravity = -9.81f;
    public float groundedForce = -2f; // îl ține lipit de sol
    private Vector3 velocity;
    [SerializeField]
    private States state= States.targeting;
    enum States
    {
        idle,
        targeting,
        attacking,
        staggered,
        death
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.detectCollisions = false;
        hitBox.SetActive(true);
        attackbox.SetActive(false);
        anim = GetComponent<Animator>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        state = States.targeting;
    }

    void Update()
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
                anim.Play("monster2_idle");
                break;
            case States.targeting:
                TargetPlayer();
                break;
            case States.attacking:
                attackDirection();
                break;
            case States.staggered:
                break;
            case States.death:
                Death();
                break;
        }
    }
    private void TargetPlayer()
    {
        dir = player.position - transform.position;
        dir.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
        if (Vector3.Cross(transform.forward, dir.normalized).y > 0)
            anim.Play("monster2_strife_right");
        else
            anim.Play("monster2_strife_left");
        if (Vector3.Angle(transform.forward, dir) < 5f)
            AttackPlayer();
    }
    private void AttackPlayer()
    {
        state = States.attacking;
        attackbox.SetActive(true);

    }
    private void attackDirection()
    {


        distance = Vector3.Distance(transform.position, player.position);
        dir = player.position - transform.position;
        dir.y = 0;

        if (dir == Vector3.zero) return;

        // rotație
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, attackRotationSpeed * Time.deltaTime);

        // mișcare
        controller.Move(transform.forward * moveSpeed * Time.deltaTime);
        anim.Play("monster2_attack");
    }
    public void Staggered()
    {
        state = States.staggered;
        anim.Play("monster2_stagger");
    }
    public void TakeDamage(int damage)
    {
        attackbox.SetActive(false);
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        else
        {
            Staggered();
        }
    }
    void staggerEnd()
    {
        state = States.targeting;
    }
    private void Death()
    {
        if (state == States.death)
            return;
        state = States.death;
        hitBox.SetActive(false);
        attackbox.SetActive(false);
        anim.Play("monster2_death");
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
