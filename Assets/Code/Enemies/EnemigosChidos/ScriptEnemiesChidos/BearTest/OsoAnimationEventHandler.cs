using UnityEngine;

public class OsoAnimationEventHandler : AnimationEventHandler
{
    private OsoTest _oso =>  (OsoTest)animatedEntity;

    public void EnableHitbox()
    {
        _oso.EnableHitbox();
    }

    public void DisableHitbox()
    {
        _oso.DisableHitbox();
    }

    public void JabEnableHitbox()
    {
        _oso.JabEnableHitbox();
    }

    public void JabDisableHitbox()
    {
        _oso.JabDisableHitbox();
    }
    public void JabEndAttack()
    {
        _oso.JabEndAttack();
    }
    public void ShoveEnableHitbox()
    {
        _oso.ShoveEnableHitbox();
    }

    public void ShoveDisableHitbox()
    {
        _oso.ShoveDisableHitbox();
    }
    public void ShoveEndAttack()
    {
        _oso.ShoveEndAttack();
    }
    public void SlamEnableHitbox()
    {
        _oso.SlamEnableHitbox();
    }

    public void SlamDisableHitbox()
    {
        _oso.SlamDisableHitbox();
    }
    public void SlamEndAttack()
    {
        _oso.SlamEndAttack();
    }

    public void DisableMovement()
    {
        _oso.DisableMovement();
    }

    public void EnableMovement()
    {
        _oso.EnableMovement();
    }

    public void EndAttack()
    {
        _oso.EndAttack();
    }
}
