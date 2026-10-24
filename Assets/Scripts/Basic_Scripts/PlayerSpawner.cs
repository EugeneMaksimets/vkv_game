using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Ищем дверь спавна для: " + TransitionSpawnManager.LastDoorName);
        DoorSpawnPoint[] spawnPoints = Object.FindObjectsByType<DoorSpawnPoint>(FindObjectsInactive.Exclude);

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("На этой сцене не найдено ни одной точки спавна (DoorSpawnPoint)!");
            return;
        }

        bool spawned = false;

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.DoorID == TransitionSpawnManager.LastDoorName)
            {
                transform.position = spawnPoint.transform.position;
                spawned = true;
                break;
            }
        }

        if (!spawned)
        {
            Debug.Log("Точка спавна с ID '" + TransitionSpawnManager.LastDoorName + "' не найдена. Игрок остался на стартовой позиции.");
        }
    }
}