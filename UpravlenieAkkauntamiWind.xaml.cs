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
	/// Логика взаимодействия для UpravlenieAkkauntamiWind.xaml
	/// </summary>
	public partial class UpravlenieAkkauntamiWind : Window
	{ Entities entities = new Entities();	
		public UpravlenieAkkauntamiWind()
		{
			InitializeComponent();
			foreach(var user in entities.Users)
				AkkList.Items.Add(user);
			foreach (var item in entities.Role)
			{
				ComboRole.Items.Add(item);
			}
		}

		private void AkkList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var akk = AkkList.SelectedItem as Users;
			if (akk != null)
			{
				
				ComboRole.SelectedIndex = akk.Id - 1;
				ComboRole.SelectedItem = akk.Role;
			

			}
			else
			{
				ComboRole.SelectedIndex = -1;

			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			var akk = AkkList.SelectedItem as Users;
			if (akk != null)
			{
					var selectedRole = ComboRole.SelectedItem as Role;
					if (selectedRole != null)
					{
						akk.Role = selectedRole;
						entities.SaveChanges();
						MessageBox.Show("Роль успешно сохранена");
					}
					else
					{
						MessageBox.Show("Выберите роль для сохранения");
					}
				}
			else
			{
				MessageBox.Show("Выберите пользователя для изменения роли");
			}
		}
	}
}
