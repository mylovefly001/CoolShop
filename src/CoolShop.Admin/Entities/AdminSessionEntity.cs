using System;
using System.Collections.Generic;

namespace CoolShop.Admin.Entities
{
    public class AdminSessionEntity
    {
        /// <summary>
        /// 管理员ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        
        /// <summary>
        /// 是否超级管理员组
        /// </summary>
        public int IsRoot { get; set; }

        /// <summary>
        /// 当前ctr
        /// </summary>
        public string CurrentCtr { get; set; } = string.Empty;

        /// <summary>
        /// 当前act
        /// </summary>
        public string CurrentAct { get; set; } = string.Empty;
        
        public List<string> MenuKeys { get; set; } = new List<string>();

        public List<MenuDetailEntity> MenuDetail { get; set; } = new List<MenuDetailEntity>();

        public string LastLoginIp { get; set; } = string.Empty;

        public DateTime LastLoginTime { get; set; } = DateTime.Now;
    }
}