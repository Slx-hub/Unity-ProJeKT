using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ValueShelf : MonoBehaviour
{
    public TextMeshProUGUI valueShelfTextField;
    public int maxValues = 3;
    private List<int> lastValues = new();

    // Start is called before the first frame update
    void Start()
    {
        valueShelfTextField = transform.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddValueToShelf(int value)
    {
        if(valueShelfTextField == null) { return; }

        lastValues.Add(value);
        if(lastValues.Count > maxValues) { lastValues.RemoveAt(0); }

        valueShelfTextField.text = lastValues.ToSeparatedString("\t");
    }
}
