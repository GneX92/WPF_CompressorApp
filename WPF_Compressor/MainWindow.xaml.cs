using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using System.IO.Compression;
using System.IO;

namespace WPF_Compressor;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void dragMe( object sender , MouseButtonEventArgs e )
    {
        try
        {
            DragMove();
        }
        catch ( Exception )
        {
        }
    }

    private void CloseApp( object sender , RoutedEventArgs e )
    {
        Application.Current.Shutdown();
    }

    private void OpenFile( object sender , RoutedEventArgs e )
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();

        try
        {
            if ( openFileDialog.ShowDialog() == true )
                textboxOpenPath.Text = openFileDialog.FileName;
        }
        catch ( Exception ex )
        {
            MessageBox.Show( ex.Message , "Error: Invalid path" , MessageBoxButton.OK , MessageBoxImage.Error );
        }
    }

    private void SaveLocation( object sender , RoutedEventArgs e )
    {
        OpenFolderDialog openFolderDialog = new OpenFolderDialog();

        try
        {
            if ( openFolderDialog.ShowDialog() == true )
                textboxSavePath.Text = openFolderDialog.FolderName;
        }
        catch ( Exception ex )
        {
            MessageBox.Show( ex.Message , "Error: Invalid path" , MessageBoxButton.OK , MessageBoxImage.Error );
        }
    }

    private void CompressFile( object sender , RoutedEventArgs e )
    {
        bool success = false;
        string filePath = textboxOpenPath.Text;
        string savePath = textboxSavePath.Text;

        if ( string.IsNullOrEmpty( filePath ) || string.IsNullOrEmpty( savePath ) )
        {
            MessageBox.Show( "Please select both a file path and a save location." , "Error: Missing information" , MessageBoxButton.OK , MessageBoxImage.Error );
            return;
        }

        try
        {
            var newfilePath = System.IO.Path.ChangeExtension( filePath , ".zip" );
            string [] s = newfilePath.Split( @"\" );
            string compressedFileName = s [ s.Length - 1 ];

            savePath += @"\" + compressedFileName;

            if ( !Directory.Exists( savePath ) )
                Directory.CreateDirectory( savePath );

            using FileStream originalFileStream = File.Open( filePath , FileMode.Open );
            using FileStream compressedFileStream = File.Create( savePath );
            using GZipStream compressor = new( compressedFileStream , CompressionMode.Compress );
            originalFileStream.CopyTo( compressor );

            success = true;
        }
        catch ( Exception ex )
        {
            if ( ex is not UnauthorizedAccessException )
                MessageBox.Show( ex.Message , "Error: Compressing file" , MessageBoxButton.OK , MessageBoxImage.Error );
            else
                success = true;
        }
        finally
        {
            if ( success )
                MessageBox.Show( "File compressed successfully." , "Success" , MessageBoxButton.OK , MessageBoxImage.Information );
        }
    }
}