using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace PartyOn
{
    public partial class addPost : PhoneApplicationPage
    {
        private string nombreImagen = "imagen.jpg";
        private CameraCaptureTask camera = null;

        public addPost()
        {
            InitializeComponent();

            camera = new CameraCaptureTask();

            camera.Completed += new EventHandler<PhotoResult>(camera_Complete);

        }

        private void camera_Complete(object sender, PhotoResult e)
        {
            if(e.TaskResult==TaskResult.OK)
            {
                GuardarEnAlmacenamientoAislado(e.ChosenPhoto);
            }
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            camera.Show();
            
        }

        private void GuardarEnAlmacenamientoAislado(System.IO.Stream streamImagen)
        {
            // Obtener instancia de Almacenamiento Aislado de la aplicacion.
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Eliminar la imagen si ya existe.
                if (isolatedStorage.FileExists(nombreImagen))
                {
                    isolatedStorage.DeleteFile(nombreImagen);
                }

                // Crear imagen en el Almacenamiento Aislado.
                using (IsolatedStorageFileStream streamAlmacenamientoAislado = isolatedStorage.CreateFile(nombreImagen))
                {
                    // Obtener mapa de bits a partir del stream.
                    BitmapImage bmpImagen = new BitmapImage();
                    bmpImagen.SetSource(streamImagen);

                    // Guardar mapa de bits en el Almacenamiento Aislado.
                    WriteableBitmap wb = new WriteableBitmap(bmpImagen);
                    wb.SaveJpeg(streamAlmacenamientoAislado, wb.PixelWidth, wb.PixelHeight, 0, 100);
                }
            }
        }

     
    }
}