using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private List<Transform> patrollingPoints;
    [SerializeField] private float speed;
    [SerializeField] private bool shouldRotate;
    [SerializeField] private Vector3 direction;

    private Transform target;
    private Animator animator;
    private Rigidbody2D rgbody2D;
    private Vector2 movement;
    private bool isInChaseRange;
    private bool isInAttackRange;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rgbody2D = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", speed);

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Teste");
        isInChaseRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInChaseRange = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isInAttackRange = true;
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
        }

        if (isInAttackRange)
        {
            rgbody2D.velocity = Vector2.zero;
        }
    }
}
