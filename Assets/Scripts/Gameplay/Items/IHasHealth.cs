using UnityEngine;

public interface IHasHealth
{
    int CurrentHealth { get; set; }
    int MaxHealth { get; }
}