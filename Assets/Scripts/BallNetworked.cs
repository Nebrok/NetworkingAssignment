using System.Threading;
using Unity.Netcode;
using UnityEngine;

public class BallNetworked : NetworkBehaviour
{
    //Syncing stuff
    public NetworkVariable<bool> Network_pickedUp = new(false);
    public NetworkObject Network_carrier = null;


    private bool _pickedUp = false;
    private GameObject _carrier = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        if (Network_pickedUp.Value)
        {
            Debug.Log("Carried by" + Network_carrier);
            Vector3 newPos = Network_carrier.gameObject.transform.position;
            newPos.y += 2;
            transform.position = newPos;
        }

        Debug.Log(PickedUp);
    }

    



    //----------------------
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

}
