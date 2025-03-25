using System.ComponentModel;
using System.Text;

public partial class ContactManagerForm : Form, IContactView
{
    // Data binding
    private readonly BindingSource bindingSource = new BindingSource();
    private readonly BindingList<Contact> filteredContacts = new BindingList<Contact>();

    // Required controls
    public ListBox contactListView = new ListBox();
    public TextBox txtFirstName = new TextBox();
    public TextBox txtLastName = new TextBox();
    public TextBox txtEmail = new TextBox();
    public TextBox txtPhone = new TextBox();
    public TextBox txtNotes = new TextBox();
    public ComboBox cmbCategory = new ComboBox();
    public TextBox txtSearch = new TextBox();
    public ComboBox cmbCategoryFilter = new ComboBox();
    public StatusStrip statusStrip = new StatusStrip();
    public ToolStripStatusLabel lblStatus = new ToolStripStatusLabel();

    // Original contacts list (unfiltered)
    private BindingList<Contact> allContacts;

    // IContactView events
    public event EventHandler AddContact;
    public event EventHandler UpdateContact;
    public event EventHandler DeleteContact;
    public event EventHandler ExportContacts;

    // Category options
    private readonly string[] categories = { "Family", "Friend", "Work", "Other" };

    public ContactManagerForm()
    {
        InitializeComponent();
        SetupLayout();
        SetupDataBindings();
        
        // Add selection change event for debugging
        contactListView.SelectedIndexChanged += (s, e) => 
        {
            Console.WriteLine($"Selection changed. Selected index: {contactListView.SelectedIndex}");
            if (contactListView.SelectedItem != null)
            {
                Console.WriteLine($"Selected item: {((Contact)contactListView.SelectedItem).FullName}");
            }
        };

        // Add search event handler
        txtSearch.TextChanged += (s, e) => FilterContacts();
        
        // Add category filter event handler
        cmbCategoryFilter.SelectedIndexChanged += (s, e) => FilterContacts();
    }

    private void InitializeComponent()
    {
        this.Text = "Contact Manager";
        this.Size = new Size(800, 600);
        
        // Add status strip
        statusStrip.Items.Add(lblStatus);
        this.Controls.Add(statusStrip);
    }

    private void SetupLayout()
    {
        // Main container
        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(5)
        };
        
        // Set row styles - first row for search, second for content
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Search panel height
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Rest of the form

        // Search panel at top
        Panel searchPanel = CreateSearchPanel();
        mainLayout.Controls.Add(searchPanel, 0, 0);

        // Split container for list (left) and detail (right)
        SplitContainer splitContainer = new SplitContainer
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 300
        };
        mainLayout.Controls.Add(splitContainer, 0, 1);

        // Left panel: contact list with category filter
        TableLayoutPanel leftPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(0)
        };
        
        // Set row styles for left panel
        leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30)); // Category filter height
        leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Contact list takes remaining space
        
        // Category filter
        cmbCategoryFilter.Dock = DockStyle.Fill;
        cmbCategoryFilter.Items.Add("All Categories");
        cmbCategoryFilter.Items.AddRange(categories);
        cmbCategoryFilter.SelectedIndex = 0;
        leftPanel.Controls.Add(cmbCategoryFilter, 0, 0);
        
        // Contact list
        contactListView.Dock = DockStyle.Fill;
        contactListView.BorderStyle = BorderStyle.FixedSingle;
        contactListView.Font = new Font(contactListView.Font.FontFamily, 10);
        contactListView.DrawMode = DrawMode.OwnerDrawFixed;
        contactListView.DrawItem += ContactListView_DrawItem;
        leftPanel.Controls.Add(contactListView, 0, 1);
        
        splitContainer.Panel1.Controls.Add(leftPanel);

        // Right panel: contact details
        TableLayoutPanel detailsPanel = CreateDetailsPanel();
        splitContainer.Panel2.Controls.Add(detailsPanel);

        this.Controls.Add(mainLayout);
    }

    private Panel CreateSearchPanel()
    {
        Panel panel = new Panel { Dock = DockStyle.Fill };
        
        Label lblSearch = new Label
        {
            Text = "Search:",
            AutoSize = true,
            Location = new Point(10, 12)
        };
        
        txtSearch = new TextBox
        {
            Width = 200,
            Location = new Point(70, 10)
        };
        
        panel.Controls.Add(lblSearch);
        panel.Controls.Add(txtSearch);
        
        return panel;
    }

    private TableLayoutPanel CreateDetailsPanel()
    {
        TableLayoutPanel panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 7,
            Padding = new Padding(10)
        };

        // Add form fields
        AddFormField(panel, "First Name:", txtFirstName, 0);
        AddFormField(panel, "Last Name:", txtLastName, 1);
        AddFormField(panel, "Email:", txtEmail, 2);
        AddFormField(panel, "Phone:", txtPhone, 3);
        AddFormField(panel, "Category:", cmbCategory, 4);
        AddFormField(panel, "Notes:", txtNotes, 5);

        // Populate category dropdown
        cmbCategory.Items.AddRange(categories);
        cmbCategory.SelectedIndex = 3; // Default to "Other"

        // Button panel (Add, Update, Delete, Export)
        FlowLayoutPanel buttonPanel = CreateButtonPanel();
        panel.Controls.Add(buttonPanel, 1, 6);

        return panel;
    }

    private void AddFormField(TableLayoutPanel panel, string labelText, Control control, int row)
    {
        Label label = new Label
        {
            Text = labelText,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 10, 0)
        };

        control.Dock = DockStyle.Fill;
        
        panel.Controls.Add(label, 0, row);
        panel.Controls.Add(control, 1, row);
    }

    private FlowLayoutPanel CreateButtonPanel()
    {
        FlowLayoutPanel buttonPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Fill
        };

        Button btnAdd = new Button { Text = "Add" };
        Button btnUpdate = new Button { Text = "Update" };
        Button btnDelete = new Button { Text = "Delete" };
        Button btnExport = new Button { Text = "Export" };

        // Wire up IContactView events
        btnAdd.Click += (s, e) => AddContact?.Invoke(this, EventArgs.Empty);
        btnUpdate.Click += (s, e) => UpdateContact?.Invoke(this, EventArgs.Empty);
        btnDelete.Click += (s, e) => DeleteContact?.Invoke(this, EventArgs.Empty);
        btnExport.Click += (s, e) => ExportContacts?.Invoke(this, EventArgs.Empty);

        buttonPanel.Controls.Add(btnAdd);
        buttonPanel.Controls.Add(btnUpdate);
        buttonPanel.Controls.Add(btnDelete);
        buttonPanel.Controls.Add(btnExport);

        return buttonPanel;
    }

    private void SetupDataBindings()
    {
        // First set the DisplayMember before setting the DataSource
        contactListView.DisplayMember = "FullName";
        contactListView.DataSource = bindingSource;
        
        // Create one-way bindings (DataSourceToControl) instead of two-way
        txtFirstName.DataBindings.Add(new Binding("Text", bindingSource, "FirstName", true, DataSourceUpdateMode.Never));
        txtLastName.DataBindings.Add(new Binding("Text", bindingSource, "LastName", true, DataSourceUpdateMode.Never));
        txtEmail.DataBindings.Add(new Binding("Text", bindingSource, "Email", true, DataSourceUpdateMode.Never));
        txtPhone.DataBindings.Add(new Binding("Text", bindingSource, "Phone", true, DataSourceUpdateMode.Never));
        txtNotes.DataBindings.Add(new Binding("Text", bindingSource, "Notes", true, DataSourceUpdateMode.Never));
        cmbCategory.DataBindings.Add(new Binding("Text", bindingSource, "Category", true, DataSourceUpdateMode.Never));
    }

    // Custom drawing for contact list items to show category colors
    private void ContactListView_DrawItem(object sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;
        
        e.DrawBackground();
        
        // Get the contact at this index
        Contact contact = (Contact)contactListView.Items[e.Index];
        
        // Set background color based on category
        Brush backgroundBrush = GetCategoryBrush(contact.Category);
        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
        
        // Draw the text
        e.Graphics.DrawString(contact.FullName, e.Font, Brushes.Black, e.Bounds);
        
        e.DrawFocusRectangle();
    }

    private Brush GetCategoryBrush(string category)
    {
        switch (category)
        {
            case "Family": return new SolidBrush(Color.LightBlue);
            case "Friend": return new SolidBrush(Color.LightGreen);
            case "Work": return new SolidBrush(Color.LightYellow);
            default: return new SolidBrush(Color.White);
        }
    }

    private void FilterContacts()
    {
        if (allContacts == null) return;
        
        filteredContacts.Clear();
        
        string searchText = txtSearch.Text.ToLower();
        string categoryFilter = cmbCategoryFilter.SelectedIndex == 0 
            ? null 
            : cmbCategoryFilter.SelectedItem.ToString();
        
        foreach (Contact contact in allContacts)
        {
            // Check category filter
            if (categoryFilter != null && contact.Category != categoryFilter)
                continue;
                
            // Check search text
            if (!string.IsNullOrEmpty(searchText))
            {
                bool matchesSearch = 
                    contact.FullName.ToLower().Contains(searchText) ||
                    contact.Email.ToLower().Contains(searchText) ||
                    contact.Phone.ToLower().Contains(searchText) ||
                    contact.Notes.ToLower().Contains(searchText);
                    
                if (!matchesSearch)
                    continue;
            }
            
            filteredContacts.Add(contact);
        }
        
        // Update the binding source
        bindingSource.DataSource = filteredContacts;
        
        // Update status bar
        UpdateStatusBar();
    }

    private void UpdateStatusBar()
    {
        if (allContacts == null) return;
        
        // Count contacts by category
        int familyCount = 0, friendCount = 0, workCount = 0, otherCount = 0;
        
        foreach (Contact contact in allContacts)
        {
            switch (contact.Category)
            {
                case "Family": familyCount++; break;
                case "Friend": friendCount++; break;
                case "Work": workCount++; break;
                case "Other": otherCount++; break;
            }
        }
        
        // Update status bar
        lblStatus.Text = $"Total: {allContacts.Count} | " +
                         $"Showing: {filteredContacts.Count} | " +
                         $"Family: {familyCount} | " +
                         $"Friends: {friendCount} | " +
                         $"Work: {workCount} | " +
                         $"Other: {otherCount}";
    }

    // IContactView members
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Contact CurrentContact
    {
        get => bindingSource.Current as Contact;
        set => bindingSource.Position = bindingSource.IndexOf(value);
    }

    public void SetContactList(BindingList<Contact> contacts)
    {
        // Store the original contacts list
        allContacts = contacts;
        
        // Clear and repopulate filtered contacts
        filteredContacts.Clear();
        foreach (var contact in contacts)
        {
            filteredContacts.Add(contact);
        }
        
        // Update the binding source with filtered contacts
        bindingSource.DataSource = filteredContacts;
        
        // Update status bar
        UpdateStatusBar();
    }

    public void ClearForm()
    {
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtPhone.Text = string.Empty;
        txtNotes.Text = string.Empty;
        cmbCategory.SelectedIndex = 3; // Default to "Other"
    }

    public void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void ShowReport(string report)
    {
        // Display the report in a simple dialog
        Form reportForm = new Form
        {
            Text = "Contact Report",
            Size = new Size(600, 500),
            StartPosition = FormStartPosition.CenterParent
        };
        
        TextBox txtReport = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Dock = DockStyle.Fill,
            Text = report,
            Font = new Font(FontFamily.GenericMonospace, 9)
        };
        
        reportForm.Controls.Add(txtReport);
        reportForm.ShowDialog();
    }
}







