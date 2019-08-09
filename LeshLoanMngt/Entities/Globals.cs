using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeshLoanMngt.Entities
{

        public static class Globals
        {

            public const string SUCCESS_STATUS_CODE = "0";
            public const string FAILURE_STATUS_CODE = "100";
            public const string HIDE_FAILURE_STATUS_CODE = "101";
            public const string PENDING_STATUS_CODE = "1000";
            public const string DUPLICATE_ENTRY_CODE = "102";
            public const string SUCCESS_STATUS_TEXT = "SUCCESS";

            
            public static string RETURN_URL = "https://localhost:8019/TestPegasusPaymentsGateway/TesterPage.aspx";
            public static string CURRENCY_CODE = "UGX";
            public static int MINIMUM_TRANSACTION_AMOUNT = 500;
        }
    }