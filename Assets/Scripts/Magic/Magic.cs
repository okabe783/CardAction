using System.Data;
using UnityEngine;

//火の魔法などの固形物にセット
public class Magic : MonoBehaviour
{
    //Hitしたときに与えるダメージ
    [SerializeField] private float _damage;

    //魔法の持続時間
    [SerializeField] private float _lifeTime　= 4f;

    //爆発エフェクト
    [SerializeField] private GameObject _magicExplode;

    private void Start()
    {
        //呼び出された時点で必ず消す
        Destroy(gameObject, _lifeTime);
    }

    //敵に当たった時の処理
    private void OnTriggerEnter(Collider other)
    {
        //ぶつかったものが敵であるか
        var enemy = other.gameObject.CompareTag("Enemy");
        //Todo:数回呼ばれてしまうからダメージは一度のみ与える

        if (enemy)
        {
            Debug.Log("衝突した");
            //爆発させるEffectを出す
            Instantiate(_magicExplode, transform.position, Quaternion.identity);
            //ぶつかった時点で自身を消す
            Destroy(gameObject);
        }
    }
}