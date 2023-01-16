namespace CronoTrain.Code.Utils
{
    public interface IAlertManager
    {
        void AlertBreakEnd();
        void Clear();
        void StopBreakEndAlert();
    }
}