using UnityEngine;
using System.Collections.Generic;
using System;

namespace Farm.Hotfix
{
    public enum WeaponEnum
    {
        SwordShield,
        SwordShield_PutDown,
        GiantSword,
        GiantSword_PutDown,
        Dagger,
        Dagger_PutDown,
        DoubleBlades,
        DoubleBlades_PutDown,
        Pistol,
        Pistol_PutDown,
        RevengerDoubleBlades,
        RevengerDoubleBlades_PutDown,
    }
    public class WeaponInfo:MonoBehaviour
    {
        public Dictionary<WeaponEnum, WeaponInfoCategory> weaponDir;

        public void Init()
        {
            weaponDir = new Dictionary<WeaponEnum, WeaponInfoCategory>();
            InitWeaponInfo();
        }

        private void InitWeaponInfo()
        {
            //剑盾 拿起
            weaponDir.Add(WeaponEnum.SwordShield, new WeaponInfoCategory
            {
                leftHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.021f, 0.031f, -0.008f),
                    rotation = Quaternion.Euler(-10.673f, -13.023f, -156.403f),
                    scale = Vector3.one
                },
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.08f, 0.003f, -0.03f),
                    rotation = Quaternion.Euler(-68.139f, -115.786f, -3.806f),
                    scale = Vector3.one
                }
            });

            //大剑 拿起
            weaponDir.Add(WeaponEnum.GiantSword, new WeaponInfoCategory
            {
                leftHand = null,
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.093f, 0.027f, -0.04f) ,
                    rotation = Quaternion.Euler(-10.598f, 104.244f, -97.312f),
                    scale = new Vector3(1f, 1f, 1f)
                }
            }) ;

            //匕首 拿起
            weaponDir.Add(WeaponEnum.Dagger, new WeaponInfoCategory
            {
                leftHand = null,
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.224f, 0.536f, -0.037f),
                    rotation = Quaternion.Euler(265.451f, 42.022f, -118.792f),
                    scale = Vector3.one
                }
            });

            //双刀 拿起
            weaponDir.Add(WeaponEnum.DoubleBlades, new WeaponInfoCategory
            {
                leftHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.237f, -0.344f, 0.067f),
                    rotation = Quaternion.Euler(-20.465f, -9.224f, -202.737f),
                    scale = Vector3.one,
                },
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.227f, 0.368f, 0.01f),
                    rotation = Quaternion.Euler(-15.726f, 152.72f, -17.037f),
                    scale = Vector3.one,
                }
            });

            //火枪 拿起
            weaponDir.Add(WeaponEnum.Pistol, new WeaponInfoCategory
            {
                 rightHand = new WeaponInfoTransform 
                 {
                     position = new Vector3(-0.761f, -0.004f, -0.048f),
                     rotation = Quaternion.Euler(0f, 0, 0),
                     scale = new Vector3(1f, 1f, 1f),
                 },
                leftHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.099f, 0.019f, -0.043f),
                    rotation = Quaternion.Euler(-82f, 0, 150f),
                    scale = new Vector3(1f, 1f, 1f),
                }
            });
            //复仇双刀 拿起
            weaponDir.Add(WeaponEnum.RevengerDoubleBlades, new WeaponInfoCategory
            {
                leftHand = new WeaponInfoTransform
                {
                    position = new Vector3(0.095f, 0.322f, -0.124f),
                    rotation = Quaternion.Euler(-198.428f, -6.08f, -155.9f),
                    scale = Vector3.one,
                },
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(0.0623f, -0.361f, 0f),
                    rotation = Quaternion.Euler(2.425f, -14.669f, -160.121f),
                    scale = Vector3.one,
                }
            });
            //剑盾 收起
            weaponDir.Add(WeaponEnum.SwordShield_PutDown, new WeaponInfoCategory
            {
                leftHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.078f, -0.351f, -0.125f),
                    rotation = Quaternion.Euler(12.949f, -156.263f, 28.191f),
                    scale = Vector3.one
                },
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.075f, -0.069f, -0.179f),
                    rotation = Quaternion.Euler(-62.83f, -5.7f, -232.746f),
                    scale = Vector3.one
                }
            });

            //大剑 收起
            weaponDir.Add(WeaponEnum.GiantSword_PutDown, new WeaponInfoCategory
            {
                leftHand = null,
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.136f, -0.192f, -0.271f),
                    rotation = Quaternion.Euler(355.87f, -615.162f, 655.463f),
                    scale = new Vector3(1f, 1f, 1f)
                }
            });

            //匕首 收起
            weaponDir.Add(WeaponEnum.Dagger_PutDown, new WeaponInfoCategory
            {
                leftHand = null,
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.189f, 0.251f, -0.017f),
                    rotation = Quaternion.Euler(249.416f, -200.092f, -222.428f),
                    scale = Vector3.one
                }
            });

            //双刀 收起
            weaponDir.Add(WeaponEnum.DoubleBlades_PutDown, new WeaponInfoCategory
            {
                leftHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.045f, -0.349f, -0.064f),
                    rotation = Quaternion.Euler(-16.006f, -336.26f, -182.382f),
                    scale = Vector3.one
                },
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.046f, 0.329f, -0.023f),
                    rotation = Quaternion.Euler(-19.648f, -150.343f, -1.235f),
                    scale = Vector3.one
                }
            });

            //火枪 收起
            weaponDir.Add(WeaponEnum.Pistol_PutDown, new WeaponInfoCategory
            {
                rightHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.761f, -0.004f, -0.048f),
                    rotation = Quaternion.Euler(0f, 0, 0),
                    scale = new Vector3(1f, 1f, 1f),
                },
                leftHand = new WeaponInfoTransform
                {
                    position = new Vector3(-0.18f, 0.23f, -0.05f),
                    rotation = Quaternion.Euler(-24, -160, -87),
                    scale = new Vector3(1f, 1f, 1f),
                }
            });
        }
    }

    

    public class WeaponInfoCategory
    {
        public WeaponInfoTransform leftHand;
        public WeaponInfoTransform rightHand;
    }

    public class WeaponInfoTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public  Vector3 scale;
    }   

    public class SkillInfo : MonoBehaviour
    {
        public Dictionary<WeaponEnum, List<SkillInfoItem>> skillDir;

        public List<SkillInfoItem> m_SkillGreatSword = new List<SkillInfoItem>();
        public List<SkillInfoItem> m_SkillTwoSword = new List<SkillInfoItem>();
        public List<SkillInfoItem> m_SkillSwordShield = new List<SkillInfoItem>();

        public void Init()
        {
            skillDir = new Dictionary<WeaponEnum, List<SkillInfoItem>>();

            AddGreatSwordSkillInfo();
            AddTwoSwordSkillInfo();
            AddSwordShieldSkillInfo();
            InitSkillInfo();
        }

        private void InitSkillInfo()
        {
            skillDir.Add(WeaponEnum.GiantSword, m_SkillGreatSword);
            skillDir.Add(WeaponEnum.DoubleBlades, m_SkillTwoSword);
            skillDir.Add(WeaponEnum.SwordShield, m_SkillSwordShield);
        }

        /// <summary>
        /// 大剑技能信息
        /// </summary>
        private void AddGreatSwordSkillInfo()
        {
            //1.上寮斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 1,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.77f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.8f,
                    }
                }
            });
            //2.回旋斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 2,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.75f,
                    },
                }
            });
            //3.屠龙斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 3,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.75f,
                    },
                }

            });
            //4.屠龙风暴斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 4,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.5f,
                    },
                }

            });
            //5.断头斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 5,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.6f,
                    },
                }

            });
            //6.滑步斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 6,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.72f,
                    },
                }

            });
            //7.蛮力几式
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 7,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.6f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.72f,
                    },
                    new SkillLevel
                    {
                        skillLev = 3,
                        exitTime = 0.75f,
                    },
                }

            });
            //8.蛮力突刺
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 8,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.7f,
                    },
                }
            });
            //9.肩撞
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 9,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.9f,
                    },
                }

            });
            //10.跳跃回旋斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 10,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.75f,
                    },
                }
            });
            //11.真·屠龙斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 11,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.75f,
                    },
                }
            });
            //12.真·屠龙风暴斩
            m_SkillGreatSword.Add(new SkillInfoItem
            {
                skillNum = 12,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.65f,
                    },
                }
            });

        }

        /// <summary>
        /// 双剑技能信息
        /// </summary>
        private void AddTwoSwordSkillInfo()
        {
            //1.升龙斩
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 1,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.93f,
                    }
                }
            });
            //2.双刃乱舞
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 2,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.47f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.75f,
                    }
                }
            });
            //3.双刃猛击
            m_SkillTwoSword.Add(new SkillInfoItem
            {
                skillNum = 3,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.65f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.65f,
                    },
                    new SkillLevel
                    {
                        skillLev = 3,
                        exitTime = 0.63f,
                    },
                    new SkillLevel
                    {
                        skillLev = 4,
                        exitTime = 0.6f,
                    }
                },

            });
            //4.双刃突刺
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 4,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.75f,
                    }
                }
            });
            //5.左斜斩
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 5,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.66f,
                    }
                }
            });
            //6.游龙三刀
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 6,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.8f,
                    }
                }

            });
            //7.游龙斩
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 7,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.85f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.75f,
                    }
                }

            });
            //8.游龙踢
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 8,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.8f,
                    }
                }

            });
            //9.致命一击：背刺
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 9,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.92f,
                    }
                }

            });
            //10.蜂窝刺
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 10,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.7f,
                    }
                }

            });
            //11.蜂窝跳斩
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 11,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.79f,
                    }
                }

            });
            //12.蜂舞斩
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 12,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.8f,
                    }
                }

            });
            //13.蜂舞旋风斩
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 13,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.83f,
                    }
                }

            });
            //14.退龙击
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 14,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.63f,
                    }
                }
            });
            //15.撤步斩
            m_SkillTwoSword.Add(new SkillInfoItem 
            {
                skillNum = 15,
                m_SkillLevel = 
                {
                    new SkillLevel 
                    { 
                        skillLev = 1, 
                        exitTime = 0.67f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.58f,
                    },
                }
            });
        }

        /// <summary>
        /// 剑盾技能信息
        /// </summary>
        private void AddSwordShieldSkillInfo()
        {
            //1.回旋盾击
            m_SkillSwordShield.Add(new SkillInfoItem
            { 
                skillNum = 1,
                m_SkillLevel = 
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.82f,
                    }
                },
            });
            //2.斗士冲撞
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 2,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.8f,
                    }
                },
            });
            //3.斗士回旋
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 3,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.84f,
                    }
                },
            });
            //4.斗士斩龙者
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 4,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.81f,
                    }
                },
            });
            //5.斗士猛砍
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 5,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.6f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.72f,
                    }
                },
            });
            //6.斗士盾击
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 6,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.6f,
                    }
                },
            });
            //7.斗士脚踢
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 7,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.65f,
                    }
                },
            });
            //8.斗士遗魂
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 8,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.83f,
                    }
                },
            });
            //9.斩龙击
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 9,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.75f,
                    }
                },
            });
            //10.断头击
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 10,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.7f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.82f,
                    },
                },
            });
            //11.盾牌反击
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 11,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.71f,
                    }
                },
            });
            //12.盾牌猛击
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 12,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.75f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.73f,
                    },
                    new SkillLevel
                    {
                        skillLev = 3,
                        exitTime = 0.85f,
                    },
                },
            });
            //13.突刺
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 13,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.65f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.7f,
                    },
                },
            });
            //14.贯龙击
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 14,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.82f,
                    }
                },
            });
            //15.轻刺
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 15,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.7f,
                    }
                },
            });
            //16.连续突刺
            m_SkillSwordShield.Add(new SkillInfoItem
            {
                skillNum = 16,
                m_SkillLevel =
                {
                    new SkillLevel
                    {
                        skillLev = 1,
                        exitTime = 0.78f,
                    },
                    new SkillLevel
                    {
                        skillLev = 2,
                        exitTime = 0.85f,
                    },
                },
            });

        }
    }

    public class SkillInfoItem
    {
        public int skillNum;    //技能编号
        public List<SkillLevel> m_SkillLevel = new List<SkillLevel>();
    }

    public class SkillLevel
    {
        public int skillLev;    //技能等级
        public float exitTime;  //退出时间

    }
}


