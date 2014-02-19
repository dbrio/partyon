using PartyOn.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.viewModels
{
   public class UserActivityViewModel : NotificationEnabledObject
    {
        ObservableCollection<modelActivity> userActivityList;

        public ObservableCollection<modelActivity> UserActivityList
        {
            get {
                    if (userActivityList == null)
                        {
                            userActivityList = new ObservableCollection<modelActivity>();
                        }
                    if (DesignerProperties.IsInDesignTool)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                userActivityList.Add(new modelActivity { PhotoPost_UserFirstName = Guid.NewGuid().ToString() });
                            }
                        }
                    return userActivityList; 
                  }
            set
            {
                userActivityList = value;
                OnPropertyChange();
            }
        }

        ServiceModel serviceModel = new ServiceModel();

        public UserActivityViewModel()
        {
            serviceModel.GetUserActivityCompleted += (s, a) =>
                {
                    UserActivityList = new ObservableCollection<modelActivity>(a.Results);
                };
        }

        ActionCommand getUserActivityCommand;
        public ActionCommand GetUserActivityCommand
        {
            get
            {
                if (getUserActivityCommand == null)
                {
                    getUserActivityCommand = new ActionCommand(() =>
                        {
                            serviceModel.GetUserActivity();
                        });
                }
                return getUserActivityCommand;
            }
        }

    }
}
