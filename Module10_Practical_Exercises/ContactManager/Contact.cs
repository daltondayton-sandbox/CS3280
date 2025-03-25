using System.ComponentModel;
using System.Runtime.CompilerServices;

public class Contact : INotifyPropertyChanged
{
    private int _id;
    private string _firstName = "";
    private string _lastName = "";
    private string _email = "";
    private string _phone = "";
    private string _notes = "";
    private string _category = "Other"; // Default category

    public int Id 
    { 
        get => _id; 
        set { _id = value; OnPropertyChanged(); } 
    }
    
    public string FirstName 
    { 
        get => _firstName; 
        set { _firstName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); } 
    }
    
    public string LastName 
    { 
        get => _lastName; 
        set { _lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); } 
    }
    
    public string Email 
    { 
        get => _email; 
        set { _email = value; OnPropertyChanged(); } 
    }
    
    public string Phone 
    { 
        get => _phone; 
        set { _phone = value; OnPropertyChanged(); } 
    }
    
    public string Notes 
    { 
        get => _notes; 
        set { _notes = value; OnPropertyChanged(); } 
    }
    
    public string Category 
    { 
        get => _category; 
        set { _category = value; OnPropertyChanged(); } 
    }

    // Convenience property for displaying full name
    public string FullName => $"{FirstName} {LastName}".Trim();
    
    // Override ToString to ensure proper display in ListBox
    public override string ToString() => FullName;

    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}



