using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
using System.Data.Common.CommandTrees.ExpressionBuilder;
using Exel = Microsoft.Office.Interop.Excel;

namespace KURSOVAYA____
{
	/// <summary>
	/// Логика взаимодействия для Rasschet.xaml
	/// </summary>
	public partial class Rasschet : Window
	{
		Entities entities = new Entities();
		public Rasschet()
		{
			InitializeComponent();
			foreach (var sotrudnik in entities.Sotrudniki)
				SotrudnikiLB.Items.Add(sotrudnik);
		}

		private void RasschetButton_Click(object sender, RoutedEventArgs e)
		{
			int rate = 0;
			int hours = 0;
			
			if (int.TryParse(StavkaTB.Text, out rate) && int.TryParse(Kol_vo_chTB.Text, out hours))
			{
				if (NochRadioButton.IsChecked == true)
				{
					if (hours > 8)
					{
						MessageBox.Show("Ошибка: Количество часов не может быть более 7 при выборе ночного тарифа.");
						return;
					}
					else
					{
						if (SotrudnikiLB.SelectedItem == null)
						{
							MessageBox.Show("Выберите сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
							
							return;
						}
						rate = (int)(rate * 1.2);
						int payment = rate * hours;
						Summa_k_viplateTB.Text = payment.ToString();
						double tax = payment * 0.13;
						int finalPayment = (int)(payment - tax);
						SummasvichetomTextBox.Text = finalPayment.ToString();
						RaschetZP newRasschet = new RaschetZP
						{
							Id_sotrudnika = ((Sotrudniki)SotrudnikiLB.SelectedItem).Id_sotrudnika,
							Stavka = rate,
							Kol_vo_chasov = hours,
							Summa_k_viplate_do = payment,
							Summa_k_viplate_posle =finalPayment,
							Data = DataRaschetaDatePicker.SelectedDate

						};
						entities.RaschetZP.Add(newRasschet);
						entities.SaveChanges();
					}
				}
				else if (DenButton.IsChecked == true)
				{
					if (hours > 8)
					{
						MessageBox.Show("Ошибка: Количество часов не может быть более 8 при выборе дневного тарифа.");
						return;
					}
					if (SotrudnikiLB.SelectedItem == null)
					{
						MessageBox.Show("Выберите сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
					int payment = rate * hours;
					Summa_k_viplateTB.Text = payment.ToString();
					double tax = payment * 0.13;
					int finalPayment = (int)(payment - tax);
					SummasvichetomTextBox.Text = finalPayment.ToString();
					entities.SaveChanges();
					RaschetZP newRasschet = new RaschetZP
						{
							Id_sotrudnika = ((Sotrudniki)SotrudnikiLB.SelectedItem).Id_sotrudnika,
							Stavka = rate,
							Kol_vo_chasov = hours,
							Summa_k_viplate_do = payment,
							Summa_k_viplate_posle =finalPayment,
							Data = DataRaschetaDatePicker.SelectedDate

						};
						entities.RaschetZP.Add(newRasschet);
						entities.SaveChanges();
				}
				else
				{
					MessageBox.Show("Пожалуйста, выберите тариф: ночной или дневной.");
				}
				
			}
			else
			{
				MessageBox.Show("Пожалуйста, введите корректные данные для ставки и количества часов.");
			}
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			StavkaTB.Clear();
			Kol_vo_chTB.Clear();
			Summa_k_viplateTB.Clear();
			SummasvichetomTextBox.Clear();
			SotrudnikiLB.SelectedIndex = -1;
			StavkaTB.Focus();
		}

		private void AddDelButton_Click(object sender, RoutedEventArgs e)
		{
			SotrudnikiWind window = new SotrudnikiWind();
			this.Close();
			window.ShowDialog();
		}

		private void ExportButton_Click(object sender, RoutedEventArgs e)
		{
			Exel.Application exApp = new Exel.Application();
			exApp.Workbooks.Add();
			Exel.Worksheet wsh = (Exel.Worksheet)exApp.ActiveSheet;
			Sotrudniki selectedSotrudnik = (Sotrudniki)SotrudnikiLB.SelectedItem;
			var selectedRaschets = entities.RaschetZP.Where(r => r.Id_sotrudnika == selectedSotrudnik.Id_sotrudnika);
			wsh.Cells[1, 1] = $"Фамилия";
			wsh.Cells[1, 2] = $"Имя";
			wsh.Cells[1, 3] = $"Отчество";
			wsh.Cells[1, 4] = $"Дата";
			wsh.Cells[1, 5] = $"Сумма к выплате до вычета налога";
			wsh.Cells[1, 6] = $"Сумма к выплате после вычета налога";

			int rowCount = 2;

			foreach (var raschet in selectedRaschets)
			{
				wsh.Cells[rowCount, 1] = selectedSotrudnik.Familiya;
				wsh.Cells[rowCount, 2] = selectedSotrudnik.Imya;
				wsh.Cells[rowCount, 3] = selectedSotrudnik.Otchestvo;
				wsh.Cells[rowCount, 4] = raschet.Data.ToString();
				wsh.Cells[rowCount, 5] = raschet.Summa_k_viplate_do;
				wsh.Cells[rowCount, 6] = raschet.Summa_k_viplate_posle;
				rowCount++;
			}
			exApp.Visible = true;
		}
	}
}
