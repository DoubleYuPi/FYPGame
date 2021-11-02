using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryTextBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTop1 = null;
    [SerializeField] private TextMeshProUGUI textTop2 = null;
    [SerializeField] private TextMeshProUGUI textTop3 = null;
    [SerializeField] private TextMeshProUGUI textBottom1 = null;
    [SerializeField] private TextMeshProUGUI textBottom2 = null;
    [SerializeField] private TextMeshProUGUI textBottom3 = null;

    public void SetBoxText(string txtTop1, string txtTop2, string txtTop3, string txtBottom1, string txtBottom2, string txtBottom3)
    {
        textTop1.text = txtTop1;
        textTop2.text = txtTop2;
        textTop3.text = txtTop3;
        textBottom1.text = txtBottom1;
        textBottom2.text = txtBottom2;
        textBottom3.text = txtBottom3;
    }
}
