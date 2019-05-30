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
        
        [OperationContract]
        //登录
        int login(string username, string password);

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

    }
}
