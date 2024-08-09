using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _characterController;
    private Animator _anim;

    #region 動きの変数

    [SerializeField] private float _normalSpeed = 3f; //通常時の移動速度
    [SerializeField] private float _sprintSpeed = 5f;　//ダッシュ時の移動速度
    [SerializeField] private float _gravity = 10f;　//重力の大きさ
    private Vector3 _moveDirection = Vector3.zero;

    #endregion

    #region HashAnimation

    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

    #endregion
    

    //GetInstanceID();
    //gameObject.GetInstanceID(); //GameObjectのインスタンスID
    //UnityEngine.Object
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //移動速度を取得
        var speed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _normalSpeed;

        //カメラの向きを基準にした正面方向のベクトル
        var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //前後左右の入力から、移動のためのベクトルを計算
        var moveZ = cameraForward * (Input.GetAxis("Vertical") * speed); //カメラを基準にした前後
        var moveX = Camera.main.transform.right * (Input.GetAxis("Horizontal") * speed); //カメラ基準の左右
        //接地判定
        if (_characterController.isGrounded)
            _moveDirection = moveZ + moveX;
        else
        {
            //重力を効かせる
            _moveDirection = moveZ + moveX + new Vector3(0, _moveDirection.y, 0);
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        //移動のアニメーション
        _anim.SetFloat(MoveSpeed, (moveZ + moveX).magnitude);

        //プレイヤーの向きを入力の向きに変更
        transform.LookAt(transform.position + moveZ + moveX);

        //Moveは指定したベクトルだけ移動させる命令
        _characterController.Move(_moveDirection * Time.deltaTime);
    }
}