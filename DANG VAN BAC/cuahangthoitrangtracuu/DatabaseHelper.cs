using System;
using System.Data;
using System.Data.SqlClient;

namespace cuahangthoitrang
{
    public static class DatabaseHelper
    {
        // Chuỗi kết nối CSDL (Đã khớp chuẩn SQLEXPRESS và Database QuanLyCuaHangThoiTrang)
        private static readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyCuaHangThoiTrang;Integrated Security=True;TrustServerCertificate=True;";

        // Tham số parameters = null hỗ trợ Nullable Reference Types
        public static DataTable GetData(string query, SqlParameter[]? parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}