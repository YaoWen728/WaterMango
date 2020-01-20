using System;
using Csla.Data;
using System.Data.SqlClient;

namespace TapMango.Domain.Extensions
{
    public delegate void TransactionAction(SqlTransaction tr);

    public class TransactionHandler
    {
        public TransactionHandler()
        {
        }

        public static void ProcessTransaction(TransactionAction action)
        {
            using (ConnectionManager mrg = ConnectionManager.GetManager("TapMangoDatabase"))
            {
                try
                {
                    var transaction = mrg.Connection.BeginTransaction();
                    action((SqlTransaction)transaction);
                    transaction.Commit();
                }
                catch
                {
                    mrg.Connection.Close();
                    throw;
                }
            }
        }
    }

    public static class TransactionHandlerExtension
    {
        public static SqlCommand CreateCommand(this SqlTransaction tr)
        {
            SqlCommand ret = null;
            if (tr.Connection != null)
            {
                ret = tr.Connection.CreateCommand();
                ret.Transaction = tr;
            }

            return ret;
        }
    }
}