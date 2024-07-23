using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region 魔法用の変数

    //魔法発動時に相手に飛ばすItem
    [SerializeField] private GameObject _magicItemPrefab;

    //生成される場所
    [SerializeField] private Transform _magicSpawnPos;

    //相手に向かうスピード
    [SerializeField] private float _magicSpeed = 10f;

    //魔法のクールタイム
    [SerializeField] private float _coolTime;

    #endregion

    #region 敵判定用の変数

    //配列の容量
    private static readonly int _capacity = 100;

    //円の半径
    private float _radius = 5f;

    //対象内のコライダーをリストに格納
    private readonly Collider[] _buffer = new Collider[_capacity];

    #endregion

    private void Update()
    {
        //coolTimeを計算
        _coolTime += Time.deltaTime;
        //Spaceが押されたときにクールタイムが3以上であれば
        if (Input.GetKeyDown(KeyCode.Space) && _coolTime >= 3)
        {
            _coolTime = 0;
            CastMagic();
        }
    }

    private void CastMagic()
    {
        //魔法のプレハブを生成
        var magic = Instantiate(_magicItemPrefab, _magicSpawnPos.position, Quaternion.identity);
        //魔法に力を加えて発射
        var rb = magic.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = _magicSpawnPos.forward * _magicSpeed;
        }
    }

    private int _hitCount;

    //コライダーの検索
    private void CheckCollision()
    {
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _buffer);
        var nearestCollider = FindNearestCollider(_buffer,hitCount);
        
        // for (var i = 0; i < hitCount; i++)
        // {
        //     var collider = _buffer[i];
        // }
    }
    
    // 自身の座標から最も近いコライダーを返す。
    // 範囲内にコライダーが存在しない場合nullを返す。
    private Collider FindNearestCollider(Collider[] colliders, int hitCount)
    {
        var minDistance = float.MaxValue;
        Collider result = null;

        for (int i = 0; i < hitCount; i++)
        {
            var collider = colliders[i];
            if (result == null)
            {
                result = collider;
                continue;
            }

            var collPos = collider.transform.position;
            var sqrDistance = Vector3.SqrMagnitude(collPos - transform.position);

            if (sqrDistance < minDistance)
            {
                minDistance = sqrDistance;
                result = collider;
            }
        }

        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _buffer);
        if (hitCount == 0)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawSphere(transform.position, _radius);
    }
#endif
}