using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;


public class BallNetworked : NetworkBehaviour
{   
    private enum Direction
    {
        UP, DOWN, LEFT, RIGHT, FORWARD, BACKWARD
    }


    //Syncing stuff
    private NetworkVariable<bool> Network_pickedUp = new(false);
    private NetworkObject Network_carrier = null;
    private NetworkVariable<ulong> Network_lastThrown = new();


    //local
    [SerializeField] float _skinThickness = 0.01f;
    private float _radius;
    private Vector3 _startPos;
    
    //ServerCopy Stuff
    private List<NetworkObject> _players = new List<NetworkObject>();
    private Vector3 _velocity = new Vector3(0, 0, 0);
    [SerializeField]
    private float _gravity = -9.81f;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _radius = transform.localScale.y / 2;
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        if (transform.position.y < -5)
        {
            transform.position = _startPos;
            _velocity = Vector3.zero;
        }

        if (Network_pickedUp.Value)
        {
            Vector3 newPos = Network_carrier.gameObject.transform.position;
            newPos.y += 2;
            transform.position = newPos;
        }

        _velocity += new Vector3(0, _gravity * Time.deltaTime, 0);
        if (IsGrounded() || PickedUp)
        {
            _velocity.y = 0;
        }
        if (!PickedUp) transform.position += _velocity * Time.deltaTime;

    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void SetPickUpStateRpc(bool newValue)
    {
        Network_pickedUp.Value = newValue;
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void SetCarrierRpc(ulong clientId)
    {
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].OwnerClientId == clientId)
            {
                Network_carrier = _players[i];
                Network_lastThrown.Value = clientId;
            }
        }
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void AddVelocityRpc(Vector3 newVelocity)
    {
        _velocity += newVelocity;
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("Client connected with ID: " + clientId);

        PlayerNetworkBridge[] players = FindObjectsByType<PlayerNetworkBridge>(FindObjectsSortMode.None);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].OwnerClientId == clientId)
            {
                _players.Add(players[i].gameObject.GetComponent<NetworkObject>());
            }
        }
    }

    public bool PickedUp
    {
        get { return Network_pickedUp.Value; }
    }

    public string LastThrown
    {
        get
        {
            int playerNum = 0;
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].OwnerClientId == Network_lastThrown.Value)
                {
                    playerNum = _players[i].gameObject.GetComponent<PlayerNetworkBridge>().PlayerNum;
                }
            }
            if (playerNum == 1)
            {
                return "Red";
            }
            else if (playerNum == 2)
            {
                return "Blue";
            }
            return null;
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
