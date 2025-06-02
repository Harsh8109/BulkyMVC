using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        //Initialize method is responsible for creating admin user and roles of our website
        void Initialize();
    }
}
