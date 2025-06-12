using System;
using UnityEngine;

public abstract class InputCommand : ICommand
{
    protected readonly IPlayerInputs playerInputs;

    protected InputCommand(IPlayerInputs playerInputs)
    {
        this.playerInputs = playerInputs;
    }

    public abstract void Execute(Action action);

    public static T Create<T>(IPlayerInputs playerInputs) where T : InputCommand
    {
        return (T) System.Activator.CreateInstance(typeof(T), playerInputs);
    }
}
