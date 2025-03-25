public interface IContactRepository
{
    IEnumerable<Contact> GetAll();
    Contact GetById(int id);
    Contact Add(Contact contact);
    void Update(Contact contact);
    void Delete(int id);
}


public class ContactRepository : IContactRepository
{
    private readonly List<Contact> _contacts = new List<Contact>();
    private int _nextId = 1;

    public IEnumerable<Contact> GetAll()
    {
        return _contacts;
    }

    public Contact GetById(int id)
    {
        return _contacts.FirstOrDefault(c => c.Id == id);
    }

    public Contact Add(Contact contact)
    {
        if (contact == null)
            throw new ArgumentNullException(nameof(contact));

        contact.Id = _nextId++;
        _contacts.Add(contact);
        return contact;
    }

    public void Update(Contact contact)
    {
        var existing = _contacts.FirstOrDefault(c => c.Id == contact.Id);
        if (existing != null)
        {
            existing.FirstName = contact.FirstName;
            existing.LastName = contact.LastName;
            existing.Email = contact.Email;
            existing.Phone = contact.Phone;
            existing.Notes = contact.Notes;
        }
    }

    public void Delete(int id)
    {
        var contact = _contacts.FirstOrDefault(c => c.Id == id);
        if (contact != null)
        {
            _contacts.Remove(contact);
        }
    }
}

