using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float cooldownAttack = 2f;

    private PlayerInput playerInput;
    private Rigidbody2D rgbody2D;
    private Animator animator;

    private InputAction move;
    private InputAction attack;
    private Vector2 movement;
    private float waitingTime;
    private bool isWaitingAttackCooldown = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rgbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        move = playerInput.actions["Move"];
        attack = playerInput.actions["Attack"];
    }

    private void Update()
    {
        movement = move.ReadValue<Vector2>();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        bool isAttacking = attack.ReadValue<float>() == 1;
        if(!isWaitingAttackCooldown && isAttacking)
        {
            isWaitingAttackCooldown = true;
            animator.SetBool("Attack", isAttacking);
        }

        if (isWaitingAttackCooldown)
        {
            waitingTime += Time.deltaTime;

            if(waitingTime >= cooldownAttack)            
            {
                waitingTime = 0f;
                isWaitingAttackCooldown = false;                
            }
        }

    }

    private void FixedUpdate()
    {
        rgbody2D.MovePosition(rgbody2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
