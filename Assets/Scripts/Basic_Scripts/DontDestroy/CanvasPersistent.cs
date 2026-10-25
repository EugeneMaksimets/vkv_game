using UnityEngine;

public class CanvasPersistent : MonoBehaviour
{
    private static CanvasPersistent _canvasInstance;

    void Awake()
    {
        if (_canvasInstance == null)
        {
            _canvasInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}