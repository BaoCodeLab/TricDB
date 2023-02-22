using Microsoft.AspNetCore.Mvc;
using Main.platform;

namespace Main.ViewModels
{
    public class BaseViewModel
    {

        private readonly Controller controller;

        public BaseViewModel()
        {
        }

        public BaseViewModel(Controller controller)
        {
            this.controller = controller;
        }

        public bool checkPermission(string roleOrPer)
        {
            if (controller == null) {
                return false;
            }
            return Permission.check(controller.HttpContext, roleOrPer);
        }
    }
}
