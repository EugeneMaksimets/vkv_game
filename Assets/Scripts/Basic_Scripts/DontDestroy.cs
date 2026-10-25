using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy _instance;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
