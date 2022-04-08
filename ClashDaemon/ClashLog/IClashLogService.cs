using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashDaemon.ClashLog
{
    public interface IClashLogService
    {
        public void HandleLog(string? origin);
    }
}
