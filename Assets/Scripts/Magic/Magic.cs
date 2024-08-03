using UnityEngine;

//あたり判定の必要ない魔法用のエフェクトにアタッチする
public class Magic : MonoBehaviour
{
    //魔法の持続時間
    [SerializeField] private float _lifeTime　= 4f;

    protected virtual void Start()
    {
        //呼び出された時点で必ず消す
        Destroy(gameObject, _lifeTime);
    }
}