using System.Linq;
using System;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Properties;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Services;

public static class UserAccountService
{
    public static string GetGeneratedPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, random.Next(5, 15))
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static void SelectTheImage(System.Windows.Controls.Image image)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files (*.jpg; *.jpeg)|*.jpg;*.jpeg"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                using (var originalImage = new Bitmap(openFileDialog.FileName))
                {
                    int newWidth = 400;
                    int newHeight = 400;

                    using (var resizedImage = new Bitmap(originalImage, new Size(newWidth, newHeight)))
                    {
                        // Сохраняем в MemoryStream
                        using (var memoryStream = new MemoryStream())
                        {
                            // Сохраняем сжатое изображение в формате JPEG
                            resizedImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            memoryStream.Position = 0;

                            // Загружаем в BitmapImage
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = new MemoryStream(memoryStream.ToArray());
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            bitmap.Freeze();

                            image.Source = bitmap;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBoxHelper.Show(Strings.Error, $"{Strings.Error}: {ex.Message}");
            }
        }
        else
        {
            SelectTheDefaultImage(image);
        }
    }

    public static byte[] ConvertImageSourceToBytes(ImageSource imageSource)
    {
        if (imageSource == null)
            return null;

        BitmapSource bitmapSource = (BitmapSource)imageSource;

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

        using (var memoryStream = new MemoryStream())
        {
            encoder.Save(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public static ImageSource ConvertBytesToImageSource(byte[] imageBytes)
    {
        try
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream(imageBytes))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
        catch
        {
            return null;
        }
    }

    public static void SelectTheDefaultImage(System.Windows.Controls.Image image)
    {
        try
        {
            image.Source = new BitmapImage(new Uri("/Resources/Pictures/SmallUserIcon.png", UriKind.Relative));
        }
        catch { }
    }

    public static void SetImageSource(byte[] imageBytes, System.Windows.Controls.Image image)
    {
        if (imageBytes != null && imageBytes.Length > 0)
        {
            var newImageSource = ConvertBytesToImageSource(imageBytes);
            image.Source = newImageSource;

            if (image.Source == null)
            {
                SelectTheDefaultImage(image);
            }
        }
        else
        {
            SelectTheDefaultImage(image);
        }
    }

    public static async Task ExecuteTask(Control control, Func<Task> task)
    {
        control.IsEnabled = false;

        try
        {
            await task();
        }
        finally
        {
            control.IsEnabled = true;
        }
    }
}
