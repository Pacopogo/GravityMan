using UnityEngine;

[CreateAssetMenu(fileName = "Input Keys", menuName = "Key/Keys", order = 1)]
public class PlayerInputKeys : ScriptableObject
{
    [Header("Directions")]
    public KeyCode[] UP;
    public KeyCode[] RIGHT;
    public KeyCode[] LEFT;
    public KeyCode[] DOWN;

    [Header("Actions")]
    public KeyCode[] Jump;

}
