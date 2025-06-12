using System;
using UnityEngine;

public interface ICommand
{
    public void Execute(Action action);
}
