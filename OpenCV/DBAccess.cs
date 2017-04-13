using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV
{
    public class DBAccess : IDBAccess
    {
        //public string SaveFace(string username, byte[] faceBlob)
        //{
        //    //var connection = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
        //    using (SqlConnection conn = new SqlConnection())
        //    {
        //        conn.ConnectionString = "Server=[server_name];Database=[database_name];Trusted_Connection=true";
        //        // using the code here...
        //    }
        //}

        //public List<Face> CallFaces(string username)
        //{
        //    throw new NotImplementedException();
        //}

        public bool IsUsernameValid(string username)
        {
            throw new NotImplementedException();
        }

        public string SaveAdmin(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public int GetUserId(string username)
        {
            throw new NotImplementedException();
        }

        public int GenerateUserId()
        {
            throw new NotImplementedException();
        }

        public string GetUsername(int userId)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllUsernames()
        {
            throw new NotImplementedException();
        }
    }
}
