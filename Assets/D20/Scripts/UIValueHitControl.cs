using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIValueHitControl : MonoBehaviour
{
    public Canvas parentCanvas;
    public GameObject uiTextTemplate;
    public int initialPoolSize = 3;
    public float upSpeed;
    public float[] phasees = new float[] { -79f, 55.5f, -92.75f, -3.94f, 71.3f };

    private Dictionary<GameObject, float> uiTextFieldPool = new();
    // Start is called before the first frame update
    void Start()
    {
        if (parentCanvas == null)
        {
            this.enabled = false;
            return;
        }
        for (int i = 0; i < initialPoolSize; i++)
            EnlargePool();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i <uiTextFieldPool.Keys.Count; i++)
        {
            var go = uiTextFieldPool.Keys.ElementAt(i);

            if (uiTextFieldPool[go] > 0f)
            {
                uiTextFieldPool[go] -= Time.deltaTime;
                var rect = go.GetComponent<RectTransform>();
                rect.position += Vector3.up * upSpeed * Time.deltaTime + Vector3.right * phasees[i % phasees.Length] * Time.deltaTime;
            }
            else if (go.activeSelf) { go.SetActive(false); go.GetComponent<RectTransform>().SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); }
        }
    }

    public void HitValue(string text)
    {
        if (!enabled) return;
        var go = uiTextFieldPool.FirstOrDefault(x => !x.Key.activeSelf).Key ?? EnlargePool();
        go.SetActive(true);
        go.GetComponent<TextMeshProUGUI>().text= text;
        uiTextFieldPool[go] = 5f;
    }

    private GameObject EnlargePool()
    {
        var go = GameObject.Instantiate(uiTextTemplate, parentCanvas.transform);
        go.SetActive(false);
        //go.transform.position = uiTextTemplate.transform.position;
        //go.transform.localScale = uiTextTemplate.transform.localScale;
        var f = 0f;
        uiTextFieldPool.Add(go, f);

        return go;
    }
}
