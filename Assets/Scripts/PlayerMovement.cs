using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _jumpPeakTime = .5f;
    [SerializeField] private float _jumpFallTime = .5f;
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _jumpDistance = 4.0f;

    private float _speed = 5.0f;
    private float _jumpVelocity = 1f;
    private float deceleration = 5.0f;
    private Rigidbody2D _player;

    private float _jumpGravity = Physics.gravity.y;
    private float _fallGravity;

    private float groundCheckDistance = 0.01f;
    private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Rigidbody2D>();
    }

    private void Awake() {
        CalculateMovementParameters();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOnFloor()) {
            if (_player.velocity.y > 0) {
                _player.velocity -= new Vector2(0, _jumpGravity * Time.deltaTime);
            } else {
                _player.velocity -= new Vector2(0, _fallGravity * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.A))
            _player.velocity = new Vector2(-1 * _speed, _player.velocity.y);
        if (Input.GetKey(KeyCode.D))
            _player.velocity = new Vector2(1 * _speed, _player.velocity.y);
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && IsOnFloor())
            _player.velocity = new Vector2(_player.velocity.x, _jumpVelocity);

        _player.velocity = new Vector2(Mathf.Lerp(_player.velocity.x, 0, deceleration * Time.deltaTime), _player.velocity.y);
    }

    private void CalculateMovementParameters() {
        _jumpGravity = (2 * _jumpHeight) / Mathf.Pow(_jumpPeakTime, 2);
        _fallGravity = (2 * _jumpHeight) / Mathf.Pow(_jumpFallTime, 2);
        _jumpVelocity = _jumpGravity * _jumpPeakTime;
        _speed = _jumpDistance / (_jumpPeakTime + _jumpFallTime);
    }

    private bool IsOnFloor() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);
        return hit.collider != null; 
    }
}
