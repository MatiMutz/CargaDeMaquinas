using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChain.HelperService
{
    public interface ILoginService
    {
        Task Login(Usuarios Usuario);
        Task Logout();
    }
}
