using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum PlayerStates
    {
        Walk,
        Attack
    }

    private enum PlayerDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    [SerializeField] private List<GameObject> hearts;
    [SerializeField] private GameObject coinIcon;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float cooldownAttack = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int hitPoints;

    private PlayerInput playerInput;
    private Rigidbody2D rgbody2D;
    private Animator animator;
    private FlashAnimation flashAnimation;

    private InputAction move;
    private InputAction attack;
    private Vector2 movement;
    private PlayerStates currentState;
    private Vector2 lastMovement = new Vector2(0f, 1f);
    private bool hasCoin = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        rgbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flashAnimation = GetComponent<FlashAnimation>();

        move = playerInput.actions["Move"];
        attack = playerInput.actions["Attack"];

        currentState = PlayerStates.Walk;

        hitPoints = hearts.Count();

        if (!hasCoin)
        {
            coinIcon.SetActive(false);
        }
    }

    private void Update()
    {
        if (hasCoin)
        {
            coinIcon.SetActive(true);
        }

        if (hitPoints == 0)
        {
            GameController.Instance.AddDeath();
            GameController.Instance.SendToMenu();
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
        Debug.Log(lastMovement);

        rgbody2D.velocity = Vector2.zero;

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

        int x = Mathf.RoundToInt(lastMovement.x);
        int y = Mathf.RoundToInt(lastMovement.y);

        var playerDirection = GetPlayerDirection(x, y);
        direction = GetVector2Direction(playerDirection);
        projectileRotation = GetProjectileRotation(playerDirection);

        var vector = Vector2.zero;

        projectile.GetComponent<Rigidbody2D>().velocity += direction * 10;
        projectile.transform.Rotate(projectileRotation);
    }

    private Vector2 GetVector2Direction(PlayerDirection playerDirection)
    {
        switch (playerDirection)
        {
            case PlayerDirection.Down:
                return Vector2.down;
            case PlayerDirection.Right:
                return Vector2.right;
            case PlayerDirection.Left:
                return Vector2.left;
            default:
                return Vector2.up;
        }
    }

    private Vector3 GetProjectileRotation(PlayerDirection playerDirection)
    {
        switch (playerDirection)
        {
            case PlayerDirection.Down:
                return new Vector3(0f, 0f, -180f);
            case PlayerDirection.Right:
                return new Vector3(0f, 0f, -90f);
            case PlayerDirection.Left:
                return new Vector3(0f, 0f, 90f);
            default:
                return new Vector3(0f, 0f, 0f);
        }
    }

    private PlayerDirection GetPlayerDirection(int x, int y)
    {
        if (x == 1 && y == -1)
        {
            return PlayerDirection.Down;
        }
        else if (x == 1 && y == 1)
        {
            return PlayerDirection.Right;
        }
        else if (x == -1 && y == 1)
        {
            return PlayerDirection.Left;
        }
        else if (x == -1 && y == -1)
        {
            return PlayerDirection.Down;
        }
        else if (x == 1)
        {
            return PlayerDirection.Right;
        }
        else if (y == 1)
        {
            return PlayerDirection.Up;
        }
        else if (x == -1)
        {
            return PlayerDirection.Left;
        }
        else if (y == -1)
        {
            return PlayerDirection.Down;
        }
        else
        {
            return PlayerDirection.Up;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerStates.Walk)
            rgbody2D.MovePosition(rgbody2D.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void LoseHitPoints()
    {
        hitPoints -= 1;

        if (hearts.Count() > 0)
        {
            flashAnimation.Flash();
            var heart = hearts.Last();
            hearts.Remove(heart);
            Destroy(heart);
        }
    }

    public bool SetHasCoin(bool has) => hasCoin = has;
    public bool GetHasCoin() => hasCoin;
}

