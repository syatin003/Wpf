using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Data.Model
{
    public partial class User
    {
        public String InternalOrFirstName
        {
            get
            {
                if (InternalName != String.Empty)
                    return InternalName;
                else
                    return FirstName;
            }

        }


    }
}
