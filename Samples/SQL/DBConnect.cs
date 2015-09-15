using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using MySql.Data.MySqlClient;

namespace SQL
{
	public class DBConnect
	{
		private MySqlConnection connection;
		private string server;
		private string database;
		private string SQLUID;
		private string SQLPassword;
		private int age;

		public DBConnect ()
		{
			Initialize ();
		}

		private void Initialize ()
		{
			server = "localhost";
			database = "sqlsample";
			SQLUID = "root";
			SQLPassword = "";
			string connectionString = "Server=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + SQLUID + ";" + "PASSWORD=" + SQLPassword + ";";
			connection = new MySqlConnection (connectionString);
		}

		private bool OpenConnection ()
		{
			try {
				connection.Open ();
				return true;
			} catch (MySqlException ex) {
				switch (ex.Number) {
				case 0:
					Console.WriteLine ("Cannot connect to server.  Contact administrator");
					break;

				case 1045:
					Console.WriteLine ("Invalid username/password, please try again");
					break;
				default:
					Console.WriteLine ("Generel SQL ERROR");
					break;
				}
				return false;
			}
		}

		private bool CloseConnection ()
		{
			try {
				connection.Close ();
				return true;
			} catch (MySqlException ex) {
				Console.WriteLine (ex.Message);
				return false;
			}
		}

		public void Insert (string username, string password, int age)
		{
			string query = "INSERT INTO user (Username, Password, Age) VALUES('" + username + "', '" + password + "', '" + age + "')";
			if (OpenConnection () == true) {
				MySqlCommand command = new MySqlCommand (query, connection);
				command.ExecuteNonQuery ();
				CloseConnection ();
			}
		}

		// Update password in this example
		public void Update (string username, string password)
		{
			string query = "UPDATE user SET Password='" + password + "' WHERE Username='" + username + "'";

			if (OpenConnection () == true) {
				MySqlCommand command = new MySqlCommand ();
				command.CommandText = query;
				command.Connection = connection;
				command.ExecuteNonQuery ();
				CloseConnection ();
			}
		}

		public void Delete (string username)
		{
			string query = "DELETE FROM user WHERE Username='" + username + "'";
			if (OpenConnection () == true) {
				MySqlCommand command = new MySqlCommand (query, connection);
				command.ExecuteNonQuery ();
				CloseConnection ();
			}
		}

		public List<string> [] Select ()
		{
			string query = "SELECT * FROM user";

			List<string>[] list = new List<string>[3];
			list [0] = new List<string> ();
			list [1] = new List<string> ();
			list [2] = new List<string> ();

			if (OpenConnection () == true) {
				MySqlCommand command = new MySqlCommand (query, connection);
				MySqlDataReader dataReader = command.ExecuteReader ();
				while (dataReader.Read ()) {
					list [0].Add (dataReader ["Username"] + "");
					list [1].Add (dataReader ["Password"] + "");
					list [2].Add (dataReader ["Age"] + "");
				}
				dataReader.Close ();
				CloseConnection ();
				return list;
			} else {
				return list;
			}
		}

		public int Count ()
		{
			string query = "SELECT Count(*) FROM user";
			int count = -1;
			if (OpenConnection () == true) {
				MySqlCommand command = new MySqlCommand (query, connection);
				count = int.Parse (command.ExecuteScalar () + "");
				CloseConnection ();
				return count;
			} else {
				return count;
			}
		}

		public void BackUp ()
		{
			try {
				DateTime time = DateTime.Now;
				int year = time.Year;
				int month = time.Month;
				int day = time.Day;
				int hour = time.Hour;
				int minute = time.Minute;
				int second = time.Second;
				int millisecond = time.Millisecond;

				string path = "$HOME/.mysql_backup/mysql_backup" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
				StreamWriter file = new StreamWriter (path);

				ProcessStartInfo psi = new ProcessStartInfo ();
				psi.FileName = "mysqldump";
				psi.RedirectStandardInput = false;
				psi.RedirectStandardOutput = true;
				psi.Arguments = string.Format (@"-u {0} -p {1} -h {2} {3}", SQLUID, SQLPassword, server, database);
				psi.UseShellExecute = false;

				Process process = Process.Start (psi);

				string output = process.StandardOutput.ReadToEnd ();
				file.WriteLine (output);
				process.WaitForExit ();
				file.Close ();
				process.Close ();
			} catch (IOException ex) {
				Console.WriteLine ("Error, unable to backup!");
			}
		}

		public void Restore ()
		{
			// Not implemented as some sort of file selector is needed.
			throw new NotImplementedException ();
		}
	}
}

