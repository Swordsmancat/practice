using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class InviteCodeForm : MonoBehaviour
{
    [Title("�������Ӱ�ť")]
    [SerializeField]
    private Button copyButton;

    [Title("��������")]
    [SerializeField]
    private string link;

    private void Start()
    {
        copyButton.onClick.AddListener(OnClickCopyButton);
    }

    //��������
    private void OnClickCopyButton()
    {
        GUIUtility.systemCopyBuffer = link;
    }
}
