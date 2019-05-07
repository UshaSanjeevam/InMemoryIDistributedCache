using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryIDistributedCache.EF;
using InMemoryIDistributedCache;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using InMemoryIDistributedCache.Models;

namespace InMemoryIDistributedCache.Services
{
    public class UserService : IUserService
    {
        private Common _common;
        private DataContext _datacontext;

        public UserService(DataContext dataContext,Common common)
        {
            _common = common;
            _datacontext = dataContext;
        }
        public List<string> GetCacheData(int UserID)
        {
            List<String> Users = new List<String>();
            try
            {
                using (SqlCommand cm = new SqlCommand("WEB_stpUserDetails", (SqlConnection)_common.conString()))
                {
                    _common.conString().Open();
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.Add("@Application", SqlDbType.NVarChar,50).Value = "";
                    cm.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                    cm.Parameters.Add("@User", SqlDbType.NVarChar,200).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserName", SqlDbType.NVarChar,50).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserEmail", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@BranchID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@TimeZoneID", SqlDbType.NVarChar,100).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@BrandID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@ContactTypes", SqlDbType.NVarChar,1000).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@LastAccessedCustomerID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@LastAccessedCustomer", SqlDbType.NVarChar,200).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserType", SqlDbType.NVarChar,200).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserAcceptedTermsConditions", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserLogonExpired", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserSLITermsPath", SqlDbType.NVarChar,200).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@CustomerInventoryEnabled", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserInventoryEnabled", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@CreateSLITemplate", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@SubmitSLI", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@EditSLI", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@EmailShipmentDocs", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@ISSystemGeneratedPwd", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserCustomerDocsEnabled", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserReportsEnabled", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserDashboardEnabled", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserDashboardID", SqlDbType.NVarChar,1000).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserDashboardParams", SqlDbType.NVarChar,1000).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserDashboardServiceUser", SqlDbType.NVarChar,1000).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@UserDashboardServiceUserPWD", SqlDbType.NVarChar,1000).Direction = ParameterDirection.Output;
                    cm.Parameters.Add("@IDBSubModules", SqlDbType.NVarChar,1000).Direction = ParameterDirection.Output;
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.ExecuteNonQuery();
                    Users.Add((string)cm.Parameters["@User"].Value);
                    Users.Add((string)cm.Parameters["@UserName"].Value);
                    Users.Add((string)cm.Parameters["@UserEmail"].Value);
                    Users.Add((string)cm.Parameters["@UserDashboardServiceUserPWD"].Value);
                    Users.Add((string)cm.Parameters["@TimeZoneID"].Value);
                    Users.Add((string)cm.Parameters["@ContactTypes"].Value);
                    Users.Add((string)cm.Parameters["@UserType"].Value); 
                   
                      Users.Add((string)cm.Parameters["@ContactTypes"].Value); 
                      Users.Add((string)cm.Parameters["@UserDashboardID"].Value); 
                      Users.Add((string)cm.Parameters["@UserDashboardParams"].Value); 
                       Users.Add((string)cm.Parameters["@UserDashboardServiceUser"].Value); 
                      Users.Add((string)cm.Parameters["@UserDashboardServiceUserPWD"].Value); 
                       Users.Add((string)cm.Parameters["@IDBSubModules"].Value);
                    Users.Add((string)cm.Parameters["@LastAccessedCustomer"].Value); 
                    _common.conString().Close();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Users;
        }
        public DataTable Authenticate(string username, string password)
        {
            DataTable dsUserDetails = new DataTable();
            try
            {
                var commands = _datacontext.Database.GetDbConnection().CreateCommand();
                commands.CommandText = "[dbo].[getUserdetails_Test]";
                commands.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@Username", username);
                SqlParameter param1 = new SqlParameter("@password", password);
                commands.Parameters.Add(param);
                commands.Parameters.Add(param1);
                DbDataAdapter adapters = DbProviderFactories.GetFactory(_datacontext.Database.GetDbConnection()).CreateDataAdapter();
                adapters.SelectCommand = commands;
                adapters.Fill(dsUserDetails);
                if (dsUserDetails.Rows.Count > 0)
                {
                    dsUserDetails.TableName = "UserDetails";
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return dsUserDetails;
        }
    }
}
