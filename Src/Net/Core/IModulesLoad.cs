﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Core
{
    /// <summary>
    /// 模块加载
    /// </summary>
    public interface IModulesLoad
    {
        /// <summary>
        /// 配置模块
        /// </summary>
        void Execute();
    }
}
