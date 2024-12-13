class Program
{
    static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("请选择操作: 1. 注册 2. 登录 3. 退出");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("请输入用户名: ");
                    string registerUsername = Console.ReadLine();
                    Console.Write("请输入密码: ");
                    string registerPassword = Console.ReadLine();
                    UserManager.Register(registerUsername, registerPassword);
                    break;
                case "2":
                    Console.Write("请输入用户名: ");
                    string loginUsername = Console.ReadLine();
                    Console.Write("请输入密码: ");
                    string loginPassword = Console.ReadLine();
                    if (UserManager.Login(loginUsername, loginPassword))
                    {
                        Console.WriteLine("登录成功！");
                    }
                    else
                    {
                        Console.WriteLine("登录失败，用户名或密码错误！");
                    }
                    break;
                case "3":
                    exit = true;
                    Console.WriteLine("再见！");
                    break;
                default:
                    Console.WriteLine("无效的选择，请重新输入！");
                    break;
            }
        }
    }
}