using _21._102_02_PreFinalExam.dbModel;
using _21._102_02_PreFinalExam.ViewModel;
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

namespace _21._102_02_PreFinalExam.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationList.xaml
    /// </summary>
    public partial class RegistrationList : Window
    {
        private Services _service;

        public RegistrationList(Services service)
        {
            InitializeComponent();
            btnDelete.Click += BtnDelete_Click;
            btnAdd.Click += BtnAdd_Click;

            _service = service;

            Load();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (Entities db = new Entities())
            {
                if (dgRegistrations.SelectedItem != null && dgRegistrations.SelectedItem is RegistrationData registrationData)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show($"Вы хотите удалить запись?",
                    "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult != MessageBoxResult.Yes) return;

                    Registration reg = new Registration { ID = registrationData.registration.ID};
                    db.Registration.Attach(reg);
                    db.Entry(reg).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    Load();
                }
                else
                {
                    MessageBox.Show($"Нужно выбрать запись для удаления", "Удаление", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddRegistration addRegistration = new AddRegistration(_service);
            addRegistration.Closed += AddRegistration_Closed;
        }

        private void AddRegistration_Closed(object sender, EventArgs e)
        {
            Load();
        }

        public void Load()
        {
            using (Entities db = new Entities())
            {
                List<RegistrationData> registrationDatas = new List<RegistrationData>();
                List<Registration> registrations = db.Registration.Where(x => x.ServiceID == _service.ID).ToList();
                List<Clients> clients = db.Clients.ToList();

                foreach (Registration registration in registrations) {
                    registrationDatas.Add(new RegistrationData
                    {
                        registration = registration,
                        client = clients.Where(x => x.ID == registration.ClientID).FirstOrDefault(),
                    });
                }

                dgRegistrations.ItemsSource = registrationDatas;
            }
        }
    }
}
