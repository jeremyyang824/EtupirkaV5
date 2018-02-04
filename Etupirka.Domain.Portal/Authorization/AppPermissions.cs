namespace Etupirka.Domain.Portal.Authorization
{
    /// <summary>
    /// 系统权限名
    /// </summary>
    public static class AppPermissions
    {
        public const string Root = ".";

        //仓库管理
        //Commmon
        public const string WarehouseManage = "WarehouseManage";    //仓库管理        
        public const string WarehouseManage_ListWarehouses = "WarehouseManage_ListWarehouses";    //仓库定义
        public const string WarehouseManage_ListInventories = "WarehouseManage_ListInventories";    //库存查询
        public const string WarehouseManage_ListInventoryBills = "WarehouseManage_ListInventoryBills";    //出入库单
        //Host
        public const string WarehouseManage_ListItems = "WarehouseManage_ListItems";    //物料管理

        //寄售(领料流程)管理
        //Tenancy
        public const string ItemReceiveManage = "ItemReceiveManage"; //领用管理
        public const string ItemReceiveManage_ItemShop = "ItemReceiveManage_ItemShop"; //物料列表
        public const string ItemReceiveManage_Cart = "ItemReceiveManage_Cart"; //领料篮
        public const string ItemReceiveManage_ListReceiveBills = "ItemReceiveManage_ListReceiveBills"; //领用单列表
        public const string ItemReceiveManage_ListVerifyReceiveBills = "ItemReceiveManage_ListVerifyReceiveBills"; //领用单审批列表
        public const string ItemReceiveManage_ListIssueReceiveBills = "ItemReceiveManage_ListIssueReceiveBills"; //领用单仓库发料列表

        //寄售结算
        //Host
        public const string AccountManage = "AccountManage";    //寄售结算
        public const string AccountManage_ListPendings = "AccountManage_ListPendings";   //待结算列表
        public const string AccountManage_ListAccountBills = "AccountManage_ListAccountBills";   //结算单列表

        //待办通知
        //Common
        public const string TaskNotify = "TaskNotify";    //待办通知
        //Tenancy
        public const string TaskNotify_MyCartItems = "TaskNotify_MyCartItems";    //领料篮项
        public const string TaskNotify_ToVerifyReceiveBills = "TaskNotify_ToVerifyReceiveBills";    //待审核申领单
        public const string TaskNotify_MyVerifiedReceiveBills = "TaskNotify_MyVerifiedReceiveBills";    //待领料申领单
        public const string TaskNotify_ToIssueReceiveBills = "TaskNotify_ToIssueReceiveBills";    //待发料申领单

        //系统配置
        //Common
        public const string SystemConfig = "SystemConfig";    //系统配置
        public const string SystemConfig_ListUsers = "SystemConfig_ListUsers";    //用户管理
        public const string SystemConfig_ListRoles = "SystemConfig_ListRoles";    //角色管理
        //Host
        public const string SystemConfig_ListTenants = "SystemConfig_ListTenants"; //烟厂列表

    }
}
