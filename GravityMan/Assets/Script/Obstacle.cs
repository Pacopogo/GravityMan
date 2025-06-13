using UnityEngine;

//public enum ObjectType
//{
//    None,
//    Damage,
//    Heal
//}

[CreateAssetMenu(fileName = "Obstacles", menuName = "Obstacle/Obstacle", order = 1)]
public class Obstacle : ScriptableObject
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Settings")]
    //public ObjectType Type;
    public float Speed = 6f;
}
