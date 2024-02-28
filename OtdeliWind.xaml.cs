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
	/// Логика взаимодействия для OtdeliWind.xaml
	/// </summary>
	public partial class OtdeliWind : Window
	{
		Entities entities = new Entities();
		public OtdeliWind()
		{
			InitializeComponent();
			foreach (var otdel in entities.Otdeli)
				OtdeliListBox.Items.Add(otdel);
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			var otdel = OtdeliListBox.SelectedItem as Otdeli;
			if (Namenovanie_otdelaTextBox.Text == "" || Rukovoditel_otdelaTextBox.Text == "" || Contactnie_dannieTextBox.Text == "")
				MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			else
			{
				if (otdel == null)
				{
					otdel = new Otdeli();
					entities.Otdeli.Add(otdel);
					OtdeliListBox.Items.Add(otdel);
				}
				otdel.Namenovanie_otdela = Namenovanie_otdelaTextBox.Text;
				otdel.Rukovoditel_otdela = Rukovoditel_otdelaTextBox.Text;
				otdel.Contactnie_dannie = Contactnie_dannieTextBox.Text;
				entities.SaveChanges();
				OtdeliListBox.Items.Refresh();

				MessageBox.Show("Запись сохранена", "Нажмите ОК для продолжения", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			var delete = OtdeliListBox.SelectedItem as Otdeli;
			if (delete != null)
			{
				if (delete.Sotrudniki.Count == 0)
				{
					var result = MessageBox.Show("Вы действительно хотите удалить запись ?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
					if (result == MessageBoxResult.Yes)
					{
						entities.Otdeli.Remove(delete);
						entities.SaveChanges();
						Namenovanie_otdelaTextBox.Clear();
						Rukovoditel_otdelaTextBox.Clear();
						Contactnie_dannieTextBox.Clear();
						OtdeliListBox.Items.Remove(delete);

					}
				}
				else
					MessageBox.Show("Невозможно удалить запись, так как она связана с другими записями.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			else
				MessageBox.Show("Нет удаляемых объектов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			Namenovanie_otdelaTextBox.Text = "";
			Rukovoditel_otdelaTextBox.Text = "";
			Contactnie_dannieTextBox.Text = "";
			OtdeliListBox.SelectedIndex = -1;
			Namenovanie_otdelaTextBox.Focus();
		}

		private void OtdeliListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var otdel = OtdeliListBox.SelectedItem as Otdeli;
			if (otdel != null)
			{

				Namenovanie_otdelaTextBox.Text = otdel.Namenovanie_otdela.ToString();
				Rukovoditel_otdelaTextBox.Text = otdel.Rukovoditel_otdela.ToString();
				Contactnie_dannieTextBox.Text = otdel.Contactnie_dannie.ToString();
			}
			else
			{
				Namenovanie_otdelaTextBox.Text = "";
				Rukovoditel_otdelaTextBox.Text = "";
				Contactnie_dannieTextBox.Text = "";
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			AdminWindow window = new AdminWindow();
			this.Close();
			window.ShowDialog();
		}
	}
}

