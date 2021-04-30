using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace Wpf_AlyTalo_Palautettava
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Lights olohuone = new Lights();
        Lights keittio = new Lights();
        Thermostat talo = new Thermostat();
        Sauna sauna1 = new Sauna();
        public DispatcherTimer SaunaLammitin = new DispatcherTimer();
        SpeechSynthesizer sayItAloud = new SpeechSynthesizer();


        List<Changelog> ChangelogList = new List<Changelog>();
       

        public MainWindow()
        {
            InitializeComponent();

            SaunaLammitin.Tick += Ajastimen_Tick;

            SaunaLammitin.Interval = new TimeSpan(0, 0, 1);
            AsetaDatagridinKentat();





        }

        /*-------------------------------------------valot-----------------------------------------------------*/


        private void BtnKeittioValo_Click(object sender, RoutedEventArgs e)
        {
            if (keittio.Switched == true) //tarkistaa onko valot päällä tai pois, ja toimii sen mukaan
            {
                keittio.ValotPois();
                sliderDimmerKeittio.IsEnabled = false;
                sliderDimmerKeittio.Value = 0;
                lblKeittioValo.Content = keittio.Dimmer;
                lblKeittioValo.Background = Brushes.Gray;

            }
            else
            {
                keittio.ValotPaalle();

                sliderDimmerKeittio.IsEnabled = true;
                sliderDimmerKeittio.Value = 1;
                lblKeittioValo.Content = keittio.Dimmer;
                lblKeittioValo.Background = Brushes.Yellow;
            }
            SomethingChanged();
            SaveToList();
        }

        private void BtnOlohuoneValo_Click(object sender, RoutedEventArgs e)
        {
            if (olohuone.Switched == true) //tarkistaa onko valot päällä tai pois, ja toimii sen mukaan
            {
                olohuone.ValotPois();
                sliderDimmerOlohuone.IsEnabled = false;
                sliderDimmerOlohuone.Value = 0;
                lblOlohuoneValo.Content = olohuone.Dimmer;
                lblOlohuoneValo.Background = Brushes.Gray;
            }
            else
            {
                olohuone.ValotPaalle();

                sliderDimmerOlohuone.IsEnabled = true;
                sliderDimmerOlohuone.Value = 1;
                lblOlohuoneValo.Content = olohuone.Dimmer;
                lblOlohuoneValo.Background = Brushes.Yellow;
            }
            SomethingChanged();
            SaveToList();
        }


        private void btnSaateValot_Click(object sender, RoutedEventArgs e)
        {
            if (keittio.Switched == true)
            {
                keittio.Dimmer = sliderDimmerKeittio.Value.ToString();
                lblKeittioValo.Content = keittio.Dimmer;
            }
            else
            {
                lblKeittioValo.Content = keittio.Dimmer; //testing if switch sets dimmer to 0
            }


            if (olohuone.Switched == true)
            {
                olohuone.Dimmer = sliderDimmerOlohuone.Value.ToString();
                lblOlohuoneValo.Content = olohuone.Dimmer;
            }
            else
            {
                lblOlohuoneValo.Content = olohuone.Dimmer; //testing if switch sets dimmer to 0
            }
            SomethingChanged();
            SaveToList();
        }



        /*-----------------------------------------lämpötilat-------------------------------------------------------*/




        Boolean asetettuJo = false;
        private void BtnTavoite_Click(object sender, RoutedEventArgs e)
        {
            if (asetettuJo == true)
            {
                talo.Temperature = talo.TavoiteLampo;
                lblAikaisempi.Content = "Aikaisempi lämpötila on: " + talo.TavoiteLampo.ToString() + " °C";
            }
            asetettuJo = true;
            try
            {
                //***************************************tahalleen tehty virhe, jota voimme  uipath testauksen varten, kun syötetään 50, se näyttää 52
                if (int.Parse(txtTavoiteLampo.Text)==50)
                {
                    talo.TavoiteLampo = 52;
                }
                else
                {
                talo.TavoiteLampo = int.Parse(txtTavoiteLampo.Text);

                }


            }
            catch (Exception)
            {

            }

            lblTavoite.Content = "Tavoite lämpötila on: " + talo.TavoiteLampo.ToString() + " °C";
            txtTavoiteLampo.Text = "";
            SomethingChanged();
            SaveToList();
        }

        private void TxtTavoiteLampo_TextChanged(object sender, TextChangedEventArgs e)
        {



            if (Regex.IsMatch(txtTavoiteLampo.Text, "^[0-9]{1,2}$")) //regex checks if its number and 2 character long max.
            {
                txtTavoiteLampo.Background = Brushes.White;
            }
            else if (txtTavoiteLampo.Text.Length > 0)      //if its not removes last character
            {
                txtTavoiteLampo.Text = txtTavoiteLampo.Text.Remove(txtTavoiteLampo.Text.Length - 1, 1);
                txtTavoiteLampo.SelectionStart = txtTavoiteLampo.Text.Length; //puts cursor to last position
                txtTavoiteLampo.Background = Brushes.Tomato;

            }
        }

        /*----------------------------------------------sauna--------------------------------------------------*/
        private void BtnSauna_Click(object sender, RoutedEventArgs e)
        {


            if (sauna1.switched == false)             //sauna päälle
            {
                sauna1.switched = true;
                lblSaunaOn.Content = "SAUNA PÄÄLLÄ";
                lblSaunaOn.Background = Brushes.Green;
                SaunaLammitin.Start();

            




            }
            else if (sauna1.switched == true)                 //sauna pois päältä
            {
                sauna1.switched = false;
                lblSaunaOn.Content = "SAUNA EI OLE PÄÄLLÄ";
                lblSaunaOn.Background = Brushes.Red;
                SaunaLammitin.Start();
            }
            SomethingChanged();
            SaveToList();
        }
        //-------------------------------------------Sauna Ajastin---------------------------------------
        private void Ajastimen_Tick(object sender, EventArgs e) //Ajastimen asettaminen
        {
            if (sauna1.switched == true)
            {
                if (sauna1.lampotila < talo.Temperature)
                {
                    sauna1.lampotila = talo.Temperature;
                    lblSaunanLampo.Content = sauna1.lampotila.ToString() + " °C";

                }
                else if ((sauna1.lampotila < 110 && sauna1.lampotila >= talo.Temperature))        //sauna lämpö starts at kotilämpö ja 
                {                                                                              //ei mene yli 110           

                    sauna1.lampotila = sauna1.lampotila + 0.5;
                    lblSaunanLampo.Content = sauna1.lampotila.ToString() + " °C";
                    


                }
                else
                {
                    SaunaLammitin.Stop() ;
                }
            }
            else if (sauna1.switched == false)
            {
                if (sauna1.lampotila > talo.Temperature)        //kylmänää talon lämpötilan asti
                {
                    if (sauna1.lampotila-0.5==talo.Temperature)     //extra condition koska muuten vois mennä talon tmpin alla 0.5llä
                    {
                        sauna1.lampotila = talo.Temperature;                    
                        lblSaunanLampo.Content = sauna1.lampotila.ToString() + " °C";
                        SaunaLammitin.Stop();
                    }
                    else
                    {
                        sauna1.lampotila = sauna1.lampotila - 1;  //jos ei ole paalla, vahentaa 1
                        lblSaunanLampo.Content = sauna1.lampotila.ToString() + " °C";
                    }
                    


                }
                else
                {
                    sauna1.lampotila = talo.Temperature;  //jos ei ole paalla, vahentaa 1
                    lblSaunanLampo.Content = sauna1.lampotila.ToString() + " °C";
                    SaunaLammitin.Stop();
                }
            }
            SomethingChanged();
            SaveToList();

        }
        //-------------------------------------------talon tilanteet kerto änni englanniksi--------------------
        private void BtnKerro_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sayItAloud.SetOutputToDefaultAudioDevice();
                if (keittio.Switched)
                {
                    sayItAloud.Speak("The Kitchen light is on");
                }
                else
                {
                    sayItAloud.Speak("The Kitchen light is off");
                }
                if (olohuone.Switched)
                {
                    sayItAloud.Speak("The livingroom light is on");

                }
                else
                {
                    sayItAloud.Speak("The livingroom light is off");
                }
                sayItAloud.Speak("The temperature of the house is " + talo.Temperature.ToString() + " °C");
                if (sauna1.switched)
                {
                    sayItAloud.Speak("The sauna is on and the temperature inside is " + sauna1.lampotila.ToString() + " °C ");
                }
                else
                {
                    sayItAloud.Speak("The sauna is turned off and the temperature inside is " + sauna1.lampotila.ToString() + " °C ");
                }
            }
            catch (Exception)
            {
            }

        }


        //--------------------------------------------------Data Grid stuff----------------------------------------------

        private void SomethingChanged()
        {
            Changelog settingsChanged = new Changelog();


            settingsChanged.TimeOfChange = DateTime.Now.ToString("HH:mm:ss");

            if (keittio.Switched)
            {
                settingsChanged.KitchenLight = "on";
            }
            else
            {
                settingsChanged.KitchenLight = "off";
            }

            if (olohuone.Switched)
            {
                settingsChanged.LivingroomLight = "on";
            }
            else
            {
                settingsChanged.LivingroomLight = "off";
            }
            settingsChanged.KitchenDimmer = keittio.Dimmer;
            settingsChanged.LivingroomDimmer = olohuone.Dimmer;
            settingsChanged.HouseTemp = talo.Temperature.ToString();
            if (sauna1.switched)
            {
                settingsChanged.SaunaOn = "on";
            }
            else
            {
                settingsChanged.SaunaOn = "off";
            }
            settingsChanged.SaunaTemp = sauna1.lampotila.ToString();
            ChangelogList.Add(settingsChanged);
            

        }

        private void AsetaDatagridinKentat()
        {
            DataGridTextColumn textColTimeOfChange = new DataGridTextColumn();
            DataGridTextColumn textColKitchenLight = new DataGridTextColumn();
            DataGridTextColumn textColKitchenDimmer = new DataGridTextColumn();
            DataGridTextColumn textColLivingroomLight = new DataGridTextColumn();
            DataGridTextColumn textColLivingroomDimmer = new DataGridTextColumn();
            DataGridTextColumn textColHouseTemp = new DataGridTextColumn();
            DataGridTextColumn textColSaunaOn = new DataGridTextColumn();
            DataGridTextColumn textColSaunaTemp = new DataGridTextColumn();

            textColTimeOfChange.Binding = new Binding("TimeOfChange");
            textColKitchenLight.Binding = new Binding("KitchenLight");
            textColKitchenDimmer.Binding = new Binding("KitchenDimmer");
            textColLivingroomLight.Binding = new Binding("LivingroomLight");
            textColLivingroomDimmer.Binding = new Binding("LivingroomDimmer");
            textColHouseTemp.Binding = new Binding("HouseTemp");
            textColSaunaOn.Binding = new Binding("SaunaOn");
            textColSaunaTemp.Binding = new Binding("SaunaTemp");

            textColTimeOfChange.Header = "Time of change";
            textColKitchenLight.Header = "K. light on/off";
            textColKitchenDimmer.Header = "K. dimmer";
            textColLivingroomLight.Header = "O. light on/off";
            textColLivingroomDimmer.Header = "O. dimmer";
            textColHouseTemp.Header = "House temp";
            textColSaunaOn.Header = "Sauna on/off";
            textColSaunaTemp.Header = "Sauna temp";


            dgChangeLog.Columns.Add(textColTimeOfChange);
            dgChangeLog.Columns.Add(textColKitchenLight);
            dgChangeLog.Columns.Add(textColKitchenDimmer);
            dgChangeLog.Columns.Add(textColLivingroomLight);
            dgChangeLog.Columns.Add(textColLivingroomDimmer);
            dgChangeLog.Columns.Add(textColHouseTemp);
            dgChangeLog.Columns.Add(textColSaunaOn);
            dgChangeLog.Columns.Add(textColSaunaTemp);




        }

        private void SaveToList()
        {
            foreach (Changelog settingsChanged in ChangelogList)
            {
                dgChangeLog.Items.Add(settingsChanged);
                
                
            }
            ChangelogList.Clear();
            
            
        }

        
    }
}