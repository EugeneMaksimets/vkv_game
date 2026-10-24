using UnityEngine;

public class DoorSpawnPoint : MonoBehaviour
{
    [SerializeField] private string _doorID;
    public string DoorID => _doorID;
}