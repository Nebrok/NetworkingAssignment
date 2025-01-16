using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PointSystemNetworked : NetworkBehaviour
{
    private NetworkVariable<int> Network_teamBluePoints = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> Network_teamRedPoints = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private TMP_Text _blueText;
    [SerializeField] private TMP_Text _redText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Rpc(SendTo.Server)]
    public void IncrementBlueScoreRpc()
    {
        Network_teamBluePoints.Value += 1;
        _blueText.text = Network_teamBluePoints.Value.ToString();
    }

    [Rpc(SendTo.Server)]
    public void IncrementRedScoreRpc()
    {
        Network_teamRedPoints.Value += 1;
        _redText.text = Network_teamRedPoints.Value.ToString();
    }

}
