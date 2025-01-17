using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] string _team;
    [SerializeField] PointSystemNetworked _points;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            BallNetworked ball = other.gameObject.GetComponent<BallNetworked>();
            if (ball.LastThrown == _team && _team == "Blue")
            {
                _points.IncrementBlueScoreRpc();
            }
            if (ball.LastThrown == _team && _team == "Red")
            {
                _points.IncrementRedScoreRpc();
            }
        }
    }

}
