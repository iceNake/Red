using UnityEngine;

public class PlayerAnimationEventHandler : AnimationEventHandler
{
    private Player _player => (Player)animatedEntity;

    
    public void EnableHitbox(int attackIndex)
    {
        Debug.Log($"Player encontrado: {_player}");
        Debug.Log($"HitboxModifier encontrado: {_player.playerHitBoxModifier}");

        Debug.Log(_player.playerHitBoxModifier + "   modifier");
        Debug.Log(_player + "   player");


        Debug.Log(attackIndex + "   INDEX");

        _player.playerHitBoxModifier.ActivateHitbox(attackIndex);
    }
    public void DisableHitbox() 
    {
        _player.playerHitBoxModifier.DeActivateHitbox();
    }

    public void ReturnToNormal()
    {
        EnableAttack();
        EnableDodge();
        EnableJump();
        EnableTurn();
        EnableMove();
        DisableHitbox();
    }
    public void DisableInterruption() 
    {
        DisableAttack();
        DisableDodge();
        DisableJump();
        DisableTurn();
        DisableMove();
    }

    
    private void EnableAttack()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Attack
        );
    }

    private void DisableAttack()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Attack
        );
    }

    private void EnableDodge()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Dodge
        );
    }

    private void DisableDodge()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Dodge
        );
    }

    private void EnableJump()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Jump
        );
    }

    private void DisableJump()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Jump
        );
    }

    private void EnableTurn()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Turn
        );
    }

    private void DisableTurn()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Turn
        );
    }

    private void EnableMove()
    {
        _player.ChangeStateOnAnimationEvent(
            true,
            CharacterRestrictions.Move
        );
    }

    private void DisableMove()
    {
        _player.ChangeStateOnAnimationEvent(
            false,
            CharacterRestrictions.Move
        );
    }
}

