﻿using Microsoft.Data.SqlClient;
using System.Data;
using BusinessModel;

namespace DataAccess
{
	public class UpdateContactsDataAccess : IUpdateContacts
	{
		private string connString = GlobalValues.ConnectionString;
		private readonly ParamUpdateContactsModel? _contacts;

		public UpdateContactsDataAccess(ParamUpdateContactsModel? contacts)
		{
			
			_contacts = contacts;
		}


        async public Task<ReturnUpdateContactModel> UpdateContacts()
        {
			ReturnUpdateContactModel dataModel = new();

			using (SqlConnection conn = new SqlConnection(connString))
			{
				conn.Open();
				using (SqlCommand cmd = new SqlCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "[speedx.hrms.contacts].[spUpdateContacts]";
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.Add(new SqlParameter("@MasterPersonID", SqlDbType.Int));
					cmd.Parameters["@MasterPersonID"].Value = _contacts?.MasterPersonID;

					cmd.Parameters.Add(new SqlParameter("@PropertyName", SqlDbType.NVarChar));
					cmd.Parameters["@PropertyName"].Value = _contacts?.PropertyName;

					cmd.Parameters.Add(new SqlParameter("@PropertyValue", SqlDbType.NVarChar));
					cmd.Parameters["@PropertyValue"].Value = _contacts?.PropertyValue;

					using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
					{
						//Check for errors and if true, retreive the error message!

						if (reader.GetSchemaTable().Rows[0].ItemArray[0]?.ToString() == "ErrorMessage")
						{
							if (reader.HasRows)
							{
								reader.Read();
								dataModel.HasError = true;
								dataModel.ErrorMessage = reader["ErrorMessage"].ToString();
							}
						}
						else
						{
							if (reader.HasRows)
							{
								reader.Read();
                                dataModel.MasterPersonID = Convert.ToInt32(reader["MasterPersonID"]);
                                dataModel.PropertyName = reader["PropertyName"].ToString();
                                dataModel.PropertyValue = reader["PropertyValue"].ToString();
                            }
							reader.NextResult();
                            if (reader.HasRows)
                            {
                                reader.Read();
                                dataModel.StatusCodeNumber = Convert.ToInt32(reader["StatusCodenumber"]);
                            }

                        }
					}
				}
			}
			return dataModel;
		}
    }
}




