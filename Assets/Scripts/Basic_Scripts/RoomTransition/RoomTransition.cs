using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomTransition : MonoBehaviour
{
    [Header("Target Scene")]
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private string _doorName;
    private bool _isPlayerInside = false;
    private string _tag = "Player";

    void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(_tag))
        {
            _isPlayerInside = true;
            // Debug.Log("Player Enter");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag(_tag))
        {
            _isPlayerInside = false;
            // Debug.Log("Player Exit");
        }
    }

    void Update()
    {
        if (_isPlayerInside)
        {
            bool isInteractPressed = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
            if(isInteractPressed)
            {
                LoadNextScene();
            }
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(_sceneToLoad))
        {
            TransitionSpawnManager.LastDoorName = _doorName;
            SceneManager.LoadScene(_sceneToLoad);
        }
        else
        {
            Debug.Log("Dont find name: " + _sceneToLoad);
        }
    }

}
