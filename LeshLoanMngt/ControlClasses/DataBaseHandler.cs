using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace LeshLoanMngt.ControlClasses
{
    public class DataBaseHandler
    {
            Database db;
            private DbCommand command;
            public string ConnectionString = "LeshLoanConString";

            public DataBaseHandler()
            {
                try
                {
                    db = DatabaseFactory.CreateDatabase(ConnectionString);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            internal DataSet ExecuteDataSet(string storedProcedureName, params string[] Parameters)
            {
                try
                {
                    command = db.GetStoredProcCommand(storedProcedureName,
                                                               Parameters
                                                              );
                    DataSet ds = db.ExecuteDataSet(command);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            internal DataTable ExecuteDataTable(string storedProcedureName, params string[] Parameters)
            {
                try
                {
                    command = db.GetStoredProcCommand(storedProcedureName,
                                                               Parameters
                                                              );
                    DataTable dt = db.ExecuteDataSet(command).Tables[0];
                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            internal int ExecuteNonQuery(string storedProcedureName, params object[] Parameters)
            {
                try
                {
                    command = db.GetStoredProcCommand(storedProcedureName,
                                                               Parameters
                                                              );
                    int rows = db.ExecuteNonQuery(command);
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
