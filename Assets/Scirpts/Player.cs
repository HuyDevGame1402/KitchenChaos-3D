using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 7f;
    private float rotateSpeed = 10f;
    private float playerRadius = .7f;
    private float playerHeight = 2f;
    private Vector3 moverDirX = new Vector3(0, 0, 0);
    private Vector3 moverDirZ = new Vector3(0, 0, 0);
    private bool canMove = true;
    private Vector3 moveDir;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;

    private void Update()
    {
        moveDir = new Vector3(gameInput.GetMovementVectorNormalized().x, 0
            , gameInput.GetMovementVectorNormalized().y);

        canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight, playerRadius, 
            moveDir, moveSpeed * Time.deltaTime);

        if (!canMove)
        {
            moverDirX.x = moveDir.x;
            canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight, playerRadius,
            moverDirX, moveSpeed * Time.deltaTime);
            if (canMove)
            {
                moveDir = moverDirX;
            }
            else
            {
                moverDirZ.z = moveDir.z;
                canMove = !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * playerHeight, playerRadius,
                    moverDirZ, moveSpeed * Time.deltaTime);
                if (canMove)
                {
                    moveDir = moverDirZ;
                }
                else
                {

                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        isWalking = gameInput.GetMovementVectorNormalized().magnitude > 0;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime);
    }
    public bool IsWalking()
    {
        return isWalking;
    }
}
