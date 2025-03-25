static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        IContactRepository repository = new ContactRepository();
        var mainForm = new ContactManagerForm();
        var presenter = new ContactPresenter(mainForm, repository);
        Application.Run(mainForm);
    }
}
