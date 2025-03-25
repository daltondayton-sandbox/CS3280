using System.ComponentModel;
using System.Diagnostics;
using System.Text;

public class ContactPresenter
{
    // Core dependencies for the presenter
    private readonly IContactView _view;
    private readonly IContactRepository _repository;
    
    // BindingList provides change notifications when items are added/removed
    private readonly BindingList<Contact> _contacts;

    public ContactPresenter(IContactView view, IContactRepository repository)
    {
        // Store dependencies
        _view = view;
        _repository = repository;
        
        // Initialize the binding list that will hold our contacts
        _contacts = new BindingList<Contact>();

        // Connect UI events to their handler methods
        _view.AddContact += OnAddContact;
        _view.UpdateContact += OnUpdateContact;
        _view.DeleteContact += OnDeleteContact;
        _view.ExportContacts += OnExportContacts;

        // Populate the UI with initial data
        LoadContacts();
    }

    private void LoadContacts()
    {
        try
        {
            // Get all contacts from the repository
            var contacts = _repository.GetAll();
            
            // Clear existing contacts and add the retrieved ones
            _contacts.Clear();
            foreach (var contact in contacts)
            {
                _contacts.Add(contact);
            }
            
            // Update the UI with the contacts
            _view.SetContactList(_contacts);
        }
        catch (Exception ex)
        {
            // Show error message if loading fails
            _view.ShowError($"Error loading contacts: {ex.Message}");
        }
    }

    private void OnAddContact(object sender, EventArgs e)
    {
        try
        {
            // Create a new contact from form input fields
            var form = (ContactManagerForm)_view;
            var newContact = new Contact
            {
                FirstName = form.txtFirstName.Text,
                LastName = form.txtLastName.Text,
                Email = form.txtEmail.Text,
                Phone = form.txtPhone.Text,
                Notes = form.txtNotes.Text,
                Category = form.cmbCategory.Text
            };
            
            // Validate the contact before saving
            if (ValidateNewContact(newContact))
            {
                // Save to repository and get back the contact with ID assigned
                var savedContact = _repository.Add(newContact);
                
                // Add the saved contact to the binding list
                _contacts.Add(savedContact);
                
                // Reset the form for new entry
                _view.ClearForm();
                
                // Force refresh the UI - this is critical
                _view.SetContactList(_contacts);
                
                // Debug info
                Debug.WriteLine($"Added contact: {savedContact.FullName}, Total: {_contacts.Count}");
            }
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error adding contact: {ex.Message}");
        }
    }

    private void OnUpdateContact(object sender, EventArgs e)
    {
        try
        {
            // Get the currently selected contact
            var contact = _view.CurrentContact;
            if (contact == null)
            {
                _view.ShowError("No contact selected.");
                return;
            }
            
            // Manually update the contact with form values
            var form = (ContactManagerForm)_view;
            contact.FirstName = form.txtFirstName.Text;
            contact.LastName = form.txtLastName.Text;
            contact.Email = form.txtEmail.Text;
            contact.Phone = form.txtPhone.Text;
            contact.Notes = form.txtNotes.Text;
            contact.Category = form.cmbCategory.Text;
            
            // Validate the contact
            if (ValidateContact(contact))
            {
                // Update the contact in the repository
                _repository.Update(contact);
                
                // Reset the form and refresh the contact list
                _view.ClearForm();
                _view.SetContactList(_contacts);
            }
        }
        catch (Exception ex)
        {
            // Show error message if updating fails
            _view.ShowError($"Error updating contact: {ex.Message}");
        }
    }

    private void OnDeleteContact(object sender, EventArgs e)
    {
        try
        {
            // Check if a contact is selected
            if (_view.CurrentContact != null)
            {
                // Delete from repository
                _repository.Delete(_view.CurrentContact.Id);
                
                // Remove from the binding list
                _contacts.Remove(_view.CurrentContact);
                
                // Reset the form and refresh the contact list
                _view.ClearForm();
                _view.SetContactList(_contacts);
            }
        }
        catch (Exception ex)
        {
            // Show error message if deletion fails
            _view.ShowError($"Error deleting contact: {ex.Message}");
        }
    }

    private void OnExportContacts(object sender, EventArgs e)
    {
        try
        {
            // Generate report
            StringBuilder report = new StringBuilder();
            report.AppendLine("Contact Manager Report");
            report.AppendLine("=====================");
            report.AppendLine($"Generated: {DateTime.Now}");
            report.AppendLine($"Total Contacts: {_contacts.Count}");
            report.AppendLine();
            
            // Count by category
            int familyCount = 0, friendCount = 0, workCount = 0, otherCount = 0;
            foreach (var contact in _contacts)
            {
                switch (contact.Category)
                {
                    case "Family": familyCount++; break;
                    case "Friend": friendCount++; break;
                    case "Work": workCount++; break;
                    default: otherCount++; break;
                }
            }
            report.AppendLine("Contacts by Category:");
            report.AppendLine($"  Family: {familyCount}");
            report.AppendLine($"  Friend: {friendCount}");
            report.AppendLine($"  Work: {workCount}");
            report.AppendLine($"  Other: {otherCount}");
            report.AppendLine();
            
            // Add contact details
            report.AppendLine("Contact Details:");
            report.AppendLine("---------------");
            foreach (var contact in _contacts)
            {
                report.AppendLine($"Name: {contact.FullName}");
                report.AppendLine($"Email: {contact.Email}");
                report.AppendLine($"Phone: {contact.Phone}");
                report.AppendLine($"Category: {contact.Category}");
                report.AppendLine($"Notes: {contact.Notes}");
                report.AppendLine();
            }
            
            // Show report in a message box
            _view.ShowReport(report.ToString());
        }
        catch (Exception ex)
        {
            _view.ShowError($"Error generating report: {ex.Message}");
        }
    }

    // Validates an existing contact
    private bool ValidateContact(Contact contact)
    {
        // Check required fields
        if (string.IsNullOrWhiteSpace(contact.FirstName))
        {
            _view.ShowError("First name is required.");
            return false;
        }
        if (string.IsNullOrWhiteSpace(contact.Email))
        {
            _view.ShowError("Email is required.");
            return false;
        }
        return true;
    }

    // Validates a new contact before adding
    private bool ValidateNewContact(Contact contact)
    {
        // Check required fields
        if (string.IsNullOrWhiteSpace(contact.FirstName))
        {
            _view.ShowError("First name is required.");
            return false;
        }
        if (string.IsNullOrWhiteSpace(contact.Email))
        {
            _view.ShowError("Email is required.");
            return false;
        }
        return true;
    }
}





