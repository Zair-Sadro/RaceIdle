using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Machine/AutoRepairData")]
public class AutoRepairData : ScriptableObject
{
    [SerializeField] private List<AutoRequierments> levels;

    public AutoRequierments GetRequierments(int level)
    {
        if (level >= levels.Count)
            return levels[levels.Count - 1];

        else
            return levels[level];
    }


}


[System.Serializable]
public struct AutoRequierments
{
    [SerializeField] private List<ProductRequierment> requierments;
    public IReadOnlyList<ProductRequierment> RequiermentsList => requierments;
    public int level;
}
