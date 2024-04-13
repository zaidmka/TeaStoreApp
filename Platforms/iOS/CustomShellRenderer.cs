using System;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;
using CoreGraphics;

namespace TeaStoreApp.Platforms.iOS
{
    public class CustomShellRenderer : ShellRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (ViewController is UINavigationController navController)
            {
                navController.NavigationBarHidden = true;
            }
        }
        protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
        {
            return new CustomShellTabBarAppearanceTracker();
        }
    }

    public class CustomShellTabBarAppearanceTracker : IShellTabBarAppearanceTracker
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void ResetAppearance(UITabBarController controller)
        {
            //throw new NotImplementedException();
        }

        public void SetAppearance(UITabBarController controller, ShellAppearance appearance)
        {
            //throw new NotImplementedException();
        }

        public void UpdateLayout(UITabBarController controller)
        {
            // Define the color you want to use for the tab bar items
            var selectItemColor = UIColor.FromRGB(0, 0, 0); // Red color
            var unselectItemColor = UIColor.FromRGB(255, 128, 0); // Red color

            foreach (var tabBarItem in controller.TabBar.Items)
            {
                var prevImage = tabBarItem.Image;

                // Resize the image as before
                var size = new CGSize(25, 25);
                UIGraphics.BeginImageContextWithOptions(size, false, 0);
                prevImage.Draw(new CGRect(new CGPoint(0, 0), size));
                var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                // Set both the normal and selected images with template rendering mode
                tabBarItem.Image = resizedImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                tabBarItem.SelectedImage = resizedImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                // Set the colors for the tab bar item
                controller.TabBar.TintColor = selectItemColor; // Color for the selected item
                controller.TabBar.UnselectedItemTintColor = unselectItemColor; // Also sets the unselected item color to maintain uniformity
            }
        }


    }
}

