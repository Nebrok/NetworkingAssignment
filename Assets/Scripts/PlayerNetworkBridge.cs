using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkBridge : NetworkBehaviour
{
    [SerializeField] PlayerController _controller;
    [SerializeField] BallControl _ballControl;
    [SerializeField] Material _player1mat;
    [SerializeField] Material _player2mat;

    [SerializeField]
    private NetworkVariable<int> Network_playerNumber = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> Network_spawnPos = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        _controller.enabled = false;
        _ballControl.enabled = false;
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Network_spawnPos.OnValueChanged += SetSpawn;

        enabled = IsClient;
        if (!IsOwner)
        {
            enabled = false;
            _controller.enabled = false;
            _ballControl.enabled = false;
            //return;
        }
        else
        {
            if (IsHost)
            {
                Network_playerNumber.Value = 1;
                Debug.Log("PLay1");
            }
            else
            {
                Debug.Log("PLay2");
                Network_playerNumber.Value = 2;
            }


            _controller.enabled = true;
            _ballControl.enabled = true;
        }
        SetPlayerColourAndLocation();

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        Network_spawnPos.OnValueChanged -= SetSpawn;

    }

    private void SetPlayerColourAndLocation()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (Network_playerNumber.Value % 2 == 0)
        {
            Material[] materials = new Material[1];
            materials[0] = _player2mat;
            meshRenderer.materials = materials;
        }
        if (Network_playerNumber.Value % 2 == 1)
        {
            Material[] materials = new Material[1];
            materials[0] = _player1mat;
            meshRenderer.materials = materials;
        }
    }

    private void SetSpawn(Vector3 oldValue, Vector3 newValue)
    {
        Debug.Log(newValue.x);
        transform.position = newValue;
    }

    public int PlayerNum
    {
        get { return Network_playerNumber.Value; }
    }
}
