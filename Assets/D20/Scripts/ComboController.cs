using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    private D20FaceEmissionControl FaceEmissionControl;

    private ComboDefinition ActiveCombo;

    private int ComboStage = 0;
    private string debugState = "None";
    private List<int> RolledValues;

    void Start()
    {
        FaceEmissionControl = GetComponent<D20FaceEmissionControl>();
    }

    public void StartCombo(string name)
    {
        FailCombo();
        ActiveCombo = ComboConfig.GetComboDefinitionByName(name);
        FaceEmissionControl.HighlightValues(GetCurrentStageValues().ToArray());

        debugState = "Doing " + name;
    }

    public void ValidateRoll(int roll)
    {
        if (ActiveCombo == null) return;

        if (GetCurrentStageValues().Contains(roll))
            AdvanceStage(roll);
        else
            FailCombo();
    }

    private void AdvanceStage(int roll)
    {
        RolledValues.Add(roll);

        debugState = "Cleared Stage " + ComboStage;

        if (++ComboStage == ActiveCombo.GetStageCount())
        {
            debugState = "Done!";
            ClearState();
            return;
        }
        FaceEmissionControl.HighlightValues(GetCurrentStageValues().ToArray());
    }

    private void FailCombo()
    {
        ClearState();
        debugState = "Failed";
    }

    private void ClearState()
    {
        ActiveCombo = null;
        ComboStage = 0;
        RolledValues = new List<int>();
        FaceEmissionControl.ClearHighlight();
    }

    private List<int> GetCurrentStageValues()
    {
        return ActiveCombo.GetStage(ComboStage).AllowedValues;
    }

    public override string ToString()
    {
        return "> ComboController:\n  State:\t\t\t" + debugState;
    }
}
