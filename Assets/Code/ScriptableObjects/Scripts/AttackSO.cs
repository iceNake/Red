using UnityEngine;
[CreateAssetMenu(fileName = "Attacks")]
public class AttackSO : ScriptableObject
{
    public float damage;
    public Vector2 direction;
    public float startup;
    public float endlag;
    public float forceKnockback;
    public Vector2 rangeAttack;
    public float durationAttack;
    public float comboWindow;
}
