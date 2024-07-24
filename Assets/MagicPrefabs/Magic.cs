using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _lifeTime　= 4;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        var enemy = other.gameObject.CompareTag("Enemy");

        if (enemy)
        {
            Debug.Log("衝突した");
        }

        Destroy(gameObject);
    }
}