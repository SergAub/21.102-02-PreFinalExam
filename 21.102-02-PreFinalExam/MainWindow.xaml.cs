using _21._102_02_PreFinalExam.dbModel;
using _21._102_02_PreFinalExam.View;
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

namespace _21._102_02_PreFinalExam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load();
            cbSort.SelectionChanged += CbSort_SelectionChanged;
            btnAdd.Click += BtnAdd_Click;
            btnDelete.Click += BtnDelete_Click;
            btnClients.Click += BtnClients_Click;
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            if (dgServices.SelectedItem != null && dgServices.SelectedItem is Services service)
            {
                RegistrationList registration = new RegistrationList(service);
                registration.ShowDialog();
            }
            else
            {
                MessageBox.Show($"Нужно выбрать услугу для получения списка записей", "Список записей", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Load();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Entities db = new Entities())
            {
                if (dgServices.SelectedItem != null && dgServices.SelectedItem is Services service)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show($"Вы хотите удалить услугу {service.Name}?",
                    "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult != MessageBoxResult.Yes) return;

                    db.Services.Attach(service);
                    db.Services.Remove(service);
                    db.SaveChanges();
                    Load();
                }
                else
                {
                    MessageBox.Show($"Нужно выбрать услугу для удаления", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddService addService = new AddService();
            addService.Closed += AddService_Closed;
            addService.ShowDialog();
        }

        private void AddService_Closed(object sender, EventArgs e)
        {
            Load();
        }

        public void Load()
        {
            using (Entities db = new Entities())
            {
                List<Services> services = db.Services.ToList();
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        services = services.OrderBy(s => s.Name).ToList();
                        break;
                    case 2:
                        services = services.OrderByDescending(s => s.Name).ToList();
                        break;
                }
                dgServices.ItemsSource = services;
            }
        }
    }
}
