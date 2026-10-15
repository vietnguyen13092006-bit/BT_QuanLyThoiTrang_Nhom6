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
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvLichSu).BeginInit();
            SuspendLayout();
            // 
            // dgvLichSu
            // 
            dgvLichSu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLichSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLichSu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvLichSu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLichSu.Location = new Point(12, 84);
            dgvLichSu.Name = "dgvLichSu";
            dgvLichSu.Size = new Size(845, 384);
            dgvLichSu.TabIndex = 0;
            // 
            // btnSua
            // 
            btnSua.Anchor = AnchorStyles.Bottom;
            btnSua.Location = new Point(393, 474);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(102, 54);
            btnSua.TabIndex = 2;
            btnSua.Text = "Sửa phiếu";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += BtnSua_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 40F);
            label1.Location = new Point(178, 9);
            label1.Name = "label1";
            label1.Size = new Size(500, 72);
            label1.TabIndex = 3;
            label1.Text = "Quản lý phiếu nhập";
            // 
            // FrmLichSuPhieuNhap
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(869, 540);
            Controls.Add(label1);
            Controls.Add(btnSua);
            Controls.Add(dgvLichSu);
            Name = "FrmLichSuPhieuNhap";
            Text = "FrmLichSuPhieuNhap";
            ((System.ComponentModel.ISupportInitialize)dgvLichSu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvLichSu;
        private Button btnSua;
        private Label label1;
    }
}