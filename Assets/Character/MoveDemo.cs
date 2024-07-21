using UnityEngine;

public class MoveDemo : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;
    
    //Playerの移動方向を示すベクトル
    private Vector3 _direction;
    
    //最新の位置を記録するためのベクトル
    private Vector3 _latestPos;
    
    //Playerの移動方向の倍率
    [SerializeField] private float _speedMagnification = 5.0f;
    
    #region HashAnimation

    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    #endregion

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        // 初期位置の記録
        _latestPos = transform.position;
    }

    private void Update()
    {
        // 入力から移動方向を取得
        var x = Input.GetAxisRaw("Horizontal");　//水平方向の入力
        var z = Input.GetAxisRaw("Vertical");　//垂直方向の入力
        //移動方向の正規化
        _direction = new Vector3(x, 0, z).normalized;
    }

    private void FixedUpdate()
    {
        // RigidBodyを使用して移動
        var velocity = _direction * _speedMagnification;
        //RigidBodyの速度を設定
        //ToDo:現在の速度と目標の速度をSlerp
        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);

        // 前フレームとの位置の差から進行方向を計算
        var differenceDis = new Vector3(transform.position.x, 0, transform.position.z) -
                                new Vector3(_latestPos.x, 0, _latestPos.z);
        //差が一定以上の場合、進行方向に回転
        if (differenceDis.sqrMagnitude > 0.0001f)
        {
            //進行方向を向く回転を計算
            var targetRotation = Quaternion.LookRotation(differenceDis);
            //現在の回転から目標の回転までをスムーズに補完
            targetRotation = Quaternion.Slerp(_rb.rotation, targetRotation, 0.2f);
            
            //RigidBodyの回転を更新
            _rb.MoveRotation(targetRotation);
        }
        
        _anim.SetFloat(MoveSpeed,_rb.velocity.magnitude);

        // 最新の位置を更新
        _latestPos = transform.position;
    }
    
    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger(Attack1);
        }
    }
}