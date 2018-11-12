using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ArtifactPlanner.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ArtifactPlanner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public void NavigateTopElement(Type destination, Guid deck)
        {
            contentFrame.Navigate(destination, deck);
        }

        #region mainNavigation event handlers

        private void mainNavigation_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem
            foreach (NavigationViewItemBase item in mainNavigation.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Home_Page")
                {
                    mainNavigation.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(Home));
        }

        private void nvTopLevelNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            // Nothing yet...
        }

        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (!(args.InvokedItemContainer is NavigationViewItem item)) return;
            switch (item.Tag)
            {
                case "Home_Page":

                    contentFrame.Navigate(typeof(Home));
                    break;

                case "Deck_Builder":
                    contentFrame.Navigate(typeof(Deck_Builder));
                    break;

                case "Deck_Tracker":
                    contentFrame.Navigate(typeof(Deck_Tracker));
                    break;

                case "Edit_Decks":
                    contentFrame.Navigate(typeof(Edit_Saved));
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
