using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private Entity owner;
    public AttackSO attackData;
    

    private void Start()
    {
        owner = GetComponentInParent<Entity>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity hitEntity = collision.gameObject.GetComponent<Entity>();
        if(hitEntity != null)
            if (hitEntity != owner && attackData != null)
            {
                Debug.Log("entity hit");
                HitOtherEntity(hitEntity);
            }
    }

    public void HitOtherEntity(Entity entity)
    {
        entity.TakeDamage(attackData.damage);
        Vector2 knockbackDirection;
        if (owner.isPlayerFlipped)
        {
            knockbackDirection = attackData.direction * new Vector2(-1, 1);
        }
        else
        {
            knockbackDirection = attackData.direction;
        }
            entity.TakeKnockback(knockbackDirection, attackData.forceKnockback);
        //falta el hitStun
    }
}
