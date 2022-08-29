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

        //tartalmazza az �sszes prefabot
        Dictionary<Guid, GameObject> m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
        //marker �ltal megjelen�tett prefabot tartalmazza
        Dictionary<Guid, GameObject> m_Instantiated = new Dictionary<Guid, GameObject>();
        ARTrackedImageManager m_TrackedImageManager;
        ARSession arSession;
        ARTrackedImage currentTrackedImage;

        [SerializeField]
        [Tooltip("Reference Image Library")]
        XRReferenceImageLibrary m_ImageLibrary;

        //Touch action
        private Vector2 touchPosition = default;

        [SerializeField]
        PrefabState[] prefab;

        [SerializeField]
        private Camera arCamera;

        //library be�ll�t�sa
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
                            Debug.Log("k�rd�s");
                        }

                        if (hitObject.collider.gameObject.name.Equals("home_obj"))
                        {
                            SceneManager.LoadScene(0);
                            resetARSession();
                        }


                        if (prefabState != null)
                        {
                            Guid key;

                            foreach (var kpv in m_PrefabsDictionary)
                            {
                                if (kpv.Value.transform.Find(gameObject.name) != null)
                                {
                                    key = kpv.Key;
                                    break;
                                }
                            }
                            ChangeSelectedObject(prefabState, gameObject, key);
                        }
                        else
                        {
                            Debug.Log("null");
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
        }

        void OnEnable()
        {
            m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        void OnDisable()
        {
            m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
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
                        resetARSession();
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
        }

        void ChangeSelectedObject(PrefabState selected, GameObject gameObject, Guid key)
        {

            if (key != null)
            {
                MeshRenderer objMesh = m_Instantiated[key].transform.Find(gameObject.name).GetComponent<MeshRenderer>();

                if (selected.isSelected != true)
                {
                    if (selected.thisIsTheGoodSolution == true)
                    {
                        selected.isSelected = true;
                        objMesh.material.color = Color.green;
                    }
                    else
                    {
                        selected.isSelected = true;
                        objMesh.material.color = Color.red;


                        StartCoroutine(this.SetBackTheColor(objMesh, selected));
                    }

                    PlaySoundManager playSoundManager = FindObjectOfType(typeof(PlaySoundManager)) as PlaySoundManager; ; ;
                    playSoundManager.playSound(selected.thisIsTheGoodSolution);
                }
                else
                {
                    selected.isSelected = false;
                    objMesh.material.color = selected.originalColor;
                }
            }
            else
            {
                Debug.Log("key null!!");
            }

        }

        IEnumerator<WaitForSeconds> SetBackTheColor(MeshRenderer objMesh, PrefabState selected)
        {
            yield return new WaitForSeconds(5);

            selected.isSelected = false;
            objMesh.material.color = selected.originalColor;
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

            //ellen�rzi hogy v�ltozott e library
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