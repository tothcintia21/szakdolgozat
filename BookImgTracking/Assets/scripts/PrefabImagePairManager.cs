using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// This component listens for images detected by the <c>XRImageTrackingSubsystem</c>
    /// and overlays some prefabs on top of the detected image.
    /// </summary>
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class PrefabImagePairManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Used to associate an `XRReferenceImage` with a Prefab by using the `XRReferenceImage`'s guid as a unique identifier for a particular reference image.
        /// </summary>
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
        private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);
        ARSession arSession;

        [SerializeField]
        [Tooltip("Reference Image Library")]
        XRReferenceImageLibrary m_ImageLibrary;

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

        void Awake()
        {
            m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
            arSession = FindObjectOfType<ARSession>();
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

                        //dict nem �r�l ki, de nem is szaporodik fel benne a prefabok sz�ma
                        Debug.Log("Reset megt�rt�nt! Inst. sz�ma: " + m_Instantiated.Count); 
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

        void resetARSession() {
            arSession.Reset();
        }

        void AssignPrefab(ARTrackedImage trackedImage)
        {
            if (m_PrefabsDictionary.TryGetValue(trackedImage.referenceImage.guid, out var prefab)) {
                trackedImage.transform.localScale = scaleFactor;

                m_Instantiated[trackedImage.referenceImage.guid] = Instantiate(prefab, trackedImage.transform);
                m_Instantiated[trackedImage.referenceImage.guid].SetActive(true);
            }

            //if (!m_Instantiated.ContainsKey(trackedImage.referenceImage.guid))
            //{
            //m_Instantiated[trackedImage.referenceImage.guid] = Instantiate(prefab, trackedImage.transform);
            //}
            //else
            //{
            //Debug.Log("szerepel");
            //}
        }

        void setState(ARTrackedImage trackedImage)
        {
            Guid img = trackedImage.referenceImage.guid;

            foreach (var kpv in m_Instantiated)
            {
               
                if (kpv.Key.CompareTo(img) != 0)
                {
                    m_Instantiated[kpv.Key].SetActive(false);
                }
                else {
                    m_Instantiated[kpv.Key].SetActive(true);
                }
            }
        }


//UNITY_EDITOR
        public GameObject GetPrefabForReferenceImage(XRReferenceImage referenceImage)
            => m_PrefabsDictionary.TryGetValue(referenceImage.guid, out var prefab) ? prefab : null;

//Unity prefab list megad�sa -> reference img �s a hozz�val� prefab
//updateli a listet amint v�ltozik a referenceImgLibrary
//Unity Editor
#if UNITY_EDITOR
       
        [CustomEditor(typeof(PrefabImagePairManager))]
        class PrefabImagePairManagerInspector : Editor
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
               
                var behaviour = serializedObject.targetObject as PrefabImagePairManager;

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