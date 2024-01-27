using System.Collections.Generic;
using UnityEngine;

public class ComboConfig : MonoBehaviour
{
    public TextAsset ConfigFile;

    private static Dictionary<string, ComboDefinition> DefinitionDict = new();

    private void Start()
    {
        DefinitionDict.Clear();
        LoadComboDefinitions();
    }

    public static ComboDefinition GetComboDefinitionByName(string name)
    {
        return DefinitionDict[name.ToLower()];
    }

    private void LoadComboDefinitions()
    {
        var combos = ConfigFile.text.Split(';');

        foreach (var combo in combos)
        {
            if (combo.Trim().Length > 0)
                LoadComboDefinition(combo.Trim());
        }
    }

    private void LoadComboDefinition(string text)
    {
        var splitContent = text.Split(':');
        var name = splitContent[0].Trim();

        var combo = new ComboDefinition(splitContent[1]);

        DefinitionDict.Add(name.ToLower(), combo);
    }
}