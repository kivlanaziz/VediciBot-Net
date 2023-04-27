using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VediciBot_Net.Core.Services
{
    interface ICommandHandler
    {
        public Task InitializeAsync();

    }
}
