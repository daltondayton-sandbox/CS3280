using System;
using System.Drawing;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;

public class UserRegistrationForm : Form
{
    // Main layout and groups
    private TableLayoutPanel mainLayout;
    private GroupBox grpPersonal;
    private TableLayoutPanel tblPersonal;
    
    // Labels and input fields for personal information
    private Label lblName, lblEmail, lblPhone, lblAddress, lblGender, lblPreferredContact;
    private TextBox txtName, txtEmail, txtAddress;
    private MaskedTextBox mtbPhone;
    private ComboBox cmbGender;
    
    // Preferred Contact Method (Radio Buttons)
    private GroupBox grpPreferredContact;
    private RadioButton rdoContactEmail, rdoContactPhone, rdoContactMail;
    
    // Buttons
    private FlowLayoutPanel buttonPanel;
    private Button btnSubmit, btnClear, btnPreview;
    
    // Status Bar
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    
    // Error Provider for validation feedback
    private ErrorProvider errorProvider;
    
    // Change tracking flag
    private bool _hasUnsavedChanges;

    public UserRegistrationForm()
    {
        InitializeComponents();
        SetupEventHandlers();
    }

    private void InitializeComponents()
    {
        // Form properties
        this.Text = "User Registration";
        this.Size = new Size(500, 600);
        
        // Initialize main layout (vertical stacking: form fields, buttons, status bar)
        mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1,
            AutoSize = true,
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));  // fields area
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));  // buttons
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));  // status bar
        
        // GroupBox for personal information
        grpPersonal = new GroupBox
        {
            Text = "Personal Information",
            Dock = DockStyle.Fill,
            AutoSize = true
        };
        
        // TableLayoutPanel inside GroupBox for organized fields
        tblPersonal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 6,
            Padding = new Padding(10),
            AutoSize = true,
        };
        tblPersonal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        tblPersonal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        
        // Create labels with asterisks indicating required fields
        lblName = new Label { Text = "Name *", Anchor = AnchorStyles.Left, AutoSize = true };
        lblEmail = new Label { Text = "Email *", Anchor = AnchorStyles.Left, AutoSize = true };
        lblPhone = new Label { Text = "Phone *", Anchor = AnchorStyles.Left, AutoSize = true };
        lblAddress = new Label { Text = "Address *", Anchor = AnchorStyles.Left, AutoSize = true };
        lblGender = new Label { Text = "Gender *", Anchor = AnchorStyles.Left, AutoSize = true };
        lblPreferredContact = new Label { Text = "Preferred Contact *", Anchor = AnchorStyles.Left, AutoSize = true };
        
        // Create input controls
        txtName = new TextBox { Dock = DockStyle.Fill };
        txtEmail = new TextBox { Dock = DockStyle.Fill };
        // MaskedTextBox for phone with a phone number mask (e.g., (555) 123-4567)
        mtbPhone = new MaskedTextBox { Dock = DockStyle.Fill, Mask = "(999) 000-0000" };
        txtAddress = new TextBox { Dock = DockStyle.Fill };
        
        // ComboBox for Gender selection
        cmbGender = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbGender.Items.AddRange(new string[] { "Male", "Female", "Other" });
        
        // GroupBox for Preferred Contact Method with radio buttons
        grpPreferredContact = new GroupBox
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Text = ""
        };
        FlowLayoutPanel flpPreferred = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        rdoContactEmail = new RadioButton { Text = "Email", AutoSize = true };
        rdoContactPhone = new RadioButton { Text = "Phone", AutoSize = true };
        rdoContactMail = new RadioButton { Text = "Mail", AutoSize = true };
        flpPreferred.Controls.Add(rdoContactEmail);
        flpPreferred.Controls.Add(rdoContactPhone);
        flpPreferred.Controls.Add(rdoContactMail);
        grpPreferredContact.Controls.Add(flpPreferred);
        
        // Add controls to the table layout for personal info
        tblPersonal.Controls.Add(lblName, 0, 0);
        tblPersonal.Controls.Add(txtName, 1, 0);
        tblPersonal.Controls.Add(lblEmail, 0, 1);
        tblPersonal.Controls.Add(txtEmail, 1, 1);
        tblPersonal.Controls.Add(lblPhone, 0, 2);
        tblPersonal.Controls.Add(mtbPhone, 1, 2);
        tblPersonal.Controls.Add(lblAddress, 0, 3);
        tblPersonal.Controls.Add(txtAddress, 1, 3);
        tblPersonal.Controls.Add(lblGender, 0, 4);
        tblPersonal.Controls.Add(cmbGender, 1, 4);
        tblPersonal.Controls.Add(lblPreferredContact, 0, 5);
        tblPersonal.Controls.Add(grpPreferredContact, 1, 5);
        
        grpPersonal.Controls.Add(tblPersonal);
        
        // Initialize buttons panel
        buttonPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Fill,
            AutoSize = true,
            Padding = new Padding(10)
        };
        btnSubmit = new Button { Text = "Submit", AutoSize = true };
        btnClear = new Button { Text = "Clear", AutoSize = true };
        btnPreview = new Button { Text = "Preview", AutoSize = true };
        buttonPanel.Controls.Add(btnSubmit);
        buttonPanel.Controls.Add(btnClear);
        buttonPanel.Controls.Add(btnPreview);
        
        // Initialize status strip for validation status
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel { Text = "Status: " };
        statusStrip.Items.Add(statusLabel);
        
        // Initialize the ErrorProvider for field validations
        errorProvider = new ErrorProvider();
        
        // Assemble the main layout
        mainLayout.Controls.Add(grpPersonal, 0, 0);
        mainLayout.Controls.Add(buttonPanel, 0, 1);
        mainLayout.Controls.Add(statusStrip, 0, 2);
        
        this.Controls.Add(mainLayout);
    }

    private void SetupEventHandlers()
    {
        // Form events
        this.Load += Form_Load;
        this.FormClosing += Form_Closing;
        
        // Text fields change events
        txtName.TextChanged += (s, e) => { ValidateForm(); TrackChanges(); };
        txtEmail.TextChanged += (s, e) => { ValidateForm(); TrackChanges(); };
        txtAddress.TextChanged += (s, e) => { ValidateForm(); TrackChanges(); };
        
        // MaskedTextBox for phone
        mtbPhone.TextChanged += (s, e) => { ValidateForm(); TrackChanges(); };
        
        // ComboBox for gender
        cmbGender.SelectedIndexChanged += (s, e) => { ValidateForm(); TrackChanges(); };
        
        // Radio buttons for preferred contact
        rdoContactEmail.CheckedChanged += (s, e) => { ValidateForm(); TrackChanges(); };
        rdoContactPhone.CheckedChanged += (s, e) => { ValidateForm(); TrackChanges(); };
        rdoContactMail.CheckedChanged += (s, e) => { ValidateForm(); TrackChanges(); };
        
        // Button click events
        btnSubmit.Click += BtnSubmit_Click;
        btnClear.Click += BtnClear_Click;
        btnPreview.Click += BtnPreview_Click;
    }

    private void Form_Load(object sender, EventArgs e)
    {
        ValidateForm();
    }

    private void Form_Closing(object sender, FormClosingEventArgs e)
    {
        if (_hasUnsavedChanges)
        {
            DialogResult result = MessageBox.Show(
                "There are unsaved changes. Do you really want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result == DialogResult.No)
                e.Cancel = true;
        }
    }

    private void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            SaveUser();
            MessageBox.Show("User registered successfully!");
            ClearForm();
        }
        else
        {
            MessageBox.Show("Please correct the errors before submitting.",
                            "Validation Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }

    private void BtnClear_Click(object sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show(
            "Are you sure you want to clear the form?",
            "Clear Form Confirmation",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            ClearForm();
        }
    }

    private void BtnPreview_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            string preferredContact = rdoContactEmail.Checked ? "Email" :
                                      rdoContactPhone.Checked ? "Phone" :
                                      rdoContactMail.Checked ? "Mail" : "N/A";

            string previewMessage = $"[Personal Information]\n" +
                                    $"Name: {txtName.Text}\n" +
                                    $"Email: {txtEmail.Text}\n" +
                                    $"Phone: {mtbPhone.Text}\n" +
                                    $"Address: {txtAddress.Text}\n" +
                                    $"Gender: {cmbGender.SelectedItem}\n" +
                                    $"Preferred Contact: {preferredContact}\n\n" +
                                    $"Status: All fields valid";
            MessageBox.Show(previewMessage, "Preview", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show("Please fix the validation errors before previewing.",
                            "Validation Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }

    private bool ValidateForm()
    {
        bool isValid = true;
        // Validate Name
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "Name is required");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }
        
        // Validate Email
        if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
        {
            errorProvider.SetError(txtEmail, "A valid email is required");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtEmail, "");
        }
        
        // Validate Phone (ensure the MaskedTextBox input is complete)
        if (!mtbPhone.MaskCompleted)
        {
            errorProvider.SetError(mtbPhone, "A valid phone number is required");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(mtbPhone, "");
        }
        
        // Validate Address
        if (string.IsNullOrWhiteSpace(txtAddress.Text))
        {
            errorProvider.SetError(txtAddress, "Address is required");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtAddress, "");
        }
        
        // Validate Gender selection
        if (cmbGender.SelectedIndex < 0)
        {
            errorProvider.SetError(cmbGender, "Please select a gender");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbGender, "");
        }
        
        // Validate Preferred Contact (ensure one radio button is checked)
        if (!rdoContactEmail.Checked && !rdoContactPhone.Checked && !rdoContactMail.Checked)
        {
            errorProvider.SetError(grpPreferredContact, "Select a preferred contact method");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(grpPreferredContact, "");
        }
        
        // Update status label accordingly
        statusLabel.Text = isValid ? "Status: All fields valid" : "Status: Please fix validation errors";
        
        // Enable or disable the Submit button based on overall validity
        btnSubmit.Enabled = isValid;
        
        return isValid;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private void SaveUser()
    {
        // Create a user object with the entered information
        var user = new User
        {
            Name = txtName.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            Phone = mtbPhone.Text,
            Address = txtAddress.Text.Trim(),
            Gender = cmbGender.SelectedItem?.ToString(),
            PreferredContact = rdoContactEmail.Checked ? "Email" :
                               rdoContactPhone.Checked ? "Phone" :
                               rdoContactMail.Checked ? "Mail" : ""
        };

        try
        {
            // Simulate saving to a database (mock implementation)
            SaveUserToDatabase(user);
            _hasUnsavedChanges = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving user: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }

    private void SaveUserToDatabase(User user)
    {
        // Mock database operation: simulate a delay
        System.Threading.Thread.Sleep(500);
    }

    private void ClearForm()
    {
        txtName.Clear();
        txtEmail.Clear();
        mtbPhone.Clear();
        txtAddress.Clear();
        cmbGender.SelectedIndex = -1;
        rdoContactEmail.Checked = false;
        rdoContactPhone.Checked = false;
        rdoContactMail.Checked = false;
        errorProvider.Clear();
        statusLabel.Text = "Status: ";
        _hasUnsavedChanges = false;
    }

    private void TrackChanges()
    {
        _hasUnsavedChanges = true;
    }

    // Nested User class representing the data model
    private class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string PreferredContact { get; set; }
    }
}
