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

namespace KURSOVAYA____
{
	/// <summary>
	/// Логика взаимодействия для Registr.xaml
	/// </summary>
	public partial class Registr : Window
	{ 
		public Entities entities = new Entities();

		public Registr()
		{
		
			InitializeComponent();
			foreach (var item in entities.Role)
			{
				ComboRole.Items.Add(item);
			}
		}

		private void RegistrButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Users user = new Users();
				if (ComboRole.SelectedItem != null)
				{
					user.Id_role = (ComboRole.SelectedItem as Role)?.Id;
				}
				user.login = LoginTextBox.Text;
				user.password = PasswordTextBox.Text;

				if (!string.IsNullOrEmpty(user.login) || !string.IsNullOrEmpty(user.password) || ComboRole.SelectedItem != null)
				{
					entities.Users.Add(user);
					entities.SaveChanges();
					MessageBox.Show("Успешно зарегистрирован " + LoginTextBox.Text, "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
				{
					MessageBox.Show("Заполните все поля!");
				}
			}
			catch
			{
				MessageBox.Show("Ошибка! Заполните все поля корректно!");
				return;
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();
			this.Close();
			window.ShowDialog();
		}

		private void ComboRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			

		}
	}
}
