using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace KURSOVAYA____
{
	/// <summary>
	/// Логика взаимодействия для SotrudnikiWind.xaml
	/// </summary>
	public partial class SotrudnikiWind : Window
	{

		Entities entities = new Entities();
		string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

		public SotrudnikiWind()
		{
			InitializeComponent();
			foreach (var sotrudnik in entities.Sotrudniki)
				SotrudnikiList.Items.Add(sotrudnik);
			foreach (var dolzhnost in entities.Dolzhnost)
				DolzhnostCombo.Items.Add(dolzhnost);
			foreach (var otdel in entities.Otdeli)
				OtdelCombo.Items.Add(otdel);
			ComboBoxFiltr.Items.Add("Программист");
			ComboBoxFiltr.Items.Add("Тим-лидер");
			ComboBoxFiltr.Items.Add("Системный администратор");
			ComboBoxFiltr.Items.Add("Аналитик");
			ComboBoxFiltr.Items.Add("Бухгалтер");
			ComboBoxFiltr.Items.Add("Все записи");

		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (userDb.role == "администратор")
			{
				DeleteButton.IsEnabled = true;
				var rezult = MessageBox.Show("Вы действительно хотите удалить Запись?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (rezult == MessageBoxResult.No)
					return;
				var delete = SotrudnikiList.SelectedItem as Sotrudniki;
				if (delete != null)
				{
					// Удаляем запись из таблицы Sotrudniki
					
					ImyaBox.Clear();
					FamiliyaBox.Clear();
					OtchestvoBox.Clear();
					im.Source = new BitmapImage();
					SotrudnikiList.Items.Remove(delete);
					OtdelCombo.SelectedIndex = -1;
					DolzhnostCombo.SelectedIndex = -1;

					// Удаляем связанные записи из таблицы RaschetZP
					foreach (var raschet in entities.RaschetZP.Where(r => r.Id_sotrudnika == delete.Id_sotrudnika).ToList())
					{
						entities.RaschetZP.Remove(raschet);
					}

					// Удаляем связанные записи из таблицы Lichnie_dannie
					foreach (var lichnie in entities.Lichnie_dannie.Where(l => l.Id_sotrudnika == delete.Id_sotrudnika).ToList())
					{
						entities.Lichnie_dannie.Remove(lichnie);
					}
					entities.Sotrudniki.Remove(delete);
					entities.SaveChanges();
					
				}
				else
				{
					MessageBox.Show("Нет удаляемых объектов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

				}
			}
			else { DeleteButton.IsEnabled = false; }

		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			var sotrudnik = SotrudnikiList.SelectedItem as Sotrudniki;
			if (ImyaBox.Text == "" || FamiliyaBox.Text == "" || OtchestvoBox.Text == "" || DolzhnostCombo.SelectedIndex == -1 || im.Source == null || OtdelCombo.SelectedIndex == -1)
				MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			else
			{
				if (sotrudnik == null)
				{

					sotrudnik = new Sotrudniki();
					entities.Sotrudniki.Add(sotrudnik);
					SotrudnikiList.Items.Add(sotrudnik);
				}

				sotrudnik.Imya = ImyaBox.Text;
				sotrudnik.Otchestvo = OtchestvoBox.Text;
				sotrudnik.Familiya = FamiliyaBox.Text;
				string fullFileName = im.Source.ToString();
				fullFileName = fullFileName.Replace(@"file:///", "");
				FileInfo fileInfo = new FileInfo(fullFileName);
				sotrudnik.Photo = fileInfo.Name;
				sotrudnik.Dolzhnost = DolzhnostCombo.SelectedItem as Dolzhnost;
				sotrudnik.Otdeli = OtdelCombo.SelectedItem as Otdeli;
				entities.SaveChanges();
				SotrudnikiList.Items.Refresh();


				MessageBox.Show("Запись сохранена", "Нажмите ОК для продолжения", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			FamiliyaBox.Text = "";
			ImyaBox.Text = "";
			OtchestvoBox.Text = "";

			im.Source = new BitmapImage();
			SotrudnikiList.SelectedIndex = -1;
			DolzhnostCombo.SelectedIndex = -1;
			OtdelCombo.SelectedIndex = -1;
			FamiliyaBox.Focus();
		}

		private void AddPhoto_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();

			dlg.InitialDirectory = projectDirectory + "\\pic\\";

			if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
				im.Source = new BitmapImage(new Uri(dlg.FileName));
			try
			{
				File.Copy(dlg.FileName, dlg.InitialDirectory + dlg.SafeFileName); MessageBox.Show("Ошибка: ");
			}
			catch
			{


			}
		}
		private void UpdateResultsCount(int totalCount)
		{
			resultsCountTextBlock.Text = $"Общее количество записей: {totalCount}";
		}

		private void UpdateSearchResultsCount(int count)
		{
			searchResultsCountTextBlock.Text = $"Количество записей в режиме поиска: {count}";
		}

		private void ShowNoResults()
		{
			noResultsTextBlock.Text = "Нет результатов поиска";
		}

		private void HideNoResults()
		{
			noResultsTextBlock.Text = string.Empty;
		}

		private void Poisk_TextChanged(object sender, TextChangedEventArgs e)
		{
			SotrudnikiList.Items.Clear();
			string text = Poisk.Text.ToLower();
			ObservableCollection<Sotrudniki> searchResults;

			if (string.IsNullOrEmpty(text))
			{
				searchResults = new ObservableCollection<Sotrudniki>(entities.Sotrudniki.ToList());
			}
			else
			{
				searchResults = new ObservableCollection<Sotrudniki>(entities.Sotrudniki.ToList()
				.Where(item => item.Imya.ToLower().Contains(text) || item.Familiya.ToString().Contains(text))
				.ToList());
			}
			UpdateResultsCount(entities.Sotrudniki.Count());
			UpdateSearchResultsCount(searchResults.Count);

			SotrudnikiList.Items.Clear();

			foreach (var item in searchResults)
			{
				SotrudnikiList.Items.Add(item);
			}

			if (searchResults.Count == 0)
			{
				ShowNoResults();
			}
			else
			{
				HideNoResults();
			}

		}

		private void BackBut_Click(object sender, RoutedEventArgs e)
		{
			if (userDb.role == "администратор")
			{
				BackBut.IsEnabled = true;
				AdminWindow window = new AdminWindow();
				this.Close();
				window.ShowDialog();
			}
			else
			{
				BackBut.IsEnabled = false;
			}
		}

		private void SotrudnikiList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var sotrudnik = SotrudnikiList.SelectedItem as Sotrudniki;
			if (sotrudnik != null)
			{
				BitmapImage myBitmapImage = new BitmapImage();
				myBitmapImage.BeginInit();
				myBitmapImage.UriSource = new Uri(projectDirectory + "\\pic\\" + sotrudnik.Photo);
				myBitmapImage.DecodePixelWidth = 200;
				myBitmapImage.EndInit();
				im.Source = myBitmapImage;

				FamiliyaBox.Text = sotrudnik.Familiya.ToString();
				ImyaBox.Text = sotrudnik.Imya.ToString();
				OtchestvoBox.Text = sotrudnik.Otchestvo.ToString();
				OtdelCombo.SelectedIndex = sotrudnik.Id_sotrudnika - 1;
				OtdelCombo.SelectedItem = sotrudnik.Otdeli;
				DolzhnostCombo.SelectedIndex = sotrudnik.Id_sotrudnika - 1;
				DolzhnostCombo.SelectedItem = sotrudnik.Dolzhnost;

			}
			else
			{
				FamiliyaBox.Text = "";
				ImyaBox.Text = "";
				OtchestvoBox.Text = "";
				OtdelCombo.SelectedIndex = -1;
				DolzhnostCombo.SelectedIndex = -1;

			}
		}

		private void LichDanButton_Click(object sender, RoutedEventArgs e)
		{

			Sotrudniki personalData = SotrudnikiList.SelectedItem as Sotrudniki;
			if (personalData != null)
			{
				Lichnie_dannie existingLichnie_dannie = entities.Lichnie_dannie.FirstOrDefault(ld => ld.Id_sotrudnika == personalData.Id_sotrudnika);

				if (existingLichnie_dannie == null)
				{
					MessageBox.Show("Карточка личных данных сотрудника еще не создана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				PersonalDataWindow personalDataWindow = new PersonalDataWindow(personalData.Id_sotrudnika);
				personalDataWindow.Show();
				this.Close();
				
			}
			else
			{
				MessageBox.Show("Выберите сотрудника из списка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{

			Sotrudniki personalData = SotrudnikiList.SelectedItem as Sotrudniki;

			if (personalData != null)
			{
				Lichnie_dannie existingLichnie_dannie = entities.Lichnie_dannie.FirstOrDefault(ld => ld.Id_sotrudnika == personalData.Id_sotrudnika);
				if (existingLichnie_dannie == null)
				{
					Lichnie_dannie newLichnie_dannie = new Lichnie_dannie
					{
						Id_sotrudnika = personalData.Id_sotrudnika
					};
					entities.Lichnie_dannie.Add(newLichnie_dannie);
					MessageBox.Show("Карточка личных данных сотрудника создана", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
				}

				entities.SaveChanges();
			}
			else
			{
				MessageBox.Show("Выберите сотрудника из списка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}
		private void ComboBoxFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SotrudnikiList.Items.Clear();

			if (ComboBoxFiltr.SelectedIndex == 0)
			{
				var zapros = entities.Sotrudniki.Where(s => s.Dolzhnost.Naimenovanie_dolzhnost == "Программист").ToList();
				foreach (var sotrudnik in zapros)
				{
					SotrudnikiList.Items.Add(sotrudnik);
				}
			}
			if (ComboBoxFiltr.SelectedIndex == 1)
			{
				var zapros = entities.Sotrudniki.Where(s => s.Dolzhnost.Naimenovanie_dolzhnost == "Тим-лидер").ToList();
				foreach (var sotrudnik in zapros)
				{
					SotrudnikiList.Items.Add(sotrudnik);
				}
			}
			if (ComboBoxFiltr.SelectedIndex == 2)
			{
				var zapros = entities.Sotrudniki.Where(s => s.Dolzhnost.Naimenovanie_dolzhnost == "Системный администратор").ToList();
				foreach (var sotrudnik in zapros)
				{
					SotrudnikiList.Items.Add(sotrudnik);
				}
			}
			if (ComboBoxFiltr.SelectedIndex == 3)
			{
				var zapros = entities.Sotrudniki.Where(s => s.Dolzhnost.Naimenovanie_dolzhnost == "Аналитик").ToList();
				foreach (var sotrudnik in zapros)
				{
					SotrudnikiList.Items.Add(sotrudnik);
				}
			}
			if (ComboBoxFiltr.SelectedIndex == 4)
			{
				var zapros = entities.Sotrudniki.Where(s => s.Dolzhnost.Naimenovanie_dolzhnost == "Бухгалтер").ToList();
				foreach (var sotrudnik in zapros)
				{
					SotrudnikiList.Items.Add(sotrudnik);
				}
			}

			if (ComboBoxFiltr.SelectedIndex == 5)
			{
				foreach (var sotrudnik in entities.Sotrudniki)
				{
					SotrudnikiList.Items.Add(sotrudnik);
				}
			}
			
		}
		private void FilterButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}

