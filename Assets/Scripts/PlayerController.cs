using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum PlayerStates
    {
        Walk,
        Attack
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float cooldownAttack = 2f;
    [SerializeField] private GameObject projectilePrefab;

    private PlayerInput playerInput;
    private Rigidbody2D rgbody2D;
    private Animator animator;

    private InputAction move;
    private InputAction attack;
    private Vector2 movement;
    private PlayerStates currentState;
    private Vector2 lastMovement = new Vector2(0f, 1f);

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        rgbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        move = playerInput.actions["Move"];
        attack = playerInput.actions["Attack"];

        currentState = PlayerStates.Walk;
    }

    private void Update()
    {
        bool isAttacking = attack.ReadValue<float>() == 1;
        if (isAttacking && currentState != PlayerStates.Attack)
        {
            StartCoroutine(AttackCoroutine());
        }
        else if (currentState == PlayerStates.Walk)
        {
            movement = move.ReadValue<Vector2>();

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        if (movement != Vector2.zero)
        {
            lastMovement = movement;
        }
        else
        {
            animator.SetFloat("Last Move Horizontal", lastMovement.x);
            animator.SetFloat("Last Move Vertical", lastMovement.y);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        currentState = PlayerStates.Attack;

        animator.SetBool("Attack", true);
        animator.SetFloat("Last Move Horizontal", lastMovement.x);
        animator.SetFloat("Last Move Vertical", lastMovement.y);

        Shoot();
        yield return null;

        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(0.33f);

        currentState = PlayerStates.Walk;
    }

    private void Shoot()
    {
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 projectileRotation = Vector3.zero;
        Vector2 direction = Vector2.zero;

        if (lastMovement.x == 1)
        {
            direction = Vector2.right;
            projectileRotation = new Vector3(0f, 0f, -90f);
        }
        else if (lastMovement.y == 1)
        {
            projectileRotation = new Vector3(0f, 0f, 0f);
            direction = Vector2.up;
        }
        else if (lastMovement.x == -1)
        {
            projectileRotation = new Vector3(0f, 0f, 90f);
            direction = Vector2.left;
        }
        else if (lastMovement.y == -1)
        {
            projectileRotation = new Vector3(0f, 0f, -180f);
            direction = Vector2.down;
        }
        else
        {
            projectileRotation = new Vector3(0f, 0f, 0f);
            direction = Vector2.up;
        }

        projectile.GetComponent<Rigidbody2D>().velocity += direction * 10;
        projectile.transform.Rotate(projectileRotation);
    }


    private void FixedUpdate()
    {
        if (currentState == PlayerStates.Walk)
            rgbody2D.MovePosition(rgbody2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

