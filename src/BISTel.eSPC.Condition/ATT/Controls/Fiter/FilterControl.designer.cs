namespace BISTel.eSPC.Condition.ATT.Controls.Filter
{
    partial class FilterControl
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
            this.pnlBody = new System.Windows.Forms.Panel();
            this.bcpnlFilter = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.btxt_Filter = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.lblFilter = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel3 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.pnlBody.SuspendLayout();
            this.bcpnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.bcpnlFilter);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 0);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(341, 30);
            this.pnlBody.TabIndex = 2;
            // 
            // bcpnlFilter
            // 
            this.bcpnlFilter.BottomImage = null;
            this.bcpnlFilter.BottomLeftImage = null;
            this.bcpnlFilter.BssClass = "";
            this.bcpnlFilter.Controls.Add(this.btxt_Filter);
            this.bcpnlFilter.Controls.Add(this.lblFilter);
            this.bcpnlFilter.Controls.Add(this.bLabel3);
            this.bcpnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlFilter.DownImage = null;
            this.bcpnlFilter.Key = "";
            this.bcpnlFilter.LeftImage = null;
            this.bcpnlFilter.LeftTopImage = null;
            this.bcpnlFilter.Location = new System.Drawing.Point(0, 0);
            this.bcpnlFilter.Name = "bcpnlFilter";
            this.bcpnlFilter.Padding = new System.Windows.Forms.Padding(10, 4, 15, 4);
            this.bcpnlFilter.RightBottomImage = null;
            this.bcpnlFilter.RightImage = null;
            this.bcpnlFilter.Size = new System.Drawing.Size(341, 29);
            this.bcpnlFilter.TabIndex = 3;
            this.bcpnlFilter.TopImage = null;
            this.bcpnlFilter.TopRightImage = null;
            this.bcpnlFilter.UpImage = null;
            // 
            // btxt_Filter
            // 
            this.btxt_Filter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.btxt_Filter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.btxt_Filter.BssClass = "";
            this.btxt_Filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxt_Filter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxt_Filter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxt_Filter.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxt_Filter.IsMultiLanguage = true;
            this.btxt_Filter.Key = "";
            this.btxt_Filter.Location = new System.Drawing.Point(89, 4);
            this.btxt_Filter.MaxLength = 100;
            this.btxt_Filter.Name = "btxt_Filter";
            this.btxt_Filter.Size = new System.Drawing.Size(237, 21);
            this.btxt_Filter.TabIndex = 4;
            this.btxt_Filter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btxt_Filter_KeyDown);
            // 
            // lblFilter
            // 
            this.lblFilter.BackColor = System.Drawing.Color.White;
            this.lblFilter.BssClass = "";
            this.lblFilter.CustomTextAlign = "";
            this.lblFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblFilter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFilter.ForeColor = System.Drawing.Color.Black;
            this.lblFilter.IsMultiLanguage = false;
            this.lblFilter.Key = "";
            this.lblFilter.Location = new System.Drawing.Point(20, 4);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(69, 21);
            this.lblFilter.TabIndex = 6;
            this.lblFilter.Text = "Filter";
            this.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bLabel3
            // 
            this.bLabel3.BackColor = System.Drawing.Color.White;
            this.bLabel3.BssClass = "";
            this.bLabel3.CustomTextAlign = "";
            this.bLabel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel3.ForeColor = System.Drawing.Color.Black;
            this.bLabel3.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel3.IsMultiLanguage = true;
            this.bLabel3.Key = "";
            this.bLabel3.Location = new System.Drawing.Point(10, 4);
            this.bLabel3.Name = "bLabel3";
            this.bLabel3.Size = new System.Drawing.Size(10, 21);
            this.bLabel3.TabIndex = 11;
            // 
            // FilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBody);
            this.Name = "FilterControl";
            this.Size = new System.Drawing.Size(341, 30);
            this.pnlBody.ResumeLayout(false);
            this.bcpnlFilter.ResumeLayout(false);
            this.bcpnlFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlFilter;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxt_Filter;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblFilter;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel3;

    }
}
