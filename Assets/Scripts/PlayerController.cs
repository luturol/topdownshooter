using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;

    private PlayerInput playerInput;
    private Rigidbody2D rgbody2D;

    private InputAction move;
    private InputAction attack;
    private Vector2 movement;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rgbody2D = GetComponent<Rigidbody2D>();

        move = playerInput.actions["Move"];
        attack = playerInput.actions["Attack"];
    }

    private void Update()
    {
        movement = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rgbody2D.MovePosition(rgbody2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
