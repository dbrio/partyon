﻿using PartyOn.model.getPlace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.viewModels
{
    public class UserPlaceViewModel:NotificationEnabledObject
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
        ObservableCollection<modelPlace> userPlaceList;
       public ObservableCollection<modelPlace> UserPlaceList
        {
           get
           {
               if (userPlaceList == null)
               {
                   userPlaceList = new ObservableCollection<modelPlace>();
               }
               if (DesignerProperties.IsInDesignTool)
               {
                   for (int i = 0; i < 20; i++)
                   {
                       userPlaceList.Add(new modelPlace { PlaceName = Guid.NewGuid().ToString() });
                   }
               }
               return userPlaceList;
           }
           set
           {
               userPlaceList= value;
               OnPropertyChange();
           }

        }

       serviceModelPlace serviceModelPlace = new serviceModelPlace();

        public UserPlaceViewModel()
       {
           serviceModelPlace.GetUserPlaceComplete += (s, a) =>
               {
                   UserPlaceList = new ObservableCollection<modelPlace>(a.ResultsPlace);
                   isBusy = false;

               };
       }

        private string placeName;

        public string PlaceName
        {
            get { return placeName; }
            set { placeName = value;
            OnPropertyChange();
            }
        }

        ActionCommand getUserPlaceCommand;
        public ActionCommand GetUserPlaceCommand
        {
            get
            {
                if(getUserPlaceCommand==null)
                {
                    getUserPlaceCommand = new ActionCommand(() =>
                        {
                            isBusy = true;
                            serviceModelPlace.GetUserPlace(lati,longi);
                        });
                }
                return getUserPlaceCommand;
            }
        }

        modelPlace selectedPlace;
        public modelPlace SelectedPlace
        {
            get { return selectedPlace; }
            set { selectedPlace = value;
            OnPropertyChange();
            }
            

        }
    }
 }

