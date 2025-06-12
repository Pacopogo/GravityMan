using System;
using UnityEngine;

public class PauseCommand : InputCommand
{
    public PauseCommand(IPlayerInputs player) : base(player) { }

    public override void Execute(Action action ) => action?.Invoke();

}
