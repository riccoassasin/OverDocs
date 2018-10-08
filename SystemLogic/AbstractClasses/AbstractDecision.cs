using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum.SystemLogicEnum;
using SystemLogic.Interfaces;

namespace SystemLogic.AbstractClasses
{
    public abstract class AbstractDecision : IDecsions
    {
        public ControlTypes _FinalDecision = ControlTypes.Download;
        public ControlTypes FinalDecision
        {
            get { return _FinalDecision; }
        }

        public abstract void DetermineCorrectButton();

    }
}
