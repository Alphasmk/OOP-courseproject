using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamelauncher.ViewModels
{
    public class MainViewModel
    {
        public EmailViewModel EmailViewModel { get; set; }
        public PasswordViewModel PasswordViewModel { get; set; }

        public MainViewModel()
        {
            EmailViewModel = new EmailViewModel();
            PasswordViewModel = new PasswordViewModel();
        }
    }
}
