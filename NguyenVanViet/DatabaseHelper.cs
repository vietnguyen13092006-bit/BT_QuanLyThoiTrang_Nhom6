using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace FORM_DKY
{
    public class DatabaseHelper
    {
        // Chuỗi kết nối SQL Server (dùng .\SQLEXPRESS chạy chuẩn trên mọi máy)
        private static string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QuanLyBanHangDB;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Lấy dữ liệu dạng DataTable (dùng cho SELECT)
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        // Thực thi các lệnh Thêm, Sửa, Xóa (dùng Transaction để đảm bảo toàn vẹn dữ liệu)
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            SqlConnection conn = transaction != null ? transaction.Connection : GetConnection();
            bool needClose = false;

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                needClose = true;
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (needClose)
                    conn.Close();
            }
        }
    }
}