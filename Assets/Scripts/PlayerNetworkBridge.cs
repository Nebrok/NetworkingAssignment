using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkBridge : NetworkBehaviour
{
    [SerializeField] PlayerController _controller;
    [SerializeField] BallControl _ballControl;

    [SerializeField]
    private NetworkVariable<int> Network_playerNumber = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        _controller.enabled = false;
        _ballControl.enabled = false;
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();




        enabled = IsClient;
        if (!IsOwner)
        {
            enabled = false;
            _controller.enabled = false;
            _ballControl.enabled = false;
            return;
        }
        
        if (IsHost)
        {
            Network_playerNumber.Value = 1;
        }
        else
        {
            Network_playerNumber.Value = 2;
        }

        _controller.enabled = true;
        _ballControl.enabled = true;

    }

    void Update()
    {

    }

    public int PlayerNum
    {
        get { return Network_playerNumber.Value; }
    }
}
