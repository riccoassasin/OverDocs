using Common.Enum.DBLookupEnum;
using Common.Enum.SystemLogicEnum;
using OverDocsModels.DecisionModels.PublicDocuments.IsAvaiable;
using OverDocsModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemLogic.AbstractClasses;

namespace SystemLogic.DescisionClasses.PublicViewedFiles.AvailableFiles
{
    public class PublicViewAvailableFile : AbstractDecision
    {
        /// <summary>
        /// USERID of theuser currently logged in.
        /// </summary>
        private string _ID_OfUserCurrentlyLoggedIn = "";

        /// <summary>
        /// Data required to determine the correct link to display.
        /// </summary>
        private IFileLinkDecisionModel _Model;

        /// <summary>
        /// 
        /// </summary>
        string ID_OfUserCurrentlyLoggedIn { get; }

        /// <summary>
        /// Constructor accepts tthe id of the currently login in user.
        /// </summary>
        /// <param name="UserID">ID Of the Current Person taht is logged on</param>
        public PublicViewAvailableFile(string UserID, PublicDocsAvailableDataModel Model)
        {
            this._ID_OfUserCurrentlyLoggedIn = UserID;
            this._Model = Model;
            this.IntialiseDecisionVariables();
            this.DetermineCorrectButton();

        }

        /// <summary>
        /// returns the Type of link to display
        /// </summary>
        public override ControlTypes FinalDecision
        {
            get { return _FinalDecision; }
        }

        /// <summary>
        /// Initialises the Decision Variables required to determine the correct link to display
        /// </summary>
        protected override void IntialiseDecisionVariables()
        {
            this.Determine_IS_FILE_PUBLIC_OR_PRIVATE();
            this.Determine_IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON();
            this.Determine_IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN();
        }

        /// <summary>
        /// Inherited from the common interface. Used to execute logic sequence to determine the correct button to display.
        /// </summary>
        protected override void DetermineCorrectButton()
        {
            if (IS_FILE_PUBLIC)
            {
                this._FinalDecision = ControlTypes.Download;
            }
            else
            {
                if (IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON)
                {
                    this._FinalDecision = ControlTypes.Download;
                }
                else
                {
                    if (IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN)
                    {
                        this._FinalDecision = ControlTypes.Download;
                    }
                    else
                    {
                        this._FinalDecision = ControlTypes.RequestPermissionNotifications;
                    }
                }
            }
        }

        protected override void Determine_IS_FILE_PUBLIC_OR_PRIVATE()
        {
            switch (_Model.FileSharedStautusID)
            {
                case (int)FileSharedStatus.Public:
                    this.IS_FILE_PUBLIC = true;
                    break;
                case (int)FileSharedStatus.Private:
                    this.IS_FILE_PRIVATE = true;
                    break;
                default:
                    break;
            }

        }

        protected override void Determine_IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON()
        {
            if (this._ID_OfUserCurrentlyLoggedIn == this._Model.FileOwnerID)
            {
                this.IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON = true;
            }
        }

        protected override void Determine_IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN()
        {
            if (((PublicDocsAvailableDataModel)_Model).FileID_FilesSharedWithUser.Length > 0)
            {
                string[] AllFilesSharedWithUser = ((PublicDocsAvailableDataModel)_Model).FileID_FilesSharedWithUser.Split('?');
                foreach (string SharedFileID in AllFilesSharedWithUser)
                {
                    if (SharedFileID.Equals(SharedFileID))
                    {
                        this.IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN = true;
                    }
                }
            }
            
        }
    }
}
