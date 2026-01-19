using UnityEngine;

public class checkSpawn : MonoBehaviour
{
    private BoxCollider checker;
    private bool isSomething = false;
    private void Start()
    {DisableCheckerCollider();}
    public bool verifySpawn()
    {
        EnableCheckerCollider();
        Invoke(nameof(DisableCheckerCollider),0.2f);
        return isSomething;
    }
    public void EnableCheckerCollider()
    {
        checker.enabled = true;
    }
    public void DisableCheckerCollider()
    {
        checker.enabled = false;
        isSomething=false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            isSomething=true;
            Debug.Log("something on the spawn");
        }
    }
}
