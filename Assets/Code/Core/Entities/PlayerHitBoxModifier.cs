using UnityEngine;

public class PlayerHitBoxModifier : MonoBehaviour
{
    public CircleCollider2D _hitBox;
    public WeaponSO weaponSO;

    public void ActivateHitbox(int attackIndex)
    {
        AttackSO selectedAttack = GetAttack(attackIndex);
        _hitBox.offset = selectedAttack.offSet;
        _hitBox.radius = selectedAttack.radius;
        _hitBox.enabled = true;
    }
    public void DeActivateHitbox()
    {
       _hitBox.enabled = false;
    }

    public AttackSO GetAttack(int index)
    {
        AttackSO attack;
        if(index < 2)
        {
            attack = weaponSO.GroundedNeutralAttackSos[index];
        }
        else 
        {
            switch (index)
            {
                case 3:
                    attack = weaponSO.GroundedUpAttackSos;
                    break;
                case 4:
                    attack = weaponSO.GroundedDownAttackSos;
                    break;
                case 5:
                    attack = weaponSO.GroundedSideAttackSos;
                    break;
                case 6:
                    attack = weaponSO.AirNeutralAttackSos;
                    break;
                case 7:
                    attack = weaponSO.AirUpAttackSOs;
                    break;
                case 8:
                    attack = weaponSO.AirDownAttackSos;
                    break;
                case 9:
                    attack = weaponSO.AirSideAttackSos;
                    break;
                default:
                    attack = null;
                    Debug.Log("InvalidAttackIndex");
                    break;
            }
        }
           return attack;
        
    }
}
