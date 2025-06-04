using UnityEngine;

[System.Serializable]
public class Obstacle
{
    [Header("Prefab")]
    public GameObject obj;

    [Header("Settings")]
    public bool CanDamage = false;
    public float Speed = 6f;

}
