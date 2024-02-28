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
	/// Логика взаимодействия для DolzhnostWind.xaml
	/// </summary>
	public partial class DolzhnostWind : Window
	{
		Entities entities = new Entities();
		public DolzhnostWind()
		{
			InitializeComponent();
			foreach(var dolzhnost in entities.Dolzhnost)
				DolzhnostListBox.Items.Add(dolzhnost);
		}

		private void DolzhnostListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var dolzhnost = DolzhnostListBox.SelectedItem as Dolzhnost;
			if (dolzhnost != null)
			{

				Naimenovanie_dolzhnostTextBox.Text = dolzhnost.Naimenovanie_dolzhnost.ToString();
				Uroven_dolzhnostiTextBox.Text = dolzhnost.Uroven_dolzhnosti.ToString();
				Status_dolzhnostiTextBox.Text = dolzhnost.Status_dolzhnosti.ToString();
			}
			else
			{
				Naimenovanie_dolzhnostTextBox.Text = "";
				Uroven_dolzhnostiTextBox.Text = "";
				Status_dolzhnostiTextBox.Text = "";
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			var dolzh = DolzhnostListBox.SelectedItem as Dolzhnost;
			if (Naimenovanie_dolzhnostTextBox.Text == "" || Uroven_dolzhnostiTextBox.Text == "" || Status_dolzhnostiTextBox.Text == "")
				MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			else
			{
				if (dolzh == null)
				{
					dolzh = new Dolzhnost();
					entities.Dolzhnost.Add(dolzh);
					DolzhnostListBox.Items.Add(dolzh);
				}
				dolzh.Naimenovanie_dolzhnost= Naimenovanie_dolzhnostTextBox.Text;
				dolzh.Uroven_dolzhnosti= Uroven_dolzhnostiTextBox.Text;
				dolzh.Status_dolzhnosti = Status_dolzhnostiTextBox.Text;
				entities.SaveChanges();
				DolzhnostListBox.Items.Refresh();

				MessageBox.Show("Запись сохранена", "Нажмите ОК для продолжения", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			var delete = DolzhnostListBox.SelectedItem as Dolzhnost;
			if (delete != null)
			{
				if (delete.Sotrudniki.Count == 0)
				{
					var result = MessageBox.Show("Вы действительно хотите удалить запись ?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
					if (result == MessageBoxResult.Yes)
					{
						entities.Dolzhnost.Remove(delete);
						entities.SaveChanges();
						Naimenovanie_dolzhnostTextBox.Clear();
						Uroven_dolzhnostiTextBox.Clear();
						Status_dolzhnostiTextBox.Clear();
						DolzhnostListBox.Items.Remove(delete);

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
			Naimenovanie_dolzhnostTextBox.Text = "";
			Uroven_dolzhnostiTextBox.Text = "";
			Status_dolzhnostiTextBox.Text = "";
			DolzhnostListBox.SelectedIndex = -1;
			Naimenovanie_dolzhnostTextBox.Focus();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			AdminWindow window = new AdminWindow();
			this.Close();
			window.ShowDialog();
		}

		private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}
	}
}
