using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkBridge : NetworkBehaviour
{
    [SerializeField] PlayerController _controller;
    [SerializeField] BallControl _ballControl;

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

        _controller.enabled = true;
        _ballControl.enabled = true;

    }

    void Update()
    {
        
    }
}
