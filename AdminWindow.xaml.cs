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
	/// Логика взаимодействия для AdminWindow.xaml
	/// </summary>
	public partial class AdminWindow : Window
	{
		public AdminWindow()
		{
			InitializeComponent();

		}



		private void ButtonSotrudniki_Click(object sender, RoutedEventArgs e)
		{
			SotrudnikiWind window = new SotrudnikiWind();
			this.Close();
			window.ShowDialog();
		}

		private void ButtonDolzhnosti_Click(object sender, RoutedEventArgs e)
		{
			DolzhnostWind window = new DolzhnostWind();
			this.Close();
			window.ShowDialog();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ButtonOtdeli_Click(object sender, RoutedEventArgs e)
		{
			OtdeliWind window = new OtdeliWind();
			this.Close();
			window.ShowDialog();
		}

		private void UpravAkkButton_Click(object sender, RoutedEventArgs e)
		{
			UpravlenieAkkauntamiWind window = new UpravlenieAkkauntamiWind();
			this.Close();
			window.ShowDialog();
		}
    }
}
