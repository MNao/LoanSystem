using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LeshLoanMngt.Entities;

namespace LeshLoanMngt.ControlClasses
{
    internal class BusinessLogic
    {
            public BusinessLogic()
            {
            }

            DataBaseHandler dh = new DataBaseHandler();

            public DataSet ExecuteDataSet(string storedProcName, string[] parameters)
            {
                return dh.ExecuteDataSet(storedProcName, parameters);
            }

            public int ExecuteNonQuery(string storedProcName, string[] parameters)
            {
                return dh.ExecuteNonQuery(storedProcName, parameters);
            }

            public SystemUser Login(string UserId, string Password, string Identifier)
            {
                SystemUser user = new SystemUser();

                DataTable dt = dh.ExecuteDataSet("GetSystemUserByUserId", new string[] { UserId }).Tables[0];
                DataRow dr = dt.Rows[0];
                if (dt.Rows.Count <= 0)
                {
                    user.StatusCode = Globals.FAILURE_STATUS_CODE;
                    user.StatusDesc = "USER NOT FOUND";//"USER WITH ID [" + UserId + "] NOT FOUND";
                    return user;
                }

                user.RoleCode = dr["RoleCode"].ToString();
                if (user.RoleCode == "005" && Identifier != "app")
                {
                    user.StatusCode = Globals.FAILURE_STATUS_CODE;
                    user.StatusDesc = "USER NOT ALLOWED TO LOG IN";
                    return user;
                }

                
                user.Password = dr["Password"].ToString();
                string md5HashOfPassword = Password;//SharedCommons.GenerateMD5Hash(Password);

                if (user.Password.ToUpper() != md5HashOfPassword.ToUpper())
                {
                    user.StatusCode = Globals.FAILURE_STATUS_CODE;
                    user.StatusDesc = "FAILED: INVALID USERNAME OR PASSWORD SUPPLIED";
                    return user;
                }
                string ResetPassword = dr["ResetPassword"].ToString();

                if (ResetPassword == "0")
                {
                    ResetPassword = "FALSE";
                }
                else
                {
                    ResetPassword = "TRUE";
                }
                user.ResetPassword = Convert.ToBoolean(ResetPassword);

                user.StatusCode = Globals.SUCCESS_STATUS_CODE;
                user.StatusDesc = Globals.SUCCESS_STATUS_TEXT;

                return user;
            }

            public Result LogOut(string UserId)
            {
                Result result = new Result();
                return result;
            }
            public Result LogError(string ErrorIdentifier, string StackTrace, string BankCode, string Message, string ErrorType)
            {
                Result result = new Result();
                try
                {
                    int rowsAffected = dh.ExecuteNonQuery("LogInterfaceError", ErrorIdentifier, StackTrace, BankCode, Message, ErrorType);
                }
                catch (Exception ex)
                {
                    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                    result.StatusDesc = "EXCEPTION: " + ex.Message;
                }
                return result;
            }

            public Result SaveSystemUser(SystemUser user)
            {
                Result result = new Result();

                if (!user.IsValid())
                {
                    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                    result.StatusDesc = user.StatusDesc;
                    return result;
                }
                DataTable dt = dh.ExecuteDataSet("SaveSystemUser", user.CompanyCode, user.UserId, user.Password, user.Name, user.RoleCode, user.ModifiedBy, user.IsActive, user.Email ).Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                    result.StatusDesc = "USER WITH ID [" + user.UserId + "] NOT SAVED";
                    return result;
                }

                result.StatusCode = Globals.SUCCESS_STATUS_CODE;
                result.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
                result.LoanID = dt.Rows[0][0].ToString();
                return result;
            }

            public Result SaveClientDetails(ClientDetails req)
            {
                Result result = new Result();
                if (!req.IsValid())
                {
                    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                    result.StatusDesc = req.StatusDesc;
                    return result;
                }
                //req.CustomerPhoto = GetBase64String(req.CustomerPhoto);
                //req.IDPhoto = GetBase64String(req.IDPhoto);
                //req.BackIDPhoto = GetBase64String(req.BackIDPhoto);
                //req.CustomerSignPhoto = GetBase64String(req.CustomerSignPhoto);
                //req.RegFormPhoto = GetBase64String(req.RegFormPhoto);
                //req.KeyFactsPhoto = GetBase64String(req.KeyFactsPhoto);


                DataTable dt = dh.ExecuteDataSet("SaveClientsDetails", req.ClientNo, req.ClientName, req.ClientAddress, req.ClientPhoneNumber, req.Gender, req.IDType, req.IDNumber, req.ClientPhoto, req.IDPhoto, req.Referee, req.RefrereePhoneNo, req.ClientEmail, req.ModifiedBy, req.ClientPassword).Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                    result.StatusDesc = "Client DETAILS NOT SAVED";
                    return result;
                }

                result.StatusCode = Globals.SUCCESS_STATUS_CODE;
                result.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
                result.LoanID = dt.Rows[0][0].ToString();
                return result;
            }

        public Result SaveAdditionalClientDetails(ClientDetails req)
        {
            Result result = new Result();


            DataTable dt = dh.ExecuteDataSet("UpdatewithAdditionalClientsDetails", req.ClientNo, req.DOB, req.BusinessLoc, req.Occupation, req.NoOfBeneficiaries, req.EducLevel, req.MonthlyIncome, req.ModifiedBy).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = "Additional Client DETAILS NOT SAVED";
                return result;
            }

            result.StatusCode = Globals.SUCCESS_STATUS_CODE;
            result.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
            result.LoanID = dt.Rows[0][0].ToString();
            return result;
        }
        
        public Result SaveLoanDetails(LoanDetails req)
        {
            Result result = new Result();
            if (!req.IsValid())
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = req.StatusDesc;
                return result;
            }

            DataTable dt = dh.ExecuteDataSet("SaveLoanDetails", req.ClientID, req.LoanNo, req.LoanDate, req.LoanAmount, req.InterestRate, req.ApprovedAmount, req.LoanDesc, req.LastLoanAmount, req.CurrentDebt, req.Organization, req.EasyPaidAmountPerMonth, req.ModifiedBy).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = "Loan DETAILS NOT SAVED";
                return result;
            }

            result.StatusCode = Globals.SUCCESS_STATUS_CODE;
            result.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
            result.LoanID = dt.Rows[0][0].ToString();
            return result;
        }

        private string GetBase64String(string Base64String)
            {
                byte[] byteCustPhoto = Encoding.UTF8.GetBytes(Base64String);
                //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                //MemoryStream stream = new MemoryStream(byteArray);

                //System.IO.Stream fs = ;
                //StreamReader reader = new StreamReader(Base64String);//System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                //Byte[] bytes = reader.ReadBytes((Int32)fs.Length);

                //Byte[] byteCustPhoto = Encoding.ASCII.GetBytes(Base64String);
                string base64String = Convert.ToBase64String(byteCustPhoto, 0, byteCustPhoto.Length);
                base64String = "data:image/jpg;base64," + base64String;
                return base64String;
            }

            public Result SaveSystemSetting(SystemSetting req)
            {
                Result result = new Result();

                if (!req.IsValid())
                {
                    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                    result.StatusDesc = req.StatusDesc;
                    return result;
                }

                DataTable dt = dh.ExecuteDataSet("SaveSystemSetting", new string[] { req.CompanyCode, req.SettingCode, req.SettingValue, req.ModifiedBy }).Tables[0];
                if (dt.Rows.Count <= 0)
                {
                    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                    result.StatusDesc = "FAILED: SYSTEM SETTING NOT SAVED";
                    return result;
                }

                result.StatusCode = Globals.SUCCESS_STATUS_CODE;
                result.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
                result.LoanID = dt.Rows[0][0].ToString();
                return result;
            }

        public Result SaveReceipt(Receipt recpt)
        {
            Result result = new Result();

            if (!recpt.IsValid())
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = recpt.StatusDesc;
                return result;
            }

            DataTable dt = dh.ExecuteDataSet("SaveReceipt", new string[] { recpt.CompanyCode, recpt.ClientID, recpt.ReceiptNumber, recpt.LoanNumber, recpt.PaymentType, recpt.PaymentDate, recpt.ReceiptAmount, recpt.CurrencyCode, recpt.ModifiedBy }).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                result.StatusCode = Globals.FAILURE_STATUS_CODE;
                result.StatusDesc = "FAILED: PAYMENT RECEIPT NOT SAVED";
                return result;
            }

            result.StatusCode = Globals.SUCCESS_STATUS_CODE;
            result.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
            result.LoanID = dt.Rows[0][0].ToString();
            return result;
        }


        public ClientDetails[] GetClientDetails(string BankCode, string UserID, string ClientID)
            {
            ClientDetails[] ClientDetails = new ClientDetails[0];
                List<ClientDetails> ClientDet = new List<ClientDetails>();
            ClientDetails result = new ClientDetails();
                string[] Params = { BankCode, UserID, ClientID };
                DataTable dt = dh.ExecuteDataSet("GetClientDetails", Params).Tables[0];
                //List<DataRow> ClientDet = new List<DataRow>(dt.Select());

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                    ClientDetails res = new ClientDetails();
                        res.CompanyCode = dr["CompanyCode"].ToString();
                        res.ClientNo = dr["ClientID"].ToString();
                        res.ClientName = dr["ClientName"].ToString();
                        res.ClientPhoneNumber = dr["PhoneNo"].ToString();
                        res.Gender = dr["Gender"].ToString();
                        res.IDType = dr["IDType"].ToString();
                        res.IDNumber = dr["IDNumber"].ToString();
                        //res.DateOfBirth = dr["DateOfBirth"].ToString();

                        if (!string.IsNullOrEmpty(ClientID))
                        {
                        }

                        res.ModifiedBy = dr["CapturedBy"].ToString();
                        res.ModifiedOn = dr["CapturedOn"].ToString();
                        res.StatusCode = Globals.SUCCESS_STATUS_CODE;
                        res.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
                        ClientDet.Add(res);
                        ClientDetails = ClientDet.ToArray();
                    }

                    //ClientDet.Add(res);
                    //ClientDetails = ClientDet.ToArray();
                    return ClientDetails;
                }

                result.StatusCode = "100";
                result.StatusDesc = "NO RECORDS FOUND";
                ClientDet.Add(result);
                ClientDetails = ClientDet.ToArray();
                return ClientDetails;
                //res.StatusCode = Globals.SUCCESS_STATUS_CODE;
                //res.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
            }


            public Result ResetUserCreds(SystemUser req)
            {
                Result result = new Result();

                //if (!req.IsValid())
                //{
                //    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                //    result.StatusDesc = req.StatusDesc;
                //    return result;
                //}

                int dt = dh.ExecuteNonQuery("ResetUserCreds", new string[] { req.CompanyCode, req.UserId, req.Password, req.ModifiedBy });
                if (dt <= 0)
                {
                    result.StatusCode = Globals.FAILURE_STATUS_CODE;
                    result.StatusDesc = "FAILED: USER CREDS NOT RESET";
                    return result;
                }

                result.StatusCode = Globals.SUCCESS_STATUS_CODE;
                result.StatusDesc = Globals.SUCCESS_STATUS_TEXT;
                //result.PegPayID = dt.Rows[0][0].ToString();
                return result;
            }

        }
    }
