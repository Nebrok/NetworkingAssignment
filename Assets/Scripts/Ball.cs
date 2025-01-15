using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Ball : NetworkBehaviour
{

    private enum Direction
    {   
        UP, DOWN, LEFT, RIGHT, FORWARD, BACKWARD
    }   

    private Direction _direction;

    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _skinThickness = 0.01f;

    private float _radius;

    private bool _pickedUp;
    private GameObject _carrier = null;

    [SerializeField]
    private Vector3 _velocity;

    private void Awake()
    {
        _radius = transform.localScale.y / 2;
        _pickedUp = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckSurrounds();
    }

    
    void FixedUpdate()
    {
        if (!IsServer)
        {
            return;
        }

        if (_pickedUp)
        {
            Debug.Log("Carried by" + _carrier);
            Vector3 newPos = _carrier.transform.position;
            newPos.y += 2;
            transform.position = newPos;
        }


        _velocity += new Vector3(0, _gravity * Time.fixedDeltaTime, 0);
        if (IsGrounded() || _pickedUp)
        {
            _velocity.y = 0;
        }
        
        transform.position += _velocity * Time.fixedDeltaTime;



        if (Input.GetKeyDown(KeyCode.R))
        {
            _velocity *= 0;
            transform.position = new Vector3(0, 4, 0);
        }
    }


    public void AddVelocity(Vector3 velocity)
    {
        _velocity += velocity;
    }

    public bool PickedUp
    {
        get { return _pickedUp; }
        set
        {
            _pickedUp = value;
        }
    }

    public GameObject Carrier
    {
        get { return _carrier; }
        set { _carrier = value; }
    }

    private void CheckSurrounds()
    {
        for (int i = 0; i < 6; i++)
        {
            CheckObstructed((Direction)i);
        }
    }

    bool CheckObstructed(Direction direction)
    {
        Vector3 dir;
        switch (direction)
        {
            case Direction.UP:
                dir = new Vector3(0, 1, 0);
                break;
            case Direction.DOWN:
                dir = new Vector3(0, -1, 0);
                break;
            case Direction.LEFT:
                dir = new Vector3(-1, 0, 0);
                break;
            case Direction.RIGHT:
                dir = new Vector3(1, 0, 0);
                break;
            case Direction.FORWARD:
                dir = new Vector3(0, 0, 1);
                break;
            case Direction.BACKWARD:
                dir = new Vector3(0, 0, -1);
                break;
            default:
                Debug.Log("Invalid Direction in obstruction check");
                return false;
        }


        if (Physics.Raycast(transform.position, dir, _radius + _skinThickness))
        {
            return true;
        }

        return false;
    }

    bool IsGrounded()
    {
        return CheckObstructed(Direction.DOWN);
    }
}
