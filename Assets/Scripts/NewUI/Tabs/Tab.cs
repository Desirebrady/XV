using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public Category myCategory;
    public string myName;
    public Color myColor;

    private Text textField;

    public void setName()
    {
        textField = GetComponentInChildren<Text>();
        textField.text = myName;
    }

    public void onClick()
    {
        UIElementManager.Instance.ShowOfCategory(myCategory);
    }

}
