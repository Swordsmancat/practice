//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace Farm.Hotfix
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId : byte
    {
        Undefined = 0,

        /// <summary>
        /// 弹出框。
        /// </summary>
        DialogForm = 1,

        /// <summary>
        /// 弹出框选项。
        /// </summary>
        DialogOptionsForm=2,

        /// <summary>
        /// 加载页面。
        /// </summary>
        LoadingForm = 100,

        /// <summary>
        /// 登录页面
        /// </summary>
        LoginForm =101,

        /// <summary>
        /// 设置页面。
        /// </summary>
        SettingForm = 109,

        /// <summary>
        /// 关于。
        /// </summary>
        AboutForm = 103,

        /// <summary>
        /// 选角页面。
        /// </summary>
        SelectRoleForm = 104,

        /// <summary>
        /// 竞技场页面。
        /// </summary>
        ArenaForm = 105,

        /// <summary>
        /// 铁匠铺页面。
        /// </summary>
        BlacksmithForm = 106,

        /// <summary>
        /// 装备页面。
        /// </summary>
        EquipmentForm = 107,

        /// <summary>
        /// 地下城页面。
        /// </summary>
        UndergroundCityForm = 108,

        /// <summary>
        /// 主页面。
        /// </summary>
        MainForm = 110,

        /// <summary>
        /// 排行榜页面。
        /// </summary>
        UIRanking = 111,

        /// <summary>
        /// 英雄选择页面。
        /// </summary>
        UISelectHero = 112,

        /// <summary>
        /// 钱包页面。
        /// </summary>
        UIMoneyBag = 113,

        /// <summary>
        /// 锁定图标
        /// </summary>
        UILock =114,

        /// <summary>
        /// 瞄准图标
        /// </summary>
        UIGunAim = 115,
    }
}
