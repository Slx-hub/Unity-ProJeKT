using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ComboController : MonoBehaviour
{
    private D20FaceEmissionControl FaceEmissionControl;
    private UIValueHitControl uivhc;

    private ComboDefinition ActiveCombo;

    private int ComboStage = 0;
    private string debugState = "None";
    private List<int> RolledValues;

    void Start()
    {
        FaceEmissionControl = GetComponent<D20FaceEmissionControl>();
        uivhc = GetComponent<UIValueHitControl>();
    }

    public void StartCombo(string name)
    {
        FailCombo(-1);
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
            FailCombo(roll);
    }

    private void AdvanceStage(int roll)
    {
        RolledValues.Add(roll);
        uivhc.HitValue("<color=green>" + roll + "</color>");
        debugState = "Cleared Stage " + ComboStage;

        if (++ComboStage == ActiveCombo.GetStageCount())
        {
            uivhc.HitValue("<color=#FFD400>Total: " + RolledValues.Sum() + "</color>");
            debugState = "Done!";
            ClearState();
            return;
        }
        FaceEmissionControl.HighlightValues(GetCurrentStageValues().ToArray());
    }

    private void FailCombo(int roll)
    {
        ClearState();
        if (roll > 0)
            uivhc.HitValue("<color=red>" + roll + "</color>");
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