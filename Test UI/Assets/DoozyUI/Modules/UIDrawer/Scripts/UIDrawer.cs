// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DoozyUI.Gestures;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DoozyUI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    public class UIDrawer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public const string SYMBOL = "dUI_UIDrawer";

#if UNITY_EDITOR
        public const int MENU_PRIORITY = 7;
        public const string COMPONENT_MENU = "DoozyUI/UIDrawer";
        public const string GAMEOBJECT_MENU = "GameObject/DoozyUI/UIDrawer";

        [UnityEditor.MenuItem(GAMEOBJECT_MENU, false, MENU_PRIORITY)]
        private static void CreateDrawer(UnityEditor.MenuCommand menuCommand)
        {
            GameObject targetParent = null;
            GameObject selectedGO = menuCommand.context as GameObject;

#if dUI_DoozyUI
            if(selectedGO != null) //check that a gameObject is selected
            {
                if(selectedGO.GetComponent<UICanvas>() != null) //check if the selected gameObject is an UICanvas, otherwise get the root and check)
                {
                    targetParent = selectedGO;
                }
                else if(selectedGO.transform.root.GetComponent<UICanvas>() != null)  //check if there is an UICanvas on the root of the selected gameOhject
                {
                    targetParent = selectedGO.transform.root.gameObject;
                }
            }
            if(targetParent == null)
            {
                targetParent = UIManager.GetMasterCanvas().gameObject;
            }
#else
            if(selectedGO != null) //check that a gameObject is selected
            {
                if(selectedGO.GetComponent<Canvas>() != null) //check if the selected gameObject has a Canvas, otherwise get the root and check
                {
                    targetParent = selectedGO;
                }
                else if (selectedGO.transform.root.GetComponent<Canvas>() != null) ///check if there is a Canvas on the root of the selected gameObject
                {
                    targetParent = selectedGO.transform.root.gameObject;
                }
            }
#endif

            GameObject uiDrawer = new GameObject("UIDrawer", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster), typeof(UIDrawer));
            if(targetParent != null)
            {
                UnityEditor.GameObjectUtility.SetParentAndAlign(uiDrawer, targetParent);
                //uiDrawer.transform.SetParent(targetParent.transform);
                uiDrawer.transform.localPosition = Vector3.zero;
            }
            else
            {
                uiDrawer.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            }
            FullScreenRectTransform(uiDrawer.GetComponent<RectTransform>());
            UnityEditor.Undo.RegisterCreatedObjectUndo(uiDrawer, "Create " + uiDrawer.name);

            GameObject overlay = new GameObject("Overlay", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup), typeof(Image));
            overlay.transform.SetParent(uiDrawer.transform);
            FullScreenRectTransform(overlay.GetComponent<RectTransform>());
            overlay.transform.localPosition = Vector3.zero;
            overlay.GetComponent<Image>().color = new Color(0, 0, 0, 205f / 255f);

            GameObject container = new GameObject("Container", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup));
            container.transform.SetParent(uiDrawer.transform);
            FullScreenRectTransform(container.GetComponent<RectTransform>());
            container.transform.localPosition = Vector3.zero;

            GameObject background = new GameObject("Background", typeof(RectTransform), typeof(Image));
            background.transform.SetParent(container.transform);
            FullScreenRectTransform(background.GetComponent<RectTransform>());
            background.transform.localPosition = Vector3.zero;
            background.GetComponent<Image>().color = new Color(31f / 255f, 136f / 255f, 201f / 255f);

            GameObject arrowContainer = new GameObject("ArrowContainer", typeof(RectTransform));
            arrowContainer.transform.SetParent(uiDrawer.transform);
            FullScreenRectTransform(arrowContainer.GetComponent<RectTransform>());
            arrowContainer.transform.localPosition = Vector3.zero;

            GameObject leftDrawerArrowHolder = new GameObject("LeftDrawerArrowHolder", typeof(RectTransform));
            leftDrawerArrowHolder.transform.SetParent(arrowContainer.transform);
            ResetArrowHolder(leftDrawerArrowHolder.GetComponent<RectTransform>(), SimpleSwipe.Left);
            GameObject leftDrawerArrowClosedPosition = new GameObject("ClosedPosition", typeof(RectTransform));
            leftDrawerArrowClosedPosition.transform.SetParent(leftDrawerArrowHolder.transform);
            ResetClosedArrow(leftDrawerArrowClosedPosition.GetComponent<RectTransform>(), SimpleSwipe.Left);
            GameObject leftDrawerArrowOpenedPosition = new GameObject("OpenedPosition", typeof(RectTransform));
            leftDrawerArrowOpenedPosition.transform.SetParent(leftDrawerArrowHolder.transform);
            ResetOpenedArrow(leftDrawerArrowOpenedPosition.GetComponent<RectTransform>(), SimpleSwipe.Left);


            GameObject rightDrawerArrowHolder = new GameObject("RightDrawerArrowHolder", typeof(RectTransform));
            rightDrawerArrowHolder.transform.SetParent(arrowContainer.transform);
            ResetArrowHolder(rightDrawerArrowHolder.GetComponent<RectTransform>(), SimpleSwipe.Right);
            GameObject rightDrawerArrowClosedPosition = new GameObject("ClosedPosition", typeof(RectTransform));
            rightDrawerArrowClosedPosition.transform.SetParent(rightDrawerArrowHolder.transform);
            ResetClosedArrow(rightDrawerArrowClosedPosition.GetComponent<RectTransform>(), SimpleSwipe.Right);
            GameObject rightDrawerArrowOpenedPosition = new GameObject("OpenedPosition", typeof(RectTransform));
            rightDrawerArrowOpenedPosition.transform.SetParent(rightDrawerArrowHolder.transform);
            ResetOpenedArrow(rightDrawerArrowOpenedPosition.GetComponent<RectTransform>(), SimpleSwipe.Right);

            GameObject upDrawerArrowHolder = new GameObject("UpDrawerArrowHolder", typeof(RectTransform));
            upDrawerArrowHolder.transform.SetParent(arrowContainer.transform);
            ResetArrowHolder(upDrawerArrowHolder.GetComponent<RectTransform>(), SimpleSwipe.Up);
            GameObject upDrawerArrowClosedPosition = new GameObject("ClosedPosition", typeof(RectTransform));
            upDrawerArrowClosedPosition.transform.SetParent(upDrawerArrowHolder.transform);
            ResetClosedArrow(upDrawerArrowClosedPosition.GetComponent<RectTransform>(), SimpleSwipe.Up);
            GameObject upDrawerArrowOpenedPosition = new GameObject("OpenedPosition", typeof(RectTransform));
            upDrawerArrowOpenedPosition.transform.SetParent(upDrawerArrowHolder.transform);
            ResetOpenedArrow(upDrawerArrowOpenedPosition.GetComponent<RectTransform>(), SimpleSwipe.Up);

            GameObject downDrawerArrowHolder = new GameObject("DownDrawerArrowHolder", typeof(RectTransform));
            downDrawerArrowHolder.transform.SetParent(arrowContainer.transform);
            ResetArrowHolder(downDrawerArrowHolder.GetComponent<RectTransform>(), SimpleSwipe.Down);
            GameObject downDrawerArrowClosedPosition = new GameObject("ClosedPosition", typeof(RectTransform));
            downDrawerArrowClosedPosition.transform.SetParent(downDrawerArrowHolder.transform);
            ResetClosedArrow(downDrawerArrowClosedPosition.GetComponent<RectTransform>(), SimpleSwipe.Down);
            GameObject downDrawerArrowOpenedPosition = new GameObject("OpenedPosition", typeof(RectTransform));
            downDrawerArrowOpenedPosition.transform.SetParent(downDrawerArrowHolder.transform);
            ResetOpenedArrow(downDrawerArrowOpenedPosition.GetComponent<RectTransform>(), SimpleSwipe.Down);

            GameObject arrow = new GameObject("Arrow", typeof(RectTransform), typeof(UIDrawerArrow));
            arrow.transform.SetParent(arrowContainer.transform);
            arrow.GetComponent<RectTransform>().localScale = Vector3.one;
            arrow.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            arrow.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            arrow.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            arrow.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            arrow.transform.localPosition = Vector3.zero;

            GameObject rotator = new GameObject("Rotator", typeof(RectTransform));
            rotator.transform.SetParent(arrow.transform);
            rotator.GetComponent<RectTransform>().localScale = Vector3.one;
            rotator.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            rotator.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            rotator.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            rotator.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            rotator.transform.localPosition = Vector3.zero;

            GameObject leftBar = new GameObject("LeftBar", typeof(RectTransform), typeof(Image));
            leftBar.transform.SetParent(rotator.transform);
            leftBar.GetComponent<RectTransform>().localScale = Vector3.one;
            leftBar.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);
            leftBar.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0.5f);
            leftBar.GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
            leftBar.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 6);
            leftBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(52, 0);
            leftBar.transform.localPosition = new Vector3(2, 0, 0);

            GameObject rightBar = new GameObject("RightBar", typeof(RectTransform), typeof(Image));
            rightBar.transform.SetParent(rotator.transform);
            rightBar.GetComponent<RectTransform>().localScale = Vector3.one;
            rightBar.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0.5f);
            rightBar.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
            rightBar.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
            rightBar.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 6);
            rightBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-52, 0);
            rightBar.transform.localPosition = new Vector3(-2, 0, 0);

            arrow.GetComponent<UIDrawerArrow>().rotator = rotator.GetComponent<RectTransform>();
            arrow.GetComponent<UIDrawerArrow>().leftBar = leftBar.GetComponent<RectTransform>();
            arrow.GetComponent<UIDrawerArrow>().rightBar = rightBar.GetComponent<RectTransform>();
            arrow.SetActive(false);

            uiDrawer.GetComponent<UIDrawer>().arrowContainer = arrowContainer.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().arrow = arrow.GetComponent<UIDrawerArrow>();

            uiDrawer.GetComponent<UIDrawer>().leftDrawerArrowHolder = leftDrawerArrowHolder.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().leftDrawerArrowClosedPosition = leftDrawerArrowClosedPosition.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().leftDrawerArrowOpenedPosition = leftDrawerArrowOpenedPosition.GetComponent<RectTransform>();

            uiDrawer.GetComponent<UIDrawer>().rightDrawerArrowHolder = rightDrawerArrowHolder.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().rightDrawerArrowClosedPosition = rightDrawerArrowClosedPosition.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().rightDrawerArrowOpenedPosition = rightDrawerArrowOpenedPosition.GetComponent<RectTransform>();

            uiDrawer.GetComponent<UIDrawer>().upDrawerArrowHolder = upDrawerArrowHolder.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().upDrawerArrowClosedPosition = upDrawerArrowClosedPosition.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().upDrawerArrowOpenedPosition = upDrawerArrowOpenedPosition.GetComponent<RectTransform>();

            uiDrawer.GetComponent<UIDrawer>().downDrawerArrowHolder = downDrawerArrowHolder.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().downDrawerArrowClosedPosition = downDrawerArrowClosedPosition.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().downDrawerArrowOpenedPosition = downDrawerArrowOpenedPosition.GetComponent<RectTransform>();

            uiDrawer.GetComponent<UIDrawer>().container = container.GetComponent<RectTransform>();
            uiDrawer.GetComponent<UIDrawer>().overlay = overlay.GetComponent<RectTransform>();

            UnityEditor.Selection.activeObject = uiDrawer;
        }

        private static void FullScreenRectTransform(RectTransform target)
        {
            target.GetComponent<RectTransform>().localScale = Vector3.one;
            target.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            target.GetComponent<RectTransform>().anchorMax = Vector2.one;
            target.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            target.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }

        private static void CenterRectTransform(RectTransform target)
        {
            target.GetComponent<RectTransform>().localScale = Vector3.one;
            target.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            target.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            target.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            target.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// Resets the arrow holder to the default position.
        /// </summary>
        /// <param name="arrowHolder">The arrow holder.</param>
        /// <param name="closeDirection">The UIDrawer close direction.</param>
        public static void ResetArrowHolder(RectTransform arrowHolder, SimpleSwipe closeDirection)
        {
            arrowHolder.localScale = Vector3.one;
            arrowHolder.transform.localPosition = Vector3.zero;

            switch(closeDirection)
            {
                case SimpleSwipe.Left:
                    arrowHolder.anchorMin = new Vector2(1f, 0.5f);
                    arrowHolder.anchorMax = new Vector2(1f, 0.5f);
                    break;
                case SimpleSwipe.Right:
                    arrowHolder.anchorMin = new Vector2(0f, 0.5f);
                    arrowHolder.anchorMax = new Vector2(0f, 0.5f);
                    break;
                case SimpleSwipe.Up:
                    arrowHolder.anchorMin = new Vector2(0.5f, 0f);
                    arrowHolder.anchorMax = new Vector2(0.5f, 0f);
                    break;
                case SimpleSwipe.Down:
                    arrowHolder.anchorMin = new Vector2(0.5f, 1f);
                    arrowHolder.anchorMax = new Vector2(0.5f, 1f);
                    break;
            }

            arrowHolder.pivot = new Vector2(0.5f, 0.5f);
            arrowHolder.sizeDelta = Vector2.zero;
            arrowHolder.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Resets the closed arrow position to its default value.
        /// </summary>
        /// <param name="closedArrow">The closed arrow.</param>
        /// <param name="closeDirection">The UIDrawer close direction.</param>
        public static void ResetClosedArrow(RectTransform closedArrow, SimpleSwipe closeDirection)
        {
            CenterRectTransform(closedArrow);
            closedArrow.localPosition = Vector3.zero;

            switch(closeDirection)
            {
                case SimpleSwipe.Left: closedArrow.anchoredPosition = new Vector2(50, 0); break;
                case SimpleSwipe.Right: closedArrow.anchoredPosition = new Vector2(-50, 0); break;
                case SimpleSwipe.Up: closedArrow.anchoredPosition = new Vector2(0, -50); break;
                case SimpleSwipe.Down: closedArrow.anchoredPosition = new Vector2(0, 50); break;
            }
        }

        /// <summary>
        /// Resets the opened arrow position to its default value.
        /// </summary>
        /// <param name="openedArrow">The opened arrow.</param>
        /// <param name="closeDirection">The UIDrawer close direction.</param>
        public static void ResetOpenedArrow(RectTransform openedArrow, SimpleSwipe closeDirection)
        {
            CenterRectTransform(openedArrow);
            openedArrow.localPosition = Vector3.zero;

            switch(closeDirection)
            {
                case SimpleSwipe.Left: openedArrow.anchoredPosition = new Vector2(-50, 0); break;
                case SimpleSwipe.Right: openedArrow.anchoredPosition = new Vector2(50, 0); break;
                case SimpleSwipe.Up: openedArrow.anchoredPosition = new Vector2(0, 50); break;
                case SimpleSwipe.Down: openedArrow.anchoredPosition = new Vector2(0, -50); break;
            }
        }

        /// <summary>
        /// Matches the target rect transform to the source rect transform.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public static void MatchRectTransform(RectTransform source, RectTransform target)
        {
            CenterRectTransform(target);
            target.localPosition = Vector3.zero;

            target.localScale = source.localScale;
            target.anchorMin = source.anchorMin;
            target.anchorMax = source.anchorMax;
            target.pivot = source.pivot;
            target.sizeDelta = source.sizeDelta;
            target.anchoredPosition3D = source.anchoredPosition3D;
        }
#endif

        /// <summary>
        /// Defines all the states a drawer can be in.
        /// </summary>
        public enum DrawerState
        {
            /// <summary>
            /// Drawer is opened.
            /// </summary>
            Opened,
            /// <summary>
            /// Drawer is opening. The open animation is running.
            /// </summary>
            IsOpening,
            /// <summary>
            /// Drawer is closed.
            /// </summary>
            Closed,
            /// <summary>
            /// Drawer is closing. The close animation is running.
            /// </summary>
            IsClosing
        }

        /// <summary>
        /// This is the default drawer name for any newly created drawer.
        /// </summary>
        public const string DEFAULT_DRAWER_NAME = "~Drawer Name~";
        /// <summary>
        /// If the drawer has been dragged beyond this point from Cosed state towards Opened state, then Open it. Interval 0 to 1 (0 = 0% and 1 = 100%). Default value: 0.5f (50%)
        /// </summary>
        private const float AUTO_OPEN_IF_DRAGGED_OVER_VISIBILITY_PERCENT = 0.5f;
        /// <summary>
        /// If the drawer has been dragged beyond this point from Opened state towards Closed state, then Close it. Interval 0 to 1 (0 = 0% and 1 = 100%). Default value: 0.5f (50%)
        /// </summary>
        private const float AUTO_CLOSE_IF_DRAGGED_UNDER_VISIBLITY_PERCENT = 0.5f;
        /// <summary>
        /// How fast does the swipe speed has to be in order to determine an auto Open or Close of the drawer. This consant is used to deteminet the outcome of fast swipes on the screen.
        /// </summary>
        private const float AUTO_OPEN_OR_CLOSE_TERMINAL_SWIPE_VELOCITY = 800f;

        /// <summary>
        /// Internal static class dictionary that keeps track of all the registered UIDrawers.
        /// </summary>
        private static Dictionary<string, UIDrawer> m_drawerDatabase;
        /// <summary>
        /// Returns a registry of all the registered UIDrawers.
        /// </summary>
        public static Dictionary<string, UIDrawer> DrawerDatabase { get { if(m_drawerDatabase == null) { m_drawerDatabase = new Dictionary<string, UIDrawer>(); } return m_drawerDatabase; } }

        /// <summary>
        /// Internal class static variable that holds a reference to the currently open drawer. There can be only one drawer opened at a time.
        /// </summary>
        private static UIDrawer openedDrawer = null;
        /// <summary>
        /// Returns the reference to the currently open drawer. If no drawer is opened, it will return null. There can be only one drawer opened at a time.
        /// </summary>
        public static UIDrawer OpenedDrawer { get { return openedDrawer; } private set { openedDrawer = value; } }

        /// <summary>
        /// Internal variable of the rhe dragged UIDrawer reference.
        /// </summary>
        private static UIDrawer draggedDrawer = null;
        /// <summary>
        /// Gets the reference to the dragged UIDrawer.
        /// </summary>
        public static UIDrawer DraggedDrawer { get { return draggedDrawer; } private set { draggedDrawer = value; } }

        /// <summary>
        /// Retruns true if there is an OpenedDrawer. There can be only one drawer opened at a time.
        /// </summary>
        public static bool IsAnyDrawerOpened { get { return OpenedDrawer != null; } }

        /// <summary>
        /// Prints Debug Logs when the drawer is opened/closed.
        /// </summary>
        public bool debugDrawer = false;
        /// <summary>
        /// Prints Debug Logs when the drawer invokes any of its UnityEvents.
        /// </summary>
        public bool debugEvents = false;

        /// <summary>
        /// The name of this drawer. The name is important when opening or closing an UIDrawer using the static methods.
        /// </summary>
        public string drawerName = DEFAULT_DRAWER_NAME;

        /// <summary>
        /// The drawer position when closed (this also affects what gesture opens/closes the drawer).
        /// </summary>
        public SimpleSwipe drawerCloseDirection = SimpleSwipe.Left;

        /// <summary>
        /// Open drawer animation speed.
        /// </summary>
        public float openSpeed = 10f;
        /// <summary>
        /// Close drawer animation speed.
        /// </summary>
        public float closeSpeed = 10f;

        /// <summary>
        /// Enables/Disables the gesture detectors of this drawer. If disabled, this drawer will no longer react to gestures. Useful if you plan on opening/closing the drawer via a button or a script.
        /// </summary>
        public bool detectGestures = true;

        /// <summary>
        /// Should this UIDrawer slide from or go to a set custom position. Default is set to true.
        /// </summary>
        public bool useCustomStartAnchoredPosition = true;
        /// <summary>
        /// The custom anchored position that this UIElement slides from or goes to when opening/closing. You can use this in code to cusomize on the fly this positon.
        /// </summary>
        public Vector3 customStartAnchoredPosition = Vector3.zero;

        /// <summary>
        /// UnityEvent invoked when the drawer opened.
        /// </summary>
        public UnityEvent OnDrawerOpened = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when the drawer started the open animation.
        /// This event does not get triggered when the drawer is being dragged.
        /// </summary>
        public UnityEvent OnDrawerIsOpening = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when the drawer closed.
        /// </summary>
        public UnityEvent OnDrawerClosed = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when the drawer started the close animations.
        /// This event does not get triggered when the drawer is being dragged.
        /// </summary>
        public UnityEvent OnDrawerIsClosing = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when the drawer started being dragged.
        /// </summary>
        public UnityEvent OnDrawerBeginDrag = new UnityEvent();
        /// <summary>
        /// UnityEvent invoked when the drawer stopped being dragged.
        /// </summary>
        public UnityEvent OnDrawerEndDrag = new UnityEvent();

        /// <summary>
        /// Reference to the drawer's container. This is the part that gets animated in (when opening) and out (when closing). This also contains all the drawer's contents (like texts, buttons, scrollers, layout groups...).
        /// </summary>
        public RectTransform container;
        /// <summary>
        /// Determines if the container should fade out when closed. This controls the fade in (when opening) and fade out (when closing) animations for the container (and its contents).
        /// </summary>
        public bool fadeOutContainerWhenClosed = true;
        /// <summary>
        /// Disables this UIDrawer's container when it is not visible (it is closed) by setting its active state to false.
        /// <para>Use this only if you have scripts that you need to disable. Otherwise you don't need it as the system handles the drawcalls in an efficient manner.</para>
        /// </summary>
        public bool disableContainerWhenClosed = true;
        /// <summary>
        /// This will disable the optimization that disables the container's Canvas and GraphicRaycaster when the UIDrawer is closed. 
        /// Do not enable this unless you know what you are doing as, when set to TRUE, this will increase your draw calls.
        /// </summary>
        public bool dontDisableContainerCanvasWhenClosed = false;

        public enum ContainerSize
        {
            FullScreen,
            PercentageOfScreen,
            FixedSize
        }

        /// <summary>
        /// The container's size: FullScreen / PercentageOfScreen / FixedSize
        /// </summary>
        public ContainerSize containerSize = ContainerSize.FullScreen;
        /// <summary>
        /// The container's percentage of screen size if the containerSize is set to PercentageOfScreen.
        /// </summary>
        public float containerPercentageOfScreenSize = 0.5f;
        /// <summary>
        /// The container's minimum size of screen size if the containerSize is set to PercentageOfScreen.
        /// </summary>
        public float containerMinimumSize = 0f;
        /// <summary>
        /// The container's fixed size if the containerSize is set to FixedSize.
        /// </summary>
        public float containerFixedSize = 128f;


        /// <summary>
        /// Reference to the gameObject that is used as an overlay. It should have the following components attached: RectTransform, Canvas, GraphicRaycaster, CanvasGroup and Image.
        /// </summary>
        public RectTransform overlay;
        /// <summary>
        /// Returns true if an overlay has been references.
        /// </summary>
        public bool HasOverlay { get { return overlay != null; } }
        /// <summary>
        /// Disables this UIDrawer's overlay when it is not visible (it is closed) by setting its active state to false.
        /// <para>Use this only if you have scripts that you need to disable. Otherwise you don't need it as the system handles the drawcalls in an effecient manner.</para>
        /// </summary>
        public bool disableOverlayWhenClosed = true;
        /// <summary>
        /// This will disable the optimization that disables the overlay's Canvas and GraphicRaycaster when the UIDrawer is closed. 
        /// Do not enable this unless you know what you are doing as, when set to TRUE, this will increase your draw calls.
        /// </summary>
        public bool dontDisableOverlayCanvasWhenClosed = false;

        /// <summary>
        /// Variable used to determine if the arrow should be shown or not.
        /// </summary>
        public bool showArrow = true;

        /// <summary>
        /// Scale variable that overrides the scale of the arrow at runtime. This will set the localScale values of the arrow. (it will only override localScale.x and localScale.y, as localScale.z is useless for an UI).
        /// </summary>
        public float arrowScale = 1;

        /// <summary>
        /// If set to true, the arrow color will get interpolated between arrowColorWhenClosed and arrowColorWhenOpened colors.
        /// </summary>
        public bool overrideArrowColor;
        /// <summary>
        /// Color variable that overrides the color of the arrow at runtime. This will set the Image color of the arrow to the set values when the drawer is closed.
        /// </summary>
        public Color arrowColorWhenClosed = Color.white;
        /// <summary>
        /// Color variable that overrides the color of the arrow at runtime. This will set the Image color of the arrow to the set values when the drawer is opened.
        /// </summary>
        public Color arrowColorWhenOpened = Color.white;

        /// <summary>
        /// Reference to the arrow that gets animated when the drawer is in transition or dragged.
        /// </summary>
        public UIDrawerArrow arrow;
        /// <summary>
        /// Internal reference to the arrowContainer. This is used to sync the arrow position with the container position. Makes for a nicer animation.
        /// </summary>
        public RectTransform arrowContainer;

        /// <summary>
        ///  Internal reference to the gameObject and position where the arrow should be parented and moved if the drawer is set to close to the left side of the screen.
        /// </summary>
        public RectTransform leftDrawerArrowHolder;
        /// <summary>
        ///  Internal reference to the closed position where the arrow should move to if the drawer is set to close to the left side of the screen.
        /// </summary>
        public RectTransform leftDrawerArrowClosedPosition;
        /// <summary>
        ///  Internal reference to the opened position where the arrow should move to if the drawer is set to close to the left side of the screen.
        /// </summary>
        public RectTransform leftDrawerArrowOpenedPosition;
        /// <summary>
        ///  Internal reference to the gameObject and position where the arrow should be parented and moved if the drawer is set to close to the right side of the screen.
        /// </summary>
        public RectTransform rightDrawerArrowHolder;
        /// <summary>
        ///  Internal reference to the closed position where the arrow should move to if the drawer is set to close to the right side of the screen.
        /// </summary>
        public RectTransform rightDrawerArrowClosedPosition;
        /// <summary>
        ///  Internal reference to the opened position where the arrow should move to if the drawer is set to close to the right side of the screen.
        /// </summary>
        public RectTransform rightDrawerArrowOpenedPosition;
        /// <summary>
        ///  Internal reference to the gameObject and position where the arrow should be parented and moved if the drawer is set to close to the top (up) side of the screen.
        /// </summary>
        public RectTransform upDrawerArrowHolder;
        /// <summary>
        ///  Internal reference to the closed position where the arrow should move to if the drawer is set to close to the top (up) side of the screen.
        /// </summary>
        public RectTransform upDrawerArrowClosedPosition;
        /// <summary>
        ///  Internal reference to the opened position where the arrow should move to if the drawer is set to close to the top (up) side of the screen.
        /// </summary>
        public RectTransform upDrawerArrowOpenedPosition;
        /// <summary>
        ///  Internal reference to the gameObject and position where the arrow should be parented and moved if the drawer is set to close to the botton (down) side of the screen.
        /// </summary>
        public RectTransform downDrawerArrowHolder;
        /// <summary>
        ///  Internal reference to the closed position where the arrow should move to if the drawer is set to close to the bottom (down) side of the screen.
        /// </summary>
        public RectTransform downDrawerArrowClosedPosition;
        /// <summary>
        ///  Internal reference to the opened position where the arrow should move to if the drawer is set to close to the bottom (down) side of the screen.
        /// </summary>
        public RectTransform downDrawerArrowOpenedPosition;

        /// <summary>
        /// Internal variable that keeps tarck of the current state of the drawer. Opened - IsOpening - Closed - IsClosing
        /// </summary>
        private DrawerState currentDrawerState = DrawerState.Opened;
        /// <summary>
        /// Returns the current state of the drawer. Opened - IsOpening - Closed - IsClosing
        /// </summary>
        public DrawerState CurerntDrawerState { get { return currentDrawerState; } }

        /// <summary>
        /// Returns true if the drawer is opened.
        /// </summary>
        public bool Opened { get { return currentDrawerState == DrawerState.Opened; } }
        /// <summary>
        /// Returns true if the drawer is currently opening (the open animation is running).
        /// </summary>
        public bool IsOpening { get { return currentDrawerState == DrawerState.IsOpening; } }
        /// <summary>
        /// Returns true if the drawer is closed.
        /// </summary>
        public bool Closed { get { return currentDrawerState == DrawerState.Closed; } }
        /// <summary>
        /// Returns true if the drawer is currently closing (the close animation is running).
        /// </summary>
        public bool IsClosing { get { return currentDrawerState == DrawerState.IsClosing; } }

        /// <summary>
        /// Internal variable that is uesd to validate the drag. This variable is needed in order to filter out drags that happen on other UI components.
        /// </summary>
        private bool canBeDragged = true;

        /// <summary>
        /// Internal variable that is set to true when the drawer is being dragged.
        /// </summary>
        private bool isDragged = false;
        /// <summary>
        /// Returns true if this drawer is currently begin dragged.
        /// </summary>
        public bool IsDragged { get { return isDragged; } private set { isDragged = value; } }

        /// <summary>
        /// Internal variable used to save the start position from where the drag started.
        /// </summary>
        private Vector2 dragStartPosition;

        /// <summary>
        /// Internal variable used to detemine the open state percentage of the drawer (how much of the container is visible on screen). (0 = 0% = Closed and 1 = 100% = Open)
        /// </summary>
        private float visibility = 1f;
        /// <summary>
        /// Returns the open state percentage the drawer (how much of the container is visible on screen). (0 = 0% = Closed and 1 = 100% = Open)
        /// </summary>
        public float Visibility { get { return visibility; } }
        /// <summary>
        /// Retruns true if the drawer is open, thus the Visibility value is 1.
        /// </summary>
        public bool IsVisible { get { return Visibility == 1; } }

        /// <summary>
        /// Internal variable that holds a reference to the RectTransform component.
        /// </summary>
        private RectTransform m_rectTransform;
        /// <summary>
        /// Returns the RectTransform component.
        /// </summary>
        public RectTransform RectTransform { get { if(m_rectTransform == null) { m_rectTransform = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>(); } return m_rectTransform; } }
        /// <summary>
        /// Internal variable that holds a reference to the Canvas component.
        /// </summary>
        private Canvas m_canvas;
        /// <summary>
        /// Returns the Canvas component.
        /// </summary>
        public Canvas Canvas { get { if(m_canvas == null) { m_canvas = GetComponent<Canvas>() ?? gameObject.AddComponent<Canvas>(); } return m_canvas; } }

        /// <summary>
        /// Internal variable that holds a reference to the Canvas component of the container.
        /// </summary>
        private Canvas m_containerCanvas;
        /// <summary>
        /// Returns the Canvas component of the container.
        /// </summary>
        public Canvas ContainerCanvas { get { if(m_containerCanvas == null) { m_containerCanvas = container.GetComponent<Canvas>() ?? container.gameObject.AddComponent<Canvas>(); } return m_containerCanvas; } }
        /// <summary>
        /// Internal variable that holds a reference to the GraphicRaycaster component of the container.
        /// </summary>
        private GraphicRaycaster m_containerGraphicRaycaster;
        /// <summary>
        /// Returns the GraphicRaycaster component of the container.
        /// </summary>
        public GraphicRaycaster ContainerGraphicRaycaster { get { if(m_containerGraphicRaycaster == null) { m_containerGraphicRaycaster = container.GetComponent<GraphicRaycaster>() ?? container.gameObject.AddComponent<GraphicRaycaster>(); } return m_containerGraphicRaycaster; } }
        /// <summary>
        /// Internal variable that holds a reference to the CanvasGroup component of the container.
        /// </summary>
        private CanvasGroup m_containerCanvasGroup;
        /// <summary>
        /// Returns the CanvasGroup component of the container.
        /// </summary>
        public CanvasGroup ContainerCanvasGroup { get { if(m_containerCanvasGroup == null) { m_containerCanvasGroup = container.GetComponent<CanvasGroup>() ?? container.gameObject.AddComponent<CanvasGroup>(); } return m_containerCanvasGroup; } }

        /// <summary>
        /// Internal variable that holds a reference to the Canvas component of the overlay.
        /// </summary>
        private Canvas m_overlayCanvas;
        /// <summary>
        /// Returns the Canvas component of the overlay.
        /// </summary>
        public Canvas OverlayCanvas { get { if(m_overlayCanvas == null) { m_overlayCanvas = overlay.GetComponent<Canvas>() ?? overlay.gameObject.AddComponent<Canvas>(); } return m_overlayCanvas; } }
        /// <summary>
        /// Internal variable that holds a reference to the GraphicRaycaster component of the overlay.
        /// </summary>
        private GraphicRaycaster m_overlayGraphicRaycaster;
        /// <summary>
        /// Returns the GraphicRaycaster component of the overlay.
        /// </summary>
        public GraphicRaycaster OverlayGraphicRaycaster { get { if(m_overlayGraphicRaycaster == null) { m_overlayGraphicRaycaster = overlay.GetComponent<GraphicRaycaster>() ?? overlay.gameObject.AddComponent<GraphicRaycaster>(); } return m_overlayGraphicRaycaster; } }
        /// <summary>
        /// Internal variable that holds a reference to the CanvasGroup component of the overlay.
        /// </summary>
        private CanvasGroup m_overlayCanvasGroup;
        /// <summary>
        /// Returns the CanvasGroup component of the overlay.
        /// </summary>
        public CanvasGroup OverlayCanvasGroup { get { if(m_overlayCanvasGroup == null) { m_overlayCanvasGroup = overlay.GetComponent<CanvasGroup>() ?? overlay.gameObject.AddComponent<CanvasGroup>(); } return m_overlayCanvasGroup; } }

        /// <summary>
        /// Internal variable that holds the start RectTransform.anchoredPosition3D.
        /// </summary>
        private Vector3 startAnchoredPosition;

        /// <summary>
        /// Initial position of the container. This is the position of the container when the drawer is open.
        /// </summary>
        private Vector3 containerOpenedAnchoredPosition;
        /// <summary>
        /// Retruns hte initial position of the container. This is the position of the container when the drawer is open.
        /// </summary>
        public Vector3 ContainerOpenedAnchoredPosition { get { return containerOpenedAnchoredPosition; } }
        /// <summary>
        /// Calculated value for the container when the drawer is closed.
        /// </summary>
        private Vector3 containerClosedAnchoredPosition;
        /// <summary>
        /// Returns the calculated value of the container when the drawer is closed.
        /// </summary>
        public Vector3 ContainerClosedAnchoredPosition { get { return containerClosedAnchoredPosition; } }

        /// <summary>
        /// The container anchored position in the current frame.
        /// </summary>
        private Vector2 containerCurrentAnchoredPosition;
        /// <summary>
        /// The container anchored position in the previous frame.
        /// </summary>
        private Vector2 containerPreviousAnchoredPosition;
        /// <summary>
        /// Used by the DrawerArrow live animation.
        /// </summary>
        public Vector2 ContainerVelocity { get { return containerCurrentAnchoredPosition - containerPreviousAnchoredPosition; } }

        /// <summary>
        /// Returns the TouchManager reference.
        /// </summary>
        public TouchManager TouchManager { get { return TouchManager.Instance; } }

        /// <summary>
        /// Internal variable used to save the scaled canvas size.
        /// </summary>
        private Vector2 scaledCanvas;
        /// <summary>
        ///  Internal variable used to save the container calculated size.
        /// </summary>
        private Vector2 containerCalculatedSize;

        private void OnDrawGizmosSelected()
        {
            DrawHolders();
        }

        private void DrawHolders()
        {
            //Gizmos.DrawWireCube(leftDrawerArrowHolder.position, new Vector3(0.5f, 0.5f, 0.5f));

        }

        private void Awake()
        {
            m_rectTransform = RectTransform;
            m_canvas = Canvas;

            if(container == null)
            {
                Debug.Log("[UIDrawer] The '" + drawerName + "' drawer does not have a container referenced. " +
                          "This is the main drawer component and it should not be missing." +
                          "Either reference it or delete this gameObject. " +
                          "For this session, this gameObject has been disabled. " +
                          "(HINT: you can create a new UIDrawer to see how the container should be referenced)",
                          gameObject);

                gameObject.SetActive(false);
                return;
            }

            m_containerCanvas = ContainerCanvas;
            m_containerGraphicRaycaster = ContainerGraphicRaycaster;
            m_containerCanvasGroup = ContainerCanvasGroup;

            if(HasOverlay)
            {
                m_overlayCanvas = OverlayCanvas;
                m_overlayGraphicRaycaster = OverlayGraphicRaycaster;
                m_overlayCanvasGroup = OverlayCanvasGroup;
            }

            startAnchoredPosition = useCustomStartAnchoredPosition ? customStartAnchoredPosition : RectTransform.anchoredPosition3D;
            RectTransform.anchoredPosition3D = startAnchoredPosition;

            UpdateContainer();

            InitializeContainerPositions();
            InitializeArrow();

            InitializeDrawerName();

            Close(true);

            RegisterToDrawerDatabase();

            if(debugEvents)
            {
                OnDrawerOpened.AddListener(OnDrawerOpenedEvent);
                OnDrawerIsOpening.AddListener(OnDrawerIsOpeningEvent);
                OnDrawerClosed.AddListener(OnDrawerClosedEvent);
                OnDrawerIsClosing.AddListener(OnDrawerIsClosingEvent);
                OnDrawerBeginDrag.AddListener(OnDrawerBeginDragEvent);
                OnDrawerEndDrag.AddListener(OnDrawerEndDragEvent);
            }
        }

        private void OnDestroy()
        {
            UnregisterFromDrawerDatabase(); //unregister this UIDrawer drawerName from the DrawerDatabase
        }

        private void Update()
        {
            if(Closed) { canBeDragged = true; }

            UpdateArrow();

            UpdateContainerVelocity();

            UpdateContainerAnimation();

            if(!detectGestures) { return; } //this UIDrawer's gesture detectors are disabled -> return

            if(TouchManager.TouchInProgress) //is there a touch in progress?
            {
                if(TouchManager.CurrentTouchInfo.IsDragging) //is the TouchManager dragging anything?
                {
                    if(TouchManager.CurrentTouchInfo.draggedObject != gameObject) //this is NOT the dragged object -> return
                    {
                        return;
                    }

                    UpdateContainerDraggedPosition();

                    UpdateVisibility(); //since this drawer is being dragged -> update its visibility value (how much of the drawer is shown on the screen); the visibility is also used to set the alpa values

                    if(TouchManager.CurrentTouchInfo.touch.phase == TouchPhase.Ended || TouchManager.CurrentTouchInfo.touch.phase == TouchPhase.Canceled) //did the user end or cancel the touch?
                    {
                        OnDrawerEndDrag.Invoke();

                        if(Opened) //is this drawer Opened?
                        {
                            if(visibility < AUTO_CLOSE_IF_DRAGGED_UNDER_VISIBLITY_PERCENT //if the drawer was dragged a distance long enough -> close it
                               || Mathf.Abs(TouchManager.CurrentTouchInfo.velocity.magnitude) > AUTO_OPEN_OR_CLOSE_TERMINAL_SWIPE_VELOCITY) //if the swipe speed (velocity) was greater than the set TERMINAL_SWIPE_VELOCITY -> close it
                            {
                                Close();
                            }
                            else //otherwise -> open the drawer back
                            {
                                Open();
                            }
                        }
                        else if(Closed) //is this drawer Closed?
                        {
                            if(visibility > AUTO_OPEN_IF_DRAGGED_OVER_VISIBILITY_PERCENT //if the drawer was dragged a distance long enough -> open it
                               || Mathf.Abs(TouchManager.CurrentTouchInfo.velocity.magnitude) > AUTO_OPEN_OR_CLOSE_TERMINAL_SWIPE_VELOCITY) //if the swipe speed (velocity) was greater than the set TERMINAL_SWIPE_VELOCITY -> open it
                            {
                                Open();
                            }
                            else //otherwise -> close the drawer back
                            {
                                Close();
                            }
                        }

                        DraggedDrawer = null; //clear the DraggedDrawer slot
                        IsDragged = false;
                        TouchManager.SetDraggedObject(null); //the touch ended (or was canceled) -> tell the TouchManager that it is not dragging anything at the moment
                    }
                }
                else
                {
                    if(EventSystem.current.currentSelectedGameObject != null) //if the touch is over an interractable UI component -> return
                    {
                        return;
                    }

                    if(!canBeDragged) //if this drawer cannot be dragged -> return
                    {
                        return;
                    }

                    if(OpenedDrawer == this) //is this the opened drawer
                    {
                        if(Opened //sanity check
                           && TouchManager.GetSimpleSwipe(TouchManager.CurrentTouchInfo.direction) == drawerCloseDirection) //check the the used executed the correct swipe (in the expected direction) to start closing this drawer
                        {
                            DraggedDrawer = this; //set this as the DraggedDrawer
                            IsDragged = true;
                            TouchManager.SetDraggedObject(gameObject); //set this as the dragged object in the TouchManager
                            dragStartPosition = ScaledTouchPosition(TouchManager.CurrentTouchInfo.touch.position); //save the start position of the drag
                            OnDrawerBeginDrag.Invoke();
                        }
                    }
                    else if(!IsAnyDrawerOpened) //if no drawer is opened
                    {
                        if(Closed  //make sure that this drawer is closed and listenting for the correct swipe
                           && TouchManager.GetSimpleSwipe(TouchManager.CurrentTouchInfo.direction, true) == drawerCloseDirection) //check the the used executed the correct swipe (in the expected direction) to start opening this drawer
                        {
                            DraggedDrawer = this; //set this as the DraggedDrawer
                            IsDragged = true;
                            TouchManager.SetDraggedObject(gameObject); //set this as the dragged object in the TouchManager
                            dragStartPosition = ScaledTouchPosition(TouchManager.CurrentTouchInfo.touch.position); //save the start position of the drag

                            container.gameObject.SetActive(true); //enable the container regardless of its state
                            ToggleContainerCanvasAndGraphicRaycaster(true); //enable the Canvas and GraphicRaycaster regardless of its state
                            if(HasOverlay) //check if this Drawer uses an overlay
                            {
                                overlay.gameObject.SetActive(true); //enable the overlay regardless of its state
                                ToggleOverlayCanvasAndGraphicRaycaster(true); //enable the Canvas and GraphicRaycaster regardess of its state
                            }
                            OnDrawerBeginDrag.Invoke();
                        }
                    }
                }
            }
        }

        private void UpdateArrow()
        {
            if(showArrow) //is the arrow enabled -> sync the arrowContainer to the drawer's container position
            {
                //arrowContainer.anchoredPosition = container.anchoredPosition;
                UpdateArrowContainer();
                arrow.UpdateArrowColor(this);

                if(DraggedDrawer == null && OpenedDrawer == null)
                {
                    arrow.UpdateRotatorPosition(1);
                }
                else if(DraggedDrawer != null)
                {
                    if(DraggedDrawer != this)
                    {
                        arrow.UpdateRotatorPosition(1 - DraggedDrawer.Visibility);
                    }
                    else
                    {
                        arrow.UpdateRotatorPosition(1 - Visibility, true);
                    }
                }
                else if(OpenedDrawer != null)
                {
                    if(OpenedDrawer != this)
                    {
                        arrow.UpdateRotatorPosition(1 - OpenedDrawer.Visibility);
                    }
                    else
                    {
                        arrow.UpdateRotatorPosition(1 - Visibility, true);
                    }
                }
            }
        }
        private void UpdateContainerAnimation()
        {
            if(currentDrawerState == DrawerState.IsOpening) //if the drawer IsOpening -> execute the open tween
            {
                container.anchoredPosition3D = Vector3.LerpUnclamped(container.anchoredPosition3D, containerOpenedAnchoredPosition, Time.deltaTime * openSpeed); //open tween
                UpdateVisibility();
                if(visibility >= 0.995) { FinalizeOpen(); } //if it's almost open -> snap to opened state by executing FinalizeOpen; this snap is just a design choice (it's the best out of all the options we've tested)
            }
            else if(currentDrawerState == DrawerState.IsClosing) //if the drawer IsClosing -> execute the close tween
            {
                container.anchoredPosition3D = Vector3.LerpUnclamped(container.anchoredPosition3D, containerClosedAnchoredPosition, Time.deltaTime * closeSpeed); //close tween
                UpdateVisibility();
                if(visibility <= 0.005) { FinalizeClose(); } //if it's almost closed -> snap to closed state by executing FinalizeClose; this snap is just a design choice (it's the bese out of all the options we've tested)
            }
        }
        private void UpdateContainerVelocity()
        {
            containerPreviousAnchoredPosition = containerCurrentAnchoredPosition; //update the container's previous position; this is used to calculate the ContainerVelocity (its speed)
            containerCurrentAnchoredPosition = container.anchoredPosition3D; //update the container's current position; this is used to calculate the ContainerVelocity (its speed)
        }
        private void UpdateContainerDraggedPosition()
        {
            if(drawerCloseDirection == SimpleSwipe.Left || drawerCloseDirection == SimpleSwipe.Right) //does this drawer close to the Left or the Right of the screen?
            {
                if(Opened) //is the drawer opened?
                {
                    container.anchoredPosition3D = new Vector3(containerOpenedAnchoredPosition.x + ScaledPositionX(TouchManager.CurrentTouchInfo.touch.position.x) - dragStartPosition.x, //calculate the X 'dragged' position
                                                               container.anchoredPosition3D.y,
                                                               container.anchoredPosition3D.z);
                }
                else if(Closed) //is the drawer closed?
                {
                    container.anchoredPosition3D = new Vector3(containerClosedAnchoredPosition.x + ScaledPositionX(TouchManager.CurrentTouchInfo.touch.position.x) - dragStartPosition.x, //calculate the X 'dragged' position
                                                               container.anchoredPosition3D.y,
                                                               container.anchoredPosition3D.z);
                }

                if(drawerCloseDirection == SimpleSwipe.Left) //does this drawer close to the Left -> clamp its position accordingly
                {
                    container.anchoredPosition3D = new Vector3(Mathf.Clamp(container.anchoredPosition3D.x, containerClosedAnchoredPosition.x, containerOpenedAnchoredPosition.x), //clamp X position
                                                               container.anchoredPosition3D.y,
                                                               container.anchoredPosition3D.z);
                }
                else //this drawer closes to the Right -> clamp its position accordingly
                {
                    container.anchoredPosition3D = new Vector3(Mathf.Clamp(container.anchoredPosition3D.x, containerOpenedAnchoredPosition.x, containerClosedAnchoredPosition.x), //clamp X position
                                                               container.anchoredPosition3D.y,
                                                               container.anchoredPosition3D.z);
                }
            }
            else if(drawerCloseDirection == SimpleSwipe.Up || drawerCloseDirection == SimpleSwipe.Down) //does this drawer close Up or Down
            {
                if(Opened) //is the drawer opened?
                {
                    container.anchoredPosition3D = new Vector3(container.anchoredPosition3D.x,
                                                               containerOpenedAnchoredPosition.y + ScaledPositionY(TouchManager.CurrentTouchInfo.touch.position.y) - dragStartPosition.y, //calculate the Y 'dragged' position
                                                               container.anchoredPosition3D.z);
                }
                else if(Closed) //is the drawer closed?
                {
                    container.anchoredPosition3D = new Vector3(container.anchoredPosition3D.x,
                                                               containerClosedAnchoredPosition.y + ScaledPositionY(TouchManager.CurrentTouchInfo.touch.position.y) - dragStartPosition.y, //calculate the Y 'dragged' position
                                                               container.anchoredPosition3D.z);
                }

                if(drawerCloseDirection == SimpleSwipe.Up) //does this drawer close Up -> clamp its position accordingly
                {
                    container.anchoredPosition3D = new Vector3(container.anchoredPosition3D.x,
                                                             Mathf.Clamp(container.anchoredPosition3D.y, containerOpenedAnchoredPosition.y, containerClosedAnchoredPosition.y), //clamp Y position
                                                             container.anchoredPosition3D.z);
                }
                else //this drawer closes Down -> clamp its position accordingly
                {
                    container.anchoredPosition3D = new Vector3(container.anchoredPosition3D.x,
                                                               Mathf.Clamp(container.anchoredPosition3D.y, containerClosedAnchoredPosition.y, containerOpenedAnchoredPosition.y), //clamp Y position
                                                               container.anchoredPosition3D.z);
                }
            }
        }
        private void UpdateVisibility()
        {
            //(currentX - minX) / (maxX - minX)

            switch(drawerCloseDirection)
            {
                case SimpleSwipe.Left:
                    visibility = (container.anchoredPosition3D.x - containerClosedAnchoredPosition.x) / (containerOpenedAnchoredPosition.x - containerClosedAnchoredPosition.x);
                    break;
                case SimpleSwipe.Right:
                    visibility = (container.anchoredPosition3D.x - containerClosedAnchoredPosition.x) / (containerOpenedAnchoredPosition.x - containerClosedAnchoredPosition.x);
                    break;
                case SimpleSwipe.Up:
                    visibility = (container.anchoredPosition3D.y - containerClosedAnchoredPosition.y) / (containerOpenedAnchoredPosition.y - containerClosedAnchoredPosition.y);
                    break;
                case SimpleSwipe.Down:
                    visibility = (container.anchoredPosition3D.y - containerClosedAnchoredPosition.y) / (containerOpenedAnchoredPosition.y - containerClosedAnchoredPosition.y);
                    break;
            }

            if(HasOverlay)
            {
                OverlayCanvasGroup.alpha = visibility;
            }
        }

        private void LateUpdate()
        {
            if(IsOpening || IsClosing) { UpdateVisibility(); } //if the drawer is opening or closing the alpha value needs to be updated
            if(fadeOutContainerWhenClosed) { ContainerCanvasGroup.alpha = visibility; } //if fadeOutContainerWhenClosed is set to true -> fade out the entire container by updating the visibility (alpha value)
        }

        private void OnRectTransformDimensionsChange()
        {
            if(container == null) { return; } //if there is no container referenced this UIDrawer needs to get disabled; this line prevents any errors in the console (a debug log is already provided in Awake)

            UpdateContainer();

            InitializeContainerPositions(); //orentation just changed -> the drawer size and positions need to be recalculated

            if(Opened) { container.anchoredPosition3D = containerOpenedAnchoredPosition; } //if the drawer is opened on orientation change -> adjust its position
            else if(Closed) { container.anchoredPosition3D = containerClosedAnchoredPosition; } //if the drawer is closed on orientation change -> adjust its position

            UpdateArrowContainer();
        }

        /// <summary>
        /// Opens the drawer.
        /// </summary>
        /// <param name="instantAction">If set to <c>true</c> it will open the drawer in zero seconds.</param>
        public void Open(bool instantAction = false)
        {
            if(Opened && !IsDragged) { return; }

            if(debugDrawer) { Debug.Log("[UIDrawer] '" + drawerName + "' - Opened " + (instantAction ? "in zero seconds!" : "with animation."), gameObject); } //if debugDrawer is enabled -> print debug log 


            if(IsAnyDrawerOpened && !Opened)
            {
                OpenedDrawer.Close(true);
            }

            InitiateOpen(); //prepare the drawer to be opened

            if(instantAction) //is it an instant action (skip the animation) ?
            {
                FinalizeOpen(); //finalize the drawer settings for the opened position
            }
            else
            {
                currentDrawerState = DrawerState.IsOpening; //drawer needs to be animated -> activate the animation in the Update method
                OnDrawerIsOpening.Invoke(); //invoke the proper UnityEvent
            }
        }
        /// <summary>
        /// Initiates the open setup.
        /// </summary>
        private void InitiateOpen()
        {
            OpenedDrawer = this; //set this as the opened drawer
            canBeDragged = false; //set this drawer as not draggable (we need to validate the drag)

            container.gameObject.SetActive(true); //enable the container (it might be disabled)
            ToggleContainerCanvasAndGraphicRaycaster(true); //enable the canvas and graphic raycaster (they should have gotten disabled by default, when the drawer closed)
            if(HasOverlay) //check if the drawer has an overlay referenced
            {
                overlay.gameObject.SetActive(true); //enable the overlay (it might be disabled)
                ToggleOverlayCanvasAndGraphicRaycaster(true); //enable the canvas and graphic raycaster (they should have gotten disabled by default, when the drawer closed)
            }
        }
        /// <summary>
        /// Finalizes the open settings.
        /// </summary>
        private void FinalizeOpen()
        {
            container.anchoredPosition3D = containerOpenedAnchoredPosition; //snap to the opened position
            currentDrawerState = DrawerState.Opened; //set the drawer state to Opened
            visibility = 1; //set the visibility to 1 (just in case)
            if(HasOverlay) //check if the drawer has an overlay referenced
            {
                OverlayCanvasGroup.alpha = visibility; //set the overlay visibility to 1 because the drawer is opened and the overlay should be visible
            }

            OnDrawerOpened.Invoke(); //invoke the proper UnityEvent
        }

        /// <summary>
        /// Closes the drawer.
        /// </summary>
        /// <param name="instantAction">If set to <c>true</c> it will close the drawer in zero seconds.</param>
        public void Close(bool instantAction = false)
        {
            if(Closed && !IsDragged) { return; }

            if(debugDrawer) { Debug.Log("[UIDrawer] '" + drawerName + "' - Closed " + (instantAction ? "in zero seconds!" : "with animation."), gameObject); } //if debugDrawer is enabled -> print debug log 
            InitiateClose(); //prepare the drawer to be closed
            if(instantAction) //is it an instant action (skip the animation) ?
            {
                FinalizeClose(); //finalize the drawer settings for the closed position
            }
            else
            {
                currentDrawerState = DrawerState.IsClosing; //drawer needs to be animated -> activate the animation in the Update method
                OnDrawerIsClosing.Invoke(); //invoke the proper UnityEvent
            }
        }
        /// <summary>
        /// Initiates the close setup.
        /// </summary>
        private void InitiateClose()
        {
        }
        /// <summary>
        /// Finalizes the close settings.
        /// </summary>
        private void FinalizeClose()
        {
            OpenedDrawer = null; //clear the opened drawer slot (make way for another one)
            canBeDragged = true; //set this drawer as draggable

            container.anchoredPosition3D = containerClosedAnchoredPosition; //snap to the closed position
            currentDrawerState = DrawerState.Closed; //set the drawer state to Closed
            visibility = 0; //set the visibility to o (just in case)
            if(disableContainerWhenClosed) { container.gameObject.SetActive(false); } //disable the container gameObject if needed
            if(!dontDisableContainerCanvasWhenClosed) { ToggleContainerCanvasAndGraphicRaycaster(false); } //disable the Canvas and GraphicRaycaster of the container
            if(HasOverlay) //check if the drawer has an overlay referenced
            {
                OverlayCanvasGroup.alpha = visibility; //set the overlay visibility to 0 because the drawer is closed and the overlay should not be visible
                if(disableOverlayWhenClosed) { overlay.gameObject.SetActive(false); } //disable the overlay gameObejct if needed
                if(!dontDisableOverlayCanvasWhenClosed) { ToggleOverlayCanvasAndGraphicRaycaster(false); } //disable the Canvas and GraphicRaycaster of the overlay
            }

            OnDrawerClosed.Invoke(); //invoke the proper UnityEvent
        }

        /// <summary>
        /// Toggles the drawer's state. If it's open it will close and vice versa.
        /// </summary>
        /// <param name="instantAction">If set to <c>true</c> it will close or open the drawer in zero seconds.</param>
        public void Toggle(bool instantAction = false)
        {
            if(Opened || IsOpening)
            {
                Close(instantAction);
                return;
            }

            if(Closed || IsClosing)
            {
                Open(instantAction);
                return;
            }
        }

        /// <summary>
        /// Disables Gesture Detection for this UIDrawer.
        /// </summary>
        public void DisableGestureDetection()
        {
            detectGestures = false;
        }
        /// <summary>
        /// Enables Gesture Detection for this UIDrawer.
        /// </summary>
        public void EnableGestureDetection()
        {
            detectGestures = true;
        }
        /// <summary>
        /// Toggles Gesture Detection for this UIDrawer.
        /// </summary>
        public void ToggleGestureDetection()
        {
            detectGestures = !detectGestures;
        }
        /// <summary>
        /// Toggles Gesture Detection for this UIDrawer, to the set state.
        /// </summary>
        /// <param name="enabled">If true, Gesture Detection for this UIDrawer will be enabled. If false, Gesture Detection for this UIDrawer will be disabled.</param>
        public void ToggleGestureDetection(bool enabled)
        {
            detectGestures = enabled;
        }

        /// <summary>
        /// Internal method that is used to disable the Canvas and the GraphicRaycaster when the UIDrawer is closed and to enable them when the UIDrawer is opened.
        /// This manages the draw calls without setting the gameObject's active state to false.
        /// </summary>
        /// <param name="isEnabled"></param>
        void ToggleContainerCanvasAndGraphicRaycaster(bool isEnabled)
        {
            ContainerCanvas.enabled = isEnabled;
            ContainerGraphicRaycaster.enabled = isEnabled;
        }
        /// <summary>
        /// Internal method that is used to disable the Canvas and the GraphicRaycaster when the UIDrawer is closed and to enable them when the UIDrawer is opened.
        /// This manages the draw calls without setting the gameObject's active state to false.
        /// </summary>
        /// <param name="isEnabled"></param>
        void ToggleOverlayCanvasAndGraphicRaycaster(bool isEnabled)
        {
            OverlayCanvas.enabled = isEnabled;
            OverlayGraphicRaycaster.enabled = isEnabled;
        }

        /// <summary>
        /// Initializes the container positions.
        /// </summary>
        private void InitializeContainerPositions()
        {
            containerOpenedAnchoredPosition = startAnchoredPosition;
            containerClosedAnchoredPosition = GetContainerClosedPosition();

            containerCurrentAnchoredPosition = container.anchoredPosition3D;
            containerPreviousAnchoredPosition = containerCurrentAnchoredPosition;
        }
        /// <summary>
        /// Gets the container closed position.
        /// </summary>
        private Vector3 GetContainerClosedPosition()
        {
            float xOffset = container.rect.width;
            float yOffset = container.rect.height;

            switch(drawerCloseDirection)
            {
                case SimpleSwipe.Left: return new Vector3(containerOpenedAnchoredPosition.x - xOffset, containerOpenedAnchoredPosition.y, containerOpenedAnchoredPosition.z);
                case SimpleSwipe.Right: return new Vector3(containerOpenedAnchoredPosition.x + xOffset, containerOpenedAnchoredPosition.y, containerOpenedAnchoredPosition.z);
                case SimpleSwipe.Up: return new Vector3(containerOpenedAnchoredPosition.x, containerOpenedAnchoredPosition.y + yOffset, containerOpenedAnchoredPosition.z);
                case SimpleSwipe.Down: return new Vector3(containerOpenedAnchoredPosition.x, containerOpenedAnchoredPosition.y - yOffset, containerOpenedAnchoredPosition.z);
                default: return Vector3.zero;
            }
        }

        /// <summary>
        /// Initializes the drawer's arrow.
        /// </summary>
        private void InitializeArrow()
        {
            if(arrowContainer == null)
            {
                showArrow = false;
                return;
            }

            UpdateArrowContainer();

            if(arrow != null)
            {
                arrow.gameObject.SetActive(showArrow);
                if(showArrow)
                {
                    arrow.SetTargetDrawer(this);
                }
            }
        }

        /// <summary>
        /// Updates the arrow container, copying the container's RectTransfrom properties to itself.
        /// </summary>
        public void UpdateArrowContainer()
        {
            CopyRectTransform(container, arrowContainer);
        }

        /// <summary>
        /// Copies a RectTransform properties from the source to the target.
        /// </summary>
        public void CopyRectTransform(RectTransform source, RectTransform target)
        {
            target.localScale = source.localScale;
            target.anchorMin = source.anchorMin;
            target.anchorMax = source.anchorMax;
            target.pivot = source.pivot;
            target.sizeDelta = source.sizeDelta;
            target.anchoredPosition3D = source.anchoredPosition3D;
        }

        /// <summary>
        /// Initializes the drawer's drawerName. If no drawerName has been set then the drawerName will be {drawerCloseDirection + "Drawer"}. So it can be {LeftDrawer}, {RightDrawer}, {UpDrawer}, {DownDrawer}.
        /// Note: in order to have more than one left drawer, for example, you need to set different drawerNames to them and to manage them separately.
        /// </summary>
        private void InitializeDrawerName()
        {
            drawerName = GetDrawerName();
        }

        /// <summary>
        /// Gets the name of the drawer. If the drawer name was auto-generated it will retrun 'drawerCloseDirection.ToString() + "Drawer"', otherwise it will retrun the drawerName.
        /// This method is used by the editor.
        /// </summary>
        public string GetDrawerName()
        {
            return drawerName.Equals(DEFAULT_DRAWER_NAME) ? drawerCloseDirection.ToString() + "Drawer" : drawerName;
        }

        /// <summary>
        /// Returns the scaled X value, taking into account the changes made by the CanvasScaler on the rootCanvas
        /// </summary>
        private float ScaledPositionX(float x)
        {
            return (x / Canvas.pixelRect.width) * RectTransform.rect.width;
        }
        /// <summary>
        /// Returns the scaled Y value, taking into account the changes made by the CanvasScaler on the rootCanvas
        /// </summary>
        private float ScaledPositionY(float y)
        {
            return (y / Canvas.pixelRect.height) * RectTransform.rect.height;
        }
        /// <summary>
        /// Returns the adjusted touch position, taking into account the changes made by the CanvasScaler on the rootCanvas
        /// </summary>
        private Vector2 ScaledTouchPosition(Vector2 touchPosition)
        {
            return new Vector2(ScaledPositionX(touchPosition.x), ScaledPositionY(touchPosition.y));
        }

        /// <summary>
        /// Updates the drawer's close direction
        /// </summary>
        /// <param name="closeDirection">This is the direction the drawer will slide to in order to close.</param>
        public void UpdateDrawerCloseDirection(SimpleSwipe closeDirection)
        {
            if(!IsAnyDrawerOpened) { OpenedDrawer.Close(true); }
            Open(true);
            drawerCloseDirection = closeDirection;
            InitializeContainerPositions();
            InitializeArrow();
            UnregisterFromDrawerDatabase();
            InitializeDrawerName();
            RegisterToDrawerDatabase();
            Close(true);
        }

        /// <summary>
        /// Renames the UIDrawer and also re-registers it to the DrawersDatabase.
        /// </summary>
        /// <param name="newDrawerName">The new drawerName for this drawer.</param>
        public void RenameDrawer(string newDrawerName)
        {
            UnregisterFromDrawerDatabase();
            drawerName = newDrawerName;
            InitializeDrawerName();
            RegisterToDrawerDatabase();
        }

        /// <summary>
        /// Updates the size of the container. Use this method only to set it to FullScreen.
        /// </summary>
        public void UpdateContainerSize()
        {
            UpdateContainerSize(ContainerSize.FullScreen, containerPercentageOfScreenSize, containerMinimumSize, containerFixedSize);
        }
        /// <summary>
        /// Updates the size of the container. Use this method only to set it to PercentageOfScreen.
        /// </summary>
        /// <param name="percentageOfScreen">The percentage of screen.</param>
        /// <param name="minimumSize">The minimum size.</param>
        /// <param name="maximumSize">The maximum size.</param>
        public void UpdateContainerSize(float percentageOfScreen, float minimumSize)
        {
            UpdateContainerSize(ContainerSize.PercentageOfScreen, percentageOfScreen, minimumSize, containerFixedSize);
        }
        /// <summary>
        /// Updates the size of the container. Use this method only to set it to FixedSize.
        /// </summary>
        /// <param name="fixedSize">Fixed Size of the container.</param>
        public void UpdateContainerSize(float fixedSize)
        {
            UpdateContainerSize(ContainerSize.FixedSize, containerPercentageOfScreenSize, containerMinimumSize, fixedSize);
        }
        /// <summary>
        /// Updates the size of the container. Do not use this method unless you know what you are doing. Use the helper methods instead.
        /// </summary>
        private void UpdateContainerSize(ContainerSize size, float percentageOfScreen, float minimumSize, float fixedSize)
        {
            CopyRectTransform(RectTransform, container);

            container.localScale = Vector3.one;

            if(size == ContainerSize.FullScreen) //do nothing as the copy method above made it full screen already
            {
                container.anchorMin = new Vector2(0, 0);
                container.anchorMax = new Vector2(1, 1);
                container.pivot = new Vector2(0.5f, 0.5f);
                container.sizeDelta = new Vector2(0, 0);
                container.anchoredPosition = new Vector2(0, 0);
                return;
            }

            percentageOfScreen = Mathf.Clamp(percentageOfScreen, 0, 1); //clamp to sane values
            minimumSize = Mathf.Abs(minimumSize); //this should not be negative
            fixedSize = Mathf.Abs(fixedSize); //this should not be negative

            scaledCanvas = new Vector2(RectTransform.rect.width, RectTransform.rect.height);
            containerCalculatedSize = scaledCanvas;

            switch(drawerCloseDirection)
            {
                case SimpleSwipe.Left:
                    if(size == ContainerSize.PercentageOfScreen)
                    {
                        minimumSize = Mathf.Clamp(minimumSize, 0, scaledCanvas.x);
                        containerCalculatedSize.x *= percentageOfScreen;
                        containerCalculatedSize.x = Mathf.Clamp(containerCalculatedSize.x, minimumSize, scaledCanvas.x);
                    }
                    else if(size == ContainerSize.FixedSize)
                    {
                        containerCalculatedSize.x = Mathf.Clamp(fixedSize, 0, scaledCanvas.x);
                    }

                    container.anchorMin = new Vector2(0, 0);
                    container.anchorMax = new Vector2(0, 1);
                    container.pivot = new Vector2(0, 0.5f);
                    container.sizeDelta = new Vector2(containerCalculatedSize.x, 0);
                    break;

                case SimpleSwipe.Right:
                    if(size == ContainerSize.PercentageOfScreen)
                    {
                        minimumSize = Mathf.Clamp(minimumSize, 0, scaledCanvas.x);
                        containerCalculatedSize.x *= percentageOfScreen;
                        containerCalculatedSize.x = Mathf.Clamp(containerCalculatedSize.x, minimumSize, scaledCanvas.x);
                    }
                    else if(size == ContainerSize.FixedSize)
                    {
                        containerCalculatedSize.x = Mathf.Clamp(fixedSize, 0, scaledCanvas.x);
                    }

                    container.anchorMin = new Vector2(1, 0);
                    container.anchorMax = new Vector2(1, 1);
                    container.pivot = new Vector2(1, 0.5f);
                    container.sizeDelta = new Vector2(containerCalculatedSize.x, 0);
                    break;

                case SimpleSwipe.Up:
                    if(size == ContainerSize.PercentageOfScreen)
                    {
                        minimumSize = Mathf.Clamp(minimumSize, 0, scaledCanvas.y);
                        containerCalculatedSize.y *= percentageOfScreen;
                        containerCalculatedSize.y = Mathf.Clamp(containerCalculatedSize.y, minimumSize, scaledCanvas.y);
                    }
                    else if(size == ContainerSize.FixedSize)
                    {
                        containerCalculatedSize.y = Mathf.Clamp(fixedSize, 0, scaledCanvas.y);
                    }

                    container.anchorMin = new Vector2(0, 1);
                    container.anchorMax = new Vector2(1, 1);
                    container.pivot = new Vector2(0.5f, 1);
                    container.sizeDelta = new Vector2(0, containerCalculatedSize.y);
                    break;

                case SimpleSwipe.Down:
                    if(size == ContainerSize.PercentageOfScreen)
                    {
                        minimumSize = Mathf.Clamp(minimumSize, 0, scaledCanvas.y);
                        containerCalculatedSize.y *= percentageOfScreen;
                        containerCalculatedSize.y = Mathf.Clamp(containerCalculatedSize.y, minimumSize, scaledCanvas.y);
                    }
                    else if(size == ContainerSize.FixedSize)
                    {
                        containerCalculatedSize.y = Mathf.Clamp(fixedSize, 0, scaledCanvas.y);
                    }
                    container.anchorMin = new Vector2(0, 0);
                    container.anchorMax = new Vector2(1, 0);
                    container.pivot = new Vector2(0.5f, 0);
                    container.sizeDelta = new Vector2(0, containerCalculatedSize.y);
                    break;
            }

            container.anchoredPosition3D = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Updates the container to the set settings. This method is mostly used by the editor button to help setup and design the container.
        /// </summary>
        public void UpdateContainer()
        {
            RectTransform.anchoredPosition3D = startAnchoredPosition;
            UpdateContainerSize(containerSize, containerPercentageOfScreenSize, containerMinimumSize, containerFixedSize);

        }

        /// <summary>
        /// Registers the drawerName to the DrawerDatabase.
        /// </summary>
        private void RegisterToDrawerDatabase()
        {
            if(DrawerDatabase.ContainsKey(drawerName))
            {
                Debug.LogError("[UIDrawer] Unable to register the '" + drawerName + "' drawerName to the DrawerDatabase because another UIDrwarer, with the same drawer name, has already been regsitered.", gameObject);
                return;
            }
            DrawerDatabase.Add(drawerName, this);
        }
        /// <summary>
        /// Unregisters the drawerName from DrawerDatabase.
        /// </summary>
        private void UnregisterFromDrawerDatabase()
        {
            if(!DrawerDatabase.ContainsKey(drawerName))
            {
                return;
            }
            DrawerDatabase.Remove(drawerName);
        }

        private void OnDrawerOpenedEvent() { Debug.Log("[UIDrawer] '" + drawerName + "' - Invoked UnityEvent - OnDrawerOpened", gameObject); }
        private void OnDrawerIsOpeningEvent() { Debug.Log("[UIDrawer] '" + drawerName + "' - Invoked UnityEvent - OnDrawerIsOpening", gameObject); }
        private void OnDrawerClosedEvent() { Debug.Log("[UIDrawer] '" + drawerName + "' - Invoked UnityEvent - OnDrawerClosed", gameObject); }
        private void OnDrawerIsClosingEvent() { Debug.Log("[UIDrawer] '" + drawerName + "' - Invoked UnityEvent - OnDrawerIsClosing", gameObject); }
        private void OnDrawerBeginDragEvent() { Debug.Log("[UIDrawer] '" + drawerName + "' - Invoked UnityEvent - OnDrawerBeginDrag", gameObject); }
        private void OnDrawerEndDragEvent() { Debug.Log("[UIDrawer] '" + drawerName + "' - Invoked UnityEvent - OnDrawerEndDrag", gameObject); }

        public void OnDrag(PointerEventData eventData) { canBeDragged = true; }
        public void OnBeginDrag(PointerEventData eventData) { canBeDragged = true; }
        public void OnEndDrag(PointerEventData eventData) { canBeDragged = false; }

        /// <summary>
        /// Opens the specified drawer name.
        /// </summary>
        /// <param name="drawerName">Name of the drawer.</param>
        /// <param name="debug">if set to <c>true</c> [debug].</param>
        public static void Open(string drawerName, bool debug = false)
        {
            if(!DrawerDatabase.ContainsKey(drawerName))
            {
                if(debug) { Debug.LogError("[UIDrawer] Unable to open the '" + drawerName + "' drawerName because it was not found in the DrawerDatabase."); }
                return;
            }

            if(IsAnyDrawerOpened)
            {
                OpenedDrawer.Close(true);
            }

            DrawerDatabase[drawerName].Open();
        }
        /// <summary>
        /// Closes the specified drawer name.
        /// </summary>
        /// <param name="drawerName">Name of the drawer.</param>
        /// <param name="debug">if set to <c>true</c> [debug].</param>
        public static void Close(string drawerName, bool debug = false)
        {
            if(!DrawerDatabase.ContainsKey(drawerName))
            {
                if(debug) { Debug.LogError("[UIDrawer] Unable to close the '" + drawerName + "' drawerName because it was not found in the DrawerDatabase."); }
                return;
            }

            if(IsAnyDrawerOpened && OpenedDrawer == DrawerDatabase[drawerName])
            {
                OpenedDrawer = null;
            }

            DrawerDatabase[drawerName].Close();
        }
        /// <summary>
        /// Toggles the state of the specified drawer name. If it's open it will close and vice versa.
        /// </summary>
        /// <param name="drawerName">Name of the drawer.</param>
        /// <param name="debug">if set to <c>true</c> [debug].</param>
        public static void Toggle(string drawerName, bool debug = false)
        {
            if(!DrawerDatabase.ContainsKey(drawerName))
            {
                if(debug) { Debug.LogError("[UIDrawer] Unable to toggle the '" + drawerName + "' drawerName because it was not found in the DrawerDatabase."); }
                return;
            }

            DrawerDatabase[drawerName].Toggle();
        }
        /// <summary>
        /// Returns the UIDrawer reference to the drawer registered under the given drawerName. If no drawer is found, it will return null.
        /// </summary>
        /// <param name="drawerName">Name of the drawer.</param>
        /// <param name="debug">if set to <c>true</c> [debug].</param>
        public static UIDrawer GetDrawer(string drawerName, bool debug = false)
        {
            if(!DrawerDatabase.ContainsKey(drawerName))
            {
                if(debug) { Debug.LogError("[UIDrawer] Unable to retrieve the UIDrawer with the '" + drawerName + "' drawerName because it was not found in the DrawerDatabase."); }
                return null;
            }
            return DrawerDatabase[drawerName];
        }
    }
}
