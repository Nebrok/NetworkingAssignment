using UnityEngine;

public class BallControl : MonoBehaviour
{
    [SerializeField] float _interactionRadius = 2f;
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
            _ball.transform.position = newPos;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PickUpBall();
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
        Ball ballClass = _ball.GetComponent<Ball>();
        ballClass.AddVelocity(transform.forward * 3);
        ballClass.PickedUp = false;
        _holdingBall = false;
        _ball = null;
    }

}
