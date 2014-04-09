using Microsoft.Phone.Shell;
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
                   UpdateTile();
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

        private void UpdateTile()
        {
            ShellTile tile = ShellTile.ActiveTiles.First();
            if (tile != null)
            {
                //CycleTileData cycleData = new CycleTileData()
                //{
                //    Title = "PartyOn",
                //    Count = userHomeList.Count(),
                //    SmallBackgroundImage = new Uri("cycleTitleSmall-06.png", UriKind.Relative),
                //    CycleImages = new Uri[] 
                //    {
                //        new Uri("http://partyonapp.com/media/PhotoPosts/sentPhoto_12.jpg", UriKind.Absolute),
                //        new Uri("http://partyonapp.com/media/PhotoPosts/sentPhoto_11.jpg", UriKind.Absolute),
                //        new Uri("http://partyonapp.com/media/PhotoPosts/sentPhoto_10.jpg", UriKind.Absolute),
                //    }
                //};
                //tile.Update(cycleData);

                IconicTileData iconicTile = new IconicTileData()
                {
                    Count = userHomeList.Count(),
                    WideContent1 = string.Format("{0} near places", userHomeList.Count.ToString()),
                    WideContent2 = string.Format("Last Update: {0}", DateTime.Now.ToString())
                };
                tile.Update(iconicTile);
            }   
        }
    }
}
