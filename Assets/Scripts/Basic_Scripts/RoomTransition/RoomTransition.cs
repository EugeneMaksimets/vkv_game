using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomTransition : MonoBehaviour, IInteractable
{
    [Header("Target Scene")]
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private string _doorName;


    void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }


    public void Interact()
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(_sceneToLoad))
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            TransitionSpawnManager.LastDoorName = _doorName;
            SceneManager.LoadScene(_sceneToLoad);
        }
        else
        {
            Debug.Log("Dont find name: " + _sceneToLoad);
        }
    }

}
