using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace GlobeTanks.Objects
{
	public class AroundSphereCamera : MonoBehaviour
	{
		[SerializeField] private float distanceCameraToCenter = 3;
		[SerializeField] private float zoomStep = 0.1f;
		[SerializeField] private float minDistanceFromCenter = 1.2f;
		[SerializeField] private float maxDistanceFromCenter = 5f;

		private MainInputAction mainInputAction;

		private const float RotationFactor = 0.2f;
		private const float PositionXbounder = 89;
		private Vector2 rotation;
		private bool isRotating;
		private Vector2 dragPosition;
		private static Vector3 initialCameraPosition;
		private bool isNeedReposition;

		[Inject]
		public void Construct(MainInputAction mainInputAction)
        {
			this.mainInputAction = mainInputAction;
			isNeedReposition = true;

			mainInputAction.Main.Drag.started += DragStartHandler;
			mainInputAction.Main.Drag.canceled += DragStopHandler;
			mainInputAction.Enable();
		}

        public void OnDestroy()
        {
			mainInputAction.Disable();
			mainInputAction.Main.Drag.started -= DragStartHandler;
			mainInputAction.Main.Drag.canceled -= DragStopHandler;
		}

		private void DragStartHandler(InputAction.CallbackContext context)
        {
			if (EventSystem.current.IsPointerOverGameObject())
				return;

			isRotating = true;
			dragPosition = Mouse.current.position.ReadValue();
		}

		private void DragStopHandler(InputAction.CallbackContext context)
        {
			isRotating = false;
		}

		private void Update()
		{
			Vector2 currentMousePosition = GetCurrentMousePosition();

			if (isRotating)
			{
				ChangeRotation(new Vector2(
					-RotationFactor * (dragPosition.y - currentMousePosition.y),
					-RotationFactor * (dragPosition.x - currentMousePosition.x)));

				dragPosition = currentMousePosition;
			}

            float deltaWheel = mainInputAction.Main.MouseWheel.ReadValue<Vector2>().y;

			if (deltaWheel > 0)
				Zoom(zoomStep);
			else if (deltaWheel < 0)
				Zoom(-zoomStep);

			if (isNeedReposition)
				Reposition();
		}

		private void ChangeRotation(Vector2 deltaAngles)
		{
			rotation.x = Mathf.Clamp(rotation.x + deltaAngles.x, -PositionXbounder, PositionXbounder);
			rotation.y = rotation.y + deltaAngles.y;

			isNeedReposition = true;
		}

		private void Reposition()
		{
			isNeedReposition = false;

			initialCameraPosition.z = distanceCameraToCenter;
			transform.position = Quaternion.Euler(rotation.x, rotation.y, 0) * initialCameraPosition;
			transform.LookAt(Vector3.zero);
		}

		private void Zoom(float delta)
		{
			distanceCameraToCenter = Mathf.Clamp(distanceCameraToCenter + delta, minDistanceFromCenter, maxDistanceFromCenter);

			isNeedReposition = true;
		}

		private Vector2 GetCurrentMousePosition()
        {
			return Mouse.current.position.ReadValue();
		}
	}
}