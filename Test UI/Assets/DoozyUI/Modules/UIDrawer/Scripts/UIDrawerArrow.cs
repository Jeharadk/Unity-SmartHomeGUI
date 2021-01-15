// Copyright (c) 2015 - 2018 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DoozyUI.Gestures;
using UnityEngine;
using UnityEngine.UI;

namespace DoozyUI
{
    public class UIDrawerArrow : MonoBehaviour
    {
        private const float ROTATION_SPEED = 10f;
        private const float MAX_BAR_ROTATION = 45f;
        private const float CLOSED_DRAWER_VELOCITY = 0.75f;


        public RectTransform rotator;
        public RectTransform leftBar;
        public RectTransform rightBar;

        private Image leftBarImage;
        public Image rightBarImage;


        private RectTransform m_RectTransform;
        public RectTransform RectTransform { get { if(m_RectTransform == null) { m_RectTransform = GetComponent<RectTransform>(); } return m_RectTransform; } }

        private UIDrawer targetDrawer;
        public UIDrawer TargetDrawer
        {
            get { return targetDrawer; }
            private set { targetDrawer = value; }
        }

        private float width;
        public float Width { get { return width; } }
        private float height;
        public float Height { get { return height; } }
        public Vector2 Size { get { return new Vector2(width, height); } }

        private float velocity = 0;

        private void Awake()
        {
            if(leftBar != null) { leftBarImage = leftBar.GetComponent<Image>(); }
            if(rightBar != null) { rightBarImage = rightBar.GetComponent<Image>(); }

            UpdateSize();
        }

        private void LateUpdate()
        {
            if(targetDrawer == null) { return; } //no target UIDrawer reference -> return

            if(!targetDrawer.IsDragged && !targetDrawer.IsClosing && !targetDrawer.IsOpening) //if the target UIDrawer is not dragged, is not closing or is not opening -> set the arrow to point in the open or close direction (to be used as a visual guide)
            {
                if(targetDrawer.drawerCloseDirection == SimpleSwipe.Left || targetDrawer.drawerCloseDirection == SimpleSwipe.Up)
                {
                    velocity = targetDrawer.Closed ? -CLOSED_DRAWER_VELOCITY : CLOSED_DRAWER_VELOCITY;
                }
                else if(targetDrawer.drawerCloseDirection == SimpleSwipe.Right || targetDrawer.drawerCloseDirection == SimpleSwipe.Down)
                {
                    velocity = targetDrawer.Closed ? CLOSED_DRAWER_VELOCITY : -CLOSED_DRAWER_VELOCITY;
                }
            }
            else //this drawer is being dragged -> make the arror read to the drawer's movements
            {
                if(targetDrawer.drawerCloseDirection == SimpleSwipe.Left || targetDrawer.drawerCloseDirection == SimpleSwipe.Right)
                {
                    velocity = -targetDrawer.ContainerVelocity.x;
                }
                else if(targetDrawer.drawerCloseDirection == SimpleSwipe.Up || targetDrawer.drawerCloseDirection == SimpleSwipe.Down)
                {
                    velocity = targetDrawer.ContainerVelocity.y;
                }

                velocity /= Time.deltaTime;
                velocity /= 1000;

                velocity = velocity > 0 && velocity < 0.05f ? 0 : velocity;
                velocity = velocity < 0 && velocity > -0.05f ? 0 : velocity;
                velocity = velocity > 0 && velocity > 1f ? 1 : velocity;
                velocity = velocity < 0 && velocity < -1f ? -1 : velocity;
            }

            leftBar.localEulerAngles = new Vector3(leftBar.localEulerAngles.x,
                                                   leftBar.localEulerAngles.y,
                                                   Mathf.LerpAngle(leftBar.localEulerAngles.z,
                                                                   Mathf.Clamp(MAX_BAR_ROTATION * velocity, -MAX_BAR_ROTATION, MAX_BAR_ROTATION),
                                                                   ROTATION_SPEED * Time.deltaTime));

            rightBar.localEulerAngles = new Vector3(rightBar.localEulerAngles.x,
                                                    rightBar.localEulerAngles.y,
                                                    Mathf.LerpAngle(rightBar.localEulerAngles.z,
                                                                    Mathf.Clamp(-MAX_BAR_ROTATION * velocity, -MAX_BAR_ROTATION, MAX_BAR_ROTATION),
                                                                    ROTATION_SPEED * Time.deltaTime));
        }

        public void SetTargetDrawer(UIDrawer drawer)
        {
            TargetDrawer = drawer;
            RotateAndMoveArrowToMatchDrawerDirection(TargetDrawer);
        }

        public void UpdateLocalScale(Vector3 scale)
        {
            RectTransform.localScale = new Vector3(scale.x, scale.y, 1);
            UpdateSize();
        }
        public void UpdateLocalScale(float scale)
        {
            RectTransform.localScale = new Vector3(scale, scale, 1);
            UpdateSize();
        }

        Vector3[] rotatorCorners = new Vector3[4];
        Vector3[] drawerCorners = new Vector3[4];
        float rotatorDisableThreshold = 0.6f;

        public void UpdateRotatorPosition(float visibility, bool doNotHide = false)
        {
            //this fixes the flickering made when changing the rotator positions when out of view (issue found on Android build)
            //what we did was to disable the rotator when it was moved 40% out of view (thus the rotatorDisableThreshold value below)
            rotator.GetWorldCorners(rotatorCorners);
            rotatorCorners = AdjustCornersToIdentityRotation(rotator, rotatorCorners);
            targetDrawer.RectTransform.GetWorldCorners(drawerCorners);

            // base view of the corners - the numbers represent the vector index
            // 1 - 2
            // |   |
            // 0 - 3

            switch(targetDrawer.drawerCloseDirection)
            {
                case SimpleSwipe.Left:
                    rotator.gameObject.SetActive(!(rotatorCorners[2].x <= drawerCorners[1].x + Vector3.Distance(rotatorCorners[2], rotatorCorners[1]) * rotatorDisableThreshold)); //disable the rotator gameObject if it moved too much to the side of the canvas
                    rotator.localPosition = Vector3.Lerp(targetDrawer.leftDrawerArrowClosedPosition.localPosition, targetDrawer.leftDrawerArrowOpenedPosition.localPosition, (1 - visibility)); //move the rotator accordingly
                    break;
                case SimpleSwipe.Right:
                    rotator.gameObject.SetActive(!(rotatorCorners[1].x >= drawerCorners[2].x - Vector3.Distance(rotatorCorners[1], rotatorCorners[2]) * rotatorDisableThreshold)); //disable the rotator gameObject if it moved too much to the side of the canvas
                    rotator.localPosition = Vector3.Lerp(targetDrawer.rightDrawerArrowClosedPosition.localPosition, targetDrawer.rightDrawerArrowOpenedPosition.localPosition, (1 - visibility)); //move the rotator accordingly
                    break;
                case SimpleSwipe.Up:
                    rotator.gameObject.SetActive(!(rotatorCorners[0].y >= drawerCorners[1].y - Vector3.Distance(rotatorCorners[0], rotatorCorners[1]) * rotatorDisableThreshold)); //disable the rotator gameObject if it moved too much to the side of the canvas
                    rotator.localPosition = Vector3.Lerp(targetDrawer.upDrawerArrowClosedPosition.localPosition, targetDrawer.upDrawerArrowOpenedPosition.localPosition, (1 - visibility)); //move the rotator accordingly
                    break;
                case SimpleSwipe.Down:
                    rotator.gameObject.SetActive(!(rotatorCorners[1].y <= drawerCorners[0].y + Vector3.Distance(rotatorCorners[1], rotatorCorners[0]) * rotatorDisableThreshold)); //disable the rotator gameObject if it moved too much to the side of the canvas
                    rotator.localPosition = Vector3.Lerp(targetDrawer.downDrawerArrowClosedPosition.localPosition, targetDrawer.downDrawerArrowOpenedPosition.localPosition, (1 - visibility)); //move the rotator accordingly
                    break;
            }
        }

        private Vector3[] tempCorners = new Vector3[4]; //avoid GC
        public Vector3[] AdjustCornersToIdentityRotation(RectTransform target, Vector3[] corners)
        {
            if(target.localEulerAngles.z == 0) //target is not rotated
            {
                // base view of the corners - the numbers represent the vector index
                // 1 - 2
                // |   |
                // 0 - 3
                return corners;
            }
            else if(target.localEulerAngles.z > 0 && target.localEulerAngles.z <= 90)
            {
                // rotated view of the corners that need to be 'adjusted' to the base view - the numbers represent the vector index
                // 2 - 3
                // |   |
                // 1 - 0
                tempCorners[0] = corners[1];
                tempCorners[1] = corners[2];
                tempCorners[2] = corners[3];
                tempCorners[3] = corners[0];
                return tempCorners;
            }
            else if(target.localEulerAngles.z > 90 && target.localEulerAngles.z <= 180)
            {
                // rotated view of the corners that need to be 'adjusted' to the base view - the numbers represent the vector index
                // 3 - 0
                // |   |
                // 2 - 1
                tempCorners[0] = corners[2];
                tempCorners[1] = corners[3];
                tempCorners[2] = corners[0];
                tempCorners[3] = corners[1];
                return tempCorners;
            }
            else if(target.localEulerAngles.z > 180 && target.localEulerAngles.z <= 270)
            {
                // rotated view of the corners that need to be 'adjusted' to the base view - the numbers represent the vector index
                // 0 - 1
                // |   |
                // 3 - 2
                tempCorners[0] = corners[3];
                tempCorners[1] = corners[0];
                tempCorners[2] = corners[1];
                tempCorners[3] = corners[2];
                return tempCorners;
            }
            else
            {
                return corners;
            }

        }

        private void RotateAndMoveArrowToMatchDrawerDirection(UIDrawer drawer)
        {
            rotator.localRotation = Quaternion.identity;
            switch(drawer.drawerCloseDirection)
            {
                case SimpleSwipe.Left:
                    RectTransform.SetParent(drawer.leftDrawerArrowHolder);
                    rotator.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case SimpleSwipe.Right:
                    RectTransform.SetParent(drawer.rightDrawerArrowHolder);
                    rotator.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case SimpleSwipe.Up:
                    RectTransform.SetParent(drawer.upDrawerArrowHolder);
                    rotator.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case SimpleSwipe.Down:
                    RectTransform.SetParent(drawer.downDrawerArrowHolder);
                    rotator.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
            RectTransform.anchoredPosition = Vector2.zero;
            UpdateLocalScale(drawer.arrowScale);

            UpdateArrowColor(drawer);
        }

        public void UpdateArrowColor(UIDrawer drawer)
        {
            if(!drawer.overrideArrowColor) { return; }
            if(leftBar == null || rightBar == null) { return; }

            if(drawer.IsDragged)
            {
                leftBarImage.color = Color.Lerp(drawer.arrowColorWhenClosed, drawer.arrowColorWhenOpened, drawer.Visibility);
                rightBarImage.color = Color.Lerp(drawer.arrowColorWhenClosed, drawer.arrowColorWhenOpened, drawer.Visibility);
                return;
            }

            switch(drawer.CurerntDrawerState)
            {
                case UIDrawer.DrawerState.Opened:
                    leftBarImage.color = drawer.arrowColorWhenOpened;
                    rightBarImage.color = drawer.arrowColorWhenOpened;
                    break;
                case UIDrawer.DrawerState.IsOpening:
                    leftBarImage.color = Color.Lerp(drawer.arrowColorWhenClosed, drawer.arrowColorWhenOpened, drawer.Visibility);
                    rightBarImage.color = Color.Lerp(drawer.arrowColorWhenClosed, drawer.arrowColorWhenOpened, drawer.Visibility);
                    break;
                case UIDrawer.DrawerState.Closed:
                    leftBarImage.color = Color.Lerp(drawer.arrowColorWhenOpened, drawer.arrowColorWhenClosed, 1 - drawer.Visibility);
                    rightBarImage.color = Color.Lerp(drawer.arrowColorWhenOpened, drawer.arrowColorWhenClosed, 1 - drawer.Visibility);
                    break;
                case UIDrawer.DrawerState.IsClosing:
                    leftBarImage.color = drawer.arrowColorWhenClosed;
                    rightBarImage.color = drawer.arrowColorWhenClosed;
                    break;
            }
        }

        private void UpdateSize()
        {
            width = rotator.rect.width;
            height = rotator.rect.height;
        }

    }
}
