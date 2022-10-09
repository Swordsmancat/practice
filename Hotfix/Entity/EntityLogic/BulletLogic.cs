using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework;

namespace Farm.Hotfix
{
    public class BulletLogic : Entity
    {
        public static BulletLogic instance;
        public int amountToPool;
        public GameObject m_bullet;
        public List<GameObject> m_pooledObjectlist;

        private bool isStart;
        private Camera m_MainCamera;
        private Transform bulletStartPos;
        private float bulletSpeed = 5f;
        private float shotingTime = 0.5f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            instance = this;

            m_MainCamera = Camera.main;

            m_pooledObjectlist = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(m_bullet, transform);
                tmp.SetActive(false);
                m_pooledObjectlist.Add(tmp);
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (OBJPos())
            {
                transform.gameObject.SetActive(false);
                //transform.parent = BulletIncubator.instance.transform;
            }
        }

        bool OBJPos()
        {
            return transform.position.y < -150 || transform.position.y > 150 || transform.position.x < -150 || transform.position.x > 150 || transform.position.z > 150 || transform.position.z < -150;
        }

        /// 获取可用的子弹
        public GameObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!m_pooledObjectlist[i].activeInHierarchy)
                {
                    return m_pooledObjectlist[i];
                }
            }
            return null;
        }

        public void FireBullet(float equipState)
        {
            if (isStart && m_MainCamera)
            {
                Vector3 inputv3 = Input.mousePosition;
                Vector3 pos = m_MainCamera.WorldToScreenPoint(bulletStartPos.position);
                Vector3 m_MousePos = new Vector3(inputv3.x, inputv3.y, pos.z);
                bulletStartPos.position = m_MainCamera.ScreenToWorldPoint(m_MousePos);

                if (Time.time > shotingTime && equipState == 5)
                {
                    shotingTime = Time.time + 0.5f;
                    Vector3 rayOrigin = bulletStartPos.position;
                    Ray ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 300))
                    {
                        GameObject go = GetPooledObject();
                        go.transform.parent = null;
                        go.transform.position = bulletStartPos.position;
                        go.SetActive(true);
                        go.GetComponent<Rigidbody>().velocity = (hit.point - go.transform.position) * bulletSpeed;
                    }
                }
            }
        }
    }
}

