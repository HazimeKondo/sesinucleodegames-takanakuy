using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _text;
    //private Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.gameObject.SetActive(false);
        GameManager.Instance.OnStartPlay += () => _text.gameObject.SetActive(true);
        GameManager.Instance.OnTimeUpdated += (t) => _text.text = t.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
