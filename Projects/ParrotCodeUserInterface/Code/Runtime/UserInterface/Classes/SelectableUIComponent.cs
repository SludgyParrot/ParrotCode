using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ParrotCode.Native.Common;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public abstract class SelectableUIComponent: UIImage, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
    {
        [SerializeField, Space(5)]
        private bool interactable = true;

        [SerializeField, Space(5)]
        protected UIStateType initialInteractableState = UIStateType.Normal;

        private EventTrigger actionEvent;

        protected UIStateType currentInteractableState;

        public EventTrigger ActionEvent
        {
            get
            {
                if (actionEvent == null) 
                    actionEvent = GetComponent<EventTrigger>();
                return actionEvent;
            }
        }

        protected override void Init()
            => SetUIState(initialInteractableState);

        #region Pointer events
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            SetUIState(UIStateType.Hovered, isInteractableResults => 
            {  
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnPointerEnter(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            SetUIState(UIStateType.Normal, isInteractableResults =>
            {
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnPointerExit(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            SetUIState(UIStateType.Pressed, isInteractableResults =>
            {
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnPointerDown(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            SetUIState(UIStateType.Normal, isInteractableResults =>
            {
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnPointerUp(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            SetUIState(UIStateType.Pressed, isInteractableResults =>
            {
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnPointerClick(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }
        #endregion

        #region Drag events
        public  virtual void OnBeginDrag(PointerEventData eventData)
        {
            SetUIState(UIStateType.Selected, isInteractableResults =>
            {
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnBeginDrag(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            SetUIState(UIStateType.Selected, isInteractableResults =>
            {
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnDrag(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            SetUIState(UIStateType.Pressed, isInteractableResults =>
            {
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnDrop(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            SetUIState(UIStateType.Selected, isInteractableResults =>
            {
                if (string.IsNullOrEmpty(isInteractableResults.errorMessage))
                    ActionEvent.OnEndDrag(eventData);
                else
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
            });
        }
        #endregion

        public override void SetUIState(UIStateType stateType, Action<(UIImageState state, string errorMessage)> actionCallback = null)

        {
            if (currentInteractableState == UIStateType.Disabled || !interactable)
                return;

            base.SetUIState(stateType, isInteractableResults =>
            {
                if (!string.IsNullOrEmpty(isInteractableResults.errorMessage))
                {
                    Log(isInteractableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
                    actionCallback.Invoke((null, isInteractableResults.errorMessage));
                }
                else
                {
                    var state = isInteractableResults.state;

                    if (state.UseOptionals)
                        SetBackgroundImage(state.StateImage);

                    currentInteractableState = stateType;
                    actionCallback.Invoke((state, isInteractableResults.errorMessage));
                }
            });
        }

        public virtual void OnStateReset()
        {
            currentInteractableState = initialInteractableState;
            SetUIState(currentInteractableState);
        }
    }
}
