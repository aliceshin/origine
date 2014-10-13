namespace BISTel.eSPC.Page.Modeling
{
    partial class MailingGroupConfigurationUC
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailingGroupConfigurationUC));
            this.gbSMSList = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.clbSMSLeft = new System.Windows.Forms.CheckedListBox();
            this.clbSMDRight = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.bbtnRight = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bbtnLeft = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.gbMailingList = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.clbMailingLeft = new System.Windows.Forms.CheckedListBox();
            this.clbMailingRight = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.bButton1 = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bButton2 = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gbSMSList.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.gbMailingList.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSMSList
            // 
            this.gbSMSList.Controls.Add(this.tableLayoutPanel2);
            this.gbSMSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSMSList.Location = new System.Drawing.Point(0, 267);
            this.gbSMSList.Name = "gbSMSList";
            this.gbSMSList.Size = new System.Drawing.Size(885, 190);
            this.gbSMSList.TabIndex = 3;
            this.gbSMSList.TabStop = false;
            this.gbSMSList.Text = "SMS List";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.clbSMSLeft, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.clbSMDRight, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(879, 170);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // clbSMSLeft
            // 
            this.clbSMSLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbSMSLeft.FormattingEnabled = true;
            this.clbSMSLeft.Location = new System.Drawing.Point(12, 9);
            this.clbSMSLeft.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.clbSMSLeft.Name = "clbSMSLeft";
            this.clbSMSLeft.Size = new System.Drawing.Size(386, 152);
            this.clbSMSLeft.TabIndex = 0;
            // 
            // clbSMDRight
            // 
            this.clbSMDRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbSMDRight.FormattingEnabled = true;
            this.clbSMDRight.Location = new System.Drawing.Point(480, 9);
            this.clbSMDRight.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.clbSMDRight.Name = "clbSMDRight";
            this.clbSMDRight.Size = new System.Drawing.Size(387, 152);
            this.clbSMDRight.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.bbtnRight, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.bbtnLeft, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(413, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(52, 164);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // bbtnRight
            // 
            this.bbtnRight.AutoButtonSize = false;
            this.bbtnRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnRight.BackgroundImage")));
            this.bbtnRight.BssClass = "ConditionButton";
            this.bbtnRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnRight.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnRight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnRight.IsMultiLanguage = true;
            this.bbtnRight.Key = "";
            this.bbtnRight.Location = new System.Drawing.Point(3, 62);
            this.bbtnRight.Name = "bbtnRight";
            this.bbtnRight.Size = new System.Drawing.Size(44, 17);
            this.bbtnRight.TabIndex = 0;
            this.bbtnRight.Text = ">>";
            this.bbtnRight.ToolTipText = "";
            this.bbtnRight.UseVisualStyleBackColor = true;
            // 
            // bbtnLeft
            // 
            this.bbtnLeft.AutoButtonSize = false;
            this.bbtnLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnLeft.BackgroundImage")));
            this.bbtnLeft.BssClass = "ConditionButton";
            this.bbtnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnLeft.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnLeft.IsMultiLanguage = true;
            this.bbtnLeft.Key = "";
            this.bbtnLeft.Location = new System.Drawing.Point(3, 85);
            this.bbtnLeft.Name = "bbtnLeft";
            this.bbtnLeft.Size = new System.Drawing.Size(44, 17);
            this.bbtnLeft.TabIndex = 1;
            this.bbtnLeft.Text = "<<";
            this.bbtnLeft.ToolTipText = "";
            this.bbtnLeft.UseVisualStyleBackColor = true;
            // 
            // gbMailingList
            // 
            this.gbMailingList.Controls.Add(this.tableLayoutPanel1);
            this.gbMailingList.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbMailingList.Location = new System.Drawing.Point(0, 0);
            this.gbMailingList.Name = "gbMailingList";
            this.gbMailingList.Size = new System.Drawing.Size(885, 264);
            this.gbMailingList.TabIndex = 2;
            this.gbMailingList.TabStop = false;
            this.gbMailingList.Text = "Mailing List";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.clbMailingLeft, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.clbMailingRight, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(879, 244);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // clbMailingLeft
            // 
            this.clbMailingLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbMailingLeft.FormattingEnabled = true;
            this.clbMailingLeft.Location = new System.Drawing.Point(12, 9);
            this.clbMailingLeft.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.clbMailingLeft.Name = "clbMailingLeft";
            this.clbMailingLeft.Size = new System.Drawing.Size(386, 226);
            this.clbMailingLeft.TabIndex = 0;
            // 
            // clbMailingRight
            // 
            this.clbMailingRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbMailingRight.FormattingEnabled = true;
            this.clbMailingRight.Location = new System.Drawing.Point(480, 9);
            this.clbMailingRight.Margin = new System.Windows.Forms.Padding(12, 9, 12, 9);
            this.clbMailingRight.Name = "clbMailingRight";
            this.clbMailingRight.Size = new System.Drawing.Size(387, 226);
            this.clbMailingRight.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.bButton1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.bButton2, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(413, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(52, 238);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // bButton1
            // 
            this.bButton1.AutoButtonSize = false;
            this.bButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bButton1.BackgroundImage")));
            this.bButton1.BssClass = "ConditionButton";
            this.bButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bButton1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bButton1.IsMultiLanguage = true;
            this.bButton1.Key = "";
            this.bButton1.Location = new System.Drawing.Point(3, 99);
            this.bButton1.Name = "bButton1";
            this.bButton1.Size = new System.Drawing.Size(44, 17);
            this.bButton1.TabIndex = 0;
            this.bButton1.Text = ">>";
            this.bButton1.ToolTipText = "";
            this.bButton1.UseVisualStyleBackColor = true;
            // 
            // bButton2
            // 
            this.bButton2.AutoButtonSize = false;
            this.bButton2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bButton2.BackgroundImage")));
            this.bButton2.BssClass = "ConditionButton";
            this.bButton2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bButton2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bButton2.IsMultiLanguage = true;
            this.bButton2.Key = "";
            this.bButton2.Location = new System.Drawing.Point(3, 122);
            this.bButton2.Name = "bButton2";
            this.bButton2.Size = new System.Drawing.Size(44, 17);
            this.bButton2.TabIndex = 1;
            this.bButton2.Text = "<<";
            this.bButton2.ToolTipText = "";
            this.bButton2.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 264);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(885, 3);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // MailingGroupConfigurationUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gbSMSList);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.gbMailingList);
            this.Name = "MailingGroupConfigurationUC";
            this.Size = new System.Drawing.Size(885, 457);
            this.gbSMSList.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.gbMailingList.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.PropertyGrid propGrid;        
        //private BISTel.eSPC.Page.Common.BTChartUC unitedChartUC;
        private System.Windows.Forms.GroupBox gbMailingList;
        private System.Windows.Forms.GroupBox gbSMSList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckedListBox clbSMSLeft;
        private System.Windows.Forms.CheckedListBox clbSMDRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnRight;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckedListBox clbMailingLeft;
        private System.Windows.Forms.CheckedListBox clbMailingRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bButton1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bButton2;
        private System.Windows.Forms.Splitter splitter1;
    }
}
