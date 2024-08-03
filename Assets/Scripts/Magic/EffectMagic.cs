using UnityEngine;

//あたり判定を必要とする魔法用のエフェクトにアタッチする
public class EffectMagic : Magic
{
    //Hitしたときに与えるダメージ
    [SerializeField] private float _damage;
    //爆発エフェクト
    [SerializeField] private GameObject _magicExplode;
    //複数回当たり判定がおこなわれることを防ぐためのフラグ
    private bool _hasExploded = false;
    private void OnTriggerEnter(Collider other)
    {
        if(_hasExploded) return;
        //ぶつかったものが敵であるか
        var enemy = other.gameObject.CompareTag("Enemy");

        if (enemy)
        {
            _hasExploded = true;
            Debug.Log("衝突した");
            //爆発させるEffectを出す
            Instantiate(_magicExplode, transform.position, Quaternion.identity);
            //ぶつかった時点で自身を消す
            Destroy(gameObject);
        }
    }
}
