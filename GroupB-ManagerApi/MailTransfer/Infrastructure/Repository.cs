using MailTransfer.Model;
using MailTransfer.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MailTransfer.Infrastructure
{
    public class mailData
    {
        public string ToEmail { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
    public interface IMailRepository<TEntity>
    {
         TEntity ApproveCustomer(int endId);
         TEntity getCredentials(int EnquiryId);
         string RejectedEnquiry(int EnqId,string feedback);
         EnquiryModel DisplayEnquiry(int EnquiryId);
         List<RequestPendingRejectedApprovedModel> PendingEnquiries(int ManagerId);
         List<RequestPendingRejectedApprovedModel> ApprovedEnquiries(int ManagerId);
         List<RequestPendingRejectedApprovedModel> RejectedEnquiries(int ManagerId);

    }
    public class MailRepository: BaseDataAccess,IMailRepository<mailData>
    {
        public MailRepository(IConfiguration config) : base(config) { } //injection happen here and pass it to base class
        public mailData ApproveCustomer(int EnquiryId)
        {

            string sql = "sp_ApproveEnquiry";
            var mailDetails= new mailData();
            try
            {
                ExecuteNonQuery(
                    sqlText: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@enqid", EnquiryId)
                    );
            }catch(SqlException sqlex)
            {
                throw sqlex;
            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                closeConnection();
                mailDetails = getCredentials(EnquiryId);
            }
            return mailDetails;
        }
        public mailData getCredentials(int EnquiryId)
        {
            var mailDetails=new mailData();
            string sqlCredentials = "sp_getCredentials";
            try
            {
                var reader = ExecuteReader(
                     sqlText: sqlCredentials,
                     commandType: CommandType.StoredProcedure,
                     new SqlParameter("@enqid", EnquiryId)
                     );
                while (reader.Read())
                {
                    mailDetails.ToEmail = reader.GetString(0);
                }
                reader.NextResult();
                while(reader.Read())
                {
                    mailDetails.userName = reader.GetString(0);
                    mailDetails.password= reader.GetString(1);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                closeConnection();
            }
            return mailDetails;
        }
        public string RejectedEnquiry(int EnquiryId,string feedback)
        {
            string sql = "sp_RejectEnquiry";
            string RejectedMail="";
            try
            {
                var reader= ExecuteReader(
                    sqlText: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@enqid", EnquiryId),
                    new SqlParameter("@feedback", feedback)
                    );
                while (reader.Read())
                {
                    RejectedMail = reader.GetString(0);
                }
            }catch(SqlException sqlex)
            {
                throw sqlex;
            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                
                closeConnection();
            }
            return RejectedMail;
        }
        public List<RequestPendingRejectedApprovedModel> PendingEnquiries(int ManagerId)
        {
            string sql = "sp_GetPendingRequestsForManager";
            List<RequestPendingRejectedApprovedModel> listEnquries= new List<RequestPendingRejectedApprovedModel>();
            try
            {
                var reader = ExecuteReader(sqlText: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@managerId", ManagerId)
                    );
                while (reader.Read())
                {
                    var Enquiries = new RequestPendingRejectedApprovedModel
                    {
                        EnquiryId = reader.GetInt32(0),
                        FullName = reader.GetString(1)
                    };
                    listEnquries.Add(Enquiries);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }catch (SqlException sqlex)
            {
                throw sqlex;
            }catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                closeConnection();
            }
            return listEnquries;
        }
        public List<RequestPendingRejectedApprovedModel> ApprovedEnquiries(int ManagerId)
        {
            string sql = "sp_GetApprovedEnquiries";
            List<RequestPendingRejectedApprovedModel> listEnquries = new List<RequestPendingRejectedApprovedModel>();
            try
            {
                var reader = ExecuteReader(sqlText: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@managerId", ManagerId)
                    );
                while (reader.Read())
                {
                    var Enquiries = new RequestPendingRejectedApprovedModel
                    {
                        EnquiryId = reader.GetInt32(0),
                        FullName = reader.GetString(1)
                    };
                    listEnquries.Add(Enquiries);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                closeConnection();
            }
            return listEnquries;
        }
        public List<RequestPendingRejectedApprovedModel> RejectedEnquiries(int ManagerId)
        {
            string sql = "sp_GetRjectedEnquiries";
            List<RequestPendingRejectedApprovedModel> listEnquries = new List<RequestPendingRejectedApprovedModel>();
            try
            {
                var reader = ExecuteReader(sqlText: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@managerId", ManagerId)
                    );
                while (reader.Read())
                {
                    var Enquiries = new RequestPendingRejectedApprovedModel
                    {
                        EnquiryId = reader.GetInt32(0),
                        FullName = reader.GetString(1)
                    };
                    listEnquries.Add(Enquiries);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                closeConnection();
            }
            return listEnquries;
        }
        public EnquiryModel DisplayEnquiry(int EnquiryId)
        {
            EnquiryModel model=null;
            string sql = "sp_DisplayEnquiry";
            try
            {
                var reader = ExecuteReader(
                    sqlText: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@enqId", EnquiryId)
                    );
                while (reader.Read())
                {
                    model = new EnquiryModel
                    {
                        FirstName = (reader["FirstName"] != DBNull.Value) ? reader["FirstName"].ToString() : null,
                        LastName = (reader["LastName"] != DBNull.Value) ? reader["LastName"].ToString() : null,
                        AddressLine1 = (reader["AddressLine1"] != DBNull.Value) ? reader["AddressLine1"].ToString() : null,
                        AddressLine2 = (reader["AddressLine2"] != DBNull.Value) ? reader["AddressLine2"].ToString() : null,
                        AddressLine3 = (reader["AddressLine3"] != DBNull.Value) ? reader["AddressLine3"].ToString() : null,
                        PhoneNumber = (reader["PhoneNumber"] != DBNull.Value) ? reader["PhoneNumber"].ToString() : null,
                        EmailAddress = (reader["EmailAddress"] != DBNull.Value) ? reader["EmailAddress"].ToString() : null,
                        DateOfBirth = (reader["DateOfBirth"] != DBNull.Value) ? Convert.ToDateTime(reader["DateOfBirth"]) : default(DateTime),
                        City = (reader["City"] != DBNull.Value) ? reader["City"].ToString() : null,
                        Country = (reader["Country"] != DBNull.Value) ? reader["Country"].ToString() : null,
                        Status = (reader["Status"] != DBNull.Value) ? reader["Status"].ToString() : null,
                        PinCode = (reader["PinCode"] != DBNull.Value) ? reader["PinCode"].ToString() : null,
                        WantsCheque = (reader["WantsCheque"] != DBNull.Value) ? Convert.ToBoolean(reader["WantsCheque"]) : default(bool),
                        Feedback = (reader["Feedback"] != DBNull.Value) ? reader["Feedback"].ToString() : null,
                        AccountType = (reader["AccountType"] != DBNull.Value) ? (int)reader["AccountType"] : 0,
                        Balance = (reader["Balance"] != DBNull.Value) ? (decimal)reader["Balance"] : 0
                    };
                }
                reader.NextResult();
                while (reader.Read())
                {
                    byte[] image = (byte[])reader["document"];
                    model.Photo = Convert.ToBase64String(image);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    byte[] image = (byte[])reader["document"];
                    model.Aadhaar = Convert.ToBase64String(image);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    byte[] image = (byte[])reader["document"];
                    model.panCard = Convert.ToBase64String(image);
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                
            }catch (SqlException ex)
            {
                throw ex;
            }catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                closeConnection();
            }
            return model;
        }
    }
}
