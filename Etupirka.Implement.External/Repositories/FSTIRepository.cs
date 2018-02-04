using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Abp.Configuration;
using Abp.Domain.Uow;
using Dapper;
using Etupirka.Domain.External.Entities.Fsti;
using Etupirka.Domain.External.Fsti;
using Etupirka.Domain.External.Repositories;
using Etupirka.Implement.External.Infrasturctures;

namespace Etupirka.Implement.External.Repositories
{
    /// <summary>
    /// FS ERP FSTI
    /// </summary>
    public class FSTIRepository : EtupirkaExternalRepositoryBase, IFSTIRepository
    {
        private readonly FSTIHelper _fstiHelper;
        private readonly FSRepository _fsRepository;

        public FSTIRepository(
            FSTIHelper fstiHelper,
            FSRepository fsRepository)
        {
            this._fstiHelper = fstiHelper;
            this._fsRepository = fsRepository;
        }


        /// <summary>
        /// COMT Add_Header
        /// </summary>
        public FstiResult ComtAddHeader(FstiToken token, ComtAddInput input)
        {
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                string result = fstiService.COMT_ADDHeader(token.Token.ToString(),
                    input.CoNumber,
                    input.CustomerId);
                return FstiResult.Build(result);
            }
        }

        /// <summary>
        /// COMT Add_Line
        /// </summary>
        public List<FstiResult> ComtAddLines(FstiToken token, ComtAddInput input)
        {
            List<FstiResult> resultList = new List<FstiResult>();
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                foreach (var line in input.ComtAddLines)
                {
                    string result = fstiService.COMT_ADDLine(token.Token.ToString(),
                        input.CoNumber,
                        line.CoLineNumber.ToString(),
                        line.ItemNumber,
                        line.ItemOrderedQuantity.ToString("0.000000"),
                        line.PromisedShipDate.ToString("yyMMdd"),
                        line.CoLineStatus.ToString(),
                        line.ItemControllingNetUnitPrice.ToString("0.000000"));
                    resultList.Add(FstiResult.Build(result));
                }
            }
            return resultList;
        }

        /// <summary>
        /// COMT Add_Line_Text
        /// </summary>
        public List<FstiResult> ComtAddLineTexts(FstiToken token, ComtAddInput input)
        {
            List<FstiResult> resultList = new List<FstiResult>();
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                foreach (var line in input.ComtAddLines)
                {
                    if (line.IsNeedAddLineText())
                    {
                        string result = fstiService.COMT_ADDLineText(token.Token.ToString(),
                            input.CoNumber,
                            line.CoLineNumber.ToString(),
                            line.TextLine1 ?? "",
                            line.TextLine2 ?? "",
                            line.TextLine3 ?? "",
                            line.TextLine4 ?? "");
                        resultList.Add(FstiResult.Build(result));
                    }
                    else
                    {
                        //无需执行AddLineText接口
                        resultList.Add(FstiResult.Success);
                    }
                }
            }
            return resultList;
        }


        /// <summary>
        /// MOMT Add_Header
        /// </summary>
        public FstiResult MomtAddHeader(FstiToken token, MomtAddInput input)
        {
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                string result = fstiService.MOMT_ADDHeader(token.Token.ToString(),
                    input.MoNumber,
                    input.Planner,
                    input.WorkCenter,
                    input.DeliverTo);
                return FstiResult.Build(result);
            }
        }

        /// <summary>
        /// MOMT Add_Line
        /// </summary>
        public List<FstiResult> MomtAddLines(FstiToken token, MomtAddInput input)
        {
            List<FstiResult> resultList = new List<FstiResult>();
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                foreach (var line in input.MomtAddLines)
                {
                    string result = fstiService.MOMT_ADDLine(token.Token.ToString(),
                        input.MoNumber,
                        line.ItemNumber,
                        line.MoLineType,
                        line.ItemOrderedQuantity.ToString("0.000000"),
                        line.MoLineStatus.ToString(),
                        line.StartDate.ToString("yyMMdd"),
                        line.ScheduledDate.ToString("yyMMdd"));
                    resultList.Add(FstiResult.Build(result));
                }
            }
            return resultList;
        }

        /// <summary>
        /// MOMT Add_Line_Text
        /// </summary>
        public List<FstiResult> MomtAddLineTexts(FstiToken token, MomtAddInput input)
        {
            List<FstiResult> resultList = new List<FstiResult>();
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                foreach (var line in input.MomtAddLines)
                {
                    if (line.IsNeedAddLineText())
                    {
                        string result = fstiService.MOMT_ADDLineText(token.Token.ToString(),
                            input.MoNumber,
                            line.MoLineNumber.ToString(),
                            line.ItemNumber,
                            line.MoLineType,
                            line.TextLine1 ?? "",
                            line.TextLine2 ?? "",
                            line.TextLine3 ?? "",
                            line.TextLine4 ?? "");
                        resultList.Add(FstiResult.Build(result));
                    }
                    else
                    {
                        //无需执行AddLineText接口
                        resultList.Add(FstiResult.Success);
                    }
                }
            }
            return resultList;
        }


        /// <summary>
        /// PICK Add
        /// </summary>
        public FstiResult PickAdd(FstiToken token, PickInput input)
        {
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                string result = fstiService.PICK_ADD(token.Token.ToString(),
                    input.OrderType,
                    input.IssueType,
                    input.OrderNumber,
                    input.LineNumber.ToString(),
                    input.ComponentLineType,
                    input.PointOfUseId,
                    input.OperationSequenceNumber,
                    input.ItemNumber,
                    input.RequiredQuantity.ToString("0.000000"));
                return FstiResult.Build(result);
            }
        }

        /// <summary>
        /// PICK Edit
        /// </summary>
        public FstiResult PickEdit(FstiToken token, PickInput input)
        {
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                string result = fstiService.PICK_EDITDetail(token.Token.ToString(),
                    input.OrderType,
                    input.IssueType,
                    input.OrderNumber,
                    input.LineNumber.ToString(),
                    input.ComponentLineType,
                    input.PointOfUseId,
                    input.OperationSequenceNumber,
                    input.ItemNumber,
                    input.RequiredQuantity.ToString("0.000000"),
                    input.QuantityType);
                return FstiResult.Build(result);
            }
        }


        /// <summary>
        /// MORV
        /// 返回LotNumber
        /// </summary>
        public FstiResult<string> Morv(FstiToken token, MorvInput input)
        {
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                string result = fstiService.MORVRByLot(token.Token.ToString(),
                    input.MoNumber,
                    input.MoLineNumber.ToString(),
                    input.ItemNumber,
                    input.ReceiptQuantity.ToString("0.000000"),
                    input.StockRoom,
                    input.Bin,
                    input.InventoryCategory,
                    input.LotNumber);

                //取得lotNumber
                string lotNumber = this._fsRepository.GetLotNumber(input.MoNumber, input.MoLineNumber, input.ItemNumber);

                var fstiResult = FstiResult<string>.Build(result);
                fstiResult.ExtensionData = (lotNumber ?? "");
                return fstiResult;
            }
        }


        /// <summary>
        /// IMTR
        /// </summary>
        public FstiResult Imtr(FstiToken token, ImtrInput input)
        {
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                string result = fstiService.IMTRByLot(token.Token.ToString(),
                    input.ItemNumber,
                    input.StockroomFrom,
                    input.BinFrom,
                    input.InventoryCategoryFrom,
                    input.InventoryQuantity.ToString("0.000000"),
                    input.StockroomTo,
                    input.BinTo,
                    input.InventoryCategoryTo,
                    input.LotNumber,
                    input.OrderType,
                    input.OrderNumber,
                    input.LineNumber.ToString(),
                    input.DocumentNumber);
                return FstiResult.Build(result);
            }
        }


        /// <summary>
        /// SHIP
        /// </summary>
        public FstiResult Ship(FstiToken token, ShipInput input)
        {
            using (var fstiService = this._fstiHelper.CreateFSTIService())
            {
                string result = fstiService.SHIPIByLot(token.Token.ToString(),
                    input.CoNumber,
                    input.CoLineNumber.ToString(),
                    input.ShippedQuantity.ToString("0.000000"),
                    input.Stockroom,
                    input.Bin,
                    input.InventoryCategory,
                    input.LotNumber);
                return FstiResult.Build(result);
            }
        }

        
    }
}
