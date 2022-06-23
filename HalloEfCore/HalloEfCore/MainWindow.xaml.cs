using HalloEfCore.Data;
using HalloEfCore.Model;
using System;
using System.Linq;
using System.Windows;

namespace HalloEfCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        EfRepository repo;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            repo = new EfRepository();
            LoadMitarbeiter();
        }

        private void LoadMitarbeiter()
        {
            myGrid.ItemsSource = repo.Mitarbeiter.ToList();
        }

        private void Neu(object sender, RoutedEventArgs e)
        {
            var mit = new Mitarbeiter();
            mit.Name = "Fred";
            mit.GebDatum = DateTime.Now.AddYears(-30).AddDays(36);
            mit.Beruf = "Macht wichtige Dinge!";

            repo.Add(mit);
            repo.SaveChanges();
            LoadMitarbeiter();
        }


        private void Speichern(object sender, RoutedEventArgs e)
        {
            var changes = repo.SaveChanges();
            MessageBox.Show($"{changes} Änderungen wurden gespeichert");
        }

        private void Löschen(object sender, RoutedEventArgs e)
        {
            if (myGrid.SelectedItem is Mitarbeiter selectedMitarbeiter)
            {
                var dlgRes = MessageBox.Show($"Soll {selectedMitarbeiter.Name} wirklich gelöscht werden?",
                                            "",
                                            MessageBoxButton.YesNo);

                if (dlgRes == MessageBoxResult.Yes)
                {
                    repo.Mitarbeiter.Remove(selectedMitarbeiter);
                    repo.SaveChanges();
                    LoadMitarbeiter();
                }
            }

        }
    }
}
