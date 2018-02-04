using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Organizations;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Manufacture.Services
{
    /// <summary>
    /// 交接单创建工厂
    /// </summary>
    public class HandOverBillFactory : DomainService
    {
        private readonly IRepository<HandOverBill, int> _handOverBillRepository;

        public HandOverBillFactory(
            IRepository<HandOverBill, int> handOverBillRepository)
        {
            this._handOverBillRepository = handOverBillRepository;
        }

        /// <summary>
        /// 创建交接单
        /// </summary>
        /// <param name="creator">创建人</param>
        /// <param name="sourceDepartment">转出部门</param>
        /// <returns>交接单</returns>
        public async Task<HandOverBill> CreateHandOverBill(SysUser creator, OrganizationUnit sourceDepartment)
        {
            if (creator == null)
                throw new ArgumentNullException("creator");
            if (sourceDepartment == null)
                throw new ArgumentNullException("sourceDepartment");

            //创建交接单号
            string billCodePrefix = DateTime.Today.ToString("yyyyMM");
            string billCodeSerialNumber = (await this._handOverBillRepository.GetAll()
                .Where(b => b.BillCodePrefix == billCodePrefix)
                .MaxAsync(b => b.BillCodeSerialNumber));
            billCodeSerialNumber = !string.IsNullOrWhiteSpace(billCodeSerialNumber) ? (int.Parse(billCodeSerialNumber) + 1).ToString("D3") : "001";

            //转出部门
            var transferSource = HandOverDepartment.CreateFromOrganizationUnit(sourceDepartment);

            HandOverBill billBean = new HandOverBill
            {
                BillCodePrefix = billCodePrefix,
                BillCodeSerialNumber = billCodeSerialNumber,
                TransferSource = transferSource,
                TransferTargetType = HandOverTargetType.Department,
                TransferTargetDepartment = new HandOverDepartment(),
                TransferTargetSupplier = new HandOverSupplier(),
                Remark = "",
                CreatorUserName = creator.Surname,
                BillState = HandOverBillState.Draft
            };

            return billBean;
        }
    }
}
