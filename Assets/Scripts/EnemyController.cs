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

        animator.SetFloat("Speed", currentState == EnemyState.Chasing ? speed : 0);

        if (lastMovement != Vector2.zero)
        {
            animator.SetFloat("Last Move Horizontal", lastMovement.x);
            animator.SetFloat("Last Move Vertical", lastMovement.y);
        }

        direction = target.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();

        movement = direction;

        if (shouldRotate)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
        }

        if(currentState == EnemyState.Attack && player != null && !isInCooldownAttack)
        {
            StartCoroutine(AttackCo(player));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {                
        currentState = EnemyState.Chasing;
    }

    private void OnTriggerExit2D(Collider2D other)
    {        
        currentState = EnemyState.Idle;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentState = EnemyState.Attack;            
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
        currentState = EnemyState.Idle;
    }

    private void FixedUpdate()
    {
        if (currentState == EnemyState.Chasing)
        {
            rgbody2D.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
            lastMovement = movement;
        }

        if (currentState == EnemyState.Attack)
        {
            rgbody2D.velocity = Vector2.zero;
        }
    }

    public void LoseHitPoints() => hitPoints -= 1;
}
