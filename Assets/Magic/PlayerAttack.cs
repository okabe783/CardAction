using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region 魔法用の変数

    //発動する魔法のプレハブ
    [SerializeField] private GameObject _magicItemPrefab;

    //魔法が生成される場所
    [SerializeField] private Transform _magicSpawnPos;

    //相手に向かうスピード
    [SerializeField] private float _magicSpeed = 10f;

    //魔法のクールタイム
    [SerializeField] private float _coolTime;

    //魔法攻撃が可能であるか
    private bool _isAttack;

    #endregion

    #region 敵判定用の変数

    //配列の容量
    private static readonly int _capacity = 10;

    //敵の判定範囲の半径
    [SerializeField] private float _radius = 5f;

    //対象内のコライダーをリストに格納
    private readonly Collider[] _buffer = new Collider[_capacity];

    #endregion

    #region アニメーション

    private Animator _animator;
    private static readonly int Attack = Animator.StringToHash("Attack");

    #endregion

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        IsCheckAttack();

        //Spaceが押されたときにクールタイムが3秒以上であれば魔法を発射
        if (Input.GetKeyDown(KeyCode.Space) && _isAttack)
        {
            _isAttack = false;
            _coolTime = 0;
            //CastMagic();
            _animator.SetTrigger(Attack);
        }
    }

    //最も近い敵に向かって魔法を発動する
    private void CastMagic()
    {
        //最も近い敵を取得する
        var nearestEnemy = CheckCollision();
        //近くに敵がいる場合
        if (nearestEnemy != null)
        {
            //魔法のプレハブを生成
            var magic = Instantiate(_magicItemPrefab, _magicSpawnPos.position, Quaternion.identity);
            //魔法に力を加えて発射
            var rb = magic.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //方向ベクトルを求めてSpeedをかけて移動させる
                var direction = (nearestEnemy.bounds.center - _magicSpawnPos.position).normalized;
                rb.velocity = direction * _magicSpeed;
            }
        }
        else
        {
            //_animator.SetTrigger(Attack);
            Instantiate(_magicItemPrefab, _magicSpawnPos.position, Quaternion.identity);
        }
    }

    //コライダーの検索
    private Collider CheckCollision()
    {
        //範囲内のコライダーの数を取得
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _buffer);
        //一番近い敵を取得
        return FindNearestCollider(_buffer, hitCount);
    }

    // 自身の座標から最も近いコライダーを返す。
    // 範囲内にコライダーが存在しない場合nullを返す。
    private Collider FindNearestCollider(Collider[] colliders, int hitCount)
    {
        //一番近いコライダーを返す
        var minDistance = float.MaxValue;
        Collider result = null;

        for (var i = 0; i < hitCount; i++)
        {
            //範囲内のコライダーを検索
            var col = colliders[i];
            if (col.CompareTag("Enemy"))
            {
                //colのPositionを取得
                var colPos = col.transform.position;
                //自分と相手の距離を取得
                var sqrDistance = Vector3.SqrMagnitude(colPos - transform.position);
                if (!(sqrDistance < minDistance)) continue;
                //ソート
                minDistance = sqrDistance;
                //一番近い敵を設定
                result = col;
            }
        }

        return result;
    }
    
    //攻撃可能であるかどうかを判定する
    private void IsCheckAttack()
    {
        //魔法攻撃可能でない状態であればcoolTimeを更新する
        if (_isAttack == false)
            _coolTime += Time.deltaTime;
        //クールタイムがあがっていれば攻撃可能にする
        if (_coolTime >= 3)
            _isAttack = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Todo:前にのみ判定
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