using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class State
{
    protected string name;

    public string Name { get => name; }

    public State()
    {
        name = this.GetType().ToString();
    }

    public abstract bool CanEnter<T>(T stateMachine) where T : StateMachine;
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerExit(Collider other);

}
