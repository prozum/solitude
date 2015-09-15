/*
 * Uses a modified version of the guide found at http://www.codeproject.com/Articles/43438/Connect-C-to-MySQL
 * The database contains the fields Username, Password, Age
 * Database username is root with no password
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
	class Program
	{
		static void Main (string[] args)
		{
			DBConnect db = new DBConnect ();
			db.Insert ("user1", "passwd1", 1);
			db.Insert ("user2", "passwd2", 2);
			db.Insert ("user3", "passwd3", 3);
			db.Update ("user2", "passwd4");
			db.Delete ("user3");
		}
	}
}
