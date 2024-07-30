using UnityEngine;

public class MagicAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _magicCirclePos;

    public void MagicInstance(GameObject _magicCircle)
    {
        //魔法陣を生成
        Instantiate(_magicCircle, _magicCirclePos.transform.position, Quaternion.Euler(90f, 0f, 0f));
    }
}
