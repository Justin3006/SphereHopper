using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>true, if it was succesfully used, otherwise false</returns>
    public bool Use();
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>how many uses the ability has in total</returns>
    public int GetUsesMax();
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>how many uses the ability has left right now</returns>
    public int GetUsesRemaining();

    /// <summary>
    /// 
    /// </summary>
    /// <returns>true, if it is currently being used, otherwise false</returns>
    public bool IsUsed();

    /// <summary>
    /// 
    /// </summary>
    /// <returns>the name of the ability</returns>
    public string GetName();

    /// <summary>
    /// Resets number of uses remaining
    /// </summary>
    public void AddUses(int uses);
}
