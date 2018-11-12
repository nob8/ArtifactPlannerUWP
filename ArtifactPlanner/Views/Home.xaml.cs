using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ArtifactPlanner.DeckBuilderClasses;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ArtifactPlanner.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
        }

        private void CreateEmptyDeck(object sender, RoutedEventArgs e)
        {
            DeckDatabase.replaceUserDecks(new List<Deck>() {new Deck() {DeckCards = new List<Deck.DeckCardReference>() {new Deck.DeckCardReference() {CardId = Guid.Empty}}}});
            if (sender is Button button)
            {
                if (button.Content is TextBlock buttonText)
                {
                    buttonText.Text = DeckDatabase.DatabaseContentsToString();
                    buttonText.TextWrapping = TextWrapping.Wrap;
                }
            }

            DeckDatabase.SyncUpdateToDeckFile();
//            if (!(Parent is MainPage parent)) return;
//            parent.NavigateTopElement(typeof(Deck_Builder), Guid.Empty);
        }

        private async void TestDeckSaving(object sender, RoutedEventArgs e)
        {
            DeckDatabase.replaceUserDecks(new List<Deck>() { new Deck() { DeckCards = new List<Deck.DeckCardReference>() } });
            await Task.Run(()=>DeckDatabase.DeckFileUpdated(null, null));
            if (!(sender is Button button)) return;
            if (!(button.Content is TextBlock buttonText)) return;
            buttonText.Text = DeckDatabase.DatabaseContentsToString();
            buttonText.TextWrapping = TextWrapping.Wrap;
        }

        private void ShowRoamingFileHandle(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button)) return;
            if (!(button.Content is TextBlock buttonText)) return;
            buttonText.Text = ApplicationData.Current.RoamingFolder.Path;
            buttonText.TextWrapping = TextWrapping.Wrap;
        }
    }
}
