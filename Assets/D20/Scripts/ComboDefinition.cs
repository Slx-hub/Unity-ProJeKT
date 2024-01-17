using System.Collections.Generic;
using UnityEngine;

public class ComboStage
{
    public List<int> AllowedValues { get; private set; }

    public ComboStage(string config)
    {
        AllowedValues = new List<int>();
        foreach (var value in config.Split(','))
        {
            AllowedValues.Add(int.Parse(value.Trim()));
        }
    }
}
public class ComboDefinition
{
    List<ComboStage> ComboStages = new();

    public ComboDefinition (string config)
    {
        foreach (var stage in config.Split('\n'))
        {
            if (stage.Trim().Length > 0)
                ComboStages.Add(new ComboStage(stage));
        }
    }

    public int GetStageCount()
    {
        return ComboStages.Count;
    }

    public ComboStage GetStage(int ordinal)
    {
        return ComboStages[ordinal];
    }
}