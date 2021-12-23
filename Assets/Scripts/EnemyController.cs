using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chasing,
        Attack,
        Dead
    }

    [SerializeField] private float speed;
    [SerializeField] private bool shouldRotate;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float attackCouldown = 3f;
    [SerializeField] private int hitPoints = 2;

    private Transform target;
    private Animator animator;
    private Rigidbody2D rgbody2D;
    private Vector2 movement;
    private Vector2 lastMovement;
    private bool isInChaseRange;
    private bool isInAttackRange;
    private EnemyState currentState;
    private PlayerController player;
    private bool isInCooldownAttack = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rgbody2D = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        currentState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints == 0)
        {
            currentState = EnemyState.Dead;
            animator.SetTrigger("Die");
            Destroy(gameObject, 0.5f);
        }

        animator.SetFloat("Speed", isInChaseRange ? speed : 0);

        if (lastMovement != Vector2.zero)
        {
            animator.SetFloat("Last Move Horizontal", lastMovement.x);
            animator.SetFloat("Last Move Vertical", lastMovement.y);
        }

        // isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        // isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

        direction = target.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();

        movement = direction;

        if (shouldRotate)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
        }

        if(isInAttackRange && player != null && !isInCooldownAttack)
        {
            StartCoroutine(AttackCo(player));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        isInChaseRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInChaseRange = false;     
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInAttackRange = true;     
            player = other.gameObject.GetComponent<PlayerController>();       
        }
    }

    private IEnumerator AttackCo(PlayerController playerController)
    {
        isInCooldownAttack = true;
        Debug.Log("Atacou");
        playerController.LoseHitPoints();

        yield return new WaitForSeconds(3f);
        isInCooldownAttack = false;
        Debug.Log("Terminou o ataque");
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isInAttackRange = false;
    }

    private void FixedUpdate()
    {
        Debug.Log("Is in chase range: " + isInChaseRange);
        Debug.Log("Is in attack range: " + isInAttackRange);
        if (isInChaseRange && !isInAttackRange)
        {
            rgbody2D.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
            lastMovement = movement;
        }

        if (isInAttackRange)
        {
            rgbody2D.velocity = Vector2.zero;
        }
    }

    public void LoseHitPoints() => hitPoints -= 1;
}
