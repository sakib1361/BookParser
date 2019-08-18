using GalaSoft.MvvmLight.Ioc;

namespace BookParser.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainWindowModel>();
        }

        public MainWindowModel MainWindowModel => SimpleIoc.Default.GetInstance<MainWindowModel>();
    }
}
