using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace DataSources
{
    class Program
    {
        static SQLiteConnection m_dbConnection;
        static void Main(string[] args)
        {
            SQLiteConnection.CreateFile("MyDatabase.sqlite");

            m_dbConnection = new SQLiteConnection("Data Source=MyDataBase.sqlite;Version=3");
            m_dbConnection.Open();

            string sqlCreate = "create table highscores (name varchar(20), score int)";

            SQLiteCommand createCommand = new SQLiteCommand(sqlCreate, m_dbConnection);

            createCommand.ExecuteNonQuery();

            string sqlInsert1 = "insert into highscores(name,score) values ('Me',9001)";
            var insertCommand = new SQLiteCommand(sqlInsert1, m_dbConnection);
            insertCommand.ExecuteNonQuery();

            sqlInsert1 = "insert into highscores(name,score) values ('Me',300)";
            insertCommand = new SQLiteCommand(sqlInsert1, m_dbConnection);
            insertCommand.ExecuteNonQuery();

            sqlInsert1 = "insert into highscores(name,score) values ('MySelf',6000)";
            insertCommand = new SQLiteCommand(sqlInsert1, m_dbConnection);
            insertCommand.ExecuteNonQuery();

            sqlInsert1 = "insert into highscores(name,score) values ('I',10000)";
            insertCommand = new SQLiteCommand(sqlInsert1, m_dbConnection);
            insertCommand.ExecuteNonQuery();


            Console.ReadLine();

            string selectSql = "select * from highscores order by score desc";
            var selectCommand = new SQLiteCommand(selectSql, m_dbConnection);

            using (var reader = selectCommand.ExecuteReader())
            {
                while(reader.Read())
                {
                    Console.WriteLine("Name: {0} \n\tScore:{1}", reader["name"], reader["score"]);
                }
            }
            m_dbConnection.Close();
            Console.ReadLine();
        }
    }
}
