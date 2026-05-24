using UnityEngine;

public class DespawnArea : MonoBehaviour
{
    private static DespawnArea instance;
    public static DespawnArea Instance{get{return instance;}}
    public event System.Action<GameObject> ObjectCollision;
    
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple UnpoolAreas detected! Destroying DespawnArea at: "  + gameObject.name);
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            ObjectCollision?.Invoke(other.gameObject);
        }
    }
}
