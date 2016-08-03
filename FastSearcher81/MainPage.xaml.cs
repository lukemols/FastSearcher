using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FastSearcher81.Resources;
using FastSearcher.Classes.Entities;
using FastSearcher.Classes.Manager;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Windows.Phone.Speech.Recognition;
using FastSearcher.Classes.Support;
using FastSearcher.Classes.Ads;

namespace FastSearcher81
{
    public partial class MainPage : PhoneApplicationPage
    {
        Engine actualEngine;
        string tileEngine;
        Engine.EngineType showedType = Engine.EngineType.WEB;

        Ad ads;

        /// <summary>
        /// Costruttore
        /// </summary>
        public MainPage()
        {
            EngineListManager elm = EngineListManager.Instance; //Questo serve per creare l'istanza e quindi caricare i dati!          

            InitializeComponent();

            Loaded += (s, e) =>
            {
                SetEngineList();
                SetUpInterface();
                KeyDown += new KeyEventHandler(KeyPressed);
            };
        }


        void SetUpInterface()
        {
            RefreshInterface();
            PickerTypeSelector.SelectionChanged += PickerTypeSelector_SelectionChanged;

            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = AppResources.AppBarButtonTextPinToStart;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).Text = AppResources.AppBarButtonTextMicrophone;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).Text = AppResources.AppBarButtonTextInfo;

            Visibility lightBackgroundVisibility = (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"];
            string searchFile = @"/Assets/Images/ApplicationBar.Search";

            if (lightBackgroundVisibility == Visibility.Visible)
            {
                searchFile += "Black";
            }
            searchFile += ".png";

            Uri searchImage = new Uri(searchFile, UriKind.Relative);
            searchRectImageBrush.ImageSource = new BitmapImage(searchImage);
        }

        void RefreshInterface()
        {
            PickerTypeSelector.ItemsSource = new List<string> { AppResources.PickerWeb, AppResources.PickerImage };
            if (showedType == Engine.EngineType.WEB)
            {
                PickerTypeSelector.SelectedItem = PickerTypeSelector.Items[0];
            }
            else
            {
                PickerTypeSelector.SelectedItem = PickerTypeSelector.Items[1];
            }
            if (actualEngine.Type == Engine.EngineType.WEB)
                SearchTypeText.Text = AppResources.SearchWeb;
            else
                SearchTypeText.Text = AppResources.SearchImage;
            EngineName.Text = actualEngine.Name;

            SetAD();
        }

        void SetEngineList()
        {
            Dictionary<string, Engine> Engines;
            if (showedType == Engine.EngineType.WEB)
                Engines = EngineListManager.Instance.WebEngines;
            else
                Engines = EngineListManager.Instance.ImageEngines;

            if (Engines.Count > 0)
            {
                actualEngine = Engines.First().Value;

                if (tileEngine != null && tileEngine != "NONE")
                {
                    Engines.TryGetValue(tileEngine, out actualEngine);
                }

                EngineName.Text = actualEngine.Name;
                EngineList.ItemsSource = Engines.Keys.ToList<string>();
            }
            else
            {
                actualEngine = null;
                MessageBox.Show(AppResources.ErrorString);
            }
        }

        void SetAD()
        {
            Random r = new Random();
            string text = "";
            Ad.AdType type = Ad.AdType.INAPP;
            string uri = "";
            int x = r.Next() % 3;
            if (x == 0)
            {
                text = AppResources.Ad1;
                type = Ad.AdType.INAPP;
                uri = "11111";
            }
            else if (x == 1)
            {
                text = AppResources.Ad2;
                type = Ad.AdType.WEBLINK;
                uri = @"https://lmsoftit.wordpress.com/2015/07/27/scegli-me-di-giulia-cacciatore/";
            }
            else
            {
                text = AppResources.Ad3;
                type = Ad.AdType.TEXTONLY;
                uri = "";
            }
            ads = new Ad(text, type, uri);
            AdBlockText.Text = ads.Text;
        }

        #region Eventi pagina della ricerca

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                EngineListManager.Instance.StartResearch(actualEngine, SearchBox.Text);
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            EngineListManager.Instance.StartResearch(actualEngine, SearchBox.Text);
        }


        private async void AdBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            await ads.AdTapped();
            if (ads.AdSuccess)
                MessageBox.Show("Grazie!");

        }

        #endregion

        #region Eventi da click dei pulsanti su AppBar

        private void PinToStartIconButton_Click(object sender, EventArgs e)
        {
            TileHelper.Instance.SetTileOnStartWin8(actualEngine, AppResources.SearchItem);
        }

        private async void MicrophoneIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                string text = await SpeechHelper.Instance.GetMicrophoneSpeech(AppResources.SpeechListenText, AppResources.SpeechExampleText);
                SearchBox.Text = text;
                EngineListManager.Instance.StartResearch(actualEngine, text);
            }
            catch
            {
                MessageBox.Show(AppResources.SpeechError);
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ApplicationBarMenuItem)
            {
                string s = (sender as ApplicationBarMenuItem).Text;
                string mbTxt = "";
                string mbHead = "";
                if (s.Contains("italiano"))
                {
                    LanguageHelper.Instance.SetUserLanguage("it");
                    mbTxt = AppResources.RebootMessageForLanguageIt;
                    mbHead = AppResources.RebootMessageHeaderForLanguageIt;
                }
                else if (s.Contains("english"))
                {
                    LanguageHelper.Instance.SetUserLanguage("en");
                    mbTxt = AppResources.RebootMessageForLanguageEn;
                    mbHead = AppResources.RebootMessageHeaderForLanguageEn;
                }
                else if (s.Contains("español"))
                {
                    LanguageHelper.Instance.SetUserLanguage("es");
                    mbTxt = AppResources.RebootMessageForLanguageEs;
                    mbHead = AppResources.RebootMessageHeaderForLanguageEs;
                }
                MessageBox.Show(mbTxt, mbHead, MessageBoxButton.OK);
            }
        }

        private void InfoIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

        #endregion

        #region Eventi della pagina della selezione dei motori
        private void EngineList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EngineList.SelectedItem == null)
                return;

            Dictionary<string, Engine> Engines;
            if (showedType == Engine.EngineType.WEB)
                Engines = EngineListManager.Instance.WebEngines;
            else
                Engines = EngineListManager.Instance.ImageEngines;

            Engines.TryGetValue(EngineList.SelectedItem.ToString(), out actualEngine);
            EngineList.SelectedItem = null;
            RefreshInterface();

            ApplicationPivot.SelectedItem = ApplicationPivot.Items[0];
        }

        private void PickerTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PickerTypeSelector.SelectedItem.ToString() == AppResources.PickerImage)
                showedType = Engine.EngineType.IMAGE;
            else
                showedType = Engine.EngineType.WEB;

            SetEngineList();
        }

        #endregion


        #region Eventi della applicazione non legati a pagine particolari

        /// <summary>
        /// Evento generato al cambio della finestra del pivot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PivotItem selectedItem = (PivotItem)e.AddedItems[0];
            if (selectedItem.Name == "HomePagePivot")
                ApplicationBar.IsVisible = true;
            else
                ApplicationBar.IsVisible = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("name"))
            {
                PhoneApplicationService.Current.State.Remove("name");
            }
            PhoneApplicationService.Current.State.Add("name", actualEngine.Name);

            if (PhoneApplicationService.Current.State.ContainsKey("enginetype"))
            {
                PhoneApplicationService.Current.State.Remove("enginetype");
            }
            PhoneApplicationService.Current.State.Add("enginetype", showedType.ToString());
        }


        /// <summary>
        /// Metodo che gestisce l'avvio da tile o il ritorno da uno stato di sospensione
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            showedType = Engine.EngineType.WEB;
            tileEngine = "NONE";
            bool isAppStatePreserved = (Application.Current as App).IsAppStatePreserved;
            bool isAppOpenedFromStart = (Application.Current as App).IsAppOpenedFromStart;
            if (!isAppStatePreserved)
                GetPhoneApplicationServiceString();
            else if (isAppOpenedFromStart)
                GetNavigationContextString();
        }

        /// <summary>
        /// Metodo che ottiene i dati dalla live tile
        /// </summary>
        void GetNavigationContextString()
        {
            if (NavigationContext.QueryString.ContainsKey("name"))
            {
                tileEngine = NavigationContext.QueryString["name"];
            }
            if (NavigationContext.QueryString.ContainsKey("enginetype"))
            {
                showedType = NavigationContext.QueryString["enginetype"] == Engine.EngineType.WEB.ToString() ? Engine.EngineType.WEB : Engine.EngineType.IMAGE;
            }
        }

        /// <summary>
        /// Metodo che ottiene i dati salvati provenienti dallo stato di sospensione
        /// </summary>
        void GetPhoneApplicationServiceString()
        {
            if (PhoneApplicationService.Current.State.ContainsKey("name"))
            {
                tileEngine = PhoneApplicationService.Current.State["name"] as string;
                PhoneApplicationService.Current.State.Remove("name");
            }
            if (PhoneApplicationService.Current.State.ContainsKey("enginetype"))
            {
                showedType = PhoneApplicationService.Current.State["name"] as string == Engine.EngineType.WEB.ToString() ? Engine.EngineType.WEB : Engine.EngineType.IMAGE;
                PhoneApplicationService.Current.State.Remove("enginetype");
            }
            RefreshInterface();
        }

        #endregion

        #region Eventi generati dalla pagina delle informazioni

       

        #endregion

        
    }
}