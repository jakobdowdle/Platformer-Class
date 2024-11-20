using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _jumpVelocity = 7f;
    [SerializeField] private float _wallSlideSpeed = 2f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2f;
    private float deceleration = 5.0f;
    private Rigidbody2D _player;

    private bool _canDoubleJump;
    private bool _doubleJumping;
    private bool _isWallSliding;

    private bool _isDamaged = false;

    void Start() {
        _player = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (!GameManager.Instance.GameRunning) {
            _player.velocity = Vector2.zero;
            return;
        }
        HandleDamageState();
        HandleMovement();
        HandleJump();
        HandleWallSlide();
        UpdateAnimations();
        ApplyFallMultiplier();
    }

    private void HandleDamageState() {
        if (_isDamaged) {
            StartCoroutine(PlayDamageSequence());
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hazard") && !_isDamaged) {
            _isDamaged = true;
            PlayerBehaviour.Instance.Die();
        }
    }

    private IEnumerator PlayDamageSequence() {
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("IsDamaged", true);

        ResetAnimator(animator); // Clear other animation states
        yield return new WaitForSeconds(1f); // Wait for damage animation to play

        _isDamaged = false;
        animator.SetBool("IsDamaged", false);
    }

    private void ResetAnimator(Animator animator) {
        foreach (AnimatorControllerParameter parameter in animator.parameters) {
            if (parameter.type == AnimatorControllerParameterType.Bool) {
                animator.SetBool(parameter.name, false);
            }
        }
    }

    private void ApplyFallMultiplier() {
        if (_player.velocity.y < 0) {
            _player.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        } else if (_player.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            _player.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }
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
                _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);
                _doubleJumping = false;
            } else if (_isWallSliding) {
                _player.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * _speed, _jumpVelocity);
                _isWallSliding = false;
                _doubleJumping = true;
                _canDoubleJump = false;
            } else if (_canDoubleJump) {
                _doubleJumping = true;
                _canDoubleJump = false;
                _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);
            }
        }
    }

    private void HandleWallSlide() {
        _isWallSliding = IsTouchingWall() && !IsOnFloor() && _player.velocity.y < 0;

        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("WallSliding", _isWallSliding);

        if (_isWallSliding) {
            _player.velocity = new Vector2(_player.velocity.x, Mathf.Max(_player.velocity.y, -_wallSlideSpeed));
        }
    }

    private void UpdateAnimations() {
        Animator animator = GetComponentInChildren<Animator>();

        if (_isDamaged) return;

        if (IsOnFloor()) {
            animator.SetBool("Jumping", false);
            animator.SetBool("DoubleJumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("Running", Mathf.Abs(_player.velocity.x) > 0.1f);
            animator.SetBool("WallSliding", false);
        } else {
            animator.SetBool("Running", false);

            if (_isWallSliding) {
                animator.SetBool("WallSliding", true);
                animator.SetBool("Jumping", false);
                animator.SetBool("DoubleJumping", false);
                animator.SetBool("Falling", false);
            } else {
                animator.SetBool("WallSliding", false);

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
    }

    private bool IsOnFloor() {
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y + 0.17f);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, .01f);
        return hit.collider != null;
    }

    private bool IsTouchingWall() {
        float wallCheckDistance = 0.3f;
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        Vector2 raycastOriginTop = new Vector2(transform.position.x, transform.position.y + 0.62f);
        Vector2 raycastOriginBottom = new Vector2(transform.position.x, transform.position.y + 0.17f);

        RaycastHit2D hitTop = Physics2D.Raycast(raycastOriginTop, direction, wallCheckDistance);
        RaycastHit2D hitBottom = Physics2D.Raycast(raycastOriginBottom, direction, wallCheckDistance);

        return hitTop.collider != null || hitBottom.collider != null;
    }
}
