using System;
using UnityEngine;

// counter cắt các kitchenobject vật thể
// kế thừa basecounter để sd các thuộc tính cơ bản
// implement interface trạng thái của counter
public class CuttingCounter : BaseCounter, IHasProgress
{
    // tạo 1 event tĩnh static tất cả counter đều truy cập vào
    public static event EventHandler OnAnyCut;
    // vì là kiểu tĩnh nên phải có reset event tĩnh
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    // tạo event thay đổi trạng thái khi cắt 1 vật thể nào đó ví dụ tg trong quá trình cắt hay % cắt đc bn r
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut; // event cắt vật thể
    // tạo 1 mảng array chứa các dữ liệu cutting của game
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress; // biến lưu số lượng đã cắt
    // hàm tương tác khi ấn phím e vs counter
    public override void Interact(Player player)
    {
        // nếu counter k sở hữu kitchenobject
        if (!HasKitchenObject())
        {
            // player sở hữu kitchenobejct thì thực thi trong if
            if (player.HasKitchenObject())
            {
                // nếu vật đó có thể cắt đc chứ thịt k cắt đc còn các loại như cà chua hay rau thì đc
                if(HasRecipeWithInput(player.GetKitchenObject()
                    .GetKitChenObjectSO()))
                {
                    // nếu cắt đc thì player set lại đồ vật ý sang counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0; // đặt lại số lần cắt = 0
                    // lấy ra cutting recipe từ đầu vào của player
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(
                        GetKitchenObject().GetKitChenObjectSO());
                    // gọi Invoke cập nhật trạng thái cắt của counter
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                    //OnCut?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        // nếu counter sở hữu kitchenobject
        else
        {
            // và player cũng có kitchenobject
            if (player.HasKitchenObject())
            {
                // bắt buộc phải là cái đĩa 
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // lấy đc đĩa thì thêm kitchen object vào đĩa
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitChenObjectSO()))
                    {
                        // thêm đc thì xóa kitchenobejct tại counter đi
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            // ngược lại nếu player k có kitchenobject
            else
            {
                // player cầm lại kitchenobject đó của counter
                GetKitchenObject().SetKitchenObjectParent(player);  
            }
        }
    }
    // hàm tương tác khi ấn phím F (thường để cắt vật phẩm)
    public override void InteractAlternate(Player player)
    {
        // nếu sở hữu kitchenobject && kitchenobject đó phải có thể cắt đc
        if (HasKitchenObject() && HasRecipeWithInput(
            GetKitchenObject().GetKitChenObjectSO()))
        {
            // tăng biến cắt lên 1
            cuttingProgress++;
            // lấy ra cutting recipe SO
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(
                GetKitchenObject().GetKitChenObjectSO());
            // invoke gọi để cập nhật ui xem cắt đc bn % rồi
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            // gọi invoke cắt thường để sound
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            // nếu số lần cắt hiện tại >= số lượng cắt max của kitchenobject đó thì thực thi trong if
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // lấy ra kitchenobject sau khi cắt từ đầu vào
                KitChenObjectSO outputKitchenObjectSO = GetOutputForInput(
                GetKitchenObject().GetKitChenObjectSO());
                // xóa vật thể cũ đi
                GetKitchenObject().DestroySelf();
                // thêm vật thể ms đã đc cắt vào và set parent chính cutting counter
                KitChenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }
    // hàm để kiểm tra xem vật thể đó có thể cắt đc k
    private bool HasRecipeWithInput(KitChenObjectSO inputKitchenObjectSO)
    {
        // lấy ra cutting recipe SO nếu có thì trả về true ngc lại false 
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }
    // hàm lấy ra đồ đã đc cắt từ đầu vào
    private KitChenObjectSO GetOutputForInput(KitChenObjectSO inputKitchenObjectSO)
    {
        // tương tự phải lấy ra cutting recipe từ input đầu vào đã
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        // nếu cutting recipe != null tức cái đó có thể cắt và có đầu ra thì trả về output kitchenobject đã được cắt
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    // hàm trả về cutting recipe SO từ đầu vào
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitChenObjectSO inputKitchenObjectSO)
    {
        // duyệt ra các recipe so trong mảng array
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            // kiểm tra nếu input của cutting == input đầu vào 
            // trả về cutting recipe đó
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
