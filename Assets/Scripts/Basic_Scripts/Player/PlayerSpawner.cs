using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{

    void Start()
    {
        SpawnPlayerAtDoor();
    }

    private void SpawnPlayerAtDoor()
    {
        if (string.IsNullOrEmpty(TransitionSpawnManager.LastDoorName))
        {
            Debug.Log("PlayerSpawner: LastDoorName пуста. Спавн по умолчанию.");
            return;
        }

        DoorSpawnPoint[] spawnPoints = Object.FindObjectsByType<DoorSpawnPoint>(FindObjectsInactive.Exclude);

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("На этой сцене не найдено ни одной точки спавна (DoorSpawnPoint)!");
            return;
        }

        bool spawned = false;

        foreach (var spawnPoint in spawnPoints)
        {
            Debug.Log($"Сравниваем точку на сцене [{spawnPoint.DoorID}] с тем, что в менеджере [{TransitionSpawnManager.LastDoorName}]");

            if (spawnPoint.DoorID == TransitionSpawnManager.LastDoorName)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.position = spawnPoint.transform.position;
                    rb.linearVelocity = Vector2.zero;
                }
                else
                {
                    transform.position = spawnPoint.transform.position;
                }

                Debug.Log("Игрок успешно телепортирован к точке: " + spawnPoint.DoorID);
                spawned = true;
                break;
            }
        }

        if (!spawned)
        {
            Debug.LogWarning($"Точка спавна с ID '{TransitionSpawnManager.LastDoorName}' на этой сцене НЕ НАЙДЕНА! Проверь написание в DoorSpawnPoint.");
        }
    }
}