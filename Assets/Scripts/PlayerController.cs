using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float SkinThickness = 10f;


    private Collider _coll;


    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    private bool _grounded = false;
    private float _playerSpeed = 4.0f;
    private float _jumpHeight = 1.0f;
    private float _gravityValue = -9.8f;

    private Vector3 _trackedVelocity;
    private Vector3 _prevPosition;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _prevPosition = transform.position;

        _coll = GetComponent<Collider>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(IsGrounded());

        if (Math.Abs(_trackedVelocity.y) <= 0)
        {
            _grounded = true;
        }


        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        move.Normalize();
        _characterController.Move(move * Time.deltaTime * _playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Makes the player jump
        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -2.0f * _gravityValue);
            Debug.Log(_playerVelocity.y);
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
        
        //update physics
        _trackedVelocity = transform.position - _prevPosition;
        _prevPosition = transform.position;
        //Debug.Log(_trackedVelocity.magnitude);
    }


    bool IsGrounded()
    {
        Vector3 dwn = new Vector3(0, -1, 0);

        if (Physics.Raycast(transform.position, dwn, SkinThickness))
        {
            return true;
        }
        return false;
    }
}
