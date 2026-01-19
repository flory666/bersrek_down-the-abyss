using UnityEngine;
public class checkSpawn : MonoBehaviour
{
    private BoxCollider checker;
    private void Start()
    {
        checker = GetComponent<BoxCollider>();
        DisableCheckerCollider();
    }
    public bool verifySpawn()
    {
        Collider[] hits = Physics.OverlapBox(
            transform.position,
            transform.localScale / 2,
            transform.rotation
        );

        foreach (Collider hit in hits)
        {   
            if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
                return false;
        }
        return true;
    }
    private void EnableCheckerCollider()
    {
        checker.enabled = true;
    }
    private void DisableCheckerCollider()
    {
        checker.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Debug.Log("something on the spawn");
        }
    }
}
