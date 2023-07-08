using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private int _startSpriteOrder;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _startSpriteOrder = _sprite.sortingOrder;
        _startSpeed = _speed;
    }
    void Update()
    {
        LockInLocker();

        if (Hiding)
            return;

        Walk();
        Jump();
        CheckingGround();
    }

    [SerializeField] private float _speed;
    private float _startSpeed;
    [SerializeField] private Slider _speedBar;
    private Vector2 _moveVector;
    void Walk()
    {
        _moveVector.x = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift) && _speedBar.value > 0.1f)
        {
            _speedBar.value -= Time.deltaTime;
            _speed += 0.01f;

            if (_speed > _startSpeed * 2)
                _speed = _startSpeed * 2;
        }

        else
        {
            _speed = _startSpeed;
            _speedBar.value += Time.deltaTime / 1.5f;
        }
        _rb.velocity = new Vector2(_moveVector.x * _speed, _rb.velocity.y);
    }

    [SerializeField] private float _jumpForce;
    private bool _jumpControl;
    private float _jumpIteration = 0;
    private float _jumpValueIteration = 60;
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_onGround) { _jumpControl = true; }

        }
        else { _jumpControl = false; }

        if (_jumpControl)
        {
            if (_jumpIteration++ < _jumpValueIteration)
            {
                _rb.AddForce(Vector2.up * _jumpForce / _jumpIteration);
            }
        }
        else { _jumpIteration = 0; }
    }

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _groundLayer;
    private bool _onGround;
    void CheckingGround()
    {
        _onGround = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundCheck.position, _checkRadius);
    }

    private bool _canGoInside;
    [HideInInspector] public bool Hiding;

    void LockInLocker()
    {
        if (_canGoInside && Input.GetKey(KeyCode.E))
        {
            _rb.velocity = Vector2.zero;
            _sprite.sortingOrder--;
            Physics2D.IgnoreLayerCollision(7, 8, true);
            Hiding = true;
        }
        else
        {
            _sprite.sortingOrder = _startSpriteOrder;
            Physics2D.IgnoreLayerCollision(7, 8, false);
            Hiding = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Locker")
        {
            _canGoInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Locker")
        {
            _canGoInside = false;
        }
    }
}
