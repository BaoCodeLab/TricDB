using Newtonsoft.Json;
using System.Collections.Generic;
using TDSCoreLib;

namespace Main.ViewModels
{
    /// <summary>
    /// 复选框状态
    /// </summary>
    public class checkArr
    {

        public checkArr(string _type,string _isChecked)
        {
            type = _type;
            isChecked = _isChecked;
        }
        
        /// <summary>
        /// 多选框类型（用于每个节点有多个复选框的情况，默认为0，如需增加则依次排序）
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// isChecked属性的值范围为：0-未选中，1-选中，2-半选。
        /// </summary>
        public string isChecked { get; set; }
    }
    /// <summary>
    /// 组织机构树，属性根据dtree组件Yes
    /// https://fly.layui.com/extend/dtree/
    /// </summary>
    public partial class VM_PF_ORG_Tree
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        [JsonProperty("id")]
        public string GID { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        [JsonProperty("title"), enableSearch]
        public string TITLE { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>
        [JsonProperty("parentId")]
        public string SUPER { get; set; }
        /// <summary>
        /// 深度
        /// </summary>
        [JsonProperty("level"), enableSearch]
        public int DEPTH { get; set; }

        /// <summary>
        /// 组织路径
        /// </summary>
        [JsonProperty("basicData")]
        public string PATH { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<VM_PF_ORG_Tree> children { get; set; }
        /// <summary>
        /// 是否叶子节点
        /// </summary>
        public bool isLast { get; set; }
        /// <summary>
        /// 节点展开状态
        /// </summary>
        public bool spread { get; set; }

        /// <summary>
        /// 复选框状态，无此属性则不开启复选框
        /// </summary>
        public checkArr checkArr { get; set; }
    }
}
