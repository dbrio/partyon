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
                    IsBusy = false;
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
                            IsBusy = true;
                            serviceModel.GetUserActivity();
                        });
                }
                return getUserActivityCommand;
            }
        }

    }
}
