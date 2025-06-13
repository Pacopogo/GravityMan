using System;
using UnityEngine;

public class JumpCommand : InputCommand
{
    public JumpCommand(IPlayerInputs player) : base(player) { }

    public override void Execute(Action action) => action?.Invoke();

}
