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
    private bool _doubleJumping;

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
            _player.velocity = new Vector2(Mathf.Lerp(_player.velocity.x, 0, deceleration * Time.deltaTime), _player.velocity.y);
        }
    }

    private void HandleJump() {
        if (IsOnFloor()) {
            _canDoubleJump = true;
            _doubleJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (IsOnFloor()) {
                // First jump
                _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);
                _doubleJumping = false;
            } else if (_canDoubleJump) {
                // Double jump
                _doubleJumping = true;
                _canDoubleJump = false;
                _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);
            }
        }
    }

    private void UpdateAnimations() {
        Animator animator = GetComponentInChildren<Animator>();

        if (IsOnFloor()) {
            animator.SetBool("Jumping", false);
            animator.SetBool("DoubleJumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("Running", Mathf.Abs(_player.velocity.x) > 0.1f);
        } else {
            animator.SetBool("Running", false);

            if (_player.velocity.y > 0) {
                animator.SetBool("Falling", false);
                animator.SetBool("DoubleJumping", _doubleJumping);
                animator.SetBool("Jumping", !_doubleJumping);
            } else if (_player.velocity.y <= 0) {
                animator.SetBool("Jumping", false);
                animator.SetBool("DoubleJumping", false);
                animator.SetBool("Falling", true);
            }
        }
    }


    private bool IsOnFloor() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, .01f);
        return hit.collider != null;
    }
}
