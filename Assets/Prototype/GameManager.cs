using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        EnemyController.players = FindObjectsByType<CharacterController>();
    }
}