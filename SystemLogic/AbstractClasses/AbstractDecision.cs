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
        protected ControlTypes _FinalDecision = ControlTypes.Download;
        public abstract ControlTypes FinalDecision { get; }

        protected Boolean IS_FILE_PUBLIC = false;
        protected Boolean IS_FILE_PRIVATE = false;
        protected Boolean IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON = false;
        protected Boolean IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN = false;

        protected abstract void DetermineCorrectButton();
        protected abstract void IntialiseDecisionVariables();

        /// <summary>
        /// Variable used to etermines weather the Shared states are public or private
        /// </summary>
        protected abstract void Determine_IS_FILE_PUBLIC_OR_PRIVATE();
        /// <summary>
        /// Variable used to determines weather the File Owner and the current user logged in are the same person.
        /// </summary>
        protected abstract void Determine_IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON();
        /// <summary>
        /// 
        /// </summary>
        protected abstract void Determine_IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN();
    }
}
