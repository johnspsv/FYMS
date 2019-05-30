using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYMS.Model.Entities;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace FYMS.DAL.PublicDAL
{
    public class PubClass<T> where T:class,new()
    {
        #region 上下文
        /// <summary>
        /// 上下文
        /// </summary>
        protected FYMSEntities dbcontext = new FYMSEntities();
        #endregion

        #region 增
        /// <summary>
        /// 保存单条数据，返回结果(bool)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool Add(T t)
        {
            dbcontext.Entry<T>(t).State = EntityState.Added;
            return dbcontext.SaveChanges()>0;
        }

        /// <summary>
        /// 保存单条数据，返回保存的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual T AddEntity(T t)
        {
            dbcontext.Entry<T>(t).State = EntityState.Added;
            dbcontext.SaveChanges();
            return t;
        }

        /// <summary>
        /// 保存多条数据，返回保存结果(bool)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual bool Add(List<T> list)
        {
            //foreach(var t in list)
            //{
            //    dbcontext.Entry<T>(t).State = EntityState.Added;
            //}
            list.ForEach(c => dbcontext.Entry<T>(c).State = EntityState.Added);//同foreach
            return dbcontext.SaveChanges()>0;  
        }

        /// <summary>
        /// 保存多条数据，返回保存list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual List<T> AddEntities(List<T> list)
        {
            foreach (var t in list)
            {
                dbcontext.Entry<T>(t).State = EntityState.Added;
            }
            //list.ForEach(c => dbcontext.Entry<T>(c).State = EntityState.Added);//同foreach
            dbcontext.SaveChanges();
            return list;
        }
        #endregion

        #region 删(不推荐)
        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool Delete(T t)
        {
            dbcontext.Set<T>().Attach(t);
            dbcontext.Entry<T>(t).State = EntityState.Deleted;
            return dbcontext.SaveChanges()>0;
        }

        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual bool DeleteEntities(Func<T,bool> func)
        {
            var data = dbcontext.Set<T>().Where<T>(func).ToList();
            foreach(var p in data)
            {
                dbcontext.Entry<T>(p).State = EntityState.Deleted;
            }
            return dbcontext.SaveChanges() > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual bool DeleteEntities(List<T> list)
        {
            foreach (var t in list)
            {
                dbcontext.Entry<T>(t).State = EntityState.Deleted;
            }
            return dbcontext.SaveChanges() > 0;
        }
        #endregion

        #region 改
        /// <summary>
        /// 更新单条数据,修改所有列的值，没有赋值的属性会被赋予属性类型的默认值
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool Update(T t)
        {
            try
            {
                dbcontext.Set<T>().Attach(t);
                dbcontext.Entry<T>(t).State = EntityState.Modified;//属性都将被标记为修改状态
                return dbcontext.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新单条数据，并返回保存的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual T UpdateEntity(T t)
        {
            dbcontext.Set<T>().Attach(t);
            dbcontext.Entry<T>(t).State = EntityState.Modified;//属性都将被标记为修改状态
            dbcontext.SaveChanges();
            return t;
        }

        /// <summary>
        /// 更新单条数据，修改制定列的值
        /// </summary>
        /// <param name="t">要修改的实体对象 </param>
        /// <param name="proNames">要修改的属性名称</param>
        /// <returns></returns>
        public virtual bool Update(T t,params string[] proNames)
        {
            dbcontext.Set<T>().Attach(t);
            DbEntityEntry<T> entity = dbcontext.Entry<T>(t);
            entity.State = EntityState.Unchanged;//将属性标记为未修改
            proNames.ToList().ForEach(x => entity.Property(x).IsModified = true);
            return dbcontext.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新单条数据，并返回保存的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual T UpdateEntity(T t, params string[] proNames)
        {
            dbcontext.Set<T>().Attach(t);
            var entity = dbcontext.Entry<T>(t);
            entity.State = EntityState.Unchanged;//将属性标记为未修改
            proNames.ToList().ForEach(x => entity.Property(x).IsModified = true);
            dbcontext.SaveChanges();
            return t;
        }

        /// <summary>
        /// 批量修改返回结果
        /// </summary>
        /// <param name="t"></param>
        /// <param name="func"></param>
        /// <param name="proNames"></param>
        /// <returns></returns>
        public virtual bool UpdateEntities(T t,Func<T,bool> func, params string[] proNames)
        {
            var entities = dbcontext.Set<T>().Where(func).ToList();
            PropertyInfo[] proinfos = t.GetType().GetProperties();
            List<PropertyInfo> list = new List<PropertyInfo>();
            foreach(var p in proinfos)
            {
                if(proNames.Contains(p.Name))
                {
                    list.Add(p);
                }
            }
            entities.ForEach(x =>
            {
                foreach (var p in list)
                {
                    object value = p.GetValue(t, null);
                    p.SetValue(x, value, null);
                }
            });
            return dbcontext.SaveChanges() > 0;
        }

        #endregion

        #region 查

        /// <summary>
        /// 根据lambds表达式查询
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual IList<T> SelectEntities(Func<T,bool> func)
        {
            return dbcontext.Set<T>().Where<T>(func).ToList();
        }

        /// <summary>
        /// 查询带排序
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="func"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderbyLambds"></param>
        /// <returns></returns>
        public virtual IList<T> SelectEntities<S>(Func<T,bool> func, bool isAsc,Func<T,S> orderbyLambds)
        {
            var temp = dbcontext.Set<T>().Where<T>(func);
            if(isAsc)
            {
               return temp.OrderBy<T, S>(orderbyLambds).ToList();
            }
            else
            {
                return temp.OrderByDescending<T, S>(orderbyLambds).ToList();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereFun"></param>
        /// <param name="selectFun"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Query(Expression<Func<T, bool>> whereFun = null, Expression<Func<T, T>> selectFun = null)
        {
            IQueryable<T> result = dbcontext.Set<T>();
            if (whereFun != null) result = result.Where(whereFun);
            if (selectFun != null) result = result.Select(selectFun);
            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rows"></param>
        /// <param name="totalPage"></param>
        /// <param name="func"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderByLambds"></param>
        /// <returns></returns>
        public virtual IList<T> PageOuery<S>(int pageIndex,int pageSize,out int rows,out int totalPage,Func<T,bool> func, bool isAsc,Func<T,S> orderByLambds)
        {
            var temp = dbcontext.Set<T>().Where<T>(func);
            rows = temp.Count();
            if(rows%pageSize==0)
            {
                totalPage = rows / pageSize;
            }
            else
            {
                totalPage = rows / pageSize + 1;
            }
            if(isAsc)
            {
                temp = temp.OrderBy<T, S>(orderByLambds);
            }
            else
            {
                temp = temp.OrderByDescending<T, S>(orderByLambds);
            }
            temp = temp.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);

            return temp.ToList<T>();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageNo">页码</param>
        /// <param name="pageCount">页数</param>
        /// <param name="whereFun">查询条件</param>
        /// <param name="selectFun">返回数据</param>
        /// <param name="orderByFun">排序</param>
        /// <param name="total">总数</param>
        /// <param name="isAsc">排序，默认升序</param>
        /// <returns>list</returns>
        public virtual List<T> PageQuery<S>(int pageNo, int pageCount, Expression<Func<T, bool>> whereFun,
            Expression<Func<T, T>> selectFun, Expression<Func<T, S>> orderByFun, out int total, bool isAsc = true)
        {
            total = 0;
            int startIndex = pageCount * (pageNo - 1);
            var list = dbcontext.Set<T>().Where(whereFun);
            total = list.Count();
            if (total <= 0) return new List<T>();
            if (isAsc)
                list = list.OrderBy(orderByFun).Skip(startIndex).Take(pageCount).Select(selectFun);
            else
                list = list.OrderByDescending(orderByFun).Skip(startIndex).Take(pageCount).Select(selectFun);

            return list.ToList();
        }
        #endregion

       
    }
}
