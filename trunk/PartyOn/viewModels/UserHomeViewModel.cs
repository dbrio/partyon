using PartyOn.model.homeM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.viewModels.homeV
{
    public class UserHomeViewModel:NotificationEnabledObject
    {
        public double lati, longi;

        bool isBusy;
        public bool IsBusy
        {
            get{ return isBusy;}
            set
            {
                isBusy = value;
                OnPropertyChange();
            }
        }
        ObservableCollection<modelHome> userHomeList;
       public ObservableCollection<modelHome> UserHomeList
        {
           get
           {
               return userHomeList;
           }
           set
           {
               userHomeList= value;
               OnPropertyChange();
           }

        }

       serviceModelHome serviceModelHome = new serviceModelHome();

        public UserHomeViewModel()
       {
           serviceModelHome.GetUserHomeComplete += (s, a) =>
               {
                   UserHomeList = new ObservableCollection<modelHome>(a.ResultsHome);
                   isBusy = false;

               };
       }

        ActionCommand getUserHomeCommand;
        public ActionCommand GetUserHomeCommand
        {
            get
            {
                if(getUserHomeCommand==null)
                {
                    getUserHomeCommand = new ActionCommand(() =>
                        {
                            isBusy = true;
                            serviceModelHome.GetUserHome(lati, longi);
                        });
                }
                return getUserHomeCommand;
            }
        }
    }
}
