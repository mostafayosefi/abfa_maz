using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Services;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private static readonly HttpClient client = new HttpClient();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }



        public class view_json
        {

            public string name { get; set; }
            public string family { get; set; }
            public string fatherName { get; set; }
            public string codeMeli { get; set; }
            public int gender { get; set; }
 
        }



        [WebMethod]
        public DataSet estelam(string NationalCode, string Dateofbirth )
        {


            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("code");
            dt.Columns.Add("name");
            dt.Columns.Add("family");
            dt.Columns.Add("fatherName");
            dt.Columns.Add("gender"); 

            try
            {





            EstelamValue values = new EstelamValue
            {
                NationalCode = NationalCode,
                Dateofbirth = Dateofbirth
            };
            var json = JsonConvert.SerializeObject(values);

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "http://195.181.42.6:1415/Core/VerifyNationalCode/?Token=WEBHJ-D41YN-JCV23-QVX8-R6BK";
            var response = client.PostAsync(url, data);

            if (response.Result.StatusCode.ToString() == "OK")

            {

                     


                    var result = (response.Result.Content.ReadAsStringAsync().Result);


                view_json[] dObject = JsonConvert.DeserializeObject<view_json[]>(result);


                foreach (var item in dObject)
                {
                    DataRow dr = dt.NewRow();
                    dr["name"] = item.name;
                    dr["family"] = item.family;
                    dr["fatherName"] = item.fatherName;
                    dr["gender"] = item.gender; 
                    dt.Rows.Add(dr);

                    dr["code"] = result.ToString();
                    dt.Rows.Add(dr);
                }



                ds.Tables.Add(dt);

            }


            }
            catch (Exception ex)
            {
                DataRow dr = dt.NewRow();
                dr["code"] = ex.Message;
                dt.Rows.Add(dr);
                ds.Tables.Add(dt);
            }

            return ds;
        }





    }
}
