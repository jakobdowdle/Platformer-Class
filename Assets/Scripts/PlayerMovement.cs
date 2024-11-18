using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _jumpVelocity = 7f;
    private float deceleration = 5.0f;
    private Rigidbody2D _player;

    private bool _canDoubleJump;

    void Start() {
        _player = GetComponent<Rigidbody2D>();
    }

    void Update() {
        HandleMovement();
        HandleJump();
        UpdateAnimations();
    }

    private void HandleMovement() {
        if (Input.GetKey(KeyCode.A)) {
            _player.velocity = new Vector2(-_speed, _player.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (Input.GetKey(KeyCode.D)) {
            _player.velocity = new Vector2(_speed, _player.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            // Smoothly decelerate when no key is pressed
            _player.velocity = new Vector2(Mathf.Lerp(_player.velocity.x, 0, deceleration * Time.deltaTime), _player.velocity.y);
        }
    }

    private void HandleJump() {
        if (IsOnFloor()) {
            _canDoubleJump = true; // Reset double jump
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (IsOnFloor()) {
                // First jump
                _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);
            } else if (_canDoubleJump) {
                // Double jump
                _canDoubleJump = false;
                _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);
            }
        }
    }

    private void UpdateAnimations() {
        Animator animator = GetComponentInChildren<Animator>();

        if (IsOnFloor()) {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("Running", Mathf.Abs(_player.velocity.x) > 0.1f);
        } else {
            animator.SetBool("Running", false);
            animator.SetBool("Jumping", _player.velocity.y > 0);
            animator.SetBool("Falling", _player.velocity.y <= 0);
        }
    }


    private bool IsOnFloor() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, .01f);
        return hit.collider != null;
    }
}
