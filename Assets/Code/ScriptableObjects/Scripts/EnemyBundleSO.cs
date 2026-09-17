using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBundle", menuName = "Scriptable Objects/EnemyBundle")]
public class EnemyBundleSO : ScriptableObject
{
    public GameObject[] enemy;
}
