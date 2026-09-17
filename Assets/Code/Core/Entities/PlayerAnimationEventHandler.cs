using UnityEngine;

public class PlayerAnimationEventHandler : AnimationEventHandler
{
    private Player _player => (Player)animatedEntity;

    
    public void EnableHitbox(int attackIndex)
    {
        _player.playerHitBoxModifier.ActivateHitbox(attackIndex);
    }
    public void DisableHitbox() 
    {
        _player.playerHitBoxModifier.DeActivateHitbox();
    }
    public void EnableAttack()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Attack
        );
    }

    public void DisableAttack()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Attack
        );
    }

    public void EnableDodge()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Dodge
        );
    }

    public void DisableDodge()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Dodge
        );
    }

    public void EnableJump()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Jump
        );
    }

    public void DisableJump()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Jump
        );
    }

    public void EnableTurn()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Turn
        );
    }

    public void DisableTurn()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Turn
        );
    }

    public void EnableMove()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Move
        );
    }

    public void DisableMove()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Move
        );
    }
}

