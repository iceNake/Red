using System.IO.Enumeration;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("NameWeapon")]
    public string nameWeapon;
    [Header("GroundedAttacks")]
    public AttackSO[] GroundedNeutralAttackSos;
    public AttackSO GroundedUpAttackSos;
    public AttackSO GroundedDownAttackSos;
    public AttackSO GroundedSideAttackSos;
    [Header("AirAttacks")]
    public AttackSO AirNeutralAttackSos;
    public AttackSO AirUpAttackSOs;
    public AttackSO AirDownAttackSos;
    public AttackSO AirSideAttackSos;
}
