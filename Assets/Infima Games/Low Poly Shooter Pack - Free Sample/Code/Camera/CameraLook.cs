// Copyright 2021, Infima Games. All Rights Reserved.

using Unity.VisualScripting;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Camera Look. Handles the rotation of the camera.
    /// </summary>
    public class CameraLook : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]

        [Tooltip("Sensitivity when looking around.")]
        [SerializeField]
        private Vector2 sensitivity = new Vector2(1, 1);

        [Tooltip("Minimum and maximum up/down rotation angle the camera can have.")]
        [SerializeField]
        private Vector2 yClamp = new Vector2(-60, 60);

        [Tooltip("Should the look rotation be interpolated?")]
        [SerializeField]
        private bool smooth;

        [Tooltip("The speed at which the look rotation is interpolated.")]
        [SerializeField]
        private float interpolationSpeed = 25.0f;

        #endregion

        #region FIELDS

        /// <summary>
        /// Player Character.
        /// </summary>
        private CharacterBehaviour playerCharacter;
        /// <summary>
        /// The player character's rigidbody component.
        /// </summary>
        private Rigidbody playerCharacterRigidbody;

        /// <summary>
        /// The player character's rotation.
        /// </summary>
        private Quaternion rotationCharacter;
        /// <summary>
        /// The camera's rotation.
        /// </summary>
        private Quaternion rotationCamera;
        /// <summary>
        /// The players camera object
        /// </summary>
        private Camera cam;

        private bool shoot;

        #endregion

        #region UNITY

        private void Awake()
        {
            //Get Player Character.
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
            //Cache the rigidbody.
            playerCharacterRigidbody = playerCharacter.GetComponent<Rigidbody>();
            //Cache camera from the player
            cam = playerCharacter.GetCameraWorld();
        }
        private void Start()
        {
            //Cache the character's initial rotation.
            rotationCharacter = playerCharacter.transform.localRotation;
            //Cache the camera's initial rotation.
            rotationCamera = transform.localRotation;
        }
        public void Shoot()
        {
            shoot = true;
        }
        private void LateUpdate()
        {
            //Frame Input. The Input to add this frame!
            Vector2 frameInput = playerCharacter.IsCursorLocked() ? playerCharacter.GetInputLook() : default;
            //Sensitivity.
            frameInput *= sensitivity;

            //Yaw.
            Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x + (shoot ? 1.0f : 0.0f), 0.0f);
            //Pitch.
            Quaternion rotationPitch = Quaternion.Euler(-frameInput.y - (shoot ? 1.0f : 0.0f), 0.0f, 0.0f);

            if (shoot)
                shoot = false;

            //Save rotation. We use this for smooth rotation.
            rotationCamera *= rotationPitch;
            rotationCharacter *= rotationYaw;

            //Local Rotation.
            Quaternion localRotation = transform.localRotation;

            //Smooth.
            if (smooth)
            {
                //Interpolate local rotation.
                localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * interpolationSpeed);
                //Interpolate character rotation.
                playerCharacterRigidbody.MoveRotation(Quaternion.Slerp(playerCharacterRigidbody.rotation, rotationCharacter, Time.deltaTime * interpolationSpeed));
            }
            else
            {
                //Rotate local.
                localRotation *= rotationPitch;
                //Clamp.
                localRotation = Clamp(localRotation);

                //Rotate character.
                playerCharacterRigidbody.MoveRotation(playerCharacterRigidbody.rotation * rotationYaw);
            }

            //Set.
            transform.localRotation = localRotation;

            //Update FOV
            bool isAim = playerCharacter.IsAiming();
            bool isSprinting = playerCharacter.IsRunning();
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, isAim ? 70.0f : (isSprinting ? 110.0f : 90.0f), .1f);

        }

        #endregion

        #region FUNCTIONS

        /// <summary>
        /// Clamps the pitch of a quaternion according to our clamps.
        /// </summary>
        private Quaternion Clamp(Quaternion rotation)
        {
            rotation.x /= rotation.w;
            rotation.y /= rotation.w;
            rotation.z /= rotation.w;
            rotation.w = 1.0f;

            //Pitch.
            float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

            //Clamp.
            pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);
            rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

            //Return.
            return rotation;
        }

        #endregion
    }
}
