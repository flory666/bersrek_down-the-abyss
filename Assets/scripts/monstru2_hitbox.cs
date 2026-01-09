using UnityEngine;

public class monstru2_hitbox : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        transform.parent.GetComponentInParent<monstru2>().TakeDamage(damage);
    }
}
