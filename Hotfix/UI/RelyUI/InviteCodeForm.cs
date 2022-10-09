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
    [Title("复制链接按钮")]
    [SerializeField]
    private Button copyButton;

    [Title("邀请链接")]
    [SerializeField]
    private string link;

    private void Start()
    {
        copyButton.onClick.AddListener(OnClickCopyButton);
    }

    //复制链接
    private void OnClickCopyButton()
    {
        GUIUtility.systemCopyBuffer = link;
    }
}
