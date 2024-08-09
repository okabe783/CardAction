using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;
    private PlayerAttack _playerAttack;

    #region キャラクターを動かす変数

//最新の位置を記録するためのベクトル
    private Vector3 _latestPos;

    //Playerの移動方向の倍率
    [SerializeField] private float _speed = 1.0f;
    private float _horizontal;
    private float _vertical;

    #endregion
    
    #region アニメーションを管理

    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

    #endregion

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _playerAttack = GetComponent<PlayerAttack>();
        // 初期位置の記録
        _latestPos = transform.position;
    }

    private void Update()
    {
        // 入力から移動方向を取得
        _horizontal = Input.GetAxisRaw("Horizontal");　//水平方向の入力
        _vertical = Input.GetAxisRaw("Vertical");　//垂直方向の入力
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    //カメラの向きに移動
    private void MoveCharacter()
    {
        if (!_playerAttack.IsMove)
        {
            //移動を無効化
            _rb.velocity = Vector3.zero;
            _anim.SetFloat(MoveSpeed, 0);
            return;
        }

        //カメラの方向から、X-Z平面の単位ベクトルを取得
        var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1).normalized);
        //方向キーの入力値をとカメラの向きから、移動方向を決定
        var moveSpeed = cameraForward * _vertical + Camera.main.transform.right * _horizontal;
        //RigidBodyの速度を設定
        _rb.velocity = moveSpeed * _speed + new Vector3(0, _rb.velocity.y, 0);
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

        _anim.SetFloat(MoveSpeed, _rb.velocity.magnitude);

        // 最新の位置を更新
        _latestPos = transform.position;
    }
}