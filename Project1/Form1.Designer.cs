namespace Project1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            imageBoxOriginal = new PictureBox();
            btnLoadImage = new Button();
            imageBoxFiltered = new PictureBox();
            btnSaveImage = new Button();
            btnResetImage = new Button();
            label1 = new Label();
            label2 = new Label();
            btnApplyFilter = new Button();
            checkedListFuncFilters = new CheckedListBox();
            checkedListConvFilters = new CheckedListBox();
            btnModifyFunctionalFilter = new Button();
            checkedListNewFuncFilters = new CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)imageBoxOriginal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imageBoxFiltered).BeginInit();
            SuspendLayout();
            // 
            // imageBoxOriginal
            // 
            imageBoxOriginal.Location = new Point(57, 144);
            imageBoxOriginal.Name = "imageBoxOriginal";
            imageBoxOriginal.Size = new Size(405, 368);
            imageBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            imageBoxOriginal.TabIndex = 0;
            imageBoxOriginal.TabStop = false;
            // 
            // btnLoadImage
            // 
            btnLoadImage.Location = new Point(75, 528);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(140, 29);
            btnLoadImage.TabIndex = 1;
            btnLoadImage.Text = "Load Image";
            btnLoadImage.UseVisualStyleBackColor = true;
            btnLoadImage.Click += btnLoadImage_Click;
            // 
            // imageBoxFiltered
            // 
            imageBoxFiltered.Location = new Point(516, 144);
            imageBoxFiltered.Name = "imageBoxFiltered";
            imageBoxFiltered.Size = new Size(405, 368);
            imageBoxFiltered.SizeMode = PictureBoxSizeMode.Zoom;
            imageBoxFiltered.TabIndex = 2;
            imageBoxFiltered.TabStop = false;
            // 
            // btnSaveImage
            // 
            btnSaveImage.Location = new Point(604, 528);
            btnSaveImage.Name = "btnSaveImage";
            btnSaveImage.Size = new Size(94, 29);
            btnSaveImage.TabIndex = 3;
            btnSaveImage.Text = "Save";
            btnSaveImage.UseVisualStyleBackColor = true;
            btnSaveImage.Click += btnSaveImage_Click;
            // 
            // btnResetImage
            // 
            btnResetImage.Location = new Point(750, 528);
            btnResetImage.Name = "btnResetImage";
            btnResetImage.Size = new Size(94, 29);
            btnResetImage.TabIndex = 4;
            btnResetImage.Text = "Reset";
            btnResetImage.UseVisualStyleBackColor = true;
            btnResetImage.Click += btnResetImage_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(75, 22);
            label1.Name = "label1";
            label1.Size = new Size(140, 20);
            label1.TabIndex = 7;
            label1.Text = "select function filter";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(285, 22);
            label2.Name = "label2";
            label2.Size = new Size(164, 20);
            label2.TabIndex = 8;
            label2.Text = "select convolution filter";
            // 
            // btnApplyFilter
            // 
            btnApplyFilter.Location = new Point(272, 528);
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Size = new Size(140, 29);
            btnApplyFilter.TabIndex = 9;
            btnApplyFilter.Text = "Apply Filter";
            btnApplyFilter.UseVisualStyleBackColor = true;
            btnApplyFilter.Click += btnApplyFilter_Click;
            // 
            // checkedListFuncFilters
            // 
            checkedListFuncFilters.CheckOnClick = true;
            checkedListFuncFilters.FormattingEnabled = true;
            checkedListFuncFilters.Items.AddRange(new object[] { "Inversion", "Brightness Correction", "Contrast Enhancement", "Gamma Correction" });
            checkedListFuncFilters.Location = new Point(57, 55);
            checkedListFuncFilters.Name = "checkedListFuncFilters";
            checkedListFuncFilters.Size = new Size(190, 70);
            checkedListFuncFilters.TabIndex = 10;
            // 
            // checkedListConvFilters
            // 
            checkedListConvFilters.CheckOnClick = true;
            checkedListConvFilters.FormattingEnabled = true;
            checkedListConvFilters.Items.AddRange(new object[] { "Blur", "Gaussian Blur ", "Sharpen", "Edge Detection ", "Emboss" });
            checkedListConvFilters.Location = new Point(272, 55);
            checkedListConvFilters.Name = "checkedListConvFilters";
            checkedListConvFilters.Size = new Size(190, 70);
            checkedListConvFilters.TabIndex = 11;
            // 
            // btnModifyFunctionalFilter
            // 
            btnModifyFunctionalFilter.Location = new Point(528, 65);
            btnModifyFunctionalFilter.Name = "btnModifyFunctionalFilter";
            btnModifyFunctionalFilter.Size = new Size(179, 50);
            btnModifyFunctionalFilter.TabIndex = 12;
            btnModifyFunctionalFilter.Text = "Modify Functional Filter";
            btnModifyFunctionalFilter.UseVisualStyleBackColor = true;
            btnModifyFunctionalFilter.Click += btnModifyFunctionalFilter_Click;
            // 
            // checkedListNewFuncFilters
            // 
            checkedListNewFuncFilters.CheckOnClick = true;
            checkedListNewFuncFilters.FormattingEnabled = true;
            checkedListNewFuncFilters.Location = new Point(731, 55);
            checkedListNewFuncFilters.Name = "checkedListNewFuncFilters";
            checkedListNewFuncFilters.Size = new Size(190, 70);
            checkedListNewFuncFilters.TabIndex = 13;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(976, 569);
            Controls.Add(checkedListNewFuncFilters);
            Controls.Add(btnModifyFunctionalFilter);
            Controls.Add(checkedListConvFilters);
            Controls.Add(checkedListFuncFilters);
            Controls.Add(btnApplyFilter);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnResetImage);
            Controls.Add(btnSaveImage);
            Controls.Add(imageBoxFiltered);
            Controls.Add(btnLoadImage);
            Controls.Add(imageBoxOriginal);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)imageBoxOriginal).EndInit();
            ((System.ComponentModel.ISupportInitialize)imageBoxFiltered).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox imageBoxOriginal;
        private Button btnLoadImage;
        private PictureBox imageBoxFiltered;
        private Button btnSaveImage;
        private Button btnResetImage;
        private Label label1;
        private Label label2;
        private Button btnApplyFilter;
        private CheckedListBox checkedListFuncFilters;
        private CheckedListBox checkedListConvFilters;
        private Button btnModifyFunctionalFilter;
        private CheckedListBox checkedListNewFuncFilters;
    }
}
