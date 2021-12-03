using SystemControl.Services;
using SystemControl.Menu;

namespace SystemControl
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu mainMenu = new MainMenu();
            FileService.CreatingFileStructure();
            mainMenu.StartMenu();
        }
    }
}
