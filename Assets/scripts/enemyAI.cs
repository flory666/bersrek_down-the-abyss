using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
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
    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    private float distance;
    public GameObject rightHand;
    public GameObject leftHand;
    private int health = 30;
    private States state;
    enum States
    {
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


        switch (state)
        {
            case States.chasing:
                ChasePlayer();
                break;
            case States.attacking:
                AttackPlayer();
                AttackMovement();
                break;
            case States.staggered:
                Staggered();
                break;
            case States.death:
                Death();
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
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            lookRot,
            rotationSpeed * Time.deltaTime
        );

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
        float deltaY = Mathf.DeltaAngle(
            transform.eulerAngles.y,
            targetRot.eulerAngles.y
        );
        float clampedDeltaY = Mathf.Clamp(deltaY, -45f, 45f);
        transform.rotation = Quaternion.Euler(
            0f,
            transform.eulerAngles.y + clampedDeltaY,
            0f
        );
    }
    public void TakeDamage(int damage)
    {
        if (state == States.death) return;
        health -= damage;
        UnityEngine.Debug.Log("Enemy took " + damage + " damage. Remaining health: " + health);
        anim.Play("monster1_stagger");
        if(health <= 0)
            state = States.death;
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
        rightHand.SetActive(false);
        leftHand.SetActive(false);
        anim.Play("monster1_death");
        Destroy(gameObject, 10f);
    }
}
