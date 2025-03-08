namespace Project1
{
    partial class Filters
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
            btnLoadFilter = new Button();
            pictureBoxFuncGraph = new PictureBox();
            btnAddPoint = new Button();
            btnDeletePoint = new Button();
            btnSaveFilter = new Button();
            ((System.ComponentModel.ISupportInitialize)imageBoxOriginal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imageBoxFiltered).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFuncGraph).BeginInit();
            SuspendLayout();
            // 
            // imageBoxOriginal
            // 
            imageBoxOriginal.Location = new Point(58, 297);
            imageBoxOriginal.Name = "imageBoxOriginal";
            imageBoxOriginal.Size = new Size(590, 590);
            imageBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            imageBoxOriginal.TabIndex = 0;
            imageBoxOriginal.TabStop = false;
            // 
            // btnLoadImage
            // 
            btnLoadImage.Location = new Point(285, 915);
            btnLoadImage.Name = "btnLoadImage";
            btnLoadImage.Size = new Size(117, 42);
            btnLoadImage.TabIndex = 1;
            btnLoadImage.Text = "Load Image";
            btnLoadImage.UseVisualStyleBackColor = true;
            btnLoadImage.Click += btnLoadImage_Click;
            // 
            // imageBoxFiltered
            // 
            imageBoxFiltered.Location = new Point(731, 297);
            imageBoxFiltered.Name = "imageBoxFiltered";
            imageBoxFiltered.Size = new Size(590, 590);
            imageBoxFiltered.SizeMode = PictureBoxSizeMode.Zoom;
            imageBoxFiltered.TabIndex = 2;
            imageBoxFiltered.TabStop = false;
            // 
            // btnSaveImage
            // 
            btnSaveImage.Location = new Point(892, 915);
            btnSaveImage.Name = "btnSaveImage";
            btnSaveImage.Size = new Size(117, 42);
            btnSaveImage.TabIndex = 3;
            btnSaveImage.Text = "Save";
            btnSaveImage.UseVisualStyleBackColor = true;
            btnSaveImage.Click += btnSaveImage_Click;
            // 
            // btnResetImage
            // 
            btnResetImage.Location = new Point(1054, 915);
            btnResetImage.Name = "btnResetImage";
            btnResetImage.Size = new Size(117, 42);
            btnResetImage.TabIndex = 4;
            btnResetImage.Text = "Reset";
            btnResetImage.UseVisualStyleBackColor = true;
            btnResetImage.Click += btnResetImage_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(127, 55);
            label1.Name = "label1";
            label1.Size = new Size(146, 20);
            label1.TabIndex = 7;
            label1.Text = "Select Function Filter";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(408, 55);
            label2.Name = "label2";
            label2.Size = new Size(170, 20);
            label2.TabIndex = 8;
            label2.Text = "Select Convolution Filter";
            // 
            // btnApplyFilter
            // 
            btnApplyFilter.Location = new Point(363, 220);
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Size = new Size(117, 42);
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
            checkedListFuncFilters.Location = new Point(93, 94);
            checkedListFuncFilters.Name = "checkedListFuncFilters";
            checkedListFuncFilters.Size = new Size(209, 92);
            checkedListFuncFilters.TabIndex = 10;
            // 
            // checkedListConvFilters
            // 
            checkedListConvFilters.CheckOnClick = true;
            checkedListConvFilters.FormattingEnabled = true;
            checkedListConvFilters.Items.AddRange(new object[] { "Blur", "Gaussian Blur ", "Sharpen", "Edge Detection ", "Emboss" });
            checkedListConvFilters.Location = new Point(389, 94);
            checkedListConvFilters.Name = "checkedListConvFilters";
            checkedListConvFilters.Size = new Size(209, 92);
            checkedListConvFilters.TabIndex = 11;
            // 
            // btnLoadFilter
            // 
            btnLoadFilter.Location = new Point(208, 220);
            btnLoadFilter.Name = "btnLoadFilter";
            btnLoadFilter.Size = new Size(117, 42);
            btnLoadFilter.TabIndex = 14;
            btnLoadFilter.Text = "Load Filter";
            btnLoadFilter.UseVisualStyleBackColor = true;
            btnLoadFilter.Click += btnLoadFilter_Click;
            // 
            // pictureBoxFuncGraph
            // 
            pictureBoxFuncGraph.Location = new Point(1425, 466);
            pictureBoxFuncGraph.Name = "pictureBoxFuncGraph";
            pictureBoxFuncGraph.Size = new Size(256, 256);
            pictureBoxFuncGraph.TabIndex = 15;
            pictureBoxFuncGraph.TabStop = false;
            pictureBoxFuncGraph.Paint += pictureBoxFuncGraph_Paint;
            pictureBoxFuncGraph.MouseDown += pictureBoxFuncGraph_MouseDown;
            pictureBoxFuncGraph.MouseMove += pictureBoxFuncGraph_MouseMove;
            pictureBoxFuncGraph.MouseUp += pictureBoxFuncGraph_MouseUp;
            // 
            // btnAddPoint
            // 
            btnAddPoint.Location = new Point(1714, 498);
            btnAddPoint.Name = "btnAddPoint";
            btnAddPoint.Size = new Size(117, 42);
            btnAddPoint.TabIndex = 16;
            btnAddPoint.Text = "Add Point";
            btnAddPoint.UseVisualStyleBackColor = true;
            btnAddPoint.Click += btnAddPoint_Click;
            // 
            // btnDeletePoint
            // 
            btnDeletePoint.Location = new Point(1714, 569);
            btnDeletePoint.Name = "btnDeletePoint";
            btnDeletePoint.Size = new Size(117, 42);
            btnDeletePoint.TabIndex = 17;
            btnDeletePoint.Text = "Delete Point";
            btnDeletePoint.UseVisualStyleBackColor = true;
            btnDeletePoint.Click += btnDeletePoint_Click;
            // 
            // btnSaveFilter
            // 
            btnSaveFilter.Location = new Point(1714, 640);
            btnSaveFilter.Name = "btnSaveFilter";
            btnSaveFilter.Size = new Size(117, 42);
            btnSaveFilter.TabIndex = 18;
            btnSaveFilter.Text = "Save Filter";
            btnSaveFilter.UseVisualStyleBackColor = true;
            btnSaveFilter.Click += btnSaveFilter_Click;
            // 
            // Filters
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(btnSaveFilter);
            Controls.Add(btnDeletePoint);
            Controls.Add(btnAddPoint);
            Controls.Add(pictureBoxFuncGraph);
            Controls.Add(btnLoadFilter);
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
            Name = "Filters";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Filters";
            ((System.ComponentModel.ISupportInitialize)imageBoxOriginal).EndInit();
            ((System.ComponentModel.ISupportInitialize)imageBoxFiltered).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFuncGraph).EndInit();
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
        private Button btnLoadFilter;
        private PictureBox pictureBoxFuncGraph;
        private Button btnAddPoint;
        private Button btnDeletePoint;
        private Button btnSaveFilter;
    }
}
