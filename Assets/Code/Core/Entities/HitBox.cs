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
        if (hitEntity != owner && attackData != null)
        {
            Debug.Log("entity hit");
            HitOtherEntity(hitEntity);
        }
    }

    public void HitOtherEntity(Entity entity)
    {
        entity.TakeDamage(attackData.damage);
        entity.TakeKnockback(attackData.direction, attackData.forceKnockback);
        //falta el hitStun
    }
}
