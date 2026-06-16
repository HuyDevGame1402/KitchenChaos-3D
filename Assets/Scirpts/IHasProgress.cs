using System;

// interface giúp các class counter có thể implement để có thể thay đổi trạng thái counter trong quá trình chs
public interface IHasProgress
{
    // khởi tạo 1 EventHander vs tham số đầu vào là class kế thừa từ EventArgs 
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        // chứa thời lượng % thay đổi trạng thái của counter
        public float progressNormalized;
    }
}
