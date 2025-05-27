using System;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnUpdate();
    void OnEnter();
    void OnExit();
}

public class Statemachine
{
    private IState currentState;
    private Dictionary<Type, IState> allStates = new Dictionary<Type, IState>();
}
