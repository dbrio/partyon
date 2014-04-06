using PartyOn.model.porfile;
using PartyOn.model.profile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.viewModels
{
   public class PerfilViewModel:NotificationEnabledObject
    {
       public int id;
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

        ObservableCollection<modelProfile> perfilLista;

        public ObservableCollection<modelProfile> PerfilLista
        {
            get {
                    if (perfilLista == null)
                        {
                            perfilLista = new ObservableCollection<modelProfile>();
                        }
                    if (DesignerProperties.IsInDesignTool)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                perfilLista.Add(new modelProfile { username = Guid.NewGuid().ToString() });
                            }
                        }
                    return perfilLista; 
                  }
            set
            {
                perfilLista = value;
                OnPropertyChange();
            }
        }

        servicePerfil profileModel = new servicePerfil();

        public PerfilViewModel()
        {
            profileModel.GetUserPerfilCompleted += (s, a) =>
                {
                    PerfilLista = new ObservableCollection<modelProfile>(a.ResultsPerfil);
                    IsBusy = false;
                };
        }

        ActionCommand getUserPerfilCommand;
        public ActionCommand GetUserPerfilCommand
        {
            get
            {
                if (getUserPerfilCommand == null)
                {
                    getUserPerfilCommand = new ActionCommand(() =>
                        {
                            IsBusy = true;
                            profileModel.GetUserPerfil(id);
                        });
                }
                return getUserPerfilCommand;
            }
        }

    }
}
