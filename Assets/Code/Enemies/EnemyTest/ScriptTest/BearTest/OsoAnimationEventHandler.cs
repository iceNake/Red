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
