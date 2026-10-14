namespace FORM_DKY
{
    partial class FrmLichSuPhieuNhap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvLichSu = new DataGridView();
            btnSua = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLichSu).BeginInit();
            SuspendLayout();
            // 
            // dgvLichSu
            // 
            dgvLichSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLichSu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvLichSu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLichSu.Dock = DockStyle.Fill;
            dgvLichSu.Location = new Point(0, 0);
            dgvLichSu.Name = "dgvLichSu";
            dgvLichSu.Size = new Size(850, 501);
            dgvLichSu.TabIndex = 0;
            // 
            // btnSua
            // 
            btnSua.Dock = DockStyle.Bottom;
            btnSua.Location = new Point(0, 458);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(850, 43);
            btnSua.TabIndex = 2;
            btnSua.Text = "Sửa phiếu";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += BtnSua_Click;
            // 
            // FrmLichSuPhieuNhap
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(850, 501);
            Controls.Add(btnSua);
            Controls.Add(dgvLichSu);
            Name = "FrmLichSuPhieuNhap";
            Text = "FrmLichSuPhieuNhap";
            ((System.ComponentModel.ISupportInitialize)dgvLichSu).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvLichSu;
        private Button btnSua;
    }
}