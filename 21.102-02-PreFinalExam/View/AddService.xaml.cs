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

namespace _21._102_02_PreFinalExam.View
{
    /// <summary>
    /// Логика взаимодействия для AddService.xaml
    /// </summary>
    public partial class AddService : Window
    {
        public AddService()
        {
            InitializeComponent();
            btnCancel.Click += BtnCancel_Click;
            btnOK.Click += BtnOK_Click;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Сохранить изменения?",
    "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            try
            {
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    int count;
                    decimal cost;

                    if (tbName.Text.Length == 0) throw new Exception("Пустое имя");
                    if (tbCost.Text.Length == 0) throw new Exception("Пустая цена");
                    if (tbCount.Text.Length == 0) throw new Exception("Пустое число занятий");

                    if(!Decimal.TryParse(tbCost.Text, out cost)) throw new Exception("В поле цены введено нечисловое значение");
                    if(!Int32.TryParse(tbCount.Text, out count)) throw new Exception("В поле числа занятий введено нечисловое значение");

                    using (Entities db = new Entities())
                    {
                        Services service = new Services();
                        service.Name = tbName.Text;
                        service.Cost = cost;
                        service.Count = count;
                        db.Services.Add(service);
                        db.SaveChanges();
                    }
                    this.Close();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Отменить изменения?",
    "Отмена", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(messageBoxResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
