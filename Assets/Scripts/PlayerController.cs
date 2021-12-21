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

    private PlayerInput playerInput;
    private Rigidbody2D rgbody2D;
    private Animator animator;

    private InputAction move;
    private InputAction attack;
    private Vector2 movement;
    private PlayerStates currentState;
    private Vector2 lastMovement;

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
        if (movement != Vector2.zero)
        {
            lastMovement = movement;
        }
        else
        {
            animator.SetFloat("Last Move Horizontal", lastMovement.x);
            animator.SetFloat("Last Move Vertical", lastMovement.y);
        }

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
    }

    private IEnumerator AttackCoroutine()
    {
        animator.SetBool("Attack", true);
        animator.SetFloat("Last Move Horizontal", lastMovement.x);
        animator.SetFloat("Last Move Vertical", lastMovement.y);
        yield return null;

        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(0.33f);
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerStates.Walk)
            rgbody2D.MovePosition(rgbody2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

