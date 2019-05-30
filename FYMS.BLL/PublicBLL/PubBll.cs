using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FYMS.BLL.PublicBLL
{
    public class PubBll<T> where T : class, new()
    {
        /// <summary>
        /// 反序列化返回单一对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public virtual T ReturnEntity(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
               
                T t = JsonConvert.DeserializeObject<T>(str);
                return t;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 反序列化返回对象集合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public virtual List<T> ReturnEntities(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                List<T> list = (List<T>)JsonConvert.DeserializeObject(str);
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将单一对象序列化为Json返回Json字符串
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual string ReturnStr(T t)
        {
            if (t != null)
            {
                string str = JsonConvert.SerializeObject(t);
                return str;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将对象集合序列化为json返回json字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual string ReturnStr(List<T> list)
        {
            if (list.Count > 0 || list != null)
            {
                string str = JsonConvert.SerializeObject(list);
                return str;
            }
            else
            {
                return null;
            }
        }

        

        /// <summary>
        /// ViewMode给Model复制，省去一行一行赋值(新增)
        /// </summary>
        /// <typeparam name="T">被赋值对象</typeparam>
        /// <typeparam name="S">取值对象</typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public  T New<S>(S s)
        {
            T t = Activator.CreateInstance<T>();
            try
            {
                var types = s.GetType();
                var typesof = typeof(T);
                foreach (PropertyInfo p in types.GetProperties())
                {
                    foreach (PropertyInfo x in typesof.GetProperties())
                    {
                        if (p.Name == x.Name)
                        {
                            x.SetValue(t, p.GetValue(s));
                            break;
                            //x.SetValue(t, p.GetValue(s, null), null);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }

        /// <summary>
        /// ViewMode给Model复制，省去一行一行赋值
        /// </summary>
        /// <typeparam name="T">被赋值对象</typeparam>
        /// <typeparam name="S">取值对象</typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public T Edit<S>(S s,T t)
        {
            try
            {
                var types = s.GetType();
                var typesof = typeof(T);
                foreach (PropertyInfo p in types.GetProperties())
                {
                    foreach (PropertyInfo x in  typesof.GetProperties())
                    {
                        if (p.Name == x.Name)
                        {
                            x.SetValue(t, p.GetValue(s));
                            break;
                            //x.SetValue(t, p.GetValue(s, null), null);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }

    }
}
