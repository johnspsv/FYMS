using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FYMS.BLL;
namespace FYMS.SERVICE
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“MainService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 MainService.svc 或 MainService.svc.cs，然后开始调试。
    public class MainService : IMainService
    {
        public void DoWork()
        {
        }

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int login(string username,string password)
        {
            return ht_admin_userBLL.Login(username,password);
        }
        #endregion

        #region 管理员管理
        /// <summary>
        /// 获取所有管理员
        /// </summary>
        /// <returns></returns>
        public string AdminUserAll()
        {
            
            return ht_admin_userBLL.All();
        }

        /// <summary>
        /// 保存管理员
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Add(string str)
        {
            
            return ht_admin_userBLL.UserAdd(str);
        }

        /// <summary>
        /// 根据ID获取管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string AdminByID(int id)
        {
            
            return ht_admin_userBLL.ModelByID(id);
        }

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Update(string str)
        {
            
            return ht_admin_userBLL.UserUpdate(str);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(int id)
        {
            return ht_admin_userBLL.Delete(id);
        }
        #endregion

        #region 管理员角色管理
        /// <summary>
        /// 获取所有管理员角色
        /// </summary>
        /// <returns></returns>
        public string AdminRoleAll()
        {

            return ht_admin_userroleBLL.All();
        }

        /// <summary>
        /// 新增管理员角色
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string AdminRoleAdd(string str)
        {

            return ht_admin_userroleBLL.UserRoleAdd(str);
        }

        /// <summary>
        /// 根据ID获取管理员角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string AdminRoleByID(int id)
        {
            return ht_admin_userroleBLL.ModelByID(id);
        }

        /// <summary>
        /// 更新管理员角色
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string AdminRoleUpdate(string str)
        {

            return ht_admin_userroleBLL.UserRoleUpdate(str);
        }

        /// <summary>
        /// 删除管理角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string AdminRoleDelete(int id)
        {
            return ht_admin_userroleBLL.Delete(id);
        }
        #endregion

        #region 人员角色关系管理
        /// <summary>
        /// 获取所有管理员角色
        /// </summary>
        /// <returns></returns>
        public string AdminRoleRelAll()
        {

            return ht_userroleRelBLL.All();
        }

        /// <summary>
        /// 人员角色关系新增
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string AdminRoleRelAdd(string str)
        {
            return ht_userroleRelBLL.UserRoleRelAdd(str);
        }

        /// <summary>
        /// 人员角色关系更新
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string AdminRoleRelUpdate(string str)
        {
            return ht_userroleRelBLL.UserRoleRelUpdate(str);
        }

        /// <summary>
        /// 根据ID获取人员角色关系对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string AdminRoleRelEntity(int id)
        {
            return ht_userroleRelBLL.ModelByID(id);
        }

        /// <summary>
        /// 禁用开启
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string RoleRelOpen(int id)
        {
            return ht_userroleRelBLL.RoleRelDelete(id);
        }
        #endregion

        #region 导航栏管理
        /// <summary>
        /// 查询导航栏所有数据
        /// </summary>
        /// <returns></returns>
        public string MenuAll()
        {
            return Ht_MenuBLL.MenuAll();
        }

        /// <summary>
        /// 新增导航栏
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string MenuAdd(string str)
        {
            return Ht_MenuBLL.MenuAdd(str);
        }

        /// <summary>
        /// 更新导航栏
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string MenuUpdate(string str)
        {
            return Ht_MenuBLL.MenuUpdate(str);
        }

        /// <summary>
        /// 根据ID获取导航栏对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string MenuEntity(int id)
        {
            return Ht_MenuBLL.ModelByID(id);
        }

        /// <summary>
        /// 禁用开启
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string MenuOpen(int id)
        {
            return Ht_MenuBLL.MenuDelete(id);
        }
        #endregion
    }
}
