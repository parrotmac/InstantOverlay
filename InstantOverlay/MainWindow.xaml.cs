using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstantOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public String[] launchArgs;

        public MainWindow()
        {
            launchArgs = Environment.GetCommandLineArgs();
            InitializeComponent();
            Console.WriteLine(launchArgs);
            if (launchArgs != null)
            {
                if (launchArgs.Length > 1)
                {

                    BitmapImage interimImage = new BitmapImage(new Uri(launchArgs[1], UriKind.RelativeOrAbsolute));
                    UpadateOverlayImage(interimImage);

                }
            }
            if (OverlayImage.Source == null)
            {
                AskForImage();
            }
        }

        private bool windowShouldFloat;

        public bool SetWindowShouldFloat
        {
            get {
                return windowShouldFloat;
            }
            set {
                windowShouldFloat = value;
                /*
                 *  I really wish I was binding the boolean of the check in the context menu to the topmost property
                * */

                this.Topmost = (bool)value;
            }
        }

        private bool shouldShowShadow;

        public bool ShowWindowShadow
        {
            get
            {
                return shouldShowShadow;
            }
            set
            {
                shouldShowShadow = value;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            if (shouldShowShadow)
            {
                this.Background.Opacity = 0.4;
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background.Opacity = 0;
        }

        private void OverlayImage_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
            Debug.WriteLine(String.Format("Loaded Image With Size: {0}, {1}", OverlayImage.Width, OverlayImage.Height));
            Debug.WriteLine("Source: " + OverlayImage.Source);
        }

        private void CtxMenu_SetImgSrc(object sender, RoutedEventArgs e)
        {
            AskForImage();
        }


        private void CtxMenu_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AskForImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|All Files|*.*";
            //dialog.InitialDirectory = @"C:\";
            dialog.Title = "Please select an image file...";
            if (dialog.ShowDialog() == true)
            {
                BitmapImage interimImage = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));
                UpadateOverlayImage(interimImage);
            }
            else
            {
                if (OverlayImage.Source == null)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void UpadateOverlayImage(BitmapImage source)
        {
            this.OverlayImage.Source = source;
            this.OverlayImage.Width = source.Width;
            this.OverlayImage.Height = source.Height;
        }

        private void AdjustWindowSize()
        {
            this.Width = Math.Max(10, OverlayImage.ActualWidth);
            this.Height = Math.Max(10, OverlayImage.ActualHeight);
        }

        private void OverlayImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustWindowSize();
        }

    }
}
