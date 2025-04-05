using System.Linq;
using System;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Properties;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Data;

public static class UserAccount
{
    public static string GetGeneratedPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, random.Next(5, 15))
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static void SelectTheImage(Image image)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();

        openFileDialog.Filter = "Image Files (*.jpg; *.jpeg)|*.jpg;*.jpeg";

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                image.Source = bitmap;
            }
            catch (Exception ex)
            {
                CustomMessageBoxHelper.Show(Strings.Error, $"{Strings.Error}: {ex.Message}", false);
            }
        }
        else
        {
            SelectTheDefaultImage(image);
        }
    }

    public static void SelectTheDefaultImage(Image image)
    {
        try
        {
            image.Source = new BitmapImage(new Uri("/Resources/Pictures/SmallUserIcon.png", UriKind.Relative));
        }
        catch { }
    }
}
