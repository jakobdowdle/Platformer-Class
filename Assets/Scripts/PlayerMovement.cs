using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _jumpVelocity = 7f;
    private float deceleration = 5.0f;
    private Rigidbody2D _player;

    private float _jumpGravity = Physics.gravity.y;
    private float _fallGravity;

    private float groundCheckDistance = 0.1f;
    private LayerMask groundLayer;

    private bool _canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A)) { 
            _player.velocity = new Vector2(-1 * _speed, _player.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetKey(KeyCode.D)) { 
            _player.velocity = new Vector2(1 * _speed, _player.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        }


        _player.velocity = new Vector2(Mathf.Lerp(_player.velocity.x, 0, deceleration * Time.deltaTime), _player.velocity.y);

        if (!IsOnFloor())
        {
            GetComponentInChildren<Animator>().SetBool("Running", false);
        }

        if (IsOnFloor()) {
            _canDoubleJump = false;
            GetComponentInChildren<Animator>().SetBool("Jumping", false);
            GetComponentInChildren<Animator>().SetBool("Falling", false);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                GetComponentInChildren<Animator>().SetBool("Running", true);
            }
            else
            {
                GetComponentInChildren<Animator>().SetBool("Running", false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_canDoubleJump && IsOnFloor())
            {
                _canDoubleJump = true;
                _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);
            }
            if (_canDoubleJump && !IsOnFloor())
            {
                _canDoubleJump = false;
                _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);
            }
        }

        if (_player.velocity.y > 0)
        {
            GetComponentInChildren<Animator>().SetBool("Jumping", true);
            GetComponentInChildren<Animator>().SetBool("Running", false);
        }
        if ((_player.velocity.y <= 0) && !IsOnFloor())
        {
            GetComponentInChildren<Animator>().SetBool("Jumping", false);
            GetComponentInChildren<Animator>().SetBool("Running", false);
            GetComponentInChildren<Animator>().SetBool("Falling", true);
        }

    }


    private bool IsOnFloor() {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Check if the player is falling (downward velocity) and on the ground
        bool isFalling = rb.velocity.y <= 0f; // Player is falling or stationary

        // Check if the player is touching the ground
        bool isTouchingGround = GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));

        return isFalling && isTouchingGround;
    }
}
