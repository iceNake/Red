using UnityEngine;

public class PlayerHitBoxModifier : MonoBehaviour
{
    public CircleCollider2D _hitBox;
    public WeaponSO weaponSO;
    public Player player;
    
    public HitBox hitBoxClass;
    
    public void ActivateHitbox(int attackIndex)
    {
        AttackSO _selectedAttack = GetAttack(attackIndex);
        hitBoxClass.attackData = _selectedAttack;

        if (_selectedAttack == null)
        {
            Debug.Log("No se encontro ningun AttackSO");
            return;
        }
        

        _hitBox.enabled = true;
        if (player.isPlayerFlipped)
        {
            _hitBox.offset = _selectedAttack.offSet * new Vector2(-1, 1);
            player.ApplyMomentum(_selectedAttack.momentumDirection * new Vector2(-1, 1), _selectedAttack.momentumForce);
            Debug.Log("<color=orange>" + _selectedAttack.momentumDirection.x);
        }
        else
        {
            _hitBox.offset = _selectedAttack.offSet;
            player.ApplyMomentum(_selectedAttack.momentumDirection, _selectedAttack.momentumForce);
            Debug.Log("<color=orange>" + _selectedAttack.momentumDirection.x);
        }
        
        _hitBox.radius = _selectedAttack.radius;
        
        
    }
    public void DeActivateHitbox()
    {
       _hitBox.enabled = false;
    }

    public AttackSO GetAttack(int index)
    {
        AttackSO attack;

        if(index >= 0 && index <= 2)
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
                    Debug.Log("InvalidAttackIndex");
                    attack = null;
                    break;
            }
        }
           return attack;
        
    }
}
