using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using System.Threading.Tasks;
using System;

namespace EquipmentInventory.Classes.Helper
{
    public static class AnimationHelper
    {
        public static void ToggleSlideAnimation(Grid targetGrid)
        {
            if (targetGrid.Visibility == Visibility.Collapsed)
            {
                targetGrid.Visibility = Visibility.Visible;
                targetGrid.RenderTransform = new TranslateTransform(-350, 0);
                targetGrid.BeginAnimation(UIElement.OpacityProperty, null);

                Storyboard slideIn = (Storyboard)targetGrid.FindResource("SlideInStoryboard");
                slideIn.Begin(targetGrid);
            }
            else
            {
                Storyboard slideOut = (Storyboard)targetGrid.FindResource("SlideOutStoryboard");
                slideOut.Completed += (s, args) => targetGrid.Visibility = Visibility.Collapsed;
                slideOut.Begin(targetGrid);
            }
        }

        public static async Task AnimatedBlock(Control blockingElement)
        {
            if (blockingElement == null) throw new ArgumentNullException(nameof(blockingElement));

            blockingElement.IsEnabled = false;
            try
            {
                await Task.Delay(1000);
            }
            finally
            {
                blockingElement.IsEnabled = true;
            }
        }
    }
}
