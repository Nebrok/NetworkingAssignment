using UnityEngine;
using Unity.Netcode;

public class ReactionObject : NetworkBehaviour
{
    private float _lifeTime = 0f;
    [SerializeField] float _totalLifetime = 2f;
    [SerializeField] float _finalSizeIncrease = 2f;
    [SerializeField] float _riseHeight = 3f;

    private Vector3 _startPos;
    private Vector3 _startScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (_lifeTime > _totalLifetime)
        {
            Destroy(gameObject);
        }


        float step = _lifeTime / _totalLifetime;

        Vector3 newPos = _startPos;
        newPos.y += step * _riseHeight;
        transform.position = newPos;

        transform.localScale = _startScale * (1 + (_finalSizeIncrease - 1) * step);


        _lifeTime += Time.deltaTime;


    }
}
