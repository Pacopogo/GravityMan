using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput
{
    public KeyCode[] Keys;
    public ICommand InputCommand;
    
    public CommandManager CommandManager;
    
    public Action InputAction;


    public void CheckInput()
    {
        foreach (KeyCode keyCode in Keys)
        {
            if (!Input.GetKeyDown(keyCode))
                continue;

            CommandManager.DoCommand(InputCommand, InputAction);

            return;
        }
    }
}
