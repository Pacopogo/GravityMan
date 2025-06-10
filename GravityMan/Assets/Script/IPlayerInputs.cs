using UnityEngine;

public interface IPlayerInputs
{
    public void Jump(KeyCode[] key);
    public bool PauseGame(KeyCode[] key, bool isPaused);
}
