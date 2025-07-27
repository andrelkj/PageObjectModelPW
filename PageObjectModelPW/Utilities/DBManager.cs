using MySql.Data.MySqlClient;

namespace PageObjectModelPW.Utilities
{
    class DBManager
    {


        public static MySqlConnection Connection = null;


        public static void SetMySqldbConnection()
        {
            var connectionString = "server=127.0.0.1;user=root;password=selenium;database=seleniumdba";
            Connection = new MySqlConnection(connectionString);

            try
            {
                Connection.Open();
                Console.WriteLine("MySQL database connection established.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error establishing MySQL database connection: " + ex.Message);
            }

   
        }




        public static List<string> GetQuery(string query)
        {
            List<string> results = new List<string>();

            try
            {
                using MySqlCommand command = new MySqlCommand(query, Connection);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Assuming the SELECT statement returns a single string column
                    string value = reader.GetString(0);
                    results.Add(value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SELECT statement: " + ex.Message);
            }

            return results;
        }
       
    }
}
