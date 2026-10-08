using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    protected Entity animatedEntity;

    private void Start()
    {
        animatedEntity = GetComponentInParent<Entity>();
    }
}
