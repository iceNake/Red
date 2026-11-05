using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector2 direction;
    private float damage;

    public void Init(Vector2 dir, float dmg)
    {
        direction = dir;
        damage = dmg; ;

        Destroy(gameObject, 4f);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out  Player player))
        {
            if (player.invulnerable) return;
            
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
