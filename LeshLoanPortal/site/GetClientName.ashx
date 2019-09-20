<%@ WebHandler Language="C#" Class="GetClientName" %>

using System.Web;
using System.Collections.Generic;
using InterConnect.LeshLaonApi;
using System.Web.Script.Serialization;

public class GetClientName : IHttpHandler {
    LeshLoanAPI api = new LeshLoanAPI();
    BusinessLogic bll = new BusinessLogic();

    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        
        string Name = context.Request["term"];
        string CompanyCode = "";
        List<string> ClientNames = bll.LoadClientsforSearch(CompanyCode,Name);

        JavaScriptSerializer js = new JavaScriptSerializer();
        context.Response.Write(js.Serialize(ClientNames));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}