using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class ChangeNameForm : MonoBehaviour
{
    [Title("ȷ�ϰ�ť")]
    [SerializeField]
    private Button okButton;


    private void Start()
    {
        okButton.onClick.AddListener(OnClickOKButton);
    }

    private void OnClickOKButton()
    {
        Log.Info("ok ok ok");
    }
}
