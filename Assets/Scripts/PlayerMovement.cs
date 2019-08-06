using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10;
    private Vector3 _moveDirection;

    private Rigidbody _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        Player.Input.Keyboard.Move.performed += _ =>
        {
            Vector2 direction = _.ReadValue<Vector2>();
            _moveDirection = new Vector3(direction.x,0,direction.y);
        };
        Player.Input.Keyboard.Move.canceled += _ => _moveDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {      
        _rb.velocity = (_moveDirection*_moveSpeed);
    }
}