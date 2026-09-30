using UnityEngine;

public class ArqueroAnimationEventHandler : AnimationEventHandler
{
    private ArqueroTest _arquero => (ArqueroTest)animatedEntity;

    public void Shoot()
    {
        _arquero.Shoot();
    }

    public void DisableMovement()
    {
        _arquero.DisableMovement();
    }

    public void EnableMovement()
    {
        _arquero.EnableMovement();
    }

    public void EndAttack()
    {
        _arquero.EndAttack();
    }
}
