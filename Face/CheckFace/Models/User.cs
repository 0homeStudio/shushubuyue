using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckFace.Models
{
    public class User
    {
        /// <summary>
        /// 用户年龄
        /// </summary>
        private int age;
        public int Age
        {
            set
            {
                if (value < 0 || value > 120) age = 50;
            }
            get { return age; }
        }
        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 所在地区
        /// </summary>
        public string Local { get; set; }
    }
}