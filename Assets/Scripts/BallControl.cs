using Unity.Netcode;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    [SerializeField] float _interactionRadius = 2f;
    [SerializeField] float _throwForce = 10f;

    private GameObject _ball = null;
    private bool _holdingBall = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_ball != null)
        {
            Vector3 newPos = transform.position;
            newPos.y += 2;
            //_ball.transform.position = newPos;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NetworkPickUpBall();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowBall();
        }
    }

    GameObject CheckBallNearby()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactionRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Ball"))
            {
                return colliders[i].gameObject;
            }
        }
        return null;
    }

    private bool CheckBallFree(GameObject ball)
    {
        BallNetworked ballClass = ball.GetComponent<BallNetworked>();
        if (ballClass.PickedUp)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool NetworkPickUpBall()
    {
        GameObject ball = CheckBallNearby();
        if (ball == null)
        {
            return false;
        }
        if (!CheckBallFree(ball))
        {
            return false;
        }

        BallNetworked ballClass = ball.GetComponent<BallNetworked>();
        NetworkObject player = GetComponent<NetworkObject>();
        _ball = ball;
        _holdingBall = true;
        ballClass.SetPickUpStateRpc(true);
        ulong foo = GetComponent<NetworkObject>().OwnerClientId;
        Debug.Log(foo);
        ballClass.SetCarrierRpc(GetComponent<NetworkObject>().OwnerClientId);

        return false;
    }

    private void ThrowBall()
    {
        if (_ball == null)
        {
            return;
        }
        BallNetworked ballClass = _ball.GetComponent<BallNetworked>();
        Vector3 throwDir = new Vector3(0, 0.5f, 0);
        throwDir += transform.forward;
        throwDir.Normalize();

        ballClass.AddVelocityRpc(throwDir * _throwForce);
        ballClass.SetPickUpStateRpc(false);
        _holdingBall = false;
        _ball = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }

}
