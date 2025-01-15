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
            NetworkPickUpBallRpc();
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
                Debug.Log("Ball Nearby");
                return colliders[i].gameObject;
            }
        }
        return null;
    }

    private bool CheckBallFree(GameObject ball)
    {
        Ball ballClass = ball.GetComponent<Ball>();
        if (ballClass.PickedUp)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    [Rpc(SendTo.Server)]
    private bool NetworkPickUpBallRpc()
    {
        Debug.Log("Hello");
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
        ballClass.Network_pickedUp.Value = true;
        ballClass.Network_carrier = player;
        //ballClass.PickedUp = true;
        //ballClass.Carrier = gameObject;

        return false;
    }



    private bool PickUpBall()
    {
        GameObject ball = CheckBallNearby();
        if (ball == null)
        {
            return false;
        }
        Ball ballClass = ball.GetComponent<Ball>();
        if (!CheckBallFree(ball))
        {
            return false;
        }
        _ball = ball;
        _holdingBall = true;
        
        ballClass.PickedUp = _holdingBall;

        return false;
    }

    private void ThrowBall()
    {
        if (_ball == null)
        {
            return;
        }
        Ball ballClass = _ball.GetComponent<Ball>();
        Vector3 throwDir = new Vector3(0, 0.5f, 0);
        throwDir += transform.forward;
        throwDir.Normalize();

        ballClass.AddVelocity(throwDir * _throwForce);
        ballClass.PickedUp = false;
        _holdingBall = false;
        _ball = null;
        Debug.Log("Thrown");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }

}
