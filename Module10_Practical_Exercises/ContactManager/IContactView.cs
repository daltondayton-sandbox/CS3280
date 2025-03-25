using System.ComponentModel;

public interface IContactView
{
    // The currently selected or edited contact
    Contact CurrentContact { get; set; }

    // Provide the view with a list of contacts (BindingList supports change notifications)
    void SetContactList(BindingList<Contact> contacts);

    // Display an error message
    void ShowError(string message);
    
    // Display a report
    void ShowReport(string report);

    // Clear form inputs
    void ClearForm();

    // Events for user actions
    event EventHandler AddContact;
    event EventHandler UpdateContact;
    event EventHandler DeleteContact;
    event EventHandler ExportContacts;
}

