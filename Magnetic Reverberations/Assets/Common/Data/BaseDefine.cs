using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class BaseDefine
    {
        public int ID { get; set; } // 角色ID
        public string Name { get; set; } // 角色名称
        public string Description { get; set; } // 角色描述
        public string Copywriting { get; set; } // 文案
        public string Resource { get; set; } // 资源路径
    }
}
