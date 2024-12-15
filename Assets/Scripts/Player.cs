using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour,IKitchenObjectParent
{

    public static Player Instance {  get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs:EventArgs{
        public BaseCounter selectedCounter;
    }

   [SerializeField] private float moveSpeed = 7f;
   [SerializeField] private float roateSpeed = 10f;
   [SerializeField] private GameInput gameInput;
   [SerializeField] private Transform kitchenObjectHoldPosition;
   [SerializeField] private LayerMask countersLayerMask;


    private bool isWalking;
    private Vector3 lastInteractDir;

    private BaseCounter selectedCounter;

    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.DeleteAction += GameInput_DeleteAction;
    }

    private void GameInput_DeleteAction(object sender, EventArgs e)
    {
        if (HasKitchenObject())
        {
            kitchenObject.DestroySelf();
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }

    }


    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleOInteractions();

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleOInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance,countersLayerMask)){

            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has clear counter
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
                
            }else{
                SetSelectedCounter(null);
            }

        }else {
            SetSelectedCounter(null);
        }
    }



    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        if (moveDir != Vector3.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (!canMove)
        {
            //cannot move towards moveDir

            // Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
           
            if (canMove)
            {
                // Can move only the X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * roateSpeed);

    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPosition;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }
}
