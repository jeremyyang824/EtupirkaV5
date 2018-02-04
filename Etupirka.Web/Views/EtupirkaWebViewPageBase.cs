using Abp.Web.Mvc.Views;
using Etupirka.Domain.Portal;

namespace Etupirka.Web.Views
{
    /// <summary>
    /// WebView基类
    /// </summary>
    public abstract class EtupirkaWebViewPageBase : EtupirkaWebViewPageBase<dynamic>
    {

    }

    /// <summary>
    /// WebView基类
    /// </summary>
    public abstract class EtupirkaWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected EtupirkaWebViewPageBase()
        {
            LocalizationSourceName = EtupirkaPortalConsts.LocalizationSourceName;
        }
    }
}