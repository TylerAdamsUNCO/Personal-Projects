using System;
using System.Collections.Generic;
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
using WpfAnimatedGif;
using System.IO;

namespace Shiny_Hunt_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int globalCounter = 0;
        string odds = string.Empty;
        string method = string.Empty;
        int generation = 0;
        int topOdds = 1;
        int bottomOdds = 8192;
        bool doWeHaveCharm = false;
        static Pokemon mon = new Pokemon();
        //double dexNavProb = .00000006;
        private static readonly string[] Gen2Methods = { "Soft Reset", "Random Encounter" };
        private static readonly string[] Gen4Methods = { "Soft Reset", "Random Encounter", "Pokeradar", "Masuda Method" };
        private static readonly string[] Gen5Methods = { "Soft Reset", "Random Encounter", "Masuda Method" };
        private static readonly string[] Gen6Methods = { "Soft Reset", "Random Encounter", "Pokeradar", "Masuda Method", "Friend Safari", "Chain Fishing", "DexNav" };
        private static readonly string[] Gen7Methods = { "Soft Reset", "Random Encounter", "Masuda Method", "SOS" };
        private static readonly string[] AllPokemon = mon.AllPokemon;
        public MainWindow()
        {
            InitializeComponent();
            lblCounter.Content = globalCounter;
            cmbGeneration.Text = "2";
            cmbMethod.Text = "Soft Reset";
            generation = Convert.ToInt32(cmbGeneration.SelectedValue.ToString());
            method = cmbMethod.SelectedItem.ToString();
            doWeHaveCharm = chkCharm.IsChecked.GetValueOrDefault();
            txtTarget.ItemsSource = AllPokemon;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            globalCounter--;
            if (globalCounter < 0) globalCounter = 0;
            updateAll(generation, method, globalCounter);
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            globalCounter++;
            updateAll(generation, method, globalCounter);
        }
        //ODDS METHODS
        private void updateAll(int generation, string method, int counter)
        {
            lblCounter.Content = globalCounter;
            imgCharm.Visibility = doWeHaveCharm ? Visibility.Visible : Visibility.Hidden;

            int gen = generation;
            string met = method;
            int count = counter;
            lblCurrentGeneration.Content = gen;
            lblCurrentMethod.Content = met;
            lblCurrentTarget.Content = txtTarget.Text;

            if (met == "Pokeradar")
            {
                UpdateRadar(count);
            }
            else if (met == "Masuda Method")
            {
                if (gen == 4) { bottomOdds = 1638; }
                if (gen == 5)
                {
                    bottomOdds = doWeHaveCharm ? 1024 : 1365;
                }
                else if (gen >= 6)
                {
                    bottomOdds = doWeHaveCharm ? 512 : 683;
                }
            }
            else if (met == "Friend Safari")
            {
                bottomOdds = doWeHaveCharm ? 585 : 819;
            }
            else if (met == "Chain Fishing" && counter >= 1)
            {
                UpdateChainFish(count);
            }
            else if (met == "DexNav" && counter > 1)
            {
                UpdateDexNav(count);
            }
            else if (met == "SOS" && counter >= 1)
            {
                UpdateSOS(count);
            }
            else
            {
                BaseOdds(gen);
            }
            odds = topOdds + "/" + bottomOdds;
            lblOdds.Content = odds;
        }

        private void BaseOdds(int generation)
        {
            int currentGen = generation;
            if (currentGen >= 2 && currentGen <= 4)
            {
                topOdds = 1;
                bottomOdds = 8192;
            }
            else if (currentGen > 4)
            {
                if (currentGen == 5)
                {
                    topOdds = 1;
                    bottomOdds = doWeHaveCharm ? 2731 : 8192;
                }
                else if (currentGen > 5)
                {
                    topOdds = 1;
                    bottomOdds = doWeHaveCharm ? 1365 : 4096;
                }
            }
        }

        private void UpdateRadar(int counter)
        {
            double temp = 0;
            double prob = Math.Ceiling(65535.0 / (8200 - counter * 200)) / 65536.0;
            temp = prob * 100;
            bottomOdds = (int)Math.Round(100 / temp);
        }

        private void UpdateChainFish(int counter)
        {
            if (counter <= 20 && counter >= 1)
            {
                topOdds = doWeHaveCharm ? 2 + counter * 2 + 1 : counter * 2 + 1;
                bottomOdds = (4096 / topOdds) + 1;
                topOdds = 1;
            }
            if (counter > 20) bottomOdds = doWeHaveCharm ? 96 : 100;
        }

        private void UpdateDexNav(int Counter)
        {
            /*double temp = 0;
            if(Counter >= 1 && Counter <= 100)
            {
                if (Counter % 16.66 == 0)
                {
                    dexNavProb += .0001;
                }
            }
            if(Counter >= 101 && Counter <= 200)
            {
                if(Counter % 50 == 0)
                {
                    dexNavProb += .0001;
                }
            }
            if (Counter > 200)
            {
                if (Counter % 100 == 0)
                {
                    dexNavProb += .0001;
                }
            }
            double temp = dexNavProb * 100;
            bottomOdds = (int)Math.Round(100 / temp);*/
        }

        private void UpdateSOS(int Counter)
        {
            if (Counter >= 11 && Counter <= 20)
            {
                bottomOdds = doWeHaveCharm ? 585 : 820;
            }
            else if (Counter >= 21 && Counter <= 30)
            {
                bottomOdds = doWeHaveCharm ? 373 : 455;
            }
            else if (Counter > 30)
            {
                bottomOdds = doWeHaveCharm ? 273 : 315;
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            generation = Convert.ToInt32(cmbGeneration.SelectedValue.ToString());
            BaseOdds(generation);
            if (cmbMethod.SelectedIndex != -1) { method = cmbMethod.SelectedItem.ToString(); }
            doWeHaveCharm = chkCharm.IsChecked.GetValueOrDefault();
            updateAll(generation, method, globalCounter);
            ChangePicture(txtTarget.Text);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            globalCounter = 0;
            updateAll(generation, method, globalCounter);
        }

        private void lblCounter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int tempForConversion;
            string userCounter = Microsoft.VisualBasic.Interaction.InputBox("Enter your current encounters:", "Encounters", globalCounter.ToString());
            bool convSucc = int.TryParse(userCounter, out tempForConversion);
            if (convSucc) globalCounter = tempForConversion;
            updateAll(generation, method, globalCounter);
        }

        private void CmbGeneration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGeneration.SelectedValue.ToString() == "2" || cmbGeneration.SelectedValue.ToString() == "3")
            {
                UpdateAvailableMethods(Gen2Methods);
                chkCharm.IsChecked = false;
                chkCharm.IsEnabled = false;
            }
            else if (cmbGeneration.SelectedValue.ToString() == "4")
            {
                UpdateAvailableMethods(Gen4Methods);
                chkCharm.IsChecked = false;
                chkCharm.IsEnabled = false;
            }
            else if (cmbGeneration.SelectedValue.ToString() == "5")
            {
                UpdateAvailableMethods(Gen5Methods);
                chkCharm.IsEnabled = true;
            }
            else if (cmbGeneration.SelectedValue.ToString() == "6")
            {
                UpdateAvailableMethods(Gen6Methods);
                chkCharm.IsEnabled = true;
            }
            else if (cmbGeneration.SelectedValue.ToString() == "7")
            {
                UpdateAvailableMethods(Gen7Methods);
                chkCharm.IsEnabled = true;
            }
        }

        private void UpdateAvailableMethods(string[] generationMethods)
        {
            cmbMethod.Items.Clear();
            for (int i = 0; i < generationMethods.Length; i++)
            {
                cmbMethod.Items.Add(generationMethods[i]);
            }
            cmbMethod.SelectedItem = cmbMethod.Items[0];
        }

        private void ChangePicture(string pokemon)
        {
            string fileName = string.Empty;
            fileName = "Images/quest.png";

            ImageSource quest = new BitmapImage(new Uri(fileName, UriKind.Relative));
            ImageBehavior.SetAnimatedSource(imgSprite, quest);
            if (txtTarget.SelectedItem != null)
            {
                fileName = "Images/" + pokemon.ToLower() + ".gif";
                ImageSource img = new BitmapImage(new Uri(fileName, UriKind.Relative));
                ImageBehavior.SetAnimatedSource(imgSprite, img);
            }
        }

        private void txtTarget_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (AutoCompleteBox)sender;
            if (tb.Text.Length > 0)
            {
                tb.Text = Char.ToUpper(tb.Text[0]) + tb.Text.Substring(1).ToLower();
            }
        }
    }
}
