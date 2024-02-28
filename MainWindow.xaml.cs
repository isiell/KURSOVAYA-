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

namespace KURSOVAYA____
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Entities entities = new Entities();
		public MainWindow()
		{
			InitializeComponent();
		}

		private void SingUpButton_Click(object sender, RoutedEventArgs e)
		{
			//string login = LoginTextBox.Text.Trim();
			//string password = PasswordTextBox.Password.Trim();

			//Users user = new Users();
			//user = entities.Users.Where(p => p.login == login && p.password == password).FirstOrDefault();
			//int userCount = entities.Users.Where(p => p.login == login && p.password == password).Count();

			//if (userCount > 0)
			//{
			//	Role role = new Role();
			//	role = entities.Role.Where(p => p.Id == user.Id_role).FirstOrDefault();

			//	userDb.role = role.nazvanie_roli;
			//	Console.WriteLine(userDb.role);

			//	MessageBox.Show("Вы подключились как " + userDb.role, "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
			//	var window = new TextWind();
			//	this.Close();
			//	window.ShowDialog();
			//}
			//else
			//{
			//	MessageBox.Show("Неверный логин или пароль, попробуйте еще раз.");
			//}
			string login = LoginTextBox.Text.Trim();
			string password = PasswordTextBox.Password.Trim();

			Users user = new Users();
			user = entities.Users.Where(p => p.login == login && p.password == password).FirstOrDefault();
			int userCount = entities.Users.Where(p => p.login == login && p.password == password).Count();

			if (userCount > 0)
			{
				Role role = new Role();
				role = entities.Role.Where(p => p.Id == user.Id_role).FirstOrDefault();
				userDb.role = role.nazvanie_roli;
				Console.WriteLine(userDb.role);
				if (role.nazvanie_roli == "администратор")
				{
					AdminWindow adminWindow = new AdminWindow();
					adminWindow.Show();
					this.Close();
					
					MessageBox.Show("Вы подключились как " + userDb.role, "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				
				else if (role.nazvanie_roli == "сотрудник отдела кадров")
				{
					Rasschet userWindow = new Rasschet();
					userWindow.Show();
					this.Close();
					MessageBox.Show("Вы подключились как " + userDb.role, "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				// Дополнительные ветви для других возможных ролей

				// Console.WriteLine(userDb.role); // Если userDb не определено, можно удалить эту строку

				// MessageBox.Show("Вы подключились как " + userDb.roleы, "Успешно", MessageBoxButton.OK, MessageBoxImage.Information); // Уведомление о роли может быть не нужно в данном контексте
			}
			else
			{
				MessageBox.Show("Неверный логин или пароль, попробуйте еще раз.");
			}

		}

		private void RegistrButton_Click(object sender, RoutedEventArgs e)
		{
			var window_ = new Registr();
			Close();
			window_.ShowDialog();
		}
	}
}
