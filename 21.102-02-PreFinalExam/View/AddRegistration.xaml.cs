using _21._102_02_PreFinalExam.dbModel;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace _21._102_02_PreFinalExam.View
{
    /// <summary>
    /// Логика взаимодействия для AddRegistration.xaml
    /// </summary>
    public partial class AddRegistration : Window
    {
        private Services _service;
        public AddRegistration(Services service)
        {
            InitializeComponent();
            _service = service;

            btnCancel.Click += BtnCancel_Click;
            btnOK.Click += BtnOK_Click;
            btnCreateClient.Click += BtnCreateClient_Click;

            LoadClients();
        }

        private void BtnCreateClient_Click(object sender, RoutedEventArgs e)
        {
            AddClient addClient = new AddClient();
            addClient.Closed += AddClient_Closed;
            addClient.ShowDialog();
        }

        private void AddClient_Closed(object sender, EventArgs e)
        {
            LoadClients();
        }

        public void LoadClients()
        {
            using (Entities db = new Entities())
            {
                List<Clients> clients = db.Clients.ToList();
                cbClient.ItemsSource = clients;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Сохранить изменения?",
"Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            try
            {
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    TimeSpan time;
                    DateTime fullDateTime;

                    if (cbClient.SelectedItem == null) throw new Exception("Не выбран клиент");
                    if (dpDate.SelectedDate == null) throw new Exception("Не выбрана дата");
                    if (tbTime.Text.Length == 0) throw new Exception("Не выбрано время");

                    if (!TimeSpan.TryParse(tbTime.Text, out time)) throw new Exception("Введено некорректное время");

                    fullDateTime = dpDate.SelectedDate.Value.Date + time;

                    if(cbClient.SelectedItem is Clients client)
                    {
                        using (Entities db = new Entities())
                        {
                            Registration registration = new Registration();
                            registration.DateTime = fullDateTime;
                            registration.ServiceID = _service.ID;
                            registration.ClientID = client.ID;

                            db.Registration.Add(registration);
                            db.SaveChanges();
                        }
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Отменить изменения?",
                "Отмена", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
