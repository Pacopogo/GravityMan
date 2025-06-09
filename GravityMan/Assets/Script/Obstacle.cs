using UnityEngine;

[CreateAssetMenu(fileName = "Obstacles", menuName = "Obstacle/Obstacle", order = 1)]
public class Obstacle : ScriptableObject
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Settings")]
    public bool CanDamage = false;
    public float Speed = 6f;

}
