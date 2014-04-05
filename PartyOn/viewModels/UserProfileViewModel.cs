using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyOn.model.porfile;
using System.ComponentModel;

namespace PartyOn.viewModels
{
    public class UserProfileViewModel:NotificationEnabledObject
    {
         bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set 
            { 
                isBusy = value;
                OnPropertyChange();
            }
        }

        ObservableCollection<modelProfile> userProfileList;

        public ObservableCollection<modelProfile> UserProfileList
        {
            get {
                    if (userProfileList == null)
                        {
                            userProfileList = new ObservableCollection<modelProfile>();
                        }
                    if (DesignerProperties.IsInDesignTool)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                userProfileList.Add(new modelProfile { username = Guid.NewGuid().ToString() });
                            }
                        }
                    return userProfileList; 
                  }
            set
            {
                userProfileList = value;
                OnPropertyChange();
            }
        }

        serviceProfile profileModel = new serviceProfile();

        public UserProfileViewModel()
        {
            profileModel.GetUserProfileCompleted += (s, a) =>
                {
                    UserProfileList = new ObservableCollection<modelProfile>(a.ResultsProfile);
                    IsBusy = false;
                };
        }

        ActionCommand getUserProfileCommand;
        public ActionCommand GetUserProfileCommand
        {
            get
            {
                if (getUserProfileCommand == null)
                {
                    getUserProfileCommand = new ActionCommand(() =>
                        {
                            IsBusy = true;
                            profileModel.GetUserProfile();
                        });
                }
                return getUserProfileCommand;
            }
        }

    }
}
