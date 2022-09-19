using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// This component listens for images detected by the <c>XRImageTrackingSubsystem</c>
    /// and overlays some prefabs on top of the detected image.
    /// </summary>
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class DemoSceneImagePair : MonoBehaviour, ISerializationCallbackReceiver
    {
        [Serializable]
        struct NamedPrefab
        {
            // System.Guid isn't serializable, so we store the Guid as a string. At runtime, this is converted back to a System.Guid
            public string imageGuid;
            public GameObject imagePrefab;

            public NamedPrefab(Guid guid, GameObject prefab)
            {
                imageGuid = guid.ToString();
                imagePrefab = prefab;
            }
        }

        [SerializeField]
        [HideInInspector]
        List<NamedPrefab> m_PrefabsList = new List<NamedPrefab>();

        //tartalmazza az összes prefabot
        Dictionary<Guid, GameObject> m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
        //marker által megjelenített prefabot tartalmazza
        Dictionary<Guid, GameObject> m_Instantiated = new Dictionary<Guid, GameObject>();
        ARTrackedImageManager m_TrackedImageManager;
        ARSession arSession;
        ARTrackedImage currentTrackedImage;
        Boolean houseSoundWasPlayed = false;
        DemoSceneSoundManager soundManager;

        [SerializeField]
        [Tooltip("Reference Image Library")]
        XRReferenceImageLibrary m_ImageLibrary;

        //Touch action
        private Vector2 touchPosition = default;

        [SerializeField]
        PrefabState[] prefab;

        [SerializeField]
        private Camera arCamera;

        //library beállítása
        public XRReferenceImageLibrary imageLibrary
        {

            get => m_ImageLibrary;
            set => m_ImageLibrary = value;
        }

        public void OnBeforeSerialize()
        {
            m_PrefabsList.Clear();
            foreach (var kvp in m_PrefabsDictionary)
            {
                m_PrefabsList.Add(new NamedPrefab(kvp.Key, kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
            foreach (var entry in m_PrefabsList)
            {
                m_PrefabsDictionary.Add(Guid.Parse(entry.imageGuid), entry.imagePrefab);
            }
        }

        void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;

                if (touch.phase == TouchPhase.Began)
                {

                    Ray ray = arCamera.ScreenPointToRay(touch.position);
                    RaycastHit hitObject;
                    

                    if (Physics.Raycast(ray, out hitObject))
                    {
                        GameObject gameObject = hitObject.collider.gameObject;
                        PrefabState prefabState = hitObject.transform.GetComponent<PrefabState>();

                        if (hitObject.collider.gameObject.name.Equals("questionmark"))
                        {
                            this.soundManager.playSoundOfQuestionmark();
                        }

                        if (hitObject.collider.gameObject.name.Equals("home_obj"))
                        {
                            if (!this.houseSoundWasPlayed)
                            {
                                this.houseSoundWasPlayed = true;
                                this.soundManager.playSoundOfHouse();
                            }
                            else
                            {
                                SceneManager.LoadScene(0);
                                resetARSession();
                            }
                        }
                            
                        Guid gameObjectKey;

                        if (prefabState != null)
                        {
                            foreach (var kpv in m_PrefabsDictionary)
                            {
                                if (kpv.Value.Equals(gameObject.name)) {
                                    Debug.Log("egyenlõek a nevek!");
                                }
                                gameObjectKey = kpv.Key;
                            }

                            ChangeSelectedObject(prefabState, gameObject, gameObjectKey);
                        }
                        else
                        {
                            Debug.Log("prefabState null!");
                        }
                    }
                }
            }
        }

        void Awake()
        {
            m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
            arSession = FindObjectOfType<ARSession>();
            arCamera = FindObjectOfType<Camera>();
            this.soundManager = FindObjectOfType(typeof(DemoSceneSoundManager)) as DemoSceneSoundManager;
        }

        void OnEnable()
        {
            m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        void OnDisable()
        {
            m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        void Start() {
            StartCoroutine(this.playIntroductoryInformation());
        }

        IEnumerator<WaitForSeconds> playIntroductoryInformation()
        {
            yield return new WaitForSeconds(5);

            this.soundManager.playIntro();

            yield return new WaitForSeconds(8);

            this.soundManager.playSoundOfStructure();

            yield return new WaitForSeconds(15);

            this.soundManager.playSoundOfRule();

            yield return new WaitForSeconds(15);

            this.soundManager.playSoundOfHouse();

            yield return new WaitForSeconds(15);

            this.soundManager.playSoundOfQuestionmark();

            yield return new WaitForSeconds(15);

            this.soundManager.playSoundOfHowToPlay();
        }


        void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (ARTrackedImage trackedImage in eventArgs.added)
            {
                AssignPrefab(trackedImage);
                currentTrackedImage = trackedImage;
            }

            foreach (ARTrackedImage trackedImage in eventArgs.updated)
            {
                if (arSession != null)
                {
                    if (eventArgs.updated.Count > 1)
                    {
                        var trackedImg = trackedImage;
                        AssignPrefab(trackedImg);
                    }
                }
                else
                {
                    Debug.LogError("ARSession is null!!");
                }
            }

            foreach (ARTrackedImage trackedImage in eventArgs.removed)
            {
                m_Instantiated[trackedImage.referenceImage.guid].SetActive(false);
            }
        }

        void resetARSession()
        {
            arSession.Reset();
        }

        void AssignPrefab(ARTrackedImage trackedImage)
        {
            if (m_PrefabsDictionary.TryGetValue(trackedImage.referenceImage.guid, out var prefab))
            {
                trackedImage.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                m_Instantiated[trackedImage.referenceImage.guid] = Instantiate(prefab, trackedImage.transform);
                m_Instantiated[trackedImage.referenceImage.guid].SetActive(true);
            }

            this.soundManager.playSoundOfHowToInteract();
        }

        void ChangeSelectedObject(PrefabState selected, GameObject gameObject, Guid key)
        {
            if (key != null)
            {
                Debug.Log("inst go name: " + m_Instantiated[key].gameObject.name);
                Debug.Log("inst count: " + m_Instantiated.Count);
                MeshRenderer objMesh = m_Instantiated[key].gameObject.GetComponent<MeshRenderer>();
                
                selected.isSelected = true;
                objMesh.material.color = Color.green;

                this.soundManager.playSoundOfGoodWork();

                StartCoroutine(this.playFurtherDescription());
            }
            else
            {
                Debug.Log("key null!!");
            }

        }

        IEnumerator<WaitForSeconds> playFurtherDescription() {
            yield return new WaitForSeconds(5);

            this.soundManager.playSoundOfHowToSolve();

            yield return new WaitForSeconds(10);

            this.soundManager.playSoundOfCheckTask();

            yield return new WaitForSeconds(15);

            this.soundManager.playSoundOfEndOfIntro();
        }

        //UNITY_EDITOR
        public GameObject GetPrefabForReferenceImage(XRReferenceImage referenceImage)
            => m_PrefabsDictionary.TryGetValue(referenceImage.guid, out var prefab) ? prefab : null;

#if UNITY_EDITOR

    [CustomEditor(typeof(DemoSceneImagePair))]
    class DemoSceneImagePairManagerInspector : Editor
    {
        List<XRReferenceImage> m_ReferenceImages = new List<XRReferenceImage>();
        bool m_IsExpanded = true;

        bool HasLibraryChanged(XRReferenceImageLibrary library)
        {
            if (library == null)
                return m_ReferenceImages.Count == 0;

            if (m_ReferenceImages.Count != library.count)
                return true;

            for (int i = 0; i < library.count; i++)
            {
                if (m_ReferenceImages[i] != library[i])
                    return true;
            }

            return false;
        }

        public override void OnInspectorGUI()
        {
            var behaviour = serializedObject.targetObject as DemoSceneImagePair;

            serializedObject.Update();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            }

            var libraryProperty = serializedObject.FindProperty(nameof(m_ImageLibrary));
            EditorGUILayout.PropertyField(libraryProperty);
            var library = libraryProperty.objectReferenceValue as XRReferenceImageLibrary;

            //ellenörzi hogy változott e library
            if (HasLibraryChanged(library))
            {
                if (library)
                {
                    var tempDictionary = new Dictionary<Guid, GameObject>();
                    foreach (var referenceImage in library)
                    {
                        tempDictionary.Add(referenceImage.guid, behaviour.GetPrefabForReferenceImage(referenceImage));
                    }
                    behaviour.m_PrefabsDictionary = tempDictionary;
                }
            }

            //updatel
            m_ReferenceImages.Clear();
            if (library)
            {
                foreach (var referenceImage in library)
                {
                    m_ReferenceImages.Add(referenceImage);
                }
            }

            //prefab list
            m_IsExpanded = EditorGUILayout.Foldout(m_IsExpanded, "Prefab List");
            if (m_IsExpanded)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUI.BeginChangeCheck();

                    var tempDictionary = new Dictionary<Guid, GameObject>();
                    foreach (var image in library)
                    {

                        var prefab = (GameObject)EditorGUILayout.ObjectField(image.name, behaviour.m_PrefabsDictionary[image.guid], typeof(GameObject), false);
                        tempDictionary.Add(image.guid, prefab);

                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(target, "Update Prefab");
                        behaviour.m_PrefabsDictionary = tempDictionary;
                        EditorUtility.SetDirty(target);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    }
}