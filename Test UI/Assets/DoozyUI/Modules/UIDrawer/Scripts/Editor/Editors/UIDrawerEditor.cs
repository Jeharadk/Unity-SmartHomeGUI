// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using QuickEditor;
using QuickEngine.Extensions;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace DoozyUI
{
    [CustomEditor(typeof(UIDrawer), true)]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class UIDrawerEditor : QEditor
    {
        private static string _PATH = "";
        public static string PATH
        {
            get
            {
                if(_PATH.IsNullOrEmpty())
                {
                    _PATH = QuickEngine.IO.File.GetRelativeDirectoryPath("UIDrawer");
                }
                return _PATH;
            }
        }

        private static string _IMAGES;
        public static string IMAGES { get { if(string.IsNullOrEmpty(_IMAGES)) { _IMAGES = PATH + "/Images/"; } return _IMAGES; } }

        public static QTexture headerUIDrawer;

        UIDrawer Drawer { get { return (UIDrawer)target; } }

        SerializedProperty
            debugDrawer,
            debugEvents,

            drawerName,
            drawerCloseDirection,

            openSpeed,
            closeSpeed,

            detectGestures,

            useCustomStartAnchoredPosition,
            customStartAnchoredPosition,

            OnDrawerOpened,
            OnDrawerClosed,
            OnDrawerIsOpening,
            OnDrawerIsClosing,
            OnDrawerBeginDrag,
            OnDrawerEndDrag,

            container,
            fadeOutContainerWhenClosed,
            disableContainerWhenClosed,
            dontDisableContainerCanvasWhenClosed,
            containerSize,
            containerPercentageOfScreenSize,
            containerMinimumSize,
            containerFixedSize,

            overlay,
            disableOverlayWhenClosed,
            dontDisableOverlayCanvasWhenClosed,

            showArrow,
            arrowScale,
            overrideArrowColor,
            arrowColorWhenClosed,
            arrowColorWhenOpened,
            arrow,
            arrowContainer,

            leftDrawerArrowHolder,
            leftDrawerArrowClosedPosition,
            leftDrawerArrowOpenedPosition,

            rightDrawerArrowHolder,
            rightDrawerArrowClosedPosition,
            rightDrawerArrowOpenedPosition,

            upDrawerArrowHolder,
            upDrawerArrowClosedPosition,
            upDrawerArrowOpenedPosition,

            downDrawerArrowHolder,
            downDrawerArrowClosedPosition,
            downDrawerArrowOpenedPosition;

        AnimBool
            showPlayModeSettings,
            showCustomStartPosition,
            showEvents,
            showArrowAnimBool,
            showArrowReferences;

        bool IsUsingOnDrawerOpened { get { return Drawer.OnDrawerOpened.GetPersistentEventCount() > 0; } }
        bool IsUsingOnDrawerClosed { get { return Drawer.OnDrawerClosed.GetPersistentEventCount() > 0; } }
        bool IsUsingOnDrawerIsOpening { get { return Drawer.OnDrawerIsOpening.GetPersistentEventCount() > 0; } }
        bool IsUsingOnDrawerIsClosing { get { return Drawer.OnDrawerIsClosing.GetPersistentEventCount() > 0; } }
        bool IsUsingOnDrawerBeginDrag { get { return Drawer.OnDrawerBeginDrag.GetPersistentEventCount() > 0; } }
        bool IsUsingOnDrawerEndDrag { get { return Drawer.OnDrawerEndDrag.GetPersistentEventCount() > 0; } }

        bool IsAnyEventUsed
        {
            get
            {
                if(IsUsingOnDrawerOpened) { return true; }
                if(IsUsingOnDrawerClosed) { return true; }
                if(IsUsingOnDrawerIsOpening) { return true; }
                if(IsUsingOnDrawerIsClosing) { return true; }
                if(IsUsingOnDrawerBeginDrag) { return true; }
                if(IsUsingOnDrawerEndDrag) { return true; }
                return false;
            }
        }

#if dUI_DoozyUI
        float GlobalWidth { get { return DUI.GLOBAL_EDITOR_WIDTH; } }
        int BarHeight { get { return DUI.BAR_HEIGHT; } }
        int MiniBarHeight { get { return DUI.MINI_BAR_HEIGHT; } }
#else
        float GlobalWidth { get { return 420; } }
        int BarHeight { get { return 20; } }
        int MiniBarHeight { get { return 18; } }
#endif

        float tempFloat = 0;

        protected override void SerializedObjectFindProperties()
        {
            base.SerializedObjectFindProperties();

            debugDrawer = serializedObject.FindProperty("debugDrawer");
            debugEvents = serializedObject.FindProperty("debugEvents");

            drawerName = serializedObject.FindProperty("drawerName");
            drawerCloseDirection = serializedObject.FindProperty("drawerCloseDirection");

            openSpeed = serializedObject.FindProperty("openSpeed");
            closeSpeed = serializedObject.FindProperty("closeSpeed");

            detectGestures = serializedObject.FindProperty("detectGestures");

            useCustomStartAnchoredPosition = serializedObject.FindProperty("useCustomStartAnchoredPosition");
            customStartAnchoredPosition = serializedObject.FindProperty("customStartAnchoredPosition");

            OnDrawerOpened = serializedObject.FindProperty("OnDrawerOpened");
            OnDrawerClosed = serializedObject.FindProperty("OnDrawerClosed");
            OnDrawerIsOpening = serializedObject.FindProperty("OnDrawerIsOpening");
            OnDrawerIsClosing = serializedObject.FindProperty("OnDrawerIsClosing");
            OnDrawerBeginDrag = serializedObject.FindProperty("OnDrawerBeginDrag");
            OnDrawerEndDrag = serializedObject.FindProperty("OnDrawerEndDrag");

            container = serializedObject.FindProperty("container");
            fadeOutContainerWhenClosed = serializedObject.FindProperty("fadeOutContainerWhenClosed");
            disableContainerWhenClosed = serializedObject.FindProperty("disableContainerWhenClosed");
            dontDisableContainerCanvasWhenClosed = serializedObject.FindProperty("dontDisableContainerCanvasWhenClosed");
            containerSize = serializedObject.FindProperty("containerSize");
            containerPercentageOfScreenSize = serializedObject.FindProperty("containerPercentageOfScreenSize");
            containerMinimumSize = serializedObject.FindProperty("containerMinimumSize");
            containerFixedSize = serializedObject.FindProperty("containerFixedSize");

            overlay = serializedObject.FindProperty("overlay");
            disableOverlayWhenClosed = serializedObject.FindProperty("disableOverlayWhenClosed");
            dontDisableOverlayCanvasWhenClosed = serializedObject.FindProperty("dontDisableOverlayCanvasWhenClosed");

            showArrow = serializedObject.FindProperty("showArrow");
            arrowScale = serializedObject.FindProperty("arrowScale");
            overrideArrowColor = serializedObject.FindProperty("overrideArrowColor");
            arrowColorWhenClosed = serializedObject.FindProperty("arrowColorWhenClosed");
            arrowColorWhenOpened = serializedObject.FindProperty("arrowColorWhenOpened");
            arrow = serializedObject.FindProperty("arrow");
            arrowContainer = serializedObject.FindProperty("arrowContainer");

            leftDrawerArrowHolder = serializedObject.FindProperty("leftDrawerArrowHolder");
            leftDrawerArrowClosedPosition = serializedObject.FindProperty("leftDrawerArrowClosedPosition");
            leftDrawerArrowOpenedPosition = serializedObject.FindProperty("leftDrawerArrowOpenedPosition");

            rightDrawerArrowHolder = serializedObject.FindProperty("rightDrawerArrowHolder");
            rightDrawerArrowClosedPosition = serializedObject.FindProperty("rightDrawerArrowClosedPosition");
            rightDrawerArrowOpenedPosition = serializedObject.FindProperty("rightDrawerArrowOpenedPosition");

            upDrawerArrowHolder = serializedObject.FindProperty("upDrawerArrowHolder");
            upDrawerArrowClosedPosition = serializedObject.FindProperty("upDrawerArrowClosedPosition");
            upDrawerArrowOpenedPosition = serializedObject.FindProperty("upDrawerArrowOpenedPosition");

            downDrawerArrowHolder = serializedObject.FindProperty("downDrawerArrowHolder");
            downDrawerArrowClosedPosition = serializedObject.FindProperty("downDrawerArrowClosedPosition");
            downDrawerArrowOpenedPosition = serializedObject.FindProperty("downDrawerArrowOpenedPosition");
        }

        protected override void GenerateInfoMessages()
        {
            base.GenerateInfoMessages();

            infoMessage.Add("AutoGeneratedDrawerName",
                            new InfoMessage
                            {
                                title = Drawer.GetDrawerName() + " - will be the auto-generated Drawer Name.",
                                message = "",
                                show = new AnimBool(false),
                                type = InfoMessageType.Info
                            });
        }

        protected override void InitAnimBools()
        {
            base.InitAnimBools();

            showPlayModeSettings = new AnimBool(false, Repaint);
            showCustomStartPosition = new AnimBool(useCustomStartAnchoredPosition.boolValue, Repaint);
            showEvents = new AnimBool(false, Repaint);
            showArrowAnimBool = new AnimBool(showArrow.boolValue, Repaint);
            showArrowReferences = new AnimBool(false, Repaint);
        }

        //---------- variables used by OnSceneGUI
#pragma warning disable 0414 //usused variable
        private RectTransform holder, closedArrow, openedArrow;
        private Vector3 arrowClosedPosition, arrowOpenedPosition;
        private Quaternion closedArrowHandleRotation, openedArrowHandleRotation;
        private float handleSize = 0.4f;
        private EventType handlesEventType = EventType.Repaint;
#pragma warning restore 0414 //usused variable
        //----------
        private void OnSceneGUI()
        {
            Handles.Label(Drawer.transform.position, "Close: " + Drawer.drawerCloseDirection.ToString());

            if(!showArrow.boolValue) { return; }

            switch(Drawer.drawerCloseDirection)
            {
                case Gestures.SimpleSwipe.Left:
                    holder = Drawer.leftDrawerArrowHolder;
                    closedArrow = Drawer.leftDrawerArrowClosedPosition;
                    openedArrow = Drawer.leftDrawerArrowOpenedPosition;

                    closedArrowHandleRotation = Quaternion.Euler(0, 90, 0);
                    openedArrowHandleRotation = Quaternion.Euler(0, -90, 0);
                    break;
                case Gestures.SimpleSwipe.Right:
                    holder = Drawer.rightDrawerArrowHolder;
                    closedArrow = Drawer.rightDrawerArrowClosedPosition;
                    openedArrow = Drawer.rightDrawerArrowOpenedPosition;

                    closedArrowHandleRotation = Quaternion.Euler(0, -90, 0);
                    openedArrowHandleRotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Gestures.SimpleSwipe.Up:
                    holder = Drawer.upDrawerArrowHolder;
                    closedArrow = Drawer.upDrawerArrowClosedPosition;
                    openedArrow = Drawer.upDrawerArrowOpenedPosition;

                    closedArrowHandleRotation = Quaternion.Euler(90, 90, 0);
                    openedArrowHandleRotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case Gestures.SimpleSwipe.Down:
                    holder = Drawer.downDrawerArrowHolder;
                    closedArrow = Drawer.downDrawerArrowClosedPosition;
                    openedArrow = Drawer.downDrawerArrowOpenedPosition;

                    closedArrowHandleRotation = Quaternion.Euler(-90, 90, 0);
                    openedArrowHandleRotation = Quaternion.Euler(90, -90, 0);
                    break;
            }

            if(Handles.Button(holder.position, holder.rotation, handleSize, handleSize, Handles.SphereHandleCap))
            {
                Selection.activeGameObject = holder.gameObject;
            }

            //Handles.ConeHandleCap(1, closedArrow.position, closedArrowHandleRotation, handleSize, handlesEventType);
            //Handles.ConeHandleCap(2, openedArrow.position, openedArrowHandleRotation, handleSize, handlesEventType);

            Handles.DrawDottedLine(holder.position, closedArrow.position, handleSize);
            Handles.DrawDottedLine(holder.position, openedArrow.position, handleSize);

            Handles.Label(closedArrow.position, "C");
            Handles.Label(openedArrow.position, "O");

            arrowClosedPosition = closedArrow.position;
            QUI.BeginChangeCheck();
            arrowClosedPosition = Handles.PositionHandle(arrowClosedPosition, Quaternion.identity);
            if(QUI.EndChangeCheck())
            {
                Undo.RecordObject(closedArrow, "Update Closed Arrow");
                closedArrow.position = arrowClosedPosition;
            }

            arrowOpenedPosition = openedArrow.position;
            QUI.BeginChangeCheck();
            arrowOpenedPosition = Handles.PositionHandle(arrowOpenedPosition, Quaternion.identity);
            if(QUI.EndChangeCheck())
            {
                Undo.RecordObject(openedArrow, "Update Opened Arrow");
                openedArrow.position = arrowOpenedPosition;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            requiresContantRepaint = true;

            headerUIDrawer = new QTexture(IMAGES, "headerUIDrawer" + QResources.IsProSkinTag);
        }

        public override void OnInspectorGUI()
        {
            DrawHeader(headerUIDrawer.normal, GlobalWidth, HEIGHT_42);
            if(IsEditorLocked) { return; }
            serializedObject.Update();
            DrawPlayModeSettings(GlobalWidth);
            DrawRenameGameObjectButton(GlobalWidth);
            QUI.Space(SPACE_8);
            DrawDebug(GlobalWidth);
            QUI.Space(SPACE_4);
            DrawDrawerNameAndDirection(GlobalWidth);
            DrawDrawerSpeed(GlobalWidth);
            DrawDetectGestures(GlobalWidth);
            DrawCustomStartPostion(GlobalWidth);
            QUI.Space(SPACE_2);
            DrawEvents(GlobalWidth);
            QUI.Space(SPACE_4);
            DrawContainer(GlobalWidth);
            QUI.Space(SPACE_4);
            DrawOverlay(GlobalWidth);
            QUI.Space(SPACE_4);
            DrawArrow(GlobalWidth);
            serializedObject.ApplyModifiedProperties();
            QUI.Space(SPACE_4);
        }

        void DrawPlayModeSettings(float width)
        {
            showPlayModeSettings.target = EditorApplication.isPlayingOrWillChangePlaymode;
            if(QUI.BeginFadeGroup(showPlayModeSettings.faded))
            {
                QUI.BeginVertical(width);
                {
                    QUI.Space(SPACE_4 * showPlayModeSettings.faded);

                    QUI.BeginHorizontal(width);
                    {
                        if(QUI.GhostButton("OPEN", QColors.Color.Blue, (width - SPACE_2) / 2, BarHeight))
                        {
                            Drawer.Open();
                        }

                        if(QUI.GhostButton("CLOSE", QColors.Color.Blue, (width - SPACE_2) / 2, BarHeight))
                        {
                            Drawer.Close();
                        }
                    }
                    QUI.EndHorizontal();

                    QUI.Space(SPACE_16 * showPlayModeSettings.faded);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        void DrawRenameGameObjectButton(float width)
        {
            QUI.BeginHorizontal(width);
            {
                if(QUI.GhostButton("Rename GameObject to Drawer Name", QColors.Color.Gray, width, 18))
                {
                    if(serializedObject.isEditingMultipleObjects)
                    {
                        Undo.RecordObjects(targets, "Renamed Multiple Objects");
                        for(int i = 0; i < targets.Length; i++)
                        {
                            UIDrawer uid = (UIDrawer)targets[i];
                            uid.gameObject.name = "UID - " + uid.GetDrawerName();
                        }
                    }
                    else
                    {
                        Undo.RecordObject(Drawer.gameObject, "Renamed GameObject");
                        Drawer.gameObject.name = "UID - " + Drawer.GetDrawerName();
                    }
                }
            }
            QUI.EndHorizontal();
        }
        void DrawDebug(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("Debug Drawer", debugDrawer);
                QUI.Space(SPACE_4);
                QUI.QToggle("Debug Events", debugEvents);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawDrawerNameAndDirection(float width)
        {
            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;
            QUI.BeginHorizontal(width);
            {
                QUI.QObjectPropertyField("Drawer Name", drawerName, 260, 20, false);
                QUI.Space(SPACE_4);
                QUI.QObjectPropertyField("Close Direction", drawerCloseDirection, width - 260 - 4, 20, false);
                if((Gestures.SimpleSwipe)drawerCloseDirection.enumValueIndex == Gestures.SimpleSwipe.None)
                {
                    drawerCloseDirection.enumValueIndex = (int)Gestures.SimpleSwipe.Left;
                }
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            GUI.enabled = true;
            infoMessage["AutoGeneratedDrawerName"].title = "'" + Drawer.GetDrawerName() + "' - will be the auto-generated Drawer Name.";
            infoMessage["AutoGeneratedDrawerName"].show.target = drawerName.stringValue.Equals(UIDrawer.DEFAULT_DRAWER_NAME);
            DrawInfoMessage("AutoGeneratedDrawerName", width);
            QUI.Space(SPACE_2 * infoMessage["AutoGeneratedDrawerName"].show.faded);
        }
        void DrawDrawerSpeed(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.QObjectPropertyField("Open Speed", openSpeed, width / 2 - 1, 20, false);
                QUI.Space(SPACE_2);
                QUI.QObjectPropertyField("Close Speed", closeSpeed, width / 2 - 1, 20, false);
            }
            QUI.EndHorizontal();
        }
        void DrawDetectGestures(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("Detect Gestures", detectGestures);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawCustomStartPostion(float width)
        {
            QUI.BeginHorizontal(width);
            {
                //CUSTOM START POSITION
                QLabel.text = "custom start position";
                QLabel.style = Style.Text.Normal;
                tempFloat = width - QLabel.x - 16 - 12; //extra space after the custom start position label
                QUI.Box(QStyles.GetBackgroundStyle(Style.BackgroundType.Low, useCustomStartAnchoredPosition.boolValue ? QColors.Color.Blue : QColors.Color.Gray), QLabel.x + 16 + 12 + tempFloat * showCustomStartPosition.faded, 18 + 24 * showCustomStartPosition.faded);
                QUI.Space(-QLabel.x - 12 - 12 - tempFloat * showCustomStartPosition.faded);

                QUI.Toggle(useCustomStartAnchoredPosition);
                QUI.BeginVertical(QLabel.x + 8, QUI.SingleLineHeight);
                {
                    QUI.Label(QLabel);
                    QUI.Space(SPACE_2);
                }
                QUI.EndVertical();

                if(showCustomStartPosition.faded > 0.4f)
                {
                    QUI.PropertyField(customStartAnchoredPosition, (tempFloat - 4) * showCustomStartPosition.faded);
                }
            }
            QUI.EndHorizontal();

            showCustomStartPosition.target = useCustomStartAnchoredPosition.boolValue;

            QUI.Space(-20 * showCustomStartPosition.faded); //lift the buttons on the background

            if(showCustomStartPosition.faded > 0.4f)
            {
                tempFloat = (width - 16 - 16) / 3; //button width (3 buttons) that takes into account spaces
                QUI.BeginHorizontal(width);
                {
                    QUI.Space(20 * showCustomStartPosition.faded);
                    if(QUI.GhostButton("Get Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        customStartAnchoredPosition.vector3Value = Drawer.RectTransform.anchoredPosition3D;
                    }

                    QUI.Space(SPACE_4);

                    if(QUI.GhostButton("Set Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        Undo.RecordObject(Drawer.RectTransform, "SetPosition");
                        Drawer.RectTransform.anchoredPosition3D = customStartAnchoredPosition.vector3Value;
                    }

                    QUI.Space(SPACE_4);

                    if(QUI.GhostButton("Reset Position", QColors.Color.Blue, tempFloat * showCustomStartPosition.faded, 16 * showCustomStartPosition.faded))
                    {
                        customStartAnchoredPosition.vector3Value = Vector3.zero;
                    }

                    QUI.FlexibleSpace();
                }
                QUI.EndHorizontal();
            }

            QUI.Space(SPACE_8 * showCustomStartPosition.faded);
        }
        void DrawEvents(float width)
        {
            if(QUI.GhostBar("Unity Events", IsAnyEventUsed ? QColors.Color.Blue : QColors.Color.Gray, showEvents, width, MiniBarHeight))
            {
                showEvents.target = !showEvents.target;
            }
            QUI.BeginHorizontal(width);
            {
                QUI.Space(SPACE_8 * showEvents.faded);
                if(QUI.BeginFadeGroup(showEvents.faded))
                {
                    QUI.BeginVertical(width - SPACE_16);
                    {
                        QUI.Space(SPACE_2 * showEvents.faded);
                        QUI.SetGUIBackgroundColor(IsUsingOnDrawerOpened ? QUI.AccentColorBlue : QUI.AccentColorGray);
                        QUI.PropertyField(OnDrawerOpened, new GUIContent() { text = "OnDrawerOpened" }, width - 8);
                        QUI.ResetColors();
                        QUI.Space(SPACE_2 * showEvents.faded);
                        QUI.SetGUIBackgroundColor(IsUsingOnDrawerClosed ? QUI.AccentColorBlue : QUI.AccentColorGray);
                        QUI.PropertyField(OnDrawerClosed, new GUIContent() { text = "OnDrawerClosed" }, width - 8);
                        QUI.ResetColors();
                        QUI.Space(SPACE_4 * showEvents.faded);
                        QUI.SetGUIBackgroundColor(IsUsingOnDrawerIsOpening ? QUI.AccentColorBlue : QUI.AccentColorGray);
                        QUI.PropertyField(OnDrawerIsOpening, new GUIContent() { text = "OnDrawerIsOpening" }, width - 8);
                        QUI.ResetColors();
                        QUI.Space(SPACE_2 * showEvents.faded);
                        QUI.SetGUIBackgroundColor(IsUsingOnDrawerIsClosing ? QUI.AccentColorBlue : QUI.AccentColorGray);
                        QUI.PropertyField(OnDrawerIsClosing, new GUIContent() { text = "OnDrawerIsClosing" }, width - 8);
                        QUI.ResetColors();
                        QUI.Space(SPACE_4 * showEvents.faded);
                        QUI.SetGUIBackgroundColor(IsUsingOnDrawerBeginDrag ? QUI.AccentColorBlue : QUI.AccentColorGray);
                        QUI.PropertyField(OnDrawerBeginDrag, new GUIContent() { text = "OnDrawerBeginDrag" }, width - 8);
                        QUI.ResetColors();
                        QUI.Space(SPACE_2 * showEvents.faded);
                        QUI.SetGUIBackgroundColor(IsUsingOnDrawerEndDrag ? QUI.AccentColorBlue : QUI.AccentColorGray);
                        QUI.PropertyField(OnDrawerEndDrag, new GUIContent() { text = "OnDrawerEndDrag" }, width - 8);
                        QUI.ResetColors();
                        QUI.Space(SPACE_8 * showEvents.faded);
                    }
                    QUI.EndVertical();
                    QUI.ResetColors();
                }
                QUI.EndFadeGroup();
            }
            QUI.EndHorizontal();
        }
        void DrawContainer(float width)
        {
            QUI.QObjectPropertyField("Container", container, width, 20, true);
            QUI.BeginHorizontal(width);
            {
                QLabel.text = "When Closed";
                QLabel.style = Style.Text.Small;
                QUI.LabelWithBackground(QLabel);
                QUI.Space(SPACE_4);
                QUI.QToggle("Fade Out", fadeOutContainerWhenClosed);
                QUI.Space(SPACE_4);
                QUI.QToggle("Disable", disableContainerWhenClosed);
                QUI.Space(SPACE_4);
                QUI.QToggle("Dont Disable Canvas", dontDisableContainerCanvasWhenClosed);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.BeginHorizontal(width);
            {
                QUI.QObjectPropertyField("Container Size", containerSize, width - 160 - 2, 20, false);
                QUI.Space(SPACE_2);
                if(QUI.GhostButton("Update Container", QColors.Color.Gray, 160, 20, false))
                {
                    UpdateContainer();
                }
            }
            QUI.EndHorizontal();
            QUI.BeginHorizontal(width);
            {
                if((UIDrawer.ContainerSize)containerSize.enumValueIndex == UIDrawer.ContainerSize.PercentageOfScreen)
                {
                    QUI.QObjectPropertyField("Percentage (0 - 1)", containerPercentageOfScreenSize, (width - 4) * 0.2f, 20, false);
                    QUI.Space(SPACE_2);
                    QUI.QObjectPropertyField("Min Size", containerMinimumSize, (width - 2) * 0.6f, 20, false);

                }
                else if((UIDrawer.ContainerSize)containerSize.enumValueIndex == UIDrawer.ContainerSize.FixedSize)
                {
                    QUI.QObjectPropertyField("Fixed Size", containerFixedSize, width, 20, false);
                }
            }
            QUI.EndHorizontal();

        }
        void DrawOverlay(float width)
        {
            QUI.QObjectPropertyField("Overlay", overlay, width, 20, true);
            QUI.BeginHorizontal(width);
            {
                QLabel.text = "When Closed";
                QLabel.style = Style.Text.Small;
                QUI.LabelWithBackground(QLabel);
                QUI.Space(SPACE_4);
                QUI.QToggle("Disable", disableOverlayWhenClosed);
                QUI.Space(SPACE_4);
                QUI.QToggle("Dont Disable Canvas", dontDisableOverlayCanvasWhenClosed);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        void DrawArrow(float width)
        {
            QUI.BeginHorizontal(width);
            {
                QUI.QToggle("Show Arrow", showArrow);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();

            showArrowAnimBool.target = showArrow.boolValue;

            if(QUI.BeginFadeGroup(showArrowAnimBool.faded))
            {
                QUI.BeginVertical(width);
                {
                    QUI.Space(SPACE_2 * showArrowAnimBool.faded);
                    QUI.BeginHorizontal(width);
                    {
                        QUI.QObjectPropertyField("Arrow", arrow, width - 4 - 100, 20, true);
                        QUI.Space(SPACE_4 * showArrowAnimBool.faded);
                        QUI.QObjectPropertyField("Scale", arrowScale, 100, 20, false);
                        QUI.FlexibleSpace();
                    }
                    QUI.EndHorizontal();
                    QUI.BeginHorizontal(width);
                    {
                        QUI.QToggle("Override Arrow Color", overrideArrowColor);
                        QUI.Space(SPACE_2);
                        if(overrideArrowColor.boolValue)
                        {
                            tempFloat = (width - 152) / 2; //color fields width
                            QUI.QObjectPropertyField("Closed", arrowColorWhenClosed, tempFloat, 20, false);
                            QUI.Space(SPACE_2);
                            QUI.QObjectPropertyField("Opened", arrowColorWhenOpened, tempFloat, 20, false);
                        }
                        QUI.FlexibleSpace();
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2);
                    QUI.BeginHorizontal(width);
                    {
                        if(QUI.GhostButton("Reset Arrow Holder Pos", QColors.Color.Gray, (width - 4) / 3, 18))
                        {
                            Undo.RecordObject(Drawer.leftDrawerArrowHolder, "Update Arrow Holder");
                            switch(Drawer.drawerCloseDirection)
                            {
                                case Gestures.SimpleSwipe.Left: UIDrawer.ResetArrowHolder(Drawer.leftDrawerArrowHolder, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Right: UIDrawer.ResetArrowHolder(Drawer.rightDrawerArrowHolder, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Up: UIDrawer.ResetArrowHolder(Drawer.upDrawerArrowHolder, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Down: UIDrawer.ResetArrowHolder(Drawer.downDrawerArrowHolder, Drawer.drawerCloseDirection); break;
                            }
                        }
                        QUI.Space(SPACE_2);
                        if(QUI.GhostButton("Reset Closed Arrow Pos", QColors.Color.Gray, (width - 4) / 3, 18))
                        {
                            Undo.RecordObject(Drawer.leftDrawerArrowClosedPosition, "Update Closed Arrow");
                            switch(Drawer.drawerCloseDirection)
                            {
                                case Gestures.SimpleSwipe.Left: UIDrawer.ResetClosedArrow(Drawer.leftDrawerArrowClosedPosition, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Right: UIDrawer.ResetClosedArrow(Drawer.rightDrawerArrowClosedPosition, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Up: UIDrawer.ResetClosedArrow(Drawer.upDrawerArrowClosedPosition, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Down: UIDrawer.ResetClosedArrow(Drawer.downDrawerArrowClosedPosition, Drawer.drawerCloseDirection); break;
                            }
                        }
                        QUI.Space(SPACE_2);
                        if(QUI.GhostButton("Reset Opened Arrow Pos", QColors.Color.Gray, (width - 4) / 3, 18))
                        {
                            Undo.RecordObject(Drawer.leftDrawerArrowOpenedPosition, "Update Opened Arrow");
                            switch(Drawer.drawerCloseDirection)
                            {
                                case Gestures.SimpleSwipe.Left: UIDrawer.ResetOpenedArrow(Drawer.leftDrawerArrowOpenedPosition, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Right: UIDrawer.ResetOpenedArrow(Drawer.rightDrawerArrowOpenedPosition, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Up: UIDrawer.ResetOpenedArrow(Drawer.upDrawerArrowOpenedPosition, Drawer.drawerCloseDirection); break;
                                case Gestures.SimpleSwipe.Down: UIDrawer.ResetOpenedArrow(Drawer.downDrawerArrowOpenedPosition, Drawer.drawerCloseDirection); break;
                            }
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2);
                    QUI.BeginHorizontal(width);
                    {
                        if(QUI.GhostButton("Copy - Opened Arrow Position - to - Closed Arrow Position", QColors.Color.Gray, width, 18))
                        {
                            Undo.RecordObject(Drawer.leftDrawerArrowOpenedPosition, "Update Opened Arrow");
                            switch(Drawer.drawerCloseDirection)
                            {
                                case Gestures.SimpleSwipe.Left: UIDrawer.MatchRectTransform(Drawer.leftDrawerArrowClosedPosition, Drawer.leftDrawerArrowOpenedPosition); break;
                                case Gestures.SimpleSwipe.Right: UIDrawer.MatchRectTransform(Drawer.rightDrawerArrowClosedPosition, Drawer.rightDrawerArrowOpenedPosition); break;
                                case Gestures.SimpleSwipe.Up: UIDrawer.MatchRectTransform(Drawer.upDrawerArrowClosedPosition, Drawer.upDrawerArrowOpenedPosition); break;
                                case Gestures.SimpleSwipe.Down: UIDrawer.MatchRectTransform(Drawer.downDrawerArrowClosedPosition, Drawer.downDrawerArrowOpenedPosition); break;
                            }
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2);
                    QUI.BeginHorizontal(width);
                    {
                        if(QUI.GhostButton("Copy - Closed Arrow Position - to - Opened Arrow Position", QColors.Color.Gray, width, 18))
                        {
                            Undo.RecordObject(Drawer.leftDrawerArrowClosedPosition, "Update Closed Arrow");
                            switch(Drawer.drawerCloseDirection)
                            {
                                case Gestures.SimpleSwipe.Left: UIDrawer.MatchRectTransform(Drawer.leftDrawerArrowOpenedPosition, Drawer.leftDrawerArrowClosedPosition); break;
                                case Gestures.SimpleSwipe.Right: UIDrawer.MatchRectTransform(Drawer.rightDrawerArrowOpenedPosition, Drawer.rightDrawerArrowClosedPosition); break;
                                case Gestures.SimpleSwipe.Up: UIDrawer.MatchRectTransform(Drawer.upDrawerArrowOpenedPosition, Drawer.upDrawerArrowClosedPosition); break;
                                case Gestures.SimpleSwipe.Down: UIDrawer.MatchRectTransform(Drawer.downDrawerArrowOpenedPosition, Drawer.downDrawerArrowClosedPosition); break;
                            }
                        }
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2);
                    if(QUI.GhostBar("Show Arrow References", showArrowReferences.target ? QColors.Color.Blue : QColors.Color.Gray, showArrowReferences, width * showArrowAnimBool.faded, MiniBarHeight))
                    {
                        showArrowReferences.target = !showArrowReferences.target;
                    }
                    QUI.BeginHorizontal(width);
                    {
                        QUI.Space(SPACE_8 * showArrowReferences.faded);
                        if(QUI.BeginFadeGroup(showArrowReferences.faded))
                        {
                            QUI.BeginVertical(width - SPACE_8);
                            {
                                QUI.Space(SPACE_4 * showArrowReferences.faded);
                                QUI.QObjectPropertyField("Arrow Container", arrowContainer, width - SPACE_8, 20, true);
                                QUI.Space(SPACE_2 * showArrowReferences.faded);
                                DrawArrowHolder("Left", leftDrawerArrowHolder, leftDrawerArrowClosedPosition, leftDrawerArrowOpenedPosition, width);
                                QUI.Space(SPACE_2 * showArrowReferences.faded);
                                DrawArrowHolder("Right", rightDrawerArrowHolder, rightDrawerArrowClosedPosition, rightDrawerArrowOpenedPosition, width);
                                QUI.Space(SPACE_2 * showArrowReferences.faded);
                                DrawArrowHolder("Up", upDrawerArrowHolder, upDrawerArrowClosedPosition, upDrawerArrowOpenedPosition, width);
                                QUI.Space(SPACE_2 * showArrowReferences.faded);
                                DrawArrowHolder("Down", downDrawerArrowHolder, downDrawerArrowClosedPosition, downDrawerArrowOpenedPosition, width);
                                QUI.Space(SPACE_2 * showArrowReferences.faded);
                            }
                            QUI.EndVertical();
                        }
                        QUI.EndFadeGroup();
                    }
                    QUI.EndHorizontal();
                    QUI.Space(SPACE_2 * showArrowAnimBool.faded);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();
        }
        void DrawArrowHolder(string direction, SerializedProperty holder, SerializedProperty closed, SerializedProperty opened, float width)
        {
            QUI.BeginHorizontal(width - SPACE_8);
            {
                QUI.Space(SPACE_8 * showArrowReferences.faded);
                QUI.QObjectPropertyField(direction + " Drawer Arrow Holder", holder, width - 16, 20, true);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.BeginHorizontal(width - SPACE_8);
            {
                QUI.Space(SPACE_16 * showArrowReferences.faded);
                QUI.QObjectPropertyField("Closed Position", closed, width - 24, 20, true);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
            QUI.BeginHorizontal(width - SPACE_8);
            {
                QUI.Space(SPACE_16 * showArrowReferences.faded);
                QUI.QObjectPropertyField("Opened Position", opened, width - 24, 20, true);
                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }

        void UpdateContainer()
        {
            if(Drawer.arrowContainer != null)
            {
                Undo.RecordObjects(new Object[] { Drawer.container, Drawer.arrowContainer }, "Update Container");
                Drawer.UpdateContainer();
                Drawer.UpdateArrowContainer();
            }
            else
            {
                Undo.RecordObject(Drawer.container, "Update Container");
                Drawer.UpdateContainer();
            }
        }
    }
}
