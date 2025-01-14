using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkBridge : NetworkBehaviour
{
    [SerializeField] PlayerController _controller;

    private void Awake()
    {
        _controller.enabled = false;
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        enabled = IsClient;
        if (!IsOwner)
        {
            enabled = false;
            _controller.enabled = false;
            return;
        }

        _controller.enabled = true;

    }

    void Update()
    {
        
    }
}
