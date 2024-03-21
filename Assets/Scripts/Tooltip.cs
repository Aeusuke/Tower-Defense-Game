using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Represents the tooltip text field
public class Tooltip : MonoBehaviour
{
    // References
    [SerializeField] Scrollbar scrollbar;
    TMP_InputField textField;

    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<TMP_InputField>();
    }

    // Updates the text content of the tooltip
    public void UpdateTooltip(string description)
    {
        textField.text = description;
    }

   
}
