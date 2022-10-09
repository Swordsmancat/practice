using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farm.Hotfix;

public class enemytest : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator = null;
    private readonly static int LockPlyer = Animator.StringToHash("LockPlyer");

    void Update()
    {
        Ray downRay = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(downRay, out hit, 10))
        {
            Debug.DrawLine(downRay.origin, hit.point);
            if(m_Animator.GetBool(LockPlyer))
                AIUtility.SmoothMove(transform.position, transform.position + (10 * Vector3.up), gameObject);
        }
        else
        {

        }

    }

}
