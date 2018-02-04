using System;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Etupirka.Domain.External.Entities.Fsti;
using Etupirka.Domain.External.Fsti;

namespace Etupirka.Domain.External.Repositories
{
    /// <summary>
    /// FS ERP FSTI
    /// </summary>
    public interface IFSTIRepository : IRepository
    {
        /// <summary>
        /// COMT Add_Header
        /// </summary>
        FstiResult ComtAddHeader(FstiToken token, ComtAddInput input);

        /// <summary>
        /// COMT Add_Line
        /// </summary>
        List<FstiResult> ComtAddLines(FstiToken token, ComtAddInput input);

        /// <summary>
        /// COMT Add_Line_Text
        /// </summary>
        List<FstiResult> ComtAddLineTexts(FstiToken token, ComtAddInput input);

        /// <summary>
        /// MOMT Add_Header
        /// </summary>
        FstiResult MomtAddHeader(FstiToken token, MomtAddInput input);

        /// <summary>
        /// MOMT Add_Line
        /// </summary>
        List<FstiResult> MomtAddLines(FstiToken token, MomtAddInput input);

        /// <summary>
        /// MOMT Add_Line_Text
        /// </summary>
        List<FstiResult> MomtAddLineTexts(FstiToken token, MomtAddInput input);

        /// <summary>
        /// PICK Add
        /// </summary>
        FstiResult PickAdd(FstiToken token, PickInput input);

        /// <summary>
        /// PICK Edit
        /// </summary>
        FstiResult PickEdit(FstiToken token, PickInput input);

        /// <summary>
        /// MORV
        /// 返回LotNumber
        /// </summary>
        FstiResult<string> Morv(FstiToken token, MorvInput input);

        /// <summary>
        /// IMTR
        /// </summary>
        FstiResult Imtr(FstiToken token, ImtrInput input);

        /// <summary>
        /// SHIP
        /// </summary>
        FstiResult Ship(FstiToken token, ShipInput input);

    }
}
