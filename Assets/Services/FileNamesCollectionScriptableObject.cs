using System.Collections.Generic;
using UnityEngine;

namespace Assets.Services
{
    [SerializeField]
    [CreateAssetMenu(fileName ="FileNames",menuName ="ScriptableObjects/FileNames",order =0)]
    public class FileNamesCollectionScriptableObject : ScriptableObject
    {
        [SerializeField] List<string> fileNames;

        public List<string> Collection 
        { 
            get
            {
                if (fileNames != null)
                    return fileNames;
                else
                    return fileNames = new List<string>();
            } 
            set => fileNames = value; 
        }

    }
}