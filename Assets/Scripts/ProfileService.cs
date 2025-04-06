public class ProfileService
{
    public int CurrentLevel;
    private static ProfileService _instance;

    public static ProfileService Instance
    {
        get
        {
            if (_instance == default)
            {
                _instance = new();
            }
            
            return _instance;
        }
    }
}