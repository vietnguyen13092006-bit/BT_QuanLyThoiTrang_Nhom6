using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHang
{
    public static class Database
    {
        // Chuỗi kết nối chuẩn trỏ trực tiếp tới ShopThoiTrang2026DB
        private static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ShopThoiTrang2026DB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static DataTable GetData(string query, SqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null) cmd.Parameters.AddRange(parameters);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return dt;
        }

        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null) cmd.Parameters.AddRange(parameters);
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi thực thi dữ liệu: " + ex.Message, "Lỗi", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return rowsAffected;
        }
    }
}