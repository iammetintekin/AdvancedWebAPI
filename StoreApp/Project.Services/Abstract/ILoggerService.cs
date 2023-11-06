using Project.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Abstract
{
    public interface ILoggerService
    {
        void Log(string message, LogType type);
    }
}
