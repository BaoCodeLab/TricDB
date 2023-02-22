using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Main.Extensions
{
    public class TypeCompare
    {
        public class CompareVal
        {
            private string _name = string.Empty;
            private string _str1 = string.Empty;
            private string _str2 = string.Empty;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            public string Val1
            {
                get
                {
                    return _str1;
                }
                set
                {
                    _str1 = value;
                }
            }
            public string Val2
            {
                get
                {
                    return _str2;
                }
                set
                {
                    _str2 = value;
                }
            }
            public CompareVal(string name,string str1,string str2)
            {
                _name = name;
                _str1 = str1;
                _str2 = str2;
            }
        }
        public static List<CompareVal> CompareResult<T>(T obj1,T obj2,Type type,List<string> ignore)
        {
            for(int i=0;i<ignore.Count;i++)
            {
                ignore[i] = ignore[i].ToUpper();
            }
            Type t = type;
            PropertyInfo[] props = t.GetProperties();
            List<CompareVal> ret = new List<CompareVal>();
            if (obj1 == null && obj2 == null)
                return ret;
            else if (obj1 == null || obj2 == null)
            {
                foreach(var po in props)
                {
                    var name = po.Name;
                    IEnumerable<Attribute> attributes = (IEnumerable<Attribute>)po.GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (attributes.Count() > 0)
                    {
                        DisplayAttribute displayName = (DisplayAttribute)attributes.First();
                        if (displayName != null)
                        {
                            name = displayName.Name;
                        }
                    }
                    if (!ignore.Contains(name))
                    {                        
                        if (obj1 != null)
                        {
                            var v1 = po.GetValue(obj1);
                            if (v1 != null && !v1.ToString().Equals(string.Empty))
                            {
                                ret.Add(new CompareVal(name,v1.ToString(), string.Empty));
                            }
                        }
                        else
                        {
                            var v2 = po.GetValue(obj2);
                            if (v2 != null && !v2.ToString().Equals(string.Empty))
                            {
                                ret.Add(new CompareVal(name,string.Empty,v2.ToString()));
                            }
                        }
                    }
                }
                return ret;
            }
            foreach (var po in props)
            {
                var name = po.Name;
                IEnumerable<Attribute> attributes = (IEnumerable<Attribute>)po.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attributes.Count() > 0)
                {
                    DisplayAttribute displayName = (DisplayAttribute)attributes.First();
                    if (displayName != null)
                    {
                        name = displayName.Name;
                    }
                }
                if (!ignore.Contains(name))
                {
                    var v1 = po.GetValue(obj1) == null ? string.Empty : po.GetValue(obj1).ToString();
                    var v2 = po.GetValue(obj2) == null ? string.Empty : po.GetValue(obj2).ToString();
                    if (!v1.Equals(v2))
                    {
                        ret.Add(new CompareVal(name, v1 == null ? string.Empty : v1.ToString(), v2 == null ? string.Empty : v2.ToString()));
                    }
                }
            }
            return ret;
        }
        public static List<CompareVal> CompareResult<T>(T obj1, T obj2, Type type)
        {
            List<string> i = new List<string>();
            return CompareResult(obj1, obj2, type, i);
        }
    }
}
