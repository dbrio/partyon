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
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Threading;

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
        private static ManualResetEvent allDone = new ManualResetEvent(false);

        private void UploadData()
        {
            string webpageContent = string.Empty;


            Uri url = new Uri("http://192.168.2.8:8000/API/APIsaveplace/", UriKind.RelativeOrAbsolute);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://192.168.2.8:8000/API/APIsaveplace/");
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            //webRequest.ContentLength = byteArray.Length;

            webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);

            //allDone.WaitOne();
            

            //WebClient webClient = new WebClient();
            //webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //var uri = new Uri("http://127.0.0.1:8000/API/APIsaveplace", UriKind.Absolute);
            //StringBuilder postData = new StringBuilder();
            //postData.AppendFormat("{0}={1}", "PlaceName", HttpUtility.UrlEncode("Manguito2 desde WP8e"));
            //postData.AppendFormat("&{0}={1}", "PlaceLat", HttpUtility.UrlEncode("27373.212"));
            //postData.AppendFormat("&{0}={1}", "PlaceLong", HttpUtility.UrlEncode("87783312.12"));

            //webClient.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
            //webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UploadStringCompleted);
            //webClient.UploadProgressChanged += webClient_UploadProgressChanged;
            //webClient.UploadStringAsync(uri, "POST", postData.ToString());
        }

        private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            string postData = "PlaceName=ManguitosWP82&PlaceLat=239231222&PlaceLong=98856578441";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            postStream.Write(byteArray, 0, postData.Length);
            postStream.Close();

            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);

        }


        private static void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamResponse);
            string responseString = streamReader.ReadToEnd();
            Console.WriteLine(responseString);
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(responseString, "Respuesta del servidor web.", MessageBoxButton.OK));
            streamResponse.Close();
            streamReader.Close();

            response.Close();
            //allDone.Set();
        }

        //void webClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        //{
        //    Debug.WriteLine(string.Format("Progress: {0}", e.ProgressPercentage));
        //}

        //private void webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        //{
        //    Debug.WriteLine("Completed.");
        //    MessageBox.Show("Completado.");
        //}


        private void camera_Complete(object sender, PhotoResult e)
        {
            if(e.TaskResult==TaskResult.OK)
            {
                UploadData();
                //GuardarEnAlmacenamientoAislado(e.ChosenPhoto);
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