namespace CSI.OpenBase.Desktop;

internal sealed class SettingsDialog : Form
{
    private readonly TextBox _workspacePath;

    public SettingsDialog(string currentDirectory)
    {
        Text = "设置";
        BackColor = DesktopTheme.Paper;
        ForeColor = DesktopTheme.Ink;
        Font = new Font("Segoe UI", 9F);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(590, 201);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;

        var layout = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 4,
            Dock = DockStyle.Fill,
            Padding = new Padding(24, 20, 24, 20),
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var heading = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 13F),
            Margin = new Padding(0, 0, 0, 6),
            Text = "工作目录",
        };
        layout.Controls.Add(heading, 0, 0);
        layout.SetColumnSpan(heading, 2);

        var description = new Label
        {
            AutoSize = true,
            ForeColor = DesktopTheme.Muted,
            Margin = new Padding(0, 0, 0, 13),
            Text = "表格、视频档案和评论数据存放在工作目录。更换目录不会移动原有数据。",
        };
        layout.Controls.Add(description, 0, 1);
        layout.SetColumnSpan(description, 2);

        _workspacePath = new TextBox
        {
            AccessibleName = "工作目录路径",
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            BackColor = DesktopTheme.Surface,
            ForeColor = DesktopTheme.Ink,
            Margin = new Padding(0, 0, 8, 0),
            ReadOnly = true,
            Text = currentDirectory,
        };
        layout.Controls.Add(_workspacePath, 0, 2);

        var changeButton = CreateButton("更改…");
        changeButton.AccessibleName = "更改工作目录";
        layout.Controls.Add(changeButton, 1, 2);

        var saveButton = CreateButton("保存", DialogResult.OK);
        saveButton.BackColor = DesktopTheme.Yellow;
        saveButton.Enabled = false;
        var cancelButton = CreateButton("取消", DialogResult.Cancel);
        cancelButton.Margin = new Padding(0, 0, 8, 0);

        var actions = new FlowLayoutPanel
        {
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            Margin = new Padding(0),
            WrapContents = false,
        };
        actions.Controls.Add(saveButton);
        actions.Controls.Add(cancelButton);
        layout.Controls.Add(actions, 0, 3);
        layout.SetColumnSpan(actions, 2);

        changeButton.Click += (_, _) =>
        {
            if (ChooseDirectory())
            {
                saveButton.Enabled = !string.Equals(
                    WorkspaceDirectory, currentDirectory, StringComparison.OrdinalIgnoreCase);
            }
        };
        AcceptButton = saveButton;
        CancelButton = cancelButton;
        Controls.Add(layout);
        ActiveControl = changeButton;
    }

    public string WorkspaceDirectory => _workspacePath.Text;

    private bool ChooseDirectory()
    {
        var initialDirectory = Directory.Exists(WorkspaceDirectory)
            ? WorkspaceDirectory
            : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        using var picker = new FolderBrowserDialog
        {
            Description = "选择 CSI OpenBase 工作目录",
            InitialDirectory = initialDirectory,
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true,
        };

        if (picker.ShowDialog(this) != DialogResult.OK)
        {
            return false;
        }

        _workspacePath.Text = picker.SelectedPath;
        _workspacePath.SelectionStart = 0;
        return true;
    }

    private static Button CreateButton(string text, DialogResult result = DialogResult.None)
    {
        var button = new Button
        {
            BackColor = DesktopTheme.Surface,
            DialogResult = result,
            FlatStyle = FlatStyle.Flat,
            ForeColor = DesktopTheme.Ink,
            Margin = new Padding(0),
            Size = new Size(88, 32),
            Text = text,
            UseVisualStyleBackColor = false,
        };
        button.FlatAppearance.BorderColor = DesktopTheme.Ink;
        return button;
    }
}
