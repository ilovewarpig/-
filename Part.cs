using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    [Serializable]
    // arduino部件的类，通过session跨页面传递识别结果.
    public class Part
    {
        string id;  // 标识码
        string name;  // 部件名称
        string subpath;  // 示例图片在本地的存储位置
        float prob;  // 模型预测出的概率
        string url;  // 数字资源的网址

        // 设置对象属性
        public void set(string id, string name, string subpath, float prob, string url)
        {
            this.id = id;
            this.name = name;
            this.subpath = subpath;
            this.prob = prob;
            this.url = url;
        }

        // 获取对象属性
        public string get_id()
        {
            return this.id;
        }

        public string get_name()
        {
            return this.name;
        }

        public string get_subpath()
        {
            return this.subpath;
        }

        public float get_prob()
        {
            return this.prob;
        }

        public string get_url()
        {
            return this.url;
        }

    }
}
