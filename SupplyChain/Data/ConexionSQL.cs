using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SupplyChain
{
    public class ConexionSQL
    {
        private string SqlConnectionString;
        private SqlConnection Connection;
        private SqlCommand Command;
        private SqlDataAdapter DataAdatper;
        private DataTable DataTable;

        public ConexionSQL(string ConnectionString)
        {
            SqlConnectionString = ConnectionString;
            Connection = new SqlConnection(SqlConnectionString);
            try
            {
                Connection.Open();
            }
            catch (Exception)
            {
                //MessageBox.Show("Commit Exception Type: {0}  " + ex.GetType() + "\n Message: {0}  " + ex.Message + "\n\n" + SqlConnectionString, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public DataTable EjecutarSQL(string SQLCommandString)
        {
            try
            {
                Command = new SqlCommand(SQLCommandString, Connection);
                Command.CommandTimeout = 0;
                DataAdatper = new SqlDataAdapter(Command);
                DataTable = new DataTable();
                DataAdatper.Fill(DataTable);
                Connection.Close();
                DataAdatper.Dispose();
            }
            catch (Exception)
            {
                //MessageBox.Show("Commit Exception Type: {0}  " + ex.GetType() + "\n Message: {0}  " + ex.Message + "\n\n" + SQLCommandString, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return DataTable;
        }

        public void EjecutarSQLNonQuery(string SQLCommandString)
        {
            using (Connection)
            {
                SqlCommand Command = Connection.CreateCommand();
                Command.CommandTimeout = 0;
                SqlTransaction Transaction;

                // Start a local transaction.
                Transaction = Connection.BeginTransaction("Transaction");
                Command.Connection = Connection;
                Command.Transaction = Transaction;
                try
                {
                    Command.CommandText = SQLCommandString;
                    Command.ExecuteNonQuery();
                    // Attempt to commit the transaction.
                    Transaction.Commit();
                }
                catch (Exception)
                {
                    //MessageBox.Show("Commit Exception Type: {0}  " + ex.GetType() + "\n Message: {0}  " + ex.Message + "\n\n" + SQLCommandString, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Attempt to roll back the transaction.
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (Exception)
                    {
                        // This catch block will handle any errors that may have occurred on the server that would cause the rollback to fail, such as a closed connection.
                        //MessageBox.Show("Rollback Exception Type: {0}" + ex2.GetType() + "\n Message: {0}  " + ex2.Message + "\n\n" + SQLCommandString, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                Connection.Close();
            }
        }

        public void EjecutarSQLNonQuerySinTransaccion(string CommandString)
        {
            try
            {
                Command = new SqlCommand(CommandString, Connection);
                Command.CommandTimeout = 0;
                Command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                //MessageBox.Show("Commit Exception Type: {0}  " + ex.GetType() + "\n Message: {0}  " + ex.Message + "\n\n" + CommandString, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Connection.Close();
        }
    }
}