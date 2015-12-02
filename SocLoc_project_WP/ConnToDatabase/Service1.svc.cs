using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;

namespace ConnToDatabase
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        string serverName;
        string databaseName;
        string userName;
        string passwordToAccount;
        MySqlConnection mySqlConn;

        Service1(string srvNm, string dbNm, string usrNm, string pswd)
        {
            serverName = srvNm;
            databaseName = dbNm;
            userName = usrNm;
            passwordToAccount = pswd;
            mySqlConn = new MySqlConnection("Server=" + serverName + "; Database=" + databaseName + "; Uid=" + userName + "; Pwd=" + passwordToAccount + ";");
        }

        public void ExMethod()
        {
            String sqlQuery = "";

            try
            {
                mySqlConn.Open();
                MySqlCommand command = new MySqlCommand(sqlQuery, mySqlConn);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        Console.WriteLine(reader[0].ToString() + reader[1].ToString());

                }
                //For non query
                //string sql = "INSERT INTO Country (Name, HeadOfState, Continent) VALUES ('Disneyland','Mickey Mouse', 'North America')";
                //MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                mySqlConn.Dispose();
            }

        }

        public Boolean LogInUser(string emailAddress, string password)
        {
            String sqlQuery = "select count(*) from users where email = " + emailAddress + "and password" + password;
            int count = 0;
            Boolean result = false;
            try
            {
                mySqlConn.Open();
                MySqlCommand command = new MySqlCommand(sqlQuery, mySqlConn);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        count = Int16.Parse(reader[0].ToString());
                    if (count == 1)
                        result = true;
                    else
                        result = false;
                }
                //For non query
                //string sql = "INSERT INTO Country (Name, HeadOfState, Continent) VALUES ('Disneyland','Mickey Mouse', 'North America')";
                //MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;

            }
            finally
            {
                mySqlConn.Dispose();
            }
            return result;
        }
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
