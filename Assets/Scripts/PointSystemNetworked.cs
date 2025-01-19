using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PointSystemNetworked : NetworkBehaviour
{
    private NetworkVariable<int> Network_teamBluePoints = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> Network_teamRedPoints = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private TMP_Text _blueText;
    [SerializeField] private TMP_Text _redText;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Network_teamBluePoints.OnValueChanged += UpdateBlueScore;
        Network_teamRedPoints.OnValueChanged += UpdateRedScore;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        Network_teamBluePoints.OnValueChanged -= UpdateBlueScore;
        Network_teamRedPoints.OnValueChanged -= UpdateRedScore;
    }

    [Rpc(SendTo.Server)]
    public void IncrementBlueScoreRpc()
    {
        Network_teamBluePoints.Value += 1;
        UpdateBlueScore(0, 0);
    }

    [Rpc(SendTo.Server)]
    public void IncrementRedScoreRpc()
    {
        Network_teamRedPoints.Value += 1;
        UpdateRedScore(0, 0);
    }

    private void UpdateBlueScore(int oldValue, int newValue)
    {
        _blueText.text = Network_teamBluePoints.Value.ToString();
    }

    private void UpdateRedScore(int oldValue, int newValue)
    {
        _redText.text = Network_teamRedPoints.Value.ToString();
    }

}
