using UnityEngine;
using Unity.Netcode;

public class ReactionManager : NetworkBehaviour
{
    [SerializeField] private GameObject _reaction;

    [Rpc(SendTo.Server)]
    public void SpawnReactionRpc(Vector3 newPos)
    {
        GameObject newInstance = Instantiate(_reaction, newPos, Quaternion.identity);
        NetworkObject newInstanceNO = newInstance.GetComponent<NetworkObject>();
        newInstanceNO.Spawn();
    }
}
