using System;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _reaction;


    [SerializeField] float SkinThickness = 0.05f;
    private float _playerHeight;


    private Collider _coll;


    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    private bool _grounded = false;
    private float _playerSpeed = 16.0f;
    private float _jumpHeight = 1.0f;
    private float _gravityValue = -9.8f;

    private Vector3 _trackedVelocity;
    private Vector3 _prevPosition;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _prevPosition = transform.position;

        _coll = GetComponent<Collider>();
        _playerHeight = transform.localScale.y * 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            _playerVelocity.y = 0;
        }


        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move.magnitude > 1) move.Normalize();
        _characterController.Move(move * Time.deltaTime * _playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -2.0f * _gravityValue);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnReaction();
        }

        //Gravity
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
        
        //update physics
        _trackedVelocity = transform.position - _prevPosition;
        _prevPosition = transform.position;
    }

    private void SpawnReaction()
    {
        Vector3 newPos = transform.position;
        newPos += transform.right;
        ReactionManager reactionManager = FindAnyObjectByType<ReactionManager>();
        reactionManager.SpawnReactionRpc(newPos);
    }

    bool IsGrounded()
    {
        Vector3 dwn = new Vector3(0, -1, 0);

        if (Physics.Raycast(transform.position, dwn, _playerHeight / 2 + SkinThickness))
        {
            return true;
        }
        return false;
    }
}
