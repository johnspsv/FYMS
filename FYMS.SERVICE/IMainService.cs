using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FYMS.BLL;

namespace FYMS.SERVICE
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IMainService”。
    [ServiceContract]
    public interface IMainService
    {

        [OperationContract]
        void DoWork();
        #region 登录
        [OperationContract]
        //登录
        string[] login(string username, string password);

        [OperationContract]
        string loginlimit(string username, string password);
        #endregion

        #region 管理员管理
        [OperationContract]
        string AdminUserAll();

        [OperationContract]
        string Add(string str);

        [OperationContract]
        string AdminByID(int id);

        [OperationContract]
        string Update(string str);

        [OperationContract]
        string Delete(int id);
        #endregion

        #region 管理员角色管理
        [OperationContract]
        string AdminRoleAll();

        [OperationContract]
        string AdminRoleAdd(string str);

        [OperationContract]
        string AdminRoleByID(int id);

        [OperationContract]
        string AdminRoleUpdate(string str);

        [OperationContract]
        string AdminRoleDelete(int id);
        #endregion

        #region 管理员角色关系管理
        [OperationContract]
        string AdminRoleRelAll();
        [OperationContract]
        string AdminRoleRelAdd(string str);
        [OperationContract]
        string AdminRoleRelUpdate(string str);
        [OperationContract]
        string AdminRoleRelEntity(int id);
        [OperationContract]
        string RoleRelOpen(int id);
        #endregion

        #region 导航栏管理
        [OperationContract]
        string MenuAll();
        [OperationContract]
        string MenuAdd(string str);
        [OperationContract]
        string MenuUpdate(string str);
        [OperationContract]
        string MenuEntity(int id);
        [OperationContract]
        string MenuOpen(int id);
        #endregion

        #region 权限管理
        [OperationContract]
        string LimitAll();

        [OperationContract]
        bool LimitAdd(string str, string str1);

        [OperationContract]
        bool LimitUpdate(string[] str);

        [OperationContract]
        string[] LimitEditData(int id);

        [OperationContract]
        string LimitDelete(int id);
        #endregion

        #region 日志
        [OperationContract]
        void loginlog(string[] str);
        [OperationContract]
        void Errorlog(string ex);
        [OperationContract]
        void controllog(string str, string functionname, string other);
        [OperationContract]
        void errorcontrollog(string str, string functionname, string other, string ex);
        #endregion

        #region 前后台日志
        [OperationContract]
        string ErrorLogAll();


        [OperationContract]
        string ErrorLogByID(int ID);

        [OperationContract]
        string AdminLogAll();

        [OperationContract]
        string ControlLogAll();

        [OperationContract]
        string ControlLogByID(int ID);


        [OperationContract]
        string CTErrorLogAll();

        [OperationContract]
        string CTLoginLogAll();


        [OperationContract]
        string CTErrorLogByID(int ID);


        [OperationContract]
        string CTControlLogAll();


        [OperationContract]
        string CTControlLogByID(int ID);

        #endregion

        #region 审核类型
        [OperationContract]
        string CheckTypeAll();
        [OperationContract]
        string CheckTypeAdd(string str);
        [OperationContract]
        string CheckTypeUpdate(string str);
        [OperationContract]
        string CheckTypeDelete(int id);

        #endregion 

        #region 角色审核权限
        [OperationContract]
        string CheckRoleRelAll();

        [OperationContract]
        bool CheckRoleRelAdd(string str);

        [OperationContract]
        bool CheckRoleRelUpdate(string str);

        [OperationContract]
        string DataByRoleID(int roleid);

        [OperationContract]
        string[] CheckRoleRelEdit(int roleid);

        [OperationContract]
        bool CheckRoleRelDelete(int roleid);
        #endregion

        #region 客户端注册
        [OperationContract]
        bool ht_CheckTableSave(string str);
        #endregion

        #region 人员审核
        [OperationContract]
        string CheckTableAll();
        [OperationContract]
        string CheckTableByID(int id);
        [OperationContract]
        bool CheckTableSave(string str);
        [OperationContract]
        bool CheckTableNoSuccess(int id);
        #endregion

        #region 账号类型
        [OperationContract]
        string AccountTypeAll();
        [OperationContract]
        string AccountTypeAdd(string str);
        [OperationContract]
        string AccountTypeUpdate(string str);
        [OperationContract]
        string AccountTypeDelete(int id);
        #endregion

    }
}
