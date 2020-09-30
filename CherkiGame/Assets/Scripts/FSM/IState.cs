using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState    //A general abstract state that can be used for all sorts of games
{                              //Not very necessary for this project
    protected string name;
    protected string nextStateName;

    public virtual string GetName { get { return name; } }
    public virtual string GetNextStateName { get { return nextStateName; } }
}
