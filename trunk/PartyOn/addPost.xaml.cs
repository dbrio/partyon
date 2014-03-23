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
        byte[] byteArray;
        public addPost()
        {
            InitializeComponent();
            
            
            camera = new CameraCaptureTask();
            camera.Show();
            camera.Completed += new EventHandler<PhotoResult>(camera_Complete);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("PlaceName"))
            {
                textPlace.Text= NavigationContext.QueryString["PlaceName"];
            }
            else
            {
                textPlace.Text = "Tabaco Bar 504";
            }
        }

      

        //private static ManualResetEvent allDone = new ManualResetEvent(false);
       
        //private void UploadData()
        //{
        //    string webpageContent = string.Empty;


        //    Uri url = new Uri("http://192.168.2.8:8000/API/APIsaveplace/", UriKind.RelativeOrAbsolute);

        //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://192.168.2.12:8000/API/savephotopost/");
        //    webRequest.ContentType = "application/x-www-form-urlencoded";
        //    webRequest.Method = "POST";
        //    //webRequest.ContentLength = byteArray.Length;

        //    webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);

        //}

        //private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        //{
        //    HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

        //    Stream postStream = request.EndGetRequestStream(asynchronousResult);

        //    string postData = "PlaceName=ManguitosWP82&PlaceLat=239231222&PlaceLong=98856578441";
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

        //    postStream.Write(byteArray, 0, postData.Length);
        //    postStream.Close();

        //    request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);

        //}


        //private static void GetResponseCallback(IAsyncResult asynchronousResult)
        //{
        //    HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

        //    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
        //    Stream streamResponse = response.GetResponseStream();
        //    StreamReader streamReader = new StreamReader(streamResponse);
        //    string responseString = streamReader.ReadToEnd();
        //    Console.WriteLine(responseString);
        //    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(responseString, "Respuesta del servidor web.", MessageBoxButton.OK));
        //    streamResponse.Close();
        //    streamReader.Close();

        //    response.Close();
        //    //allDone.Set();
        //}

        private void camera_Complete(object sender, PhotoResult e)
        {
            if(e.TaskResult==TaskResult.OK)
            {
                //UploadData();
                //GuardarEnAlmacenamientoAislado(e.ChosenPhoto);
                try
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(e.ChosenPhoto);
                    pvwPhoto.Source = image;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        WriteableBitmap btmMap = new WriteableBitmap(image);
                        Extensions.SaveJpeg(btmMap, ms, image.PixelWidth, image.PixelHeight, 0, 100);

                        byteArray = ms.ToArray();
                    }
                }
                catch (ArgumentException x)
                {
                    Console.WriteLine(x.Message);
                }
            }
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            camera.Show();
            
        }

        //private void GuardarEnAlmacenamientoAislado(System.IO.Stream streamImagen)
        //{
        //    // Obtener instancia de Almacenamiento Aislado de la aplicacion.
        //    using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        // Eliminar la imagen si ya existe.
        //        if (isolatedStorage.FileExists(nombreImagen))
        //        {
        //            isolatedStorage.DeleteFile(nombreImagen);
        //        }

        //        // Crear imagen en el Almacenamiento Aislado.
        //        using (IsolatedStorageFileStream streamAlmacenamientoAislado = isolatedStorage.CreateFile(nombreImagen))
        //        {
        //            // Obtener mapa de bits a partir del stream.
        //            BitmapImage bmpImagen = new BitmapImage();
        //            bmpImagen.SetSource(streamImagen);

        //            // Guardar mapa de bits en el Almacenamiento Aislado.
        //            WriteableBitmap wb = new WriteableBitmap(bmpImagen);
        //            wb.SaveJpeg(streamAlmacenamientoAislado, wb.PixelWidth, wb.PixelHeight, 0, 100);
        //        }
        //    }
        //}

       

        private void cameraclick(object sender, EventArgs e)
        {
            camera.Show();
        }

        private void addPostClick(object sender, EventArgs e)
        {
            //MessageBox.Show("Aqui agregara el post");
            //btnPost.IsEnabled = false;
            Dictionary<string, object> data = new Dictionary<string, object>()
                    {
                        //{"PhotoPostDateTime", DateTime.Today},
                        {"PhotoPost_PlaceID","1"},
                        {"PhotoPost_User","1"},
                        {"PhotoPost_Lat","23232323"},
                        {"PhotoPostLong","88888880"},
                        {"PhotoPostDescription",txtMessage.Text},
                        {"PhotoPostPhoto",byteArray},
                    };
            PostSubmitter post = new PostSubmitter() { url = "http://192.168.2.12:8000/API/savephotopost/", parameters = data };
            post.Submit();

        }

     
    }

    public class PostSubmitter
    {
        public Dictionary<string, object> parameters { get; set; }
        string boundary = "----------" + DateTime.Now.Ticks.ToString();
        public string url { get; set; }

        public PostSubmitter() { }

        public void Submit()
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            myRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            myRequest.Method = "POST";

            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            //writeMultipartObject(postStream, parameters);
            StreamWriter writer = new StreamWriter(postStream);
            if (parameters != null)
            {
                foreach (var entry in parameters as Dictionary<string, object>)
                {
                    WriteEntry(writer, entry.Key, entry.Value);
                }
            }
            writer.Write("--");
            writer.Write(boundary);
            writer.WriteLine("--");
            writer.Flush();

            postStream.Close();

            request.BeginGetResponse(new AsyncCallback(GetREsponseCallback), request);
        }

        private void GetREsponseCallback(IAsyncResult asynchronousResult)
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
        }

        public void writeMultipartObject(Stream stream, object data)
        {
            StreamWriter writer = new StreamWriter(stream);
            if (data != null)
            {
                foreach (var entry in data as Dictionary<string, object>)
                {
                    WriteEntry(writer, entry.Key, entry.Value);
                }
            }
            writer.Write("--");
            writer.Write(boundary);
            writer.WriteLine("--");
            writer.Flush();
        }
        private void WriteEntry(StreamWriter writer, string key, object value)
        {
            if (value != null)
            {
                writer.Write("--");
                writer.WriteLine(boundary);
                if (value is byte[])
                {
                    byte[] ba = value as byte[];

                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "sentPhoto.jpg");
                    writer.WriteLine(@"Content-Type: application/octet-stream");
                    //writer.WriteLine(@"Content-Type: image / jpeg");
                    writer.WriteLine(@"Content-Length: " + ba.Length);
                    writer.WriteLine();
                    writer.Flush();
                    Stream output = writer.BaseStream;

                    output.Write(ba, 0, ba.Length);
                    output.Flush();
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""", key);
                    writer.WriteLine();
                    writer.WriteLine(value.ToString());
                }
            }
        }

    }
}