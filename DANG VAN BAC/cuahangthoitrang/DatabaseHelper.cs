using System;
using System.Data;
using Microsoft.Data.SqlClient; // hoặc System.Data.SqlClient

namespace QuanLyCuaHangThoiTrang
{
    public static class DatabaseHelper
    {
        // Chuỗi kết nối CSDL của bạn
        private static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True";

        public static DataTable GetData(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}