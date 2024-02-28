using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KURSOVAYA____
{
	public class userDb
	{
		public static string role { get; set; }
	}
	partial class Users
	{
		public override string ToString()
		{
			return login;
		}
	}
	partial class Role
	{
		public override string ToString()
		{
			return nazvanie_roli;
		}
	}

	public partial class Sotrudniki
	{
		public override string ToString()
		{
			return "Фамилия: " + Familiya + "\n" + "Имя: " + Imya+ " \n" + "Отчество: " + Otchestvo ;

		}
	}
	public partial class Dolzhnost
	{
		public override string ToString()
		{
			return Naimenovanie_dolzhnost;
		}
	}
	public partial class Otdeli
	{
		public override string ToString()
		{
			return Namenovanie_otdela;
		}
	}
}
