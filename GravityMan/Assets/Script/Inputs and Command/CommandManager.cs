using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    public IPlayerInputs PlayerInputs;
    public JumpCommand Jump;
    public PauseCommand Pause;

    public CommandManager(IPlayerInputs playerInputs)
    {
        PlayerInputs = playerInputs;

        Jump = InputCommand.Create<JumpCommand>(playerInputs);
        Pause = InputCommand.Create<PauseCommand>(playerInputs);
    }

    public void DoCommand(ICommand command, Action action) => command.Execute(action);
}
