using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum.DBLookupEnum;
using Common.Enum.SystemLogicEnum;
using OverDocsModels.DecisionModels.PublicDocuments.IsLocked;
using OverDocsModels.Interfaces;
using SystemLogic.AbstractClasses;

namespace SystemLogic.DescisionClasses.PublicViewedFiles.LockedFiles
{
    public class PublicViewLockedFile : AbstractDecision
    {

        public PublicViewLockedFile(string UserID, IFileLinkDecisionModel Model)
        {
            this.ID_OfUserCurrentlyLoggedIn = UserID;
            this.Model = Model;
            this.IntialiseDecisionVariables();
            this.DetermineCorrectButton();
        }
        public override ControlTypes FinalDecision
        {
            get { return _FinalDecision; }
        }



        protected override void DetermineCorrectButton()
        {
            if (IS_FILE_PUBLIC)
            {
                this._FinalDecision = ControlTypes.UploadFileNotification;
            }
            else
            {
                if (IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON)
                {
                    this._FinalDecision = ControlTypes.UploadFileNotification;
                }
                else
                {
                    if (IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN)
                    {
                        this._FinalDecision = ControlTypes.UploadFileNotification;
                    }
                    else
                    {
                        this._FinalDecision = ControlTypes.RequestPermissionNotifications;
                    }
                }
            }
        }

        

        protected override void Determine_IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON()
        {
            if (this.ID_OfUserCurrentlyLoggedIn == this.Model.FileOwnerID)
            {
                this.IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON = true;
            }
        }

        protected override void Determine_IS_FILE_PUBLIC_OR_PRIVATE()
        {
            switch (Model.FileSharedStautusID)
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

        protected override void Determine_IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN()
        {
            if (((PublicDocsLockedDataModel)Model).FileID_FilesSharedWithUser.Length > 0)
            {
                string[] AllFilesSharedWithUser = ((PublicDocsLockedDataModel)Model).FileID_FilesSharedWithUser.Split('?');
                foreach (string SharedFileID in AllFilesSharedWithUser)
                {
                    if (SharedFileID.Equals(SharedFileID))
                    {
                        this.IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN = true;
                    }
                }
            }
        }

        protected override void IntialiseDecisionVariables()
        {
            this.Determine_IS_FILE_PUBLIC_OR_PRIVATE();
            this.Determine_IS_FILE_OWNER_AND_USER_LOGGED_IN_THE_SAME_PERSON();
            this.Determine_IS_THE_CURRENT_FILE_SHARED_WITH_USER_CURRENTLY_LOGGED_IN();
        }
    }
}
