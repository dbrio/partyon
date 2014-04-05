using PartyOn.model.song;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.viewModels
{
    public class UserSongViewModel:NotificationEnabledObject
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

        ObservableCollection<modelSong> userSongList;

        public ObservableCollection<modelSong> UserSongList
        {
            get {
                    if (userSongList == null)
                        {
                            userSongList = new ObservableCollection<modelSong>();
                        }
                    if (DesignerProperties.IsInDesignTool)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                userSongList.Add(new modelSong { SongPostName = Guid.NewGuid().ToString() });
                            }
                        }
                    return userSongList; 
                  }
            set
            {
                userSongList = value;
                OnPropertyChange();
            }
        }

        ServiceSong profileModel = new ServiceSong();

        public UserSongViewModel()
        {
            profileModel.GetUserSongCompleted += (s, a) =>
                {
                    UserSongList = new ObservableCollection<modelSong>(a.ResultsSong);
                    IsBusy = false;
                };
        }

        ActionCommand getUserSongCommand;
        public ActionCommand GetUserSongCommand
        {
            get
            {
                if (getUserSongCommand == null)
                {
                    getUserSongCommand = new ActionCommand(() =>
                        {
                            IsBusy = true;
                            profileModel.GetUserSong();
                        });
                }
                return getUserSongCommand;
            }
        }

    }
}
