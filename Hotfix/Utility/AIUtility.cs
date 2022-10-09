
using GameFramework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    /// <summary>
    /// AI 工具类。
    /// </summary>
    public static class AIUtility
    {
        private static Dictionary<CampPair, RelationType> s_CampPairToRelation = new Dictionary<CampPair, RelationType>();
        private static Dictionary<KeyValuePair<CampType, RelationType>, CampType[]> s_CampAndRelationToCamps = new Dictionary<KeyValuePair<CampType, RelationType>, CampType[]>();

        static AIUtility()
        {
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Player), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Enemy), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Neutral), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Player2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Enemy2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player, CampType.Neutral2), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Enemy), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Neutral), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Player2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Enemy2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy, CampType.Neutral2), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Neutral), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Player2), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Enemy2), RelationType.Neutral);
            s_CampPairToRelation.Add(new CampPair(CampType.Neutral, CampType.Neutral2), RelationType.Hostile);

            s_CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Player2), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Enemy2), RelationType.Hostile);
            s_CampPairToRelation.Add(new CampPair(CampType.Player2, CampType.Neutral2), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Enemy2, CampType.Enemy2), RelationType.Friendly);
            s_CampPairToRelation.Add(new CampPair(CampType.Enemy2, CampType.Neutral2), RelationType.Neutral);

            s_CampPairToRelation.Add(new CampPair(CampType.Neutral2, CampType.Neutral2), RelationType.Neutral);
        }

        /// <summary>
        /// 获取两个阵营之间的关系。
        /// </summary>
        /// <param name="first">阵营一。</param>
        /// <param name="second">阵营二。</param>
        /// <returns>阵营间关系。</returns>
        public static RelationType GetRelation(CampType first, CampType second)
        {
            if (first > second)
            {
                CampType temp = first;
                first = second;
                second = temp;
            }

            RelationType relationType;
            if (s_CampPairToRelation.TryGetValue(new CampPair(first, second), out relationType))
            {
                return relationType;
            }

            Log.Warning("Unknown relation between '{0}' and '{1}'.", first.ToString(), second.ToString());
            return RelationType.Unknown;
        }

        /// <summary>
        /// 获取和指定具有特定关系的所有阵营。
        /// </summary>
        /// <param name="camp">指定阵营。</param>
        /// <param name="relation">关系。</param>
        /// <returns>满足条件的阵营数组。</returns>
        public static CampType[] GetCamps(CampType camp, RelationType relation)
        {
            KeyValuePair<CampType, RelationType> key = new KeyValuePair<CampType, RelationType>(camp, relation);
            CampType[] result = null;
            if (s_CampAndRelationToCamps.TryGetValue(key, out result))
            {
                return result;
            }

            // TODO: GC Alloc
            List<CampType> camps = new List<CampType>();
            Array campTypes = Enum.GetValues(typeof(CampType));
            for (int i = 0; i < campTypes.Length; i++)
            {
                CampType campType = (CampType)campTypes.GetValue(i);
                if (GetRelation(camp, campType) == relation)
                {
                    camps.Add(campType);
                }
            }

            // TODO: GC Alloc
            result = camps.ToArray();
            s_CampAndRelationToCamps[key] = result;

            return result;
        }

        /// <summary>
        /// 获取实体间的距离。
        /// </summary>
        /// <returns>实体间的距离。</returns>
        public static float GetDistance(Entity fromEntity, Entity toEntity)
        {
            Transform fromTransform = fromEntity.CachedTransform;
            Transform toTransform = toEntity.CachedTransform;
            return (toTransform.position - fromTransform.position).magnitude;
        }

        /// <summary>
        /// 获取实体间的距离。
        /// </summary>
        /// <returns>实体间的距离。</returns>
        public static float GetDistance(Entity fromEntity, UnityGameFramework.Runtime.Entity toEntity)
        {
            Transform fromTransform = fromEntity.transform;
            Transform toTransform = toEntity.transform;
            return (toTransform.position - fromTransform.position).magnitude;
        }

        /// <summary>
        /// 获取两个向量之间距离 (多余函数  修改完可删)
        /// </summary>
        /// <param name="a">向量a</param>
        /// <param name="b">向量b</param>
        /// <returns>返回距离</returns>
        public static float GetDistance(in Vector3 a,in Vector3 b)
        {
            return (a - b).magnitude;
        }

        /// <summary>
        /// 获得水平距离忽略Y轴
        /// </summary>
        /// <param name="from">原点</param>
        /// <param name="to">目标</param>
        /// <returns>水平距离忽略Y轴</returns>
        public static float GetDistanceNoYAxis(in Vector3 from, in Vector3 to)
        {
            Vector3 a = from;
            Vector3 b = to;

            a.y = 0;
            b.y = 0;

            return (a - b).magnitude;
        }

        public static Vector2 GetAttackerDir(Entity entity ,Entity other)
        {
            Vector2 dir = Vector2.zero;
            dir.y = GetDot(entity, other);
            dir.x = GetCross(entity, other);
            return dir;
        }



        /// <summary>
        /// 获取实体平面的距离 (多余函数 修改完可删)
        /// </summary>
        /// <param name="posOne">位置1</param>
        /// <param name="posTwo">位置2</param>
        /// <returns>如果实体不在一平面上则返回0，如果在则正常返回距离</returns>
        public static float GetCheckPlaneDistance(Transform posOne,Transform posTwo)
        {
            if(Mathf.Abs(posOne.position.y - posTwo.position.y) > posOne.localScale.y + 1.0f)
            {
                //不在一个平面上
                return 0;
            }

            return Mathf.Abs((posOne.position - posTwo.position).magnitude);
        }

        /// <summary>
        /// 获取实体间的夹角
        /// </summary>
        /// <param name="fromEntity">当前实体</param>
        /// <param name="toEntity">目标实体</param>
        /// <returns>实体间的夹角</returns>
        public static float GetAngleInSeek(Entity fromEntity,Entity toEntity)
        {
            Vector3 direction = toEntity.transform.position - fromEntity.transform.position;
            float degree = Vector3.Angle(direction, fromEntity.transform.forward);
            return degree;
        }

        public static float GetDot(Entity fromEntity ,Entity toEntity)
        {
            Vector3 direction = toEntity.transform.position - fromEntity.transform.position;
            return Vector3.Dot(fromEntity.transform.forward.normalized, direction.normalized);
        }

        public static float GetCross(Entity fromEntity, Entity toEntity)
        {
            Vector3 direction = toEntity.transform.position - fromEntity.transform.position;
            return Vector3.Cross(fromEntity.transform.forward.normalized, direction.normalized).y;
        }

        /// <summary>
        /// 获取实体间的夹角
        /// </summary>
        /// <param name="fromEntity">当前实体</param>
        /// <param name="toEntity">目标实体</param>
        /// <returns>实体间的夹角</returns>
        public static float GetAngleInSeek(Entity fromEntity, UnityGameFramework.Runtime.Entity toEntity)
        {
            Vector3 direction = toEntity.transform.position - fromEntity.transform.position;
            float degree = Vector3.Angle(direction, fromEntity.transform.forward);
            return degree;
        }

        /// <summary>
        /// 返回目标的水平夹角
        /// </summary>
        /// <param name="from">当前位置</param>
        /// <param name="to">目标位置</param>
        /// <returns>负为左，正为右</returns>
        public static float GetPlaneAngle(in Entity from,in Entity to)
        {
            Vector3 TargetDir = from.CachedTransform.position - to.CachedTransform.position;
            TargetDir.y = 0;
            return Vector3.SignedAngle(TargetDir, to.CachedTransform.forward, Vector3.up);
        }

        /// <summary>
        /// 返回目标的水平夹角
        /// </summary>
        /// <param name="from">当前位置</param>
        /// <param name="to">目标位置</param>
        /// <returns>负为左，正为右</returns>
        public static float GetPlaneAngle(in Entity from, in UnityGameFramework.Runtime.Entity to)
        {
            Vector3 TargetDir = from.transform.position - to.transform.position;
            TargetDir.y = 0;
            return Vector3.SignedAngle(TargetDir, to.transform.forward, Vector3.up);
        }

        /// <summary>
        /// 判断夹角是否在指定范围内
        /// </summary>
        /// <param name="mixAgnle">最小角度范围</param>
        /// <param name="maxAngle">最大角度范围</param>
        /// <param name="Angle">角度</param>
        /// <returns>如果在范围内返回真,在范围外返回假</returns>
        public static bool CheckInAngle(in float mixAgnle,in float maxAngle,in float Angle)
        {
            if(Angle > mixAgnle && Angle < maxAngle)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 敌人绕Y轴旋转面向玩家不会作用在X轴上
        /// </summary>
        /// <param name="from">玩家实体</param>
        /// <param name="to">怪物实体</param>
        /// <param name="minAngle">最小开始旋转角度</param>
        /// <param name="maxAgnle">最大开始旋转角度</param>
        /// <param name="rotateSpeed">旋转速度</param>
        /// <param name="turnTime">旋转时间</param>
        public static void RotateToTarget( Entity from, Entity to,
             float minAngle, float maxAgnle,
             float rotateSpeed = 3f, float turnTime = 300f)
        {
            //获取怪物和玩家的平面夹角
            float angle = GetPlaneAngle(from,to);
            if(angle >= maxAgnle)
            {
                //向左旋转
                Quaternion targetRotate = Quaternion.Euler(to.transform.localEulerAngles.x, to.transform.localEulerAngles.y - rotateSpeed, to.transform.localEulerAngles.z);
                to.transform.rotation = Quaternion.RotateTowards(to.transform.rotation, targetRotate, turnTime * Time.deltaTime);
            }
            else if (angle <= minAngle)
            {
                //向右旋转
                Quaternion targetRotate = Quaternion.Euler(to.transform.localEulerAngles.x, to.transform.localEulerAngles.y + rotateSpeed, to.transform.localEulerAngles.z);
                to.transform.rotation = Quaternion.RotateTowards(to.transform.rotation, targetRotate, turnTime * Time.deltaTime);
            }

        }

        /// <summary>
        /// 平滑移动
        /// </summary>
        /// <param name="from">当前位置</param>
        /// <param name="to">目标位置</param>
        /// <param name="owner">敌人对象</param>
        /// <param name="speed">移动速度(时间区间)</param>
        public static void SmoothMove( Vector3 from, Vector3 to,EnemyLogic owner,float speed = 1)
        {
            owner.transform.position = Vector3.Lerp(from,to,Time.deltaTime * speed);
        }

        public static void SmoothMove( Vector3 from,  Vector3 to, GameObject owner, float speed = 1)
        {
            owner.transform.position = Vector3.Lerp(from, to, Time.deltaTime * speed);
        }

        public static void PerformCollisionBow(TargetableObject entity, Entity other,Vector3 point)
        {
            ArrowLogic arrow = other as ArrowLogic;
            if(arrow != null)
            {
                ImpactData entityImpactData = entity.GetImpactData();
                ImpactData bulletImpactData = arrow.GetImpactData();
                if (GetRelation(entityImpactData.Camp, bulletImpactData.Camp) == RelationType.Friendly)
                {
                    return;
                }

                int entityDamageHP = CalcDamageHP(bulletImpactData.Attack, entityImpactData.Defense);
                entity.ApplyDamage(arrow, entityDamageHP,point);
                GameEntry.Entity.HideEntity(arrow);
            }
        }

        public static void PerformCollision(TargetableObject entity, Entity other)
        {
            if (entity == null || other == null)
            {
                return;
            }
            WeaponLogicLeftHand weaponLeft = other as WeaponLogicLeftHand;
            WeaponLogicRightHand weaponRight = other as WeaponLogicRightHand;

           // WeaponLogic weapon = other as WeaponLogic;
            if (weaponLeft != null)
            {
                ImpactData entityImpactData = entity.GetImpactData();
                ImpactData bulletImpactData = weaponLeft.GetImpactData();
                if (GetRelation(entityImpactData.Camp, bulletImpactData.Camp) == RelationType.Friendly)
                {
                    return;
                }

                int entityDamageHP = CalcDamageHP(bulletImpactData.Attack, entityImpactData.Defense);

                entity.ApplyDamage(weaponLeft, entityDamageHP,Vector3.zero);
                return;
            }

            if (weaponRight != null)
            {
                ImpactData entityImpactData = entity.GetImpactData();
                ImpactData bulletImpactData = weaponRight.GetImpactData();
                if (GetRelation(entityImpactData.Camp, bulletImpactData.Camp) == RelationType.Friendly)
                {
                    return;
                }

                int entityDamageHP = CalcDamageHP(bulletImpactData.Attack, entityImpactData.Defense);

                entity.ApplyDamage(weaponRight, entityDamageHP,Vector3.zero);
                return;
            }

            //TargetableObject target = other as TargetableObject;
            //if (target != null)
            //{
            //    ImpactData entityImpactData = entity.GetImpactData();
            //    ImpactData targetImpactData = target.GetImpactData();
            //    if (GetRelation(entityImpactData.Camp, targetImpactData.Camp) == RelationType.Friendly)
            //    {
            //        return;
            //    }

            //    int entityDamageHP = CalcDamageHP(targetImpactData.Attack, entityImpactData.Defense);
            //    int targetDamageHP = CalcDamageHP(entityImpactData.Attack, targetImpactData.Defense);

            //    int delta = Mathf.Min(entityImpactData.HP - entityDamageHP, targetImpactData.HP - targetDamageHP);
            //    if (delta > 0)
            //    {
            //        entityDamageHP += delta;
            //        targetDamageHP += delta;
            //    }

            //    //entity.ApplyDamage(target, entityDamageHP);
            //    target.ApplyDamage(entity, targetDamageHP);
            //    return;
            //}

            //  Bullet bullet = other as Bullet;
            //if (bullet != null)
            //{
            //    ImpactData entityImpactData = entity.GetImpactData();
            //    ImpactData bulletImpactData = bullet.GetImpactData();
            //    if (GetRelation(entityImpactData.Camp, bulletImpactData.Camp) == RelationType.Friendly)
            //    {
            //        return;
            //    }

            //    int entityDamageHP = CalcDamageHP(bulletImpactData.Attack, entityImpactData.Defense);

            //    entity.ApplyDamage(bullet, entityDamageHP);
            //    GameEntry.Entity.HideEntity(bullet);
            //    return;
            //}
        }

        public static void PerformCollisionAttack(TargetableObject owner, TargetableObject other,Vector3 point)
        {
            if (owner == null || other == null)
            {
                return;
            }

                ImpactData ownerImpactData = owner.GetImpactData();
                ImpactData otherImpactData = other.GetImpactData();
                if (GetRelation(otherImpactData.Camp, ownerImpactData.Camp) == RelationType.Friendly)
                {
                    return;
                }
                int entityDamageHP = CalcDamageHP(ownerImpactData.Attack, otherImpactData.Defense);

            //播放击中音效
            //for (int i = 0; i < owner.Weapons.Count; i++)
            //{
            //    WeaponLogicLeftHand m_WeaponLogicLeftHand = owner.Weapons[i] as WeaponLogicLeftHand;
            //    if (m_WeaponLogicLeftHand != null)
            //    {
            //        if(m_WeaponLogicLeftHand.weaponData.WeaponHitSounds == null)
            //        {
            //            break;
            //        }
            //        int random = Utility.Random.GetRandom(0, m_WeaponLogicLeftHand.weaponData.WeaponHitSounds.Count);
            //        GameEntry.Sound.PlaySound(m_WeaponLogicLeftHand.weaponData.WeaponHitSounds[random]);
            //        break;
            //    }
            //    else
            //    {
            //        WeaponLogicRightHand m_WeaponLogicRightHand = owner.Weapons[i] as WeaponLogicRightHand;
            //        if(m_WeaponLogicRightHand != null)
            //        {
            //            if (m_WeaponLogicRightHand.weaponData.WeaponHitSounds == null)
            //            {
            //                break;
            //            }
            //            int random = Utility.Random.GetRandom(0, m_WeaponLogicRightHand.weaponData.WeaponHitSounds.Count);
            //            GameEntry.Sound.PlaySound(m_WeaponLogicRightHand.weaponData.WeaponHitSounds[random]);
            //            break;
            //        }
            //    }
            //}

            TargetBuff targetBuff = new TargetBuff();
            targetBuff.Target = other;
            targetBuff.Buff = owner.Buff;
            GameEntry.Event.Fire(owner, ApplyBuffEventArgs.Create(targetBuff));
            other.ApplyDamage(owner, entityDamageHP, point);
            return;
        }

        //public static void PerformCollisionAttack(Entity entity, TargetableObject other)
        //{
        //    if (entity == null || other == null)
        //    {
        //        return;
        //    }
        //    // WeaponLogic weapon = entity as WeaponLogic;
        //    WeaponLogicLeftHand weaponLeft = entity as WeaponLogicLeftHand;
        //    WeaponLogicRightHand weaponRight = entity as WeaponLogicRightHand;
        //    if (weaponLeft != null)
        //    {
        //        ImpactData otherImpactData = other.GetImpactData();
        //        ImpactData weaponImpactData = weaponLeft.GetImpactData();
        //        //Debug.Log(weaponLeft.transform.gameObject);
        //        if (GetRelation(otherImpactData.Camp, weaponImpactData.Camp) == RelationType.Friendly)
        //        {
        //            return;
        //        }
        //        int entityDamageHP = CalcDamageHP(weaponImpactData.Attack, otherImpactData.Defense);

        //        other.ApplyDamage(weaponLeft, entityDamageHP);
        //        return;
        //    }

        //    if (weaponRight != null)
        //    {
        //        ImpactData otherImpactData = other.GetImpactData();
        //        ImpactData weaponImpactData = weaponRight.GetImpactData();
        //        if (GetRelation(otherImpactData.Camp, weaponImpactData.Camp) == RelationType.Friendly)
        //        {
        //            return;
        //        }
        //        int entityDamageHP = CalcDamageHP(weaponImpactData.Attack, otherImpactData.Defense);
        //        other.ApplyDamage(weaponRight, entityDamageHP);
        //        return;
        //    }
        //}


        private static int CalcDamageHP(int attack, int defense)
        {
            if (attack <= 0)
            {
                return 0;
            }

            if (defense < 0)
            {
                defense = 0;
            }

            return attack * attack / (attack + defense);
        }

        [StructLayout(LayoutKind.Auto)]
        private struct CampPair
        {
            private readonly CampType m_First;
            private readonly CampType m_Second;

            public CampPair(CampType first, CampType second)
            {
                m_First = first;
                m_Second = second;
            }

            public CampType First
            {
                get
                {
                    return m_First;
                }
            }

            public CampType Second
            {
                get
                {
                    return m_Second;
                }
            }
        }
    }
}
