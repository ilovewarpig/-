using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    [Serializable]
    public class Part
    {
        string id;
        string name;
        string subpath;
        float prob;
        string url;

        public void set(string id, string name, string subpath, float prob, string url)
        {
            this.id = id;
            this.name = name;
            this.subpath = subpath;
            this.prob = prob;
            this.url = url;
        }

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