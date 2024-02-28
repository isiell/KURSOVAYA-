using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Логика взаимодействия для PersonalDataWindow.xaml
	/// </summary>
	public partial class PersonalDataWindow : Window
	{
		Entities entities = new Entities();
	
		private Lichnie_dannie personalData;
		string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

		public PersonalDataWindow(int personalDataId)
		{
			InitializeComponent();
			DateOfBirthDatePiker.SelectedDateChanged += DateOfBirthDatePicker_SelectedDateChanged;
			this.personalData = entities.Lichnie_dannie.FirstOrDefault(x => x.Id_sotrudnika == personalDataId);
			if (personalData != null)
			{
				DisplayPersonalData();
			}

		}
		private void DisplayPersonalData()
		{
			BitmapImage myBitmapImage = new BitmapImage();
			myBitmapImage.BeginInit();
			myBitmapImage.UriSource = new Uri(projectDirectory + "\\pic\\" + personalData.Sotrudniki.Photo);
			myBitmapImage.DecodePixelWidth = 200;
			myBitmapImage.EndInit();
			ImageSotrudnika.Source = myBitmapImage;
			this.FIOLabel.Content = personalData.Sotrudniki.Imya.ToString() + " " + personalData.Sotrudniki.Otchestvo.ToString() + " " + personalData.Sotrudniki.Familiya.ToString();
			this.addressTextBox.Text = personalData.Adres;
			this.genderTextBox.Text = personalData.Pol;
			this.MedPolTextBox.Text = personalData.Nomer_medpolisa;
			this.innTextBox.Text = personalData.INN;
			this.pensionNumberTextBox.Text = personalData.SNILS;
			this.DateOfBirthDatePiker.SelectedDate = personalData.Data_rozhdeniya;
			this.DateWorkDatePiker.SelectedDate = personalData.Data_priema_na_rab;
			this.DolzhnostLabel.Content = personalData.Sotrudniki.Dolzhnost.Naimenovanie_dolzhnost;
			this.OtdelLabel.Content = personalData.Sotrudniki.Otdeli.Namenovanie_otdela;
		}

		private void VozrastTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (DateTime.TryParse(VozrastTextBox.Text, out DateTime newDate))
			{
				DateOfBirthDatePiker.SelectedDate = newDate;
			}
		}
		private void DateOfBirthDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			DateTime selectedDate = DateOfBirthDatePiker.SelectedDate ?? DateTime.Now;
			int age = DateTime.Now.Year - selectedDate.Year;
			if (DateTime.Now.DayOfYear < selectedDate.DayOfYear)
			{
				age--;
			}

			if (age < 18)
			{
				MessageBox.Show("Сотрудник не может быть младше 18 лет.");
				DateOfBirthDatePiker.SelectedDate = DateTime.Now.AddYears(-18); // Устанавливаем дату на 18 лет назад
			}
			else
			{
				VozrastTextBox.Text = age.ToString();
			}
		}
		private void DateWorkDatePiker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}


		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (addressTextBox.Text == "" || genderTextBox.Text == "" || MedPolTextBox.Text == "" || innTextBox.Text == "" || pensionNumberTextBox.Text == "" || DateOfBirthDatePiker.Text == "" || DateWorkDatePiker.Text == "")
			{
				MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				if (personalData == null)
				{
					personalData = new Lichnie_dannie();
					entities.Lichnie_dannie.Add(personalData);
					entities.SaveChanges();
				}


				if (addressTextBox.Text.Length > 50)
				{
					personalData.Adres = addressTextBox.Text;
					MessageBox.Show("Длина адреса не может превышать 50 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				DateTime selectedDate;
				DateTime selectDate;
				if (DateTime.TryParse(DateOfBirthDatePiker.Text, out selectedDate) && DateTime.TryParse(DateWorkDatePiker.Text, out selectDate))
				{

					if (Regex.IsMatch(MedPolTextBox.Text, "^[0-9]{1,16}$") && Regex.IsMatch(innTextBox.Text, "^[0-9]{1,12}$") && Regex.IsMatch(pensionNumberTextBox.Text, "^[0-9]{1,12}$"))
					{
						if (genderTextBox.Text.Length == 1 && (genderTextBox.Text == "м" || genderTextBox.Text == "ж"))
						{

							personalData.Data_rozhdeniya = selectedDate;
							personalData.Data_priema_na_rab = selectDate;
							personalData.Nomer_medpolisa = MedPolTextBox.Text;
							personalData.INN = innTextBox.Text;
							personalData.SNILS = pensionNumberTextBox.Text;
							personalData.Pol = genderTextBox.Text;

							entities.SaveChanges();
							MessageBox.Show("Запись сохранена", "Нажмите ОК для продолжения", MessageBoxButton.OK, MessageBoxImage.Information);
						}
						else
						{
							MessageBox.Show("Введите корректное значение для пола (м или ж)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
					else
					{
						MessageBox.Show("Пожалуйста, введите только цифры (не более 16) для поля 'Номер медполиса',(не более 12) для поля 'ИНН', (не более 11 для поля 'Снилс'", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				else
				{
					MessageBox.Show("Некорректное значение данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void genderTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			SotrudnikiWind window = new SotrudnikiWind();
			this.Close();
			window.ShowDialog();
		}

		private void UpdateButton_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}
    
