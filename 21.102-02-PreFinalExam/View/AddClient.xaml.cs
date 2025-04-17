using _21._102_02_PreFinalExam.dbModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace _21._102_02_PreFinalExam.View
{
    /// <summary>
    /// Логика взаимодействия для AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        string _photoPath;
        public AddClient()
        {
            InitializeComponent();
            btnCancel.Click += BtnCancel_Click;
            btnOK.Click += BtnOK_Click;
            btnChoosePhoto.Click += BtnChoosePhoto_Click;
        }

        private void BtnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                _photoPath = openFileDialog.FileName;
                tblPhotoPath.Text = _photoPath;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Сохранить изменения?",
                "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult != MessageBoxResult.Yes) return;

            try
            {
                if (tbSurname.Text.Length == 0) throw new Exception("Пустая фамилия");
                if (tbName.Text.Length == 0) throw new Exception("Пустое имя");
                if (tbPhone.Text.Length == 0) throw new Exception("Пустое имя");

                if (!Regex.IsMatch(tbPhone.Text, @"^\d{10,15}$")) throw new Exception("Неверный формат телефона");

                using (Entities db = new Entities())
                {
                    Clients client = new Clients() { 
                        Surname = tbSurname.Text,
                        Name = tbName.Text,
                        Patronymic = tbPhone.Text,
                        Phone = tbPhone.Text,
                        Photo = _photoPath,
                    };
                    db.Clients.Add(client);
                    db.SaveChanges();
                }
                this.Close();
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
