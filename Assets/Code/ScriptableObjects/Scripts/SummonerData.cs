using UnityEngine;

[CreateAssetMenu(fileName = "SummonerData", menuName = "Scriptable Objects/SummonerData")]
public class SummonerData : ScriptableObject
{
    [Header("Base Stats")] 
    public float maxHealth = 100f;
    public float moveSpeed = 2f;

    [Header("Area abilities")]
    public float stunTime = 2f;
    public float stunCooldown = 3f;
    public float attackCooldown = 3f;
    public float attackInvoke = 5f;
    public LayerMask playerLayer;
    public float summons = 3f;
}
