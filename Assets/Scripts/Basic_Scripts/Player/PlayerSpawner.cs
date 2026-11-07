using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSpawner : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SpawnPlayerAtDoor();
        InitializeJoystick();
    }

    private void InitializeJoystick()
    {
        VirtualJoystick joystick =
        Object.FindAnyObjectByType<VirtualJoystick>();

        if (joystick == null)
        {
            Debug.LogWarning("[PlayerSpawner] VirtualJoystick not found!");
            return;
        }

        PlayerBrain playerBrain = GetComponent<PlayerBrain>();

        if (playerBrain == null)
        {
            Debug.LogError("[PlayerSpawner] PlayerBrain not found!");
            return;
        }

        joystick.Initialize(playerBrain);
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
                
                if (_rb != null)
                {
                    _rb.position = spawnPoint.transform.position;
                    _rb.linearVelocity = Vector2.zero;
                }
                else
                {
                    transform.position = spawnPoint.transform.position;
                }

                TransitionSpawnManager.LastDoorName = null;

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