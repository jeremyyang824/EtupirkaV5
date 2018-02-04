using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Portal.Notifications
{
    public interface IEtupirkaPortalNotifier
    {
        Task WelcomeToTheApplicationAsync(SysUser user);
    }
}
