using Common.Enum.SystemLogicEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLogic.Interfaces
{
    public interface IDecsions
    {
        //void DetermineCorrectButton();
        ControlTypes FinalDecision { get; }

    }
}
