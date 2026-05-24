using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Strategies List", menuName = "Strategies List")]
public class StrategiesList : ScriptableObject
{
    public List<InstantiationStrategy> instantiationStrategies = new List<InstantiationStrategy>();
}
