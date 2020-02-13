using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace BackEnd
{
	public class InitDB
	{
		private static MySqlConnection connection;
		private static string server = "localhost";
		private static string port = "3307";
		private static string database = "project_sylveon";
		private static string username = "init";
		private static string password = "password";
		public static void Run()
		{
			InitializeConnection();
			try
			{
				connection.Open();
				try
				{
					CreateDatabase();
					CreateTables();
					CreateProcedures();
					//DeleteInit();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
				finally
				{
					connection.Close();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}




		}

		private static void InitializeConnection()
		{
			string connectionString = "SERVER=" + server + ";PORT=" + port + ";user=" + username + ";PASSWORD=" + password + ";";

			connection = new MySqlConnection(connectionString);
		}
		private static async void CreateDatabase()
		{
			string createDB = "CREATE DATABASE " + database + ";";
			MySqlCommand command = new MySqlCommand(createDB, connection);
			await command.ExecuteNonQueryAsync();
			command.CommandText = "USE " + database + ";";
			await command.ExecuteNonQueryAsync();
		}
		private static async void CreateTables()
		{
			string createTable = "CREATE TABLE levels (level_number INT AUTO_INCREMENT PRIMARY KEY, level VARCHAR(3000));";
			MySqlCommand command = new MySqlCommand(createTable, connection);
			await command.ExecuteNonQueryAsync();
			command.CommandText = "CREATE TABLE users (id INT AUTO_INCREMENT PRIMARY KEY, name VARCHAR(100));";
			await command.ExecuteNonQueryAsync();
			command.CommandText = "CREATE TABLE scores (id INT AUTO_INCREMENT PRIMARY KEY, number_of_lines INT);";
			await command.ExecuteNonQueryAsync();
			command.CommandText = "CREATE TABLE user_scores (id INT AUTO_INCREMENT PRIMARY KEY, user_id INT, level_number INT, scores_id INT, "
								+ " FOREIGN KEY(user_id) REFERENCES users(id) ON DELETE CASCADE, "
								+ " FOREIGN KEY(level_number) REFERENCES levels(level_number) ON DELETE CASCADE, "
								+ " FOREIGN KEY(scores_id) REFERENCES scores(id) ON DELETE CASCADE)";
			await command.ExecuteNonQueryAsync();
		}
		private static async void CreateProcedures()
		{
			string procedure = "CREATE PROCEDURE addUser (IN newName nvarchar(100)) BEGIN ";
			procedure += "INSERT INTO users (name) VALUES (QUOTE(newName)); END";
			MySqlCommand command = new MySqlCommand(procedure, connection);
			await command.ExecuteNonQueryAsync();

		}
		private static void DeleteInit()
		{
			string deleteUser = "DROP USER '" + username + "'@'" + server + "';";
			MySqlCommand command = new MySqlCommand(deleteUser, connection);
			command.ExecuteNonQueryAsync();
		}
	}
}
