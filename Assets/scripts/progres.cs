using UnityEngine;

public class Progres : MonoBehaviour
{
    public static Progres Instance;
    public int enemies_killed = 0;
    public float time_survived = 0f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        time_survived += Time.fixedDeltaTime;
    }
    private void OnEnemyKilled()
    {
        enemies_killed++;
    }
}
